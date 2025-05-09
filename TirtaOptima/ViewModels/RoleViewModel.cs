using TirtaOptima.Models;

namespace TirtaOptima.ViewModels
{
    public class RoleViewModel
    {
        public List<Role> Roles { get; set; } = new();
        public long Id { get; set; }

        public string Name { get; set; } = null!;

    }
}
