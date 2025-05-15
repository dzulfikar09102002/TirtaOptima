using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class DebtsManagement
{
    public long Id { get; set; }

    public long? PiutangId { get; set; }

    public long? PembayaranId { get; set; }

    public string Status { get; set; } = null!;

    public decimal Nominal { get; set; }

    public DateTime? Tanggal { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? DeletedByNavigation { get; set; }

    public virtual Payment? Pembayaran { get; set; }

    public virtual Bill? Piutang { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
