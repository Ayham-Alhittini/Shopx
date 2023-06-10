using Microsoft.AspNetCore.Identity;
using RestSharp;
using Shopx.API.Data;
using Shopx.API.Entities;

namespace Shopx.API.Helper
{
    public class GenericMethod
    {
        public static string GetSpecification(string category, string subcategory)
        {
            if (subcategory == SubCategories.Accessories)
            {
                return "accessories";
            }
            else if (subcategory == SubCategories.Monitors)
            {
                return "monitorProduct";
            }
            else if (category == Categories.MobilesAndTablets)
            {
                return "mobile";
            }

            else if (category == Categories.ComputersAndLaptops)
            {
                return "laptop";
            }
            else if (category == Categories.Vehicles)
            {
                return "vehicle";
            }
            else if (category == Categories.Pets)
            {
                return "pet";
            }
            throw new ArgumentException("Error Occur :Specification Not Found");
        }
        public static string GetProductLink(string category, string subcategory)
        {
            if (subcategory == SubCategories.Accessories)
            {
                return "accessories";
            }
            else if (subcategory == SubCategories.Monitors)
            {
                return "monitors";
            }
            else if (category == Categories.MobilesAndTablets)
            {
                return "mobile&tablets";
            }

            else if (category == Categories.ComputersAndLaptops)
            {
                return "computers&laptops";
            }
            else if (category == Categories.Vehicles)
            {
                return "vehicles";
            }
            else if (category == Categories.Pets)
            {
                return "pets";
            }
            throw new ArgumentException("Error Occur :Product Link Not Found");
        }
        public static string GetPhoneNumberFormat(string phoneNumber)
        {
            if (phoneNumber.First() != '+')
                phoneNumber = "+" + phoneNumber;

            if (phoneNumber[4] == '0')
            {
                phoneNumber = phoneNumber.Remove(4, 1);
            }
            return phoneNumber;
        }
        public static bool CheckMobileNumber(string mobileNumber)
        {
            var client = new RestClient("https://api.apilayer.com/number_verification/validate?number=" + mobileNumber);

            var request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("apikey", "IJWDAe7s2HC76IAPQwv1h5ckW86HVISB");

            RestResponse response = client.Execute(request);

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content.ToString());
            if (result != null)
                return result["valid"];

            return false;
        }
    }
}
