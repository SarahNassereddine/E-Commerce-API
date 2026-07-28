using lab3.Models;
using System.Reflection.Metadata.Ecma335;

namespace lab3.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted {  get; set; }
    }

    public class EditableProduct
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

    }
    public class CreateProductCmd : EditableProduct
    {


        public Product toProduct()
        {
            return new Product
            {
                Name = Name,
                Price = Price,
                Description = Description,
                CategoryId = CategoryId
                
            };
        }
    }
    public class UpdateProductCmd : EditableProduct
    {
      

        public void UpdateProduct(Product p)
        {

            p.Name = Name;
            p.Price = Price;
            p.Description = Description;
            p.CategoryId = CategoryId;
            
         
        }

    }
    public class DisplayableProduct
    {
    
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
    

        public static DisplayableProduct FromProduct(Product p)
        {
            return new DisplayableProduct
            {
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                CategoryId = p.CategoryId,
      


            };
    }
    }

    public class DisplayableProductWithId: DisplayableProduct
    {
        public int ProductId { get; set; }
        public static DisplayableProductWithId FromProduct(Product p)
        {
            return new DisplayableProductWithId
            {
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                CategoryId = p.CategoryId,
                ProductId=p.ProductId



            };
        }

    }
}