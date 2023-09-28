using RestSharp;

namespace Shopx.API.Extensions
{
    public static class ConvertExtension
    {
        public static double ToDollar(this double Price)
        {
            var client = new RestClient("https://api.apilayer.com/exchangerates_data/convert?to=USD&from=JOD&amount=" + Price);

            RestRequest request = new RestRequest();
            request.Method = Method.Get;
            request.AddHeader("apikey", "IJWDAe7s2HC76IAPQwv1h5ckW86HVISB");

            var response = client.Execute(request);

            double priceInDollar = -1;

            if (response.Content != null)
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(response.Content.ToString());
                if (result != null)
                    priceInDollar = result["result"];
            }

            return priceInDollar;
        }
    }
}
