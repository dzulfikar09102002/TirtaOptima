using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class LetterDeliveriesRequest(LetterDeliveriesViewModel input, LetterDeliveriesService service)
    {
        public LetterDeliveriesViewModel UserInput { get; set; } = input;
        public LetterDeliveriesService Service { get; set; } = service;
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

            if (string.IsNullOrEmpty(UserInput.Letter?.NamaPenerima))
            {
                ErrorMessage = "Nama penerima harus diisi";
                return false ;
            }
            if (string.IsNullOrEmpty(UserInput.Letter.Status))
            {
                ErrorMessage = "Status harus dipilih";
                return false;
            }
            if(UserInput.Img == null)
            {
                ErrorMessage = "Bukti foto harus diisi";
                return false;
            }
            return true;
        }
    }
}
