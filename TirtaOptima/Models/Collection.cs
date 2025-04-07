using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class Collection
{
    public long Id { get; set; }

    public long PiutangId { get; set; }

    public long? SuratId { get; set; }

    public DateTime Tanggal { get; set; }

    public long? StatusId { get; set; }

    public string? NamaPenerima { get; set; }

    public string? NotelpPenerima { get; set; }

    public DateOnly? RencanaBayar { get; set; }

    public string Foto { get; set; } = null!;

    public string? Alasan { get; set; }

    public string? Ket { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? DeletedByNavigation { get; set; }

    public virtual Debt Piutang { get; set; } = null!;

    public virtual Status? Status { get; set; }

    public virtual Letter? Surat { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
