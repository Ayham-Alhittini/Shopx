using Shopx.API.Data;
using Shopx.API.DTOs.Initialization.Options;
using System.ComponentModel.DataAnnotations;

namespace Shopx.API.DTOs.Create_Products
{
    public class CreateMobilesDto: CreateProductDto
    {
        private MobileOptions initial = _options.GetMobile();

        private string _type;
        private string _brand;
        private string _model;
        private string _storageSize;
        private string _color;
        private string _screenSize;


        [Required]
        public string Type
        {
            get { return _type; }
            set 
            {
                if (value != "Mobile" && value != "Tablet")
                    throw new ArgumentException("Mobile & Tablet are the only available types");
                _type = value;
            }
        }

        [Required]
        public string Brand
        {
            get { return _brand; }
            set
            {
                if (!initial.Brand.Contains(value))
                    throw new ArgumentException("Brand not exist");
                _brand = value;
            }
        }

        [Required]
        public string Model
        {
            get { return _model; }
            set
            {
                var models = initial.Model.Where(m => m.name == _brand).Select(b => b.list)
                    .FirstOrDefault();

                if (!models.Contains(value))
                    throw new ArgumentException("Model not exist");

                _model = value;
            }
        }

        [Required]
        public string StorageSize
        {
            get { return _storageSize; }
            set
            {
                if (!initial.StorageSize.Contains(value))
                    throw new ArgumentException("storage size not exist");
                _storageSize = value;
            }
        }

        [Required]
        public string Color
        {
            get { return _color; }
            set
            {
                if (!initial.Color.Contains(value))
                    throw new ArgumentException("color not exist");
                _color = value;
            }
        }

        public string ScreenSize
        {
            get { return _screenSize; }
            set
            {
                if (!initial.ScreenSize.Contains(value))
                    throw new ArgumentException("screen size not exist");
                _screenSize = value;
            }
        }
    }
}
