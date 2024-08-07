using System;
using System.Collections.Generic;

namespace DAO.Data;

public partial class Type
{
    public int TypeId { get; set; }

    public string? TypeName { get; set; }

    public string? Detail { get; set; }

    public string? Note { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
