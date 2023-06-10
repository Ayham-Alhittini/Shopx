using Shopx.API.Data;

namespace Shopx.API.DTOs
{
    public class CreateOrderDto
    {
        public AddressDto Address { get; set; }
        public CardDto Card { get; set; }
        private string _deliveryOption { get; set; }
        public string DeliveryOption
        {
            get { return _deliveryOption; }
            set
            {
                if (value != DeliveryOptions.Quick 
                    && value != DeliveryOptions.Medium 
                    && value != DeliveryOptions.Slow 
                    && value != DeliveryOptions.Free)
                {
                    throw new ArgumentException("invalid delvery option");
                }
                _deliveryOption = value;
            }
        }
    }
}
