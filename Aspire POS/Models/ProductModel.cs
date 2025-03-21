﻿using Newtonsoft.Json;

public class ProductModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateModified { get; set; }
    public string Type { get; set; }
    public string Status { get; set; }
    public bool Featured { get; set; }
    public string Description { get; set; }
    public string ShortDescription { get; set; }
    public decimal Price { get; set; }
    public decimal RegularPrice { get; set; }
    public bool Purchasable { get; set; }
    public bool InStock { get; set; }

    [JsonProperty("stock_quantity")]
    public int? StockQuantity { get; set; }
    public string TaxStatus { get; set; }
    public string TaxClass { get; set; }
    public bool ShippingRequired { get; set; }
    public bool ShippingTaxable { get; set; }

    public List<CategoryModel> Categories { get; set; } = new();

    public List<ImageModel> Images { get; set; } = new();
    public List<int> RelatedIds { get; set; } = new();
}

public class CategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
}

public class ImageModel
{
    public int Id { get; set; }
    public string Src { get; set; }
    public string Name { get; set; }
    public string Alt { get; set; }
}
