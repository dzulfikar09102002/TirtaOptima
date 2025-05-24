using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class DebtManagementsRequest(DebtManagementsViewModel input, DebtManagementsService service)
    {
        public DebtManagementsViewModel UserInput { get; set; } = input;
        public DebtManagementsService Service { get; set; } = service;
        public string? ErrorMessage { get; set; }
        public bool Validate()
        {
            if (UserInput.BulanSelect <= 0 && UserInput.TahunSelect <= 0)
            {
                ErrorMessage = "Pilih Bulan dan Tahun Terlebih Dahulu";
                return false;
            }
            return true;
        }
        public bool ValidateInput()
        {
            if (UserInput.IdPelanggan <= 0 || UserInput.IdPiutang <= 0)
            {
                ErrorMessage = "Terjadi Kesalahan";
                return false;
            }
            if (UserInput.Nominal <= 0 || UserInput.Nominal == null)
            {
                ErrorMessage = "Nominal harus diisi";
                return false;
            }
            if (UserInput.Pencatatan == null)
            {
                ErrorMessage = "Tanggal pencatatan harus diisi";
                return false;
            }
            if (UserInput.Status == null)
            {
                ErrorMessage = "Status harus dipilih";
                return false;
            }
            if (UserInput.Status == "Debit" && UserInput.Pembayaran == null)
            {
                ErrorMessage = "Status debit harap isi tanggal pembayaran";
                return false;
            }
            return true;
        }
    }
}

