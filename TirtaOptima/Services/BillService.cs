using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class BillService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Bill> Bills { get; set; } = new();
        public string? Message { get; set; }
        public List<Bill> GetBills(BillViewModel model) =>
        [.._context.Bills
            .Where(x => x.Bulan == model.BulanSelect && x.Tahun == model.TahunSelect && x.IdPelangganNavigation != null)
            .Include(x => x.IdPelangganNavigation!)
                .ThenInclude(x => x.JenisNavigation!)
            .Include(x => x.IdPelangganNavigation!)
                .ThenInclude(x => x.KelurahanNavigation!)
                    .ThenInclude(x => x.KodeKecNavigation!)
            .Include(x => x.IdPelangganNavigation!)
                .ThenInclude(x => x.StatusNavigation!)
        ];
        public async Task<bool> GetBillsApiAsync(BillViewModel model)
        {
            ApiHelper api = new();
            var apiurl = $"{api.BaseUrl}datapiutangs?bulan={model.BulanSelect}&tahun={model.TahunSelect}";

            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("username", api.Username);
            client.DefaultRequestHeaders.Add("password", api.Password);

            try
            {
                var apires = await client.GetAsync(apiurl);
                if (apires.IsSuccessStatusCode)
                {
                    var resdata = await apires.Content.ReadAsStringAsync();
                    var bills = JsonSerializer.Deserialize<List<BillColumn>>(resdata);

                    if (bills != null && bills.Count > 0)
                    {
                        List<Bill> validBills = new();
                        bool hasMissingCustomers = false;

                        foreach (var item in bills)
                        {
                            if (_context.Bills.Any(x => x.IdPelanggan == item.IdPelanggan))
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

                            validBills.Add(new Bill
                            {
                                IdPelanggan = item.IdPelanggan,
                                IdPelangganNavigation = pelanggan,
                                Awal = item.Awal,
                                Akhir = item.Akhir,
                                Total = item.Total,
                                Pakai = item.Pakai,
                                Bulan = item.Bulan,
                                Tahun = item.Tahun,
                                Tagihan = item.Tagihan,
                            });
                        }

                        if (validBills.Count > 0)
                        {
                            Bills = validBills;
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
