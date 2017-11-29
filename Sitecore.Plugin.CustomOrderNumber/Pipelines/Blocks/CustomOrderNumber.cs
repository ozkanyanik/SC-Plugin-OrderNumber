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
            Condition.Requires<Order>(order).IsNotNull<Order>("The order can not be null");

            order.OrderConfirmationId = GetCustomOrderNumber(order, context, _findEntitiesInListCommand);

            return Task.FromResult<Order>(order);
        }

        /// <summary>
        /// You may customized what is returned here based on number od existing orders or date or get number from external system
        /// </summary>
        /// <param name="order"></param>
        /// <param name="context"></param>
        /// <param name="_findEntitiesInListCommand"></param>
        /// <returns></returns>
        private string GetCustomOrderNumber(Order order, CommercePipelineExecutionContext context, FindEntitiesInListCommand _findEntitiesInListCommand)
        {
            var orders = (IEnumerable<Order>)_findEntitiesInListCommand.Process<Order>(context.CommerceContext, CommerceEntity.ListName<Order>(), 0, int.MaxValue).Result.Items.ToList<Order>();

            // Return order count and increment by 1 as the new order number.
            if (orders.Any())
            {
                return (orders.Count() + 1).ToString();
            }

            // return a random guid if ther was an isssue retriving existing orders.
            return Guid.NewGuid().ToString("B");
        }


    }
}
