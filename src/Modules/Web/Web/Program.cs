using HostBuilder = Wyn.Host.Web.HostBuilder;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args) => new HostBuilder(args).Run();
    }
}
