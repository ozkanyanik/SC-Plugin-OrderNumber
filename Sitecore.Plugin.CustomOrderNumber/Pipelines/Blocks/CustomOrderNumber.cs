using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Commerce.Plugin.Orders;
using Sitecore.Framework.Conditions;
using Sitecore.Framework.Pipelines;

namespace Sitecore.Plugin.CustomOrderNumber.Pipelines.Blocks
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomOrderNumber : PipelineBlock<Order, Order, CommercePipelineExecutionContext>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly FindEntitiesInListCommand _findEntitiesInListCommand;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="findEntitiesInListCommand"></param>
        public CustomOrderNumber(FindEntitiesInListCommand findEntitiesInListCommand) : base((string)null)
        {
            // We need to use this to get all existing orders placed before the new order
            _findEntitiesInListCommand = findEntitiesInListCommand;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Task<Order> Run(Order order, CommercePipelineExecutionContext context)
        {
            // We need to ensure the order object is not null.
            Condition.Requires<Order>(order).IsNotNull<Order>("The order can not be null");

            // We call a methos that will generate our new order number. We pass parameters that may help in formulating the order number.
            order.OrderConfirmationId = GetCustomOrderNumber(order, context, _findEntitiesInListCommand);

            // return the new order so that it can be sent to the entities databse
            return Task.FromResult<Order>(order);
        }

        /// <summary>
        /// You may customized what is returned here based on number od existing orders or date or get number from external system
        /// </summary>
        /// <param name="order"></param>
        /// <param name="context"></param>
        /// <param name="findEntitiesInListCommand"></param>
        /// <returns></returns>
        private string GetCustomOrderNumber(Order order, CommercePipelineExecutionContext context, FindEntitiesInListCommand findEntitiesInListCommand)
        {
            //Get Contact Component which contains customer information
            var contactComponent = order.GetComponent<ContactComponent>();

            try
            {
                int orderCounts = 0;
                // get all existing orders.
                IEnumerable<Order> orders = (IEnumerable<Order>)findEntitiesInListCommand.Process<Order>(context.CommerceContext, CommerceEntity.ListName<Order>(), 0, int.MaxValue).Result.Items;

                // use the info you have to generate an appropriate order number. You may also use the data you have to call an external system.
                // in this instance we will just return the number of existing orders incremented by 1
                // Return order count and increment by 1 as the new order number.
                if (orders.Any())
                {
                    // Total orders
                    orderCount = orders.Count();
                }
                return (orderCount + 1).ToString();
            }
            catch (Exception)
            {
                // return a random guid if ther was an isssue retriving existing orders or all else failed.
                return Guid.NewGuid().ToString("B");
            }
        }
    }
}
