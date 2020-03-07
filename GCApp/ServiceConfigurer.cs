using Gentings;
using Gentings.Data.SqlServer;

namespace GCApp
{
    public class ServiceConfigurer : IServiceConfigurer
    {
        public void ConfigureServices(IServiceBuilder builder)
        {
            builder.AddSqlServer();
        }
    }
}
