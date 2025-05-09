using Microsoft.EntityFrameworkCore;
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
                    var payments = JsonSerializer.Deserialize<List<PaymentColumn>>(resdata);

                    if (payments != null && payments.Count > 0)
                    {
                        List<Payment> validPayments = new();
                        bool hasMissingCustomers = false;

                        foreach (var item in payments)
                        {
                            if (_context.Payments.Any(x => x.IdPelanggan == item.IdPelanggan))
                                continue;
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
                            validPayments.Add(new Payment
                            {
                                IdPelanggan = item.IdPelanggan,
                                IdPelangganNavigation = pelanggan,
                                Kasir = item.Kasir,
                                NominalBayar = item.NominalBayar,
                                TanggalBayar = item.TanggalBayar,
                                Bulan = item.Bulan,
                                Tahun = item.Tahun,

                            });
                        }
                        if(validPayments.Count == 0)
                        {
                            Message = "Periksa Data Pelanggan Terbaru";
                            return false;
                        }
                        if (validPayments.Count > 0)
                        {
                            Payments = validPayments;
                            Message = hasMissingCustomers
                                ? "Catatan : Periksa Data Pelanggan Terbaru"
                                : string.Empty;
                            return true;
                        }

                        Message = "Tidak Ada Data Terbaru";
                        return false;
                    }
                    else
                    {
                        Message = "Tidak Ada Data Terbaru";
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
    }
}
