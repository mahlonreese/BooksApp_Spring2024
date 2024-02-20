using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BooksApp_Spring2024.Models
{
    public class Category
    {
        public int CategoryID { get; set; }


        //server side validation
        [DisplayName("Category Name"), Required(ErrorMessage = "Name of the Category must be provided")]
        public string Name { get; set; }


        // ? after datatype shows that it is a nullable datatype
        [DisplayName("Category Description")]
        public string? Description { get; set; }

    }
}
