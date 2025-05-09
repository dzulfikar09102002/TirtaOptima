using System.ComponentModel.DataAnnotations;
using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class ActionTypeViewModel
    {
        public List<ActionType> Actions { get; set; } = new();
        public long Id { get; set; }

        [Required(ErrorMessage = "Jenis Tindakan Harus Diisi")]
        public string Name { get; set; } = null!;
    }
}
