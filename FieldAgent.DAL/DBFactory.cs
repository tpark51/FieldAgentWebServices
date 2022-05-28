using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FieldAgent.DAL
{
    public enum FactoryMode
    {
        TEST,
        PROD
    }

    public class DBFactory
    {
        private readonly IConfigurationRoot Config;
        private readonly FactoryMode Mode;

        public DBFactory(IConfigurationRoot config, FactoryMode mode = FactoryMode.PROD)
        {
            Config = config;
            Mode = mode;
        }

        public AppDBContext GetDbContext()
        {
            string environment = Mode == FactoryMode.TEST ? "Test" : "Prod";

            var options = new DbContextOptionsBuilder<AppDBContext>()
                .UseSqlServer(Config[$"ConnectionStrings:{environment}"])
                .Options;
            return new AppDBContext(options);
        }

        public string GetConnection()
        {
            string environment = Mode == FactoryMode.TEST ? "Test" : "Prod";
            return Config[$"ConnectionStrings:{environment}"];
        }

    }
}
