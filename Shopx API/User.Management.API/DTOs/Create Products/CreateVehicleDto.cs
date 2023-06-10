using Newtonsoft.Json.Serialization;
using Shopx.API.Data;
using Shopx.API.DTOs.Initialization.Options;
using System.ComponentModel.DataAnnotations;

namespace Shopx.API.DTOs.Create_Products
{
    public class CreateVehicleDto: CreateProductDto
    {
        private VehicleOptions initial = _options.GetVehicles();

        private string _carMake;
        private string _model;
        private int _year;
        private string _type;
        private string _transmission;
        private string _fuel;
        private string _color;
        private string _condition;
        private string _kilometers;
        private string _paint;


        [Required]
        public string CarMake
        {
            get { return _carMake; }
            set 
            {
                if (!initial.CarMake.Contains(value))
                    throw new ArgumentException("Car Make not exist");
                _carMake = value;
            }
        }

        [Required]
        public string Model
        {
            get { return _model; }
            set 
            {
                var models = initial.Model.Where(m => m.name == _carMake)
                    .Select(m => m.list).FirstOrDefault();

                if (!models.Contains(value))
                    throw new ArgumentException("Model not exist");

                _model = value;
            }
        }

        [Required]
        public int Year
        {
            get { return _year; }
            set 
            {
                if (value < 1970)
                    throw new ArgumentException("Car year can't be less than 1970");
                else if (value > DateTime.UtcNow.Year + 1) 
                    throw new ArgumentException($"Car year can't be more than {DateTime.UtcNow.Year + 1}");
                _year = value;
            }
        }

        [Required]
        public string Type
        {
            get { return _type; }
            set 
            { 
                if (_options.GetSubCategory(Categories.Vehicles, value) != null)
                    _type = value;
            }
        }

        [Required]
        public string Transmission
        {
            get { return _transmission; }
            set 
            {
                if (!initial.Transmission.Contains(value))
                    throw new ArgumentException("Transmission not exist");
                _transmission = value;
            }
        }

        [Required]
        public string Fuel
        {
            get { return _fuel; }
            set 
            {
                if (!initial.Fuel.Contains(value))
                    throw new ArgumentException("Fuel not exist");
                _fuel = value;
            }
        }

        [Required]
        public string Color
        {
            get { return _color; }
            set 
            {
                if (!initial.Color.Contains(value))
                    throw new ArgumentException("Color not exist");
                _color = value;
            }
        }

        [Required]
        public string Condition
        {
            get { return _condition; }
            set
            {
                if (!initial.Condition.Contains(value))
                    throw new ArgumentException("Condition not exist");
                _condition = value;
            }
        }

        [Required]
        public string Kilometers
        {
            get { return _kilometers; }
            set
            {
                if (!initial.Kilometers.Contains(value))
                    throw new ArgumentException("Kilometers not exist");
                _kilometers = value;
            }
        }

        [Required]
        public string Paint
        {
            get { return _paint; }
            set
            {
                if (!initial.Paint.Contains(value))
                    throw new ArgumentException("Paint not exist");
                _paint = value;
            }
        }
    }
}
