using Shopx.API.Data;
using Shopx.API.DTOs.Initialization.Options;
using System.ComponentModel.DataAnnotations;

namespace Shopx.API.DTOs.Create_Products
{
    public class CreateAccessoriesDto: CreateProductDto
    {
        private AccessorieOptions initial = _options.GetAccessorie();

        private string _category;
        private string _type;


        [Required]
        public string Category
        {
            get { return _category; }
            set 
            {
                if (!initial.Category.Contains(value))
                    throw new ArgumentException("Category not exist");
                _category = value;
            }
        }

        [Required]
        public string AccessoriesType
        {
            get { return _type; }
            set
            {
                var types = initial.AccessoriesType.Where(acc => acc.name == _category).Select(acc => acc.list).FirstOrDefault();

                if (!types.Contains(value))
                    throw new ArgumentException("Type not exist");
                _type = value;
            }
        }
    }
}
