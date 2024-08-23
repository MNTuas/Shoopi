using System;
using System.Collections.Generic;

namespace DAO.Data;

public partial class UserLogin
{
    public int UserLoginId { get; set; }

    public int UserId { get; set; }

    public string? LoginProvider { get; set; }

    public string? ProviderKey { get; set; }

    public DateTime CreatedDate { get; set; }

    public virtual User User { get; set; } = null!;
}
