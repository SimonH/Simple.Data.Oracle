using System.Collections.Specialized;
using Simple.Data.Oracle.Configuration;

namespace Simple.Data.Oracle.Tests.Configuration
{
    internal class TestConfigurationManager : IConfigurationManager
    {
        private readonly object _section;
        public TestConfigurationManager(NameValueCollection appSettings = null, object section = null)
        {
            AppSettings = appSettings ?? new NameValueCollection();
            _section = section;

        }

        public NameValueCollection AppSettings { get; private set; }
        public object GetSection(string sectionName)
        {
            SectionName = sectionName;
            return _section;
        }

        public string SectionName
        {
            get;
            private set;
        }
    }
}