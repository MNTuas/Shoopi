﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace DAO.Data;

public partial class OrderStatus
{
    public int OrderStatusId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}