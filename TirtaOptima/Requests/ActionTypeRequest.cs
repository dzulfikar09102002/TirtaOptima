using Microsoft.AspNetCore.Mvc.ModelBinding;
using TirtaOptima.Models;
using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class ActionTypeRequest(ActionTypeViewModel input, ActionTypeService service)
    {
        public ActionTypeViewModel UserInput { get; set; } = input;
        public ActionTypeService Service { get; set; } = service;
        public string? ErrorMessage { get; set; }
        public bool Validate()
        {
            if (string.IsNullOrEmpty(UserInput.Name))
            {
                ErrorMessage = "Harap Masukkan Nama Tindakan";
                return false;
            }
            string inputNormalized = UserInput.Name.ToLower().Replace(" ", "");
            var existing = Service.GetActions();
            if(UserInput?.Id > 0 || UserInput?.Id == null)
            {
                existing = existing.Where(x => x.Id != UserInput?.Id).ToList();
            }
            bool nameExists = existing.Any(x =>
                (x.Name ?? "").ToLower().Replace(" ", "") == inputNormalized
            );

            if (nameExists)
            {
                ErrorMessage = "Jenis Tindakan Sudah Ada";
                return false;
            }

            return true;
        }

    }
}
