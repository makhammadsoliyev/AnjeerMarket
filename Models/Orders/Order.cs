﻿using AnjeerMarket.Models.Commons;

namespace AnjeerMarket.Models.Orders;

public class Order : Auditable
{
    public long UserId { get; set; }
    public DateTime Date { get; set; } = DateTime.UtcNow;
}