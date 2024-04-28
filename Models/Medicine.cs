using System;
using System.Collections.Generic;

namespace Pharmacy.Models;

public partial class Medicine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Manufacture { get; set; } = null!;

    public DateOnly? ExpretionDate { get; set; }

    public int? Quantity { get; set; }

    public int? BuyPrice { get; set; }

    public int? SalePrice { get; set; }

    public virtual ICollection<MedicInOrder> MedicInOrders { get; set; } = new List<MedicInOrder>();

    public virtual ICollection<MedicInRequest> MedicInRequests { get; set; } = new List<MedicInRequest>();
}
