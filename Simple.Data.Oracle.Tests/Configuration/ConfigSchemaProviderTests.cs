using System;
using System.Configuration;
using System.Data;
using System.Linq;
using NUnit.Framework;
using Simple.Data.Ado.Schema;
using Simple.Data.Oracle.Configuration;

namespace Simple.Data.Oracle.Tests.Configuration
{
    [TestFixture]
    public class ConfigSchemaProviderTests
    {
        [Test]
        public void Construction()
        {
            Assert.That(new ConfigSchemaProvider(null), Is.Not.Null);
            Assert.That(new ConfigSchemaProvider(new SdoConfigSection()), Is.Not.Null);
            Assert.That(new ConfigSchemaProvider(ConfigurationManager.GetSection("singlenodefaultschema") as SdoConfigSection), Is.Not.Null);
        }

        [Test]
        [TestCase("singlenodefaultschema", "schemaName")]
        [TestCase("multinodefaultschema", "schemaName")]
        [TestCase("multiwithdefaultschema", "schemaName2")]
        public void DefaultSchemaName(string sectionName, string expectedSchemaName)
        {
            var section = ConfigurationManager.GetSection(sectionName) as SdoConfigSection;
            Assert.That(new ConfigSchemaProvider(section).GetDefaultSchema(), Is.EqualTo(expectedSchemaName));
        }

        public ISchemaProvider GetProvider()
        {
            return new ConfigSchemaProvider(ConfigurationManager.GetSection("testschema") as SdoConfigSection);
        }

        [Test]
        public void CorrectColumnsAreReturnedForTest1()
        {
            var cols = GetProvider().GetColumns(new Table("people", "test1", TableType.Table)).ToList();
            Assert.That(cols.Count, Is.EqualTo(4));
            Assert.That(cols.FirstOrDefault(x => x.ActualName == "id"), Is.Not.Null);
            Assert.That(cols.FirstOrDefault(x => x.ActualName == "id").DbType, Is.EqualTo(DbType.Decimal));
            Assert.That(cols.FirstOrDefault(x => x.ActualName == "firstname"), Is.Not.Null);
            Assert.That(cols.FirstOrDefault(x => x.ActualName == "firstname").DbType, Is.EqualTo(DbType.String));
            Assert.That(cols.FirstOrDefault(x => x.ActualName == "surname"), Is.Not.Null);
            Assert.That(cols.FirstOrDefault(x => x.ActualName == "surname").DbType, Is.EqualTo(DbType.String));
            Assert.That(cols.FirstOrDefault(x => x.ActualName == "telephone"), Is.Not.Null);
            Assert.That(cols.FirstOrDefault(x => x.ActualName == "telephone").DbType, Is.EqualTo(DbType.Decimal));
        }

        [Test]
        public void CorrectColumnsAreReturnedForTest2()
        {
            var cols = GetProvider().GetColumns(new Table("people", "test2", TableType.Table)).ToList();
            Assert.That(cols.Count, Is.EqualTo(3));
            Assert.That(cols.FirstOrDefault(x => x.ActualName == "id"), Is.Not.Null);
            Assert.That(cols.FirstOrDefault(x => x.ActualName == "id").DbType, Is.EqualTo(DbType.Decimal));
            Assert.That(cols.FirstOrDefault(x => x.ActualName == "name"), Is.Not.Null);
            Assert.That(cols.FirstOrDefault(x => x.ActualName == "name").DbType, Is.EqualTo(DbType.String));
            Assert.That(cols.FirstOrDefault(x => x.ActualName == "EmailId"), Is.Not.Null);
            Assert.That(cols.FirstOrDefault(x => x.ActualName == "EmailId").DbType, Is.EqualTo(DbType.Decimal));
        }

        [Test]
        public void GetPrimaryKeys()
        {
            var key = GetProvider().GetPrimaryKey(new Table("people", "test1", TableType.Table));
            Assert.That(key[0], Is.EqualTo("id"));
            Assert.That(key[1], Is.EqualTo("firstname"));
        }

        [Test]
        public void GetForeignKeys()
        {
            var table = new Table("emailAddresses", "test2", TableType.Table);
            var foreignKeys = GetProvider().GetForeignKeys(table).ToList();
            Assert.That(foreignKeys.Count, Is.EqualTo(1));
            Assert.That(foreignKeys[0].DetailTable.ToString(), Is.EqualTo(table.GetNameWithSchema()));
            Assert.That(foreignKeys[0].MasterTable.ToString(), Is.EqualTo("test2.people"));
            Assert.That(foreignKeys[0].Columns[0], Is.EqualTo("id"));
            Assert.That(foreignKeys[0].UniqueColumns[0], Is.EqualTo("EmailId"));
        }

        [Test]
        public void GetProcedures()
        {
            var procs = GetProvider().GetStoredProcedures().ToList();
            Assert.That(procs.Count, Is.EqualTo(2));
            Assert.That(procs.FirstOrDefault(x => x.Schema == "test2" && x.Name == "package__testproc"), Is.Not.Null);
            Assert.That(procs.FirstOrDefault(x => x.Schema == "test3" && x.Name == "someproc"), Is.Not.Null);
        }

        [Test]
        public void GetParameters()
        {
            var args = GetProvider().GetParameters(new Procedure("package__testproc", "package__testproc", "test2")).ToList();
            Assert.That(args.Count, Is.EqualTo(1));
            Assert.That(args[0].Name, Is.EqualTo("__ReturnValue"));
            Assert.That(args[0].Type, Is.EqualTo(typeof(Guid)));
            Assert.That(args[0].Direction, Is.EqualTo(ParameterDirection.ReturnValue));
            args = GetProvider().GetParameters(new Procedure("someproc", "someproc", "test3")).ToList();
            Assert.That(args.Count, Is.EqualTo(3));
            Assert.That(args[0].Name, Is.EqualTo("first"));
            Assert.That(args[0].Type, Is.EqualTo(typeof(decimal)));
            Assert.That(args[0].Direction, Is.EqualTo(ParameterDirection.Input));
            Assert.That(args[1].Name, Is.EqualTo("second"));
            Assert.That(args[1].Type, Is.EqualTo(typeof(string)));
            Assert.That(args[1].Direction, Is.EqualTo(ParameterDirection.Input));
            Assert.That(args[2].Name, Is.EqualTo("output"));
            Assert.That(args[2].Type, Is.EqualTo(typeof(string)));
            Assert.That(args[2].Direction, Is.EqualTo(ParameterDirection.Output));
        }
    }
}