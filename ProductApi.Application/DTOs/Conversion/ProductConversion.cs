using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Application.DTOs.Conversion
{
    public class ProductConversion
    {
        public static Product ToEntity(ProductDTO product) => new()
        {
            Id = product.Id,
            Name = product.Name,
            Quantity = product.Quantity,
            Price = product.Price,
        };

        public static (ProductDTO?, IEnumerable<ProductDTO>?) FromEntity(Product product, IEnumerable<Product>? products)
        {
            // return single
            if(product == null || product is null)
            {
                var sigleProduct = new ProductDTO
                (
                    product!.Id,
                    product.Name!,
                    product.Quantity,
                    product.Price
                ); 
                return (sigleProduct, null);
            }

            //return list
            if(products is not null || product is null)
            {
                var _products = products!.Select(p => new ProductDTO
                    (p.Id, p.Name!, p.Quantity, p.Price)).ToList();
            }

            return (null, null);
        }
    }
}
