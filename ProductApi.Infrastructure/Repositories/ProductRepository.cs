using eCommerce.SharedLibrary.Responses;
using log4net.Core;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.Repositories
{
    internal class ProductRepository(ProductDBContext context) : IProduct
    {
        public async Task<Response> CreateAsync(Product entity)
        {
            try
            {
                // check if the prduct already exists in the database by name
                var getProduct = await GetByAsync(_ => _.Name!.Equals(entity.Name));

                if (getProduct is not null && !string.IsNullOrEmpty(getProduct.Name))
                {
                    return new Response(false, $"{entity.Name} already added...");
                }

                var currentEntity = context.Products.Add(entity);
                await context.SaveChangesAsync();

                if (currentEntity is not null && currentEntity.Entity.Id > 0)
                {
                    return new Response(true, $"{entity.Name} added to database successfully...");
                }
                else 
                {
                    return new Response(false, $"Failed to add {entity.Name} to database...");
                }
            }
            catch (Exception ex) 
            {
                // Log the original exception details for debugging purposes
                ex.Message.ToString(); // Log the exception message

                // display scary-free message to the client
                return new Response(false, "Error occurred adding new product");
            }
        }

        public async Task<Response> DeleteAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if(product is null)
                {
                    return new Response(false, $"Product with Name {entity.Name} not found");
                }
                context.Products.Remove(product);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} deleted successfully...");
            }
            catch (Exception ex)
            {
                // Log the original exception details for debugging purposes
                ex.Message.ToString(); // Log the exception message

                // display scary-free message to the client
                return new Response(false, "Error occurred adding new product");
            }
        }

        

        public async Task<Product> FindByIdAsync(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {

                // Log the original exception details for debugging purposes
                ex.Message.ToString(); // Log the exception message

                // display scary-free message to the client
                throw new Exception("Error occurred retriving new product");
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            try
            {
                var products = await context.Products.AsNoTracking().ToListAsync();
                return products is not null ? products : null!;
            }
            catch (Exception ex)
            {

                // Log the original exception details for debugging purposes
                ex.Message.ToString(); // Log the exception message

                // display scary-free message to the client
                throw new Exception("Error occurred retriving new product");
            }
        }

        public async Task<Product> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                var product = await context.Products.Where(predicate).FirstOrDefaultAsync();
                return product is not null ? product : null!;
            }
            catch (Exception ex)
            {

                // Log the original exception details for debugging purposes
                ex.Message.ToString(); // Log the exception message

                // display scary-free message to the client
                throw new Exception("Error occurred retriving new product");
            }
        }

        public async Task<Response> UpdateAsync(Product entity)
        {
            try
            {
                var product = await FindByIdAsync(entity.Id);
                if (product is null)
                {
                    return new Response(false, $"Product with Name {entity.Name} not found");
                }
                context.Entry(product).State = EntityState.Detached; // Detach the existing entity from the context
                context.Products.Update(entity);
                await context.SaveChangesAsync();
                return new Response(true, $"{entity.Name} updated successfully...");
            }
            catch (Exception ex)
            {

                // Log the original exception details for debugging purposes
                ex.Message.ToString(); // Log the exception message

                // display scary-free message to the client
                return new Response(false, "Error occurred updating existing product");
            }
        }
    }
}
