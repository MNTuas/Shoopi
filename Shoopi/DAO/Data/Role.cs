using System;
using System.Collections.Generic;

namespace DAO.Data;

public partial class Role
{
    public int RoleId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Users> Users { get; set; } = new List<Users>();
}
