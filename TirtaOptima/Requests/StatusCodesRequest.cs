using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class StatusCodesRequest(StatusCodesViewModel input, StatusCodesService service)
    {
        public StatusCodesViewModel UserInput { get; set; } = input;
    public StatusCodesService Service { get; set; } = service;
    public string? ErrorMessage { get; set; }
    public bool Validate()
    {
        if (string.IsNullOrEmpty(UserInput.Name))
        {
            ErrorMessage = "Harap masukkan nama status";
            return false;
        }
        string inputNormalized = UserInput.Name.ToLower().Replace(" ", "");
        var existing = Service.GetStatuses();
        if (UserInput?.Id > 0 || UserInput?.Id == null)
        {
            existing = existing.Where(x => x.Id != UserInput?.Id).ToList();
        }
        bool nameExists = existing.Any(x =>
            (x.Name ?? "").ToLower().Replace(" ", "") == inputNormalized
        );

        if (nameExists)
        {
            ErrorMessage = "Status sudah ada";
            return false;
        }

        return true;
    }

}
}
