using System.Collections.Specialized;
using NUnit.Framework;
using Simple.Data.Oracle.Configuration;

namespace Simple.Data.Oracle.Tests.Configuration
{
    [TestFixture]
    public class ConfigurationProviderTests
    {
        [Test]
        public void Construction()
        {
            Assert.That(new ConfigurationProvider(), Is.Not.Null);
            Assert.That(new ConfigurationProvider(new TestConfigurationManager()), Is.Not.Null);
        }

        [Test]
        public void ConnectionSchemaOverrideReturnsNullIfSettingNotPresent()
        {
            Assert.That(new ConfigurationProvider(new TestConfigurationManager()).ConnnectionSchemaOverride, Is.Null);
        }

        [Test]
        public void ConnectionSchemaOverrideIsReturnedCorrectly()
        {
            Assert.That(new ConfigurationProvider(new TestConfigurationManager(new NameValueCollection { { "Simple.Data.Oracle.Schema", "schema" } })).ConnnectionSchemaOverride, Is.EqualTo("schema"));
        }

        [Test]
        public void GetSectionCalledWithDefaultName()
        {
            var testManager = new TestConfigurationManager();
            var candidate = new ConfigurationProvider(testManager).SchemaProvider;
            Assert.That(testManager.SectionName, Is.EqualTo("SimpleDataOracleConfig"));
        }

        [Test]
        public void GetSectionCalledWithSuppliedName()
        {
            var testManager = new TestConfigurationManager(new NameValueCollection { { "Simple.Data.Oracle.ConfigSectionName", "config" } });
            var candidate = new ConfigurationProvider(testManager).SchemaProvider;
            Assert.That(testManager.SectionName, Is.EqualTo("config"));
        }

        [Test]
        public void SchemaProviderIsNullIfSectionIsWrongType()
        {
            var testManager = new TestConfigurationManager(new NameValueCollection(), "test invalid type");
            var candidate = new ConfigurationProvider(testManager).SchemaProvider;
            Assert.That(candidate, Is.Null);
        }

        [Test]
        public void SchemaProviderIsReturnedIfFound()
        {
            var sdo = new SdoConfigSection();
            var testManager = new TestConfigurationManager(new NameValueCollection(), sdo);
            var candidate = new ConfigurationProvider(testManager).SchemaProvider;
            Assert.That(candidate, Is.TypeOf<ConfigSchemaProvider>());
        }
    }
}