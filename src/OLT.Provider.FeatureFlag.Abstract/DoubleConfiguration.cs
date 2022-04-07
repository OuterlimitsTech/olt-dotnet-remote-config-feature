using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;

namespace OLT.Provider.FeatureFlag.Abstract
{
    /// <summary>
    /// https://stackoverflow.com/questions/64911784/ifeaturedefinitionprovider-for-selecting-features-from-sql-server
    /// </summary>
    internal class DoubleConfiguration : IConfiguration
    {
        private readonly double _percentage;

        public DoubleConfiguration(double percentage)
        {
            _percentage = percentage;
        }

        public IEnumerable<IConfigurationSection> GetChildren()
        {
            // NOTE: no children
            return Enumerable.Empty<IConfigurationSection>();
        }

        public IChangeToken GetReloadToken()
        {
            // NOTE: this is not supported and not consumed either
            throw new NotSupportedException();
        }

        public IConfigurationSection GetSection(string key)
        {
            // NOTE: this is not supported and not consumed either
            throw new NotSupportedException();
        }

        public string this[string key]
        {
            get => _percentage.ToString("F"); // this produces the requested value

            // NOTE: this is not supported and not consumed either
            set => throw new NotSupportedException();
        }
    }
}
