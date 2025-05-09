using TirtaOptima.Services;
using TirtaOptima.ViewModels;

namespace TirtaOptima.Requests
{
    public class LeaderRequest(LeaderViewModel input, LeaderService service)
    {
        public LeaderViewModel LeaderInput { get; set; } = input;
        public LeaderService Service { get; set; } = service;
        public string? ErrorMessage { get; set; }
        public bool Validate()
        {
            return true;
        }
    }
}
