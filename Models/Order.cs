using System;
using System.Collections.Generic;

namespace Pharmacy.Models;

public partial class Order
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int? SummaryPrice { get; set; }

    public virtual ICollection<MedicInOrder> MedicInOrders { get; set; } = new List<MedicInOrder>();
}
