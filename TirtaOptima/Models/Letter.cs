using System;
using System.Collections.Generic;

namespace TirtaOptima.Models;

public partial class Letter
{
    public long Id { get; set; }

    public long PenagihanId { get; set; }

    public long TindakanId { get; set; }

    public long PimpinanId { get; set; }

    public long? KategoriId { get; set; }

    public string NomorSurat { get; set; } = null!;

    public string? Sifat { get; set; }

    public int? Lampiran { get; set; }

    public string? Ket { get; set; }

    public string? Note { get; set; }

    public string? Status { get; set; }

    public string? NamaPenerima { get; set; }

    public string? NotelpPenerima { get; set; }

    public DateOnly? RencanaBayar { get; set; }

    public string? Foto { get; set; }

    public string? Alasan { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public long? CreatedBy { get; set; }

    public long? UpdatedBy { get; set; }

    public long? DeletedBy { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? DeletedByNavigation { get; set; }

    public virtual LetterCategory? Kategori { get; set; }

    public virtual Collection Penagihan { get; set; } = null!;

    public virtual Leader Pimpinan { get; set; } = null!;

    public virtual ActionType Tindakan { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
