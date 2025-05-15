using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TirtaOptima.Helpers;
using TirtaOptima.Models;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Services
{
    public class CustomerService(DatabaseContext context)
    {
        private readonly DatabaseContext _context = context;
        public List<Customer> Customers { get; set; } = new();
        public string? Message { get; set; }
        public List<Customer> GetCustomers(CustomerViewModel model) => [.._context.Customers.Include(x => x.JenisNavigation)
            .Include(x => x.KelurahanNavigation).Include(x => x.KecamatanNavigation).Include(x => x.StatusNavigation).Include(x => x.JenisNavigation)
            .Where(x => x.Pasang.HasValue && x.Pasang.Value.Month == model.BulanSelect && x.Pasang.Value.Year == model.TahunSelect)
            ];
        public Customer? GetCustomer(long id) => _context.Customers.FirstOrDefault(x => x.Id == id && x.DeletedAt == null);
        public async Task<bool> GetCustomersApiAsync(CustomerViewModel model)
        {
            ApiHelper api = new();
            var apiurl = $"{api.BaseUrl}datapelanggan?bulan={model.BulanSelect}&tahun={model.TahunSelect}";

            using HttpClient client = new();
            client.DefaultRequestHeaders.Add("username", api.Username);
            client.DefaultRequestHeaders.Add("password", api.Password);

            try
            {
                var apires = await client.GetAsync(apiurl);
                if (apires.IsSuccessStatusCode)
                {
                    var resdata = await apires.Content.ReadAsStringAsync();
                    var customers = JsonSerializer.Deserialize<List<CustomerColumn>>(resdata);
                    if (customers?.Any() == true)
                    {
                        Customers = customers
                            .Where(item => !_context.Customers.Any(x => x.Id == item.Id))
                            .Select(item => new Customer
                            {
                                Id = item.Id,
                                Nomor = item.Nomor ?? "-",
                                Nama = item.Nama,
                                Jenis = item.Jenis,
                                Alamat = item.Alamat,
                                Pasang = item.Pasang,
                                Kecamatan = item.Kecamatan,
                                Kelurahan = item.Kelurahan,
                                Status = item.Status,
                                Cabang = item.Cabang,
                                Wilayah = item.Wilayah,
                                NoTelepon = item.NoTelepon,
                                Email = item.Email,
                                JenisNavigation = _context.CustomerTypes.FirstOrDefault(x => x.Id == item.Jenis),
                                StatusNavigation = _context.Statuses.FirstOrDefault(x => x.Id == item.Status),
                                KelurahanNavigation = _context.Villages.FirstOrDefault(x => x.Id == item.Kelurahan),
                                KecamatanNavigation = _context.Districts.FirstOrDefault(x => x.Id == item.Kecamatan)
                            })
                            .ToList();

                        if (Customers.Any())
                        {
                            var cs = Customers.ToList();
                            return true;
                        }

                        Message = "Semua data sudah ada di database";
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
