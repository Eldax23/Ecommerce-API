﻿namespace eCommerceApp.Application.DTOs.Category
{
    public class UpdateCategory : CategoryBase
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
    }

}
