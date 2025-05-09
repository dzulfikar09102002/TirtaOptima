
namespace TirtaOptima.Helpers
{
    public class ApiHelper
    {
        public string? BaseUrl { get; }
        public string? Username { get; }
        public string? Password { get; }

        public ApiHelper()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var section = configuration.GetSection("ApiSettings");

            BaseUrl = section["BaseUrl"];
            Username = section["Username"];
            Password = section["Password"];
        }
    }
}
