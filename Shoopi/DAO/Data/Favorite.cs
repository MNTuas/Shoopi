using System;
using System.Collections.Generic;

namespace Shoopi.Data;

public partial class Favorite
{
    public int FavoriteId { get; set; }

    public DateOnly? Day { get; set; }

    public string? Detail { get; set; }

    public int? UserId { get; set; }

    public int? ProductId { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
