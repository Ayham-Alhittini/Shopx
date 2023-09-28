using Microsoft.AspNetCore.Mvc;
using Shopx.API.Data;
using Shopx.API.DTOs.Initialization.Fields;
using Stripe;

namespace Shopx.API.Controllers
{
    public class InitializationController: BaseApiController
    {
        [HttpGet("get-categories")]
        public ActionResult GetCategories()
        {
            return Ok(_options.GetCategoires());
        }

        [HttpGet("get-computers&laptops")]
        public ActionResult GetComputersAndLaptops()
        {
            var options = _options.GetLaptopAndComputer();

            var model = new
            {
                Brand = new Field("Brand", FormType.select, options.Brand),
                OperatingSystem = new DependentField("Operating System",
                    FormType.select, options.OperatingSystem, "brand"),
                ScreenSize = new Field("Screen Size", FormType.select, options.ScreenSize),
                Ram = new Field("RAM", FormType.number)
            };

            return Ok(model);
        }

        [HttpGet("get-vehicles")]
        public ActionResult GetVehicles()
        {
            var options = _options.GetVehicles();

            var model = new
            {
                CarMake = new Field("Car Make", FormType.select, options.CarMake),
                Model = new DependentField("Model", FormType.select, options.Model, "carMake"),
                Year = new Field("Year", FormType.number),
                Transmission = new Field("Transmission", FormType.select, options.Transmission),
                Fuel = new Field("Fuel", FormType.select, options.Fuel),
                Color = new Field("Color", FormType.select, options.Color),
                Condition = new Field("Condition", FormType.select, options.Condition),
                Kilometers = new Field("Kilometers", FormType.select, options.Kilometers),
                Paint = new Field("Paint", FormType.select, options.Paint),
            };

            return Ok(model);
        }

        [HttpGet("get-mobile&tablets")]
        public ActionResult GetMobileAndTablets()
        {
            var options = _options.GetMobile();

            var model = new
            {
                Brand = new Field("Brand", FormType.select, options.Brand),
                Model = new DependentField("Model", FormType.select, options.Model, "brand"),
                StorageSize = new Field("Storage Size", FormType.select, options.StorageSize),
                Color = new Field("Color", FormType.select, options.Color),
                ScreenSize = new Field("Screen Size", FormType.select, options.ScreenSize)
            };

            return Ok(model);
        }

        [HttpGet("get-pets")]
        public ActionResult GetPets()
        {
            var options = _options.GetPets();


            var model = new
            {
                PetName = new Field("Pet Name", FormType.select, options.PetName),
                PetType = new DependentField("Pet Type", FormType.select, options.PetType, "petName")
            };

            return Ok(model);
        }

        [HttpGet("get-monitors")]
        public ActionResult GetMonitors()
        {
            var options = _options.GetMointer();

            var model = new
            {
                Brand = new Field("Brand", FormType.select, options.Brand),
                ScreenSize = new Field("Screen Size", FormType.select, options.ScreenSize)
            };

            return Ok(model);
        }

        [HttpGet("get-accessories")]
        public ActionResult GetAccessories()
        {
            var options = _options.GetAccessorie();

            var model = new
            {
                Category = new Field("Category", FormType.select, options.Category),
                AccessoriesType = new DependentField("Accessories Type", FormType.select, options.AccessoriesType, "category"),
            };

            return Ok(model);
        }

        [HttpGet("get-general")]
        public ActionResult GetGeneral()
        {
            var model = new
            {
                ProductName = new Field("Product Name", FormType.text),
                Price = new Field("Price", FormType.number),
                Quantity = new Field("Quantity", FormType.number),
                ProductDescription = new Field("Product Description", FormType.text)
            };
            return Ok(model);
        }

        [HttpGet("get-report-reasons")]
        public ActionResult GetReportsReasons()
        {
            return Ok(_options.ReportReasons());
        }

        [HttpGet("get-cities")]
        public ActionResult GetJordanCities()
        {
            return Ok(_options.Cities());
        }
    }
}
