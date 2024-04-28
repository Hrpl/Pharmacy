using System;
using System.Collections.Generic;

namespace Pharmacy.Models;

public partial class MedicInOrder
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public int MedicineId { get; set; }

    public int Quantity { get; set; }
    
    public int? PriceForOne { get; set; }

    public virtual Medicine Medicine { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
