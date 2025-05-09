using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
	public class DistrictsRequest(DistrictsViewModel input, DistrictsService service)
	{
		public DistrictsViewModel UserInput { get; set; } = input;
		public DistrictsService Service { get; set; } = service;
		public string? ErrorMessage { get; set; }
		public bool Validate()
		{
			if (string.IsNullOrEmpty(UserInput.Nama))
			{
				ErrorMessage = "Harap masukkan nama kecamatan";
				return false;
			}
			string inputNormalized = UserInput.Nama.ToLower().Replace(" ", "");
			var existing = Service.GetDistricts();
			if (UserInput?.KodeKec > 0 || UserInput?.KodeKec == null)
			{
				existing = existing.Where(x => x.KodeKec != UserInput?.KodeKec).ToList();
			}
			bool nameExists = existing.Any(x =>
				(x.Nama ?? "").ToLower().Replace(" ", "") == inputNormalized
			);

			if (nameExists)
			{
				ErrorMessage = "Kecamatan sudah ada";
				return false;
			}

			return true;
		}

	}
}
