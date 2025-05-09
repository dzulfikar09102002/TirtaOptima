using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class LettersCategoryRequest(LettersCategoryViewModel input, LettersCategoryService service)
    {
		LettersCategoryViewModel UserInput { get; set; } = input;
		LettersCategoryService Service { get; set; } = service;
		public string? ErrorMessage { get; set; }
		public bool Validate()
		{
			if(UserInput.Code == null)
			{
				ErrorMessage = "Harap masukkan kode surat";
				return false;
			}
			if(UserInput.Name == null)
			{
				ErrorMessage = "Harap masukkan nama kategori surat";
				return false;
			}
			var existing = Service.GetLetterCategories();
			string codeNormalized = UserInput.Code.ToLower().Replace(" ", "");
			bool codeExists = existing.Any(x =>
				(x.Code ?? "").ToLower().Replace(" ", "") == codeNormalized
			);
			string nameNormalized = UserInput.Name.ToLower().Replace(" ", "");
            if (UserInput?.Id > 0 || UserInput?.Id == null)
            {
                existing = existing.Where(x => x.Id != UserInput?.Id).ToList();
            }
            bool nameExists = existing.Any(x =>
				(x.Name ?? "").ToLower().Replace(" ", "") == nameNormalized
			);

			if (nameExists || codeExists)
			{
				ErrorMessage = "Kategori surat sudah ada";
				return false;
			}
			return true;
		}
	}
}
