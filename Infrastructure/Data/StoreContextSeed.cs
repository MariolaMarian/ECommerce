using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAgregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Infrastructure.Data
{
    public class StoreContextSeed
    {
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if(!context.ProductBrands.Any())
                {
                    var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
                    var brands = JsonConvert.DeserializeObject<IEnumerable<ProductBrand>>(brandsData);

                    foreach(var brand in brands)
                    {
                        context.ProductBrands.Add(brand);
                    }

                    await context.SaveChangesAsync();
                }

                if(!context.ProductTypes.Any())
                {
                    var productTypesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
                    var productTypes = JsonConvert.DeserializeObject<IEnumerable<ProductType>>(productTypesData);

                    foreach(var productType in productTypes)
                    {
                        context.ProductTypes.Add(productType);
                    }

                    await context.SaveChangesAsync();
                }

                if(!context.Products.Any())
                {
                    var productData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
                    var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(productData);

                    foreach(var product in products)
                    {
                        context.Products.Add(product);
                    }

                    await context.SaveChangesAsync();
                }

                if(!context.DeliveryMethods.Any())
                {
                    var deliveryMethods = File.ReadAllText("../Infrastructure/Data/SeedData/delivery.json");
                    var deliveries = JsonConvert.DeserializeObject<IEnumerable<DeliveryMethod>>(deliveryMethods);

                    foreach(var deliveryMethod in deliveries)
                    {
                        context.DeliveryMethods.Add(deliveryMethod);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }
        }
    }
}