using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class Collection
{
    public long Id { get; set; }

    public long PiutangId { get; set; }

    public long? SuratId { get; set; }

    public DateTime Tanggal { get; set; }

    public string? StatusId { get; set; }

    public long? PenagihId { get; set; }

    public long? TindakanId { get; set; }

    public string? NamaPenerima { get; set; }

    public string? NotelpPenerima { get; set; }

    public DateOnly? RencanaBayar { get; set; }

    public string? Foto { get; set; }

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

    public virtual User? Penagih { get; set; }

    public virtual Bill Piutang { get; set; } = null!;

    public virtual Letter? Surat { get; set; }

    public virtual ActionType? Tindakan { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
