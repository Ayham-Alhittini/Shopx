using Shopx.API.Data;
using Shopx.API.DTOs.Initialization.Options;
using System.ComponentModel.DataAnnotations;

namespace Shopx.API.DTOs.Create_Products
{
    public class CreateMonitorDto: CreateProductDto
    {
        private MonitorOptions initial = _options.GetMointer();

        private string _brand;
        private string _screenSize;

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
        public string ScreenSize
        {
            get { return _screenSize; }
            set 
            {
                if (!initial.ScreenSize.Contains(value))
                    throw new ArgumentException("Screen size not exist");
                _screenSize = value;
            }
        }
    }
}
