using System;
using System.Collections.Generic;

namespace Shoopi.Data;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? AliasName { get; set; }

    public string? Detail { get; set; }

    public decimal? Price { get; set; }

    public int? Quantity { get; set; }

    public string? Picture { get; set; }

    public string? Mfg { get; set; }

    public decimal? Discount { get; set; }

    public int? ViewNumber { get; set; }

    public int? TypeId { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Type? Type { get; set; }
}
