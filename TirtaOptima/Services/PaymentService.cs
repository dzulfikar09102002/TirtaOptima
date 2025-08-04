using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;
namespace TirtaOptima.Services
{
    public class PaymentService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public string? Message { get; set; }
        public List<Payment> Payments { get; set; } = new();
        public List<DebtsManagement> DebtsManagements { get; set; } = new();
        public List<Customer> GetCustomers(List<long> idpelanggan)
        {
            return _context.Customers
                .Include(x => x.JenisNavigation)
                .Include(x => x.KelurahanNavigation)
                .Include(x => x.KecamatanNavigation)
                .Include(x => x.StatusNavigation)
                .Where(x => idpelanggan.Contains(x.Id))
                .ToList();
        }
        public List<Payment> GetPayments(PaymentViewModel model) =>
            [.._context.Payments.Where(x => x.Bulan == model.BulanSelect && x.Tahun == model.TahunSelect && x.IdPelangganNavigation != null)
            .Include(x => x.IdPelangganNavigation!)
                .ThenInclude(x => x.JenisNavigation!)
            .Include(x => x.IdPelangganNavigation!)
                .ThenInclude(x => x.KelurahanNavigation!)
                    .ThenInclude(x => x.KodeKecNavigation!)
            .Include(x => x.IdPelangganNavigation!)
                .ThenInclude(x => x.StatusNavigation!)];

        public async Task<bool> GetPaymentsApiAsync(PaymentViewModel model)
        {
            ApiHelper api = new();
            var apiurl = $"{api.BaseUrl}datapembayaran?bulan={model.BulanSelect}&tahun={model.TahunSelect}";

            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("username", api.Username);
            client.DefaultRequestHeaders.Add("password", api.Password);

            try
            {
                var apires = await client.GetAsync(apiurl);
                if (apires.IsSuccessStatusCode)
                {
                    var resdata = await apires.Content.ReadAsStringAsync();
                    var payments = System.Text.Json.JsonSerializer.Deserialize<List<PaymentColumn>>(resdata);

                    if (payments != null && payments.Count > 0)
                    {
                        List<Payment> validPayments = new();
                        bool hasMissingCustomers = false;
                        bool hasMissingBills = false;

                        foreach (var item in payments)
                        {
                            // Skip jika sudah ada payment untuk pelanggan ini (opsional)
                            if (_context.Payments.Any(x => x.IdPelanggan == item.IdPelanggan &&
                                                           x.Bulan == item.Bulan &&
                                                           x.Tahun == item.Tahun))
                                continue;

                            // Cek pelanggan
                            var pelanggan = _context.Customers
                                .Include(x => x.KelurahanNavigation)
                                .Include(x => x.KecamatanNavigation)
                                .Include(x => x.JenisNavigation)
                                .Include(x => x.StatusNavigation)
                                .FirstOrDefault(x => x.Id == item.IdPelanggan);

                            if (pelanggan == null)
                            {
                                hasMissingCustomers = true;
                                continue;
                            }

                            // Cek tagihan (rekening/bill) untuk bulan dan tahun yang sama
                            var hasBill = _context.Bills.Any(b =>
                                b.IdPelanggan == item.IdPelanggan &&
                                b.Bulan == item.Bulan &&
                                b.Tahun == item.Tahun
                            );

                            if (!hasBill)
                            {
                                hasMissingBills = true;
                                continue;
                            }

                            // Hanya tambahkan jika keduanya ada
                            validPayments.Add(new Payment
                            {
                                IdPelanggan = item.IdPelanggan,
                                Kasir = item.Kasir,
                                NominalBayar = item.NominalBayar,
                                TanggalBayar = item.TanggalBayar,
                                Bulan = item.Bulan,
                                Tahun = item.Tahun
                            });
                        }

                        if (validPayments.Count == 0)
                        {
                            Message = "Gagal memuat data. Pelanggan atau rekening tidak ditemukan di database.";
                            return false;
                        }

                        Payments = validPayments;

                        if (hasMissingCustomers && hasMissingBills)
                        {
                            Message = "Catatan: Sebagian data pelanggan dan tagihan tidak ditemukan. Periksa data terbaru.";
                        }
                        else if (hasMissingCustomers)
                        {
                            Message = "Catatan: Sebagian data pelanggan tidak ditemukan. Periksa data pelanggan terbaru.";
                        }
                        else if (hasMissingBills)
                        {
                            Message = "Catatan: Sebagian data rekening/tagihan tidak ditemukan. Periksa data tagihan terbaru.";
                        }

                        else
                        {
                            Message = string.Empty;
                        }

                        return true;
                    }
                    else
                    {
                        Message = "Tidak ada data terbaru.";
                        return false;
                    }
                }
                else
                {
                    Message = "Gagal mengambil data dari server.";
                    return false;
                }
            }
            catch
            {
                Message = "Terjadi kesalahan saat memanggil API.";
                return false;
            }
        }

        public void Store(string[] selectedpayments, long userid)
        {
            var idpayment = _context.Payments.Any() ? _context.Payments.Max(x => x.Id) + 1 : 1;
            var iddm = _context.DebtsManagements.Any() ? _context.DebtsManagements.Max(x => x.Id) + 1 : 1;
            foreach (var paymentJson in selectedpayments)
            {
                var payment = JsonConvert.DeserializeObject<PaymentColumn>(paymentJson);
                if (payment != null)
                {
                    Payments.Add(new Payment
                    {
                        Id = idpayment,
                        IdPelanggan = payment.IdPelanggan,
                        NominalBayar = payment.NominalBayar,
                        Ket = payment.Ket,
                        TanggalBayar = payment.TanggalBayar,
                        Bulan = payment.Bulan,
                        Tahun = payment.Tahun,
                        Kasir = payment.Kasir,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatedBy = userid,
                        UpdatedBy = userid,
                    });
                    var idpiutang = _context.Bills.Where(x => x.DeletedAt == null
                    && x.IdPelanggan == payment.IdPelanggan
                    && x.Bulan == payment.Bulan && x.Tahun == payment.Tahun).FirstOrDefault()?.Id;
                    DebtsManagements.Add(new DebtsManagement
                    {
                        Id = iddm,
                        PiutangId = idpiutang,
                        PembayaranId = idpayment,
                        Nominal = payment.NominalBayar,
                        Status = "Debit",
                        Tanggal = payment.TanggalBayar.HasValue
                        ? payment.TanggalBayar.Value.ToDateTime(TimeOnly.MinValue)
                        : (DateTime?)null,
                        CreatedAt = DateTime.Now,
                        CreatedBy = userid,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = userid
                    });
                    idpayment++;
                    iddm++;
                }
            }
            _context.Payments.AddRange(Payments);
            _context.DebtsManagements.AddRange(DebtsManagements);
            _context.SaveChanges();
        }
    }
}
