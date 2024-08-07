using System;
using System.Collections.Generic;

namespace DAO.Data;

public partial class User
{
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string? Password { get; set; }

    public string? Gender { get; set; }

    public DateOnly? Birthday { get; set; }

    public string? Address { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Email { get; set; }

    public string? Picture { get; set; }

    public bool Status { get; set; }

    public int? RoleId { get; set; }

    public string? RandomKey { get; set; }

    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Role? Role { get; set; }
}
