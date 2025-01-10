using System;
using System.Collections.Generic;

namespace IMS.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public string? TransactionType { get; set; }

    public DateTime? TransactionDate { get; set; }

    public string? Location { get; set; }

    public virtual Product? Product { get; set; }
}
