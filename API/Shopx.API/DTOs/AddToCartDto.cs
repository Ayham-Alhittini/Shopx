﻿using Shopx.API.Entities;

namespace Shopx.API.DTOs
{
    public class AddToCartDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
