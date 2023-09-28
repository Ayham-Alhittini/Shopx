using Shopx.API.Data;
using System.ComponentModel.DataAnnotations;
using Shopx.API.DTOs.Initialization.Options;

namespace Shopx.API.DTOs.Create_Products
{
    public class CreateLaptopDto: CreateProductDto
    {
        private ComputerAndLaptopOptions initial = _options.GetLaptopAndComputer();
        private string _brand { get; set; }
        private string _operatingSystem { get; set; }
        private string _screenSize { get; set; }
        private string _type { get; set; }

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
        public string OperatingSystem 
        { 
            get { return _operatingSystem; }
            set
            {
                if (_brand == "Apple")
                {
                    if (value != "MacOS")
                        throw new ArgumentException("Operating System not exist");
                }
                else
                {
                    if (value != "Windows" && value != "Linux")
                        throw new ArgumentException("Operating System not exist");
                }
                _operatingSystem = value;
            }
        }

        [Required]
        public string ScreenSize 
        {
            get { return _screenSize; }
            set
            {
                if (!initial.ScreenSize.Contains(value))
                    throw new ArgumentException("Screen Size not exist");
               _screenSize = value;
            }
        }

        [Required]
        public int Ram { get; set; }

        [Required]
        public string Type 
        {
            get { return _type; }
            set
            {
                if (value != "Laptop" && value != "Computer")
                    throw new ArgumentException("Laptop & Computer are the only available types");
                _type = value;
            }
        }
    }
}
