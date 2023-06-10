﻿using Shopx.API.Entities;

namespace Shopx.API.DTOs
{
    public class CartDto
    {
        public string CustomerId { get; set; }
        public string CustomerUsername { get; set; }

        public string SellerId { get; set; }
        public string SellerName { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public ProductCardDto Product { get; set; }
        public double Total { get; set; }
    }
}
