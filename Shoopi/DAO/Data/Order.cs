using System;
using System.Collections.Generic;

namespace DAO.Data;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public string? FullName { get; set; }

    public string? Address { get; set; }

    public DateOnly? CreateDate { get; set; }

    public DateOnly? DeliveryDate { get; set; }

    public string? MethodPayment { get; set; }

    public string? Delivery { get; set; }

    public decimal? FeeDelivery { get; set; }

    public string? Note { get; set; }

    public int? OrderStatusId { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual OrderStatus? OrderStatus { get; set; }

    public virtual Users? User { get; set; }
}
