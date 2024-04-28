using System;
using System.Collections.Generic;

namespace Pharmacy.Models;

public partial class MedicInRequest
{
    public int Id { get; set; }

    public int MedicineId { get; set; }

    public int Quantity { get; set; }

    public int? InUnitPrice { get; set; }

    public int? AllPrice { get; set; }

    public string RequestNumber { get; set; }

    public virtual Medicine Medicine { get; set; } = null!;

}
