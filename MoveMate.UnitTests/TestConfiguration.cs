using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace MoveMate.UnitTests
{
    public class TestConfiguration : IConfiguration
    {
        Dictionary<string, string?> Configuration;

        public string? this[string key] { 
            get => Configuration[key];
            set {
                if(Configuration.ContainsKey(key))
                    Configuration[key] = value;
                else
                    Configuration.Add(key, value);
            }
        }

        public TestConfiguration()
        {
            Configuration = new Dictionary<string, string?>
            {
                { "Jwt:PrivateKey", "mysupersecretprivatekey" },
                { "Jwt:Issuer", "MoveMate" }
            };
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            return new List<IConfigurationSection>();
        }

        public IChangeToken GetReloadToken()
        {
            return default(IChangeToken)!;
        }

        public IConfigurationSection GetSection(string key)
        {
            return null!;
        }
    }
}
