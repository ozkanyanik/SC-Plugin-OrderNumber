
Sitecore Commerce Engine Customize Ordernumber plugin (Sitecore Commerce 9)
======================================

This plugin allows the user to customize order number. At the moment, Commerce Engine returns random random strings that is 8 characters long 
- It is very easy to integrate or extend to fit your needs.
- Using this plugin, you will be able to continue with your previous order numbering before moving to Commerce Engine.
- You can also fetch your order from external apps in your organization or customize based on items ordered or customer.


Sponsor
=======
This plugin was sponsored and created by Xcentium.

How to Install
==============

1. Copy it to your Sitecore Commerce Engine Solution and add it as a project 


2. Add it as a dependency to your Adventure works or Habitat or Custom Site plugin' project

3. To your ServiceCollectionExtensions.cs class, add the line below towards the end:

	              .ConfigurePipeline<IOrderPlacedPipeline>(builder => builder.Replace<OrderPlacedAssignConfirmationIdBlock, CustomOrderNumber>())

	So that the confic section looks somewhat like below:

```
        public static IServiceCollection ConfigureCartPipelines(this IServiceCollection services)
        {
            services.Sitecore().Pipelines(config => config
                .ConfigurePipeline<ICalculateCartLinesPipeline>(builder => builder
                    .Add<PopulateCartLineItemsBlock>()
                    .Add<CalculateCartLinesPriceBlock>()
                    .Add<ValidateCartLinesPriceBlock>()
                    .Add<CalculateCartLinesSubTotalsBlock>()
                    .Add<CalculateCartLinesFulfillmentBlock>()
                    .Add<ValidateCartCouponsBlock>()
                    .Add<CalculateCartLinesPromotionsBlock>()
                    .Add<CalculateCartLinesTaxBlock>()
                    .Add<CalculateCartLinesTotalsBlock>())

               .ConfigurePipeline<ICalculateCartPipeline>(builder => builder
                    .Add<CalculateCartSubTotalsBlock>()
                    .Add<CalculateCartFulfillmentBlock>()
                    .Add<CalculateCartPromotionsBlock>()
                    .Add<CalculateCartTaxBlock>()
                    .Add<CalculateCartTotalsBlock>()
                    .Add<CalculateCartPaymentsBlock>())
                    
              .ConfigurePipeline<IAddPaymentsPipeline>(builder => builder.Add<ValidateCartHasFulfillmentBlock>().After<ValidateCartAndPaymentsBlock>())
                            
              .ConfigurePipeline<IOrderPlacedPipeline>(builder => builder.Replace<OrderPlacedAssignConfirmationIdBlock, CustomOrderNumber>())

              );

            return services;
        }

```

4. It is now ready to use. It will start sending order numbers based on the number of existing numbers or the way you customize it. 

Note:
=====

- If you have any questions, comment or need us to help install, extend or adapt to your needs, do not hesitate to reachout to us at XCentium.




