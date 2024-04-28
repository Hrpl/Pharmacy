using System;
using System.Collections.Generic;

namespace Pharmacy.Models;

public partial class Request
{
    public int Id { get; set; }

    public DateTime DateTime { get; set; }

    public string ProviderName { get; set; } = null!;

    public string Status { get; set; } = null!;

    public int? SummaryPrice { get; set; }

    public string? Number { get; set; } = null!;
}
