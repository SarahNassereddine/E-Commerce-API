using Microsoft.Identity.Client;

namespace lab3.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
    }



    public class EditableCategory
    {
        public string Name { get; set; }
    
    }
    public class CreateCategoryCmd : EditableCategory
    {

        public Category toCategory()
        {
            return new Category
            {
                Name = Name

            };
        }
    }
    public class UpdateCategoryCmd : EditableCategory
    {
        

        public void UpdateCategory(Category category)
        {
            category.Name = Name;
          
        }
    }
    public class DisplayableCategory
    {
        public String Name { set; get; }

    
        public static DisplayableCategory FromCategory(Category category)
        {
            {
                return new DisplayableCategory
                {
                    Name = category.Name,
                
                };
            }

        }
    }

    public class DisplayableCategoryWithId: DisplayableCategory
    {


        public int CategoryId {  set; get; }

       
        public static DisplayableCategoryWithId FromCategory(Category category)
        {
            {
                return new DisplayableCategoryWithId
                {
                    Name = category.Name,
                    CategoryId = category.CategoryId,
         
                };
            }

        }
    }


}

