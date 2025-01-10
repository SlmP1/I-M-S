using System;
using System.Collections.Generic;

namespace IMS.Models;

public partial class Inventory
{
    public int Id { get; set; }

    public int? ProductId { get; set; }

    public int Quantity { get; set; }

    public string? Location { get; set; }

    public virtual Product? Product { get; set; }

    internal class Data
    {
        internal class ApplicationDbContext
        {
        }
    }
}
