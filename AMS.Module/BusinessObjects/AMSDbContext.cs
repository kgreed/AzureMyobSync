using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.Diagnostics;
using System.Linq;
using DevExpress.ExpressApp.EF.Updating;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EF.PermissionPolicy;
using SBD.AMS.Module.MYOB;

namespace AMS.Module.BusinessObjects
{
    public class AMSDbContext : DbContext
    {
        public AMSDbContext(string connectionString)
            : base(connectionString)
        {
        }

        public AMSDbContext(DbConnection connection)
            : base(connection, false)
        {
        }

        public AMSDbContext()
            : base("name=ConnectionString")
        {
        }

        public DbSet<ModuleInfo> ModulesInfo { get; set; }
        public DbSet<PermissionPolicyRole> Roles { get; set; }
        public DbSet<PermissionPolicyTypePermissionObject> TypePermissionObjects { get; set; }
        public DbSet<PermissionPolicyUser> Users { get; set; }
        public DbSet<ModelDifference> ModelDifferences { get; set; }
        public DbSet<ModelDifferenceAspect> ModelDifferenceAspects { get; set; }
        public DbSet<MYOBItem> MYOBItems { get; set; }
        public DbSet<MYOBCustomer> MYOBCustomers { get; set; }
        public DbSet<MYOBSupplier> MYOBSuppliers { get; set; }
        public DbSet<MYOBAccount> MYOBAccounts { get; set; }
        public DbSet<MYOBTaxCode> MYOBTaxCodes { get; set; }


        public int RunMigrations()
        {
            var configuration = new DbMigrationsConfiguration();
            var migrator = new DbMigrator(configuration);
            var pendings = migrator.GetPendingMigrations().ToArray();
            Debug.Print($"There are {pendings.Count()} migrations");
            foreach (var pending in pendings)
            {
                Debug.Print(pending);
                try
                {
                    migrator.Update(pending);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }

            return pendings.Length;
        }
    }

    internal static class TypeConfigurationExtensions
    {
        public static PrimitivePropertyConfiguration HasUniqueIndexAnnotation(
            this PrimitivePropertyConfiguration property,
            string indexName,
            int columnOrder)
        {
            var indexAttribute = new IndexAttribute(indexName, columnOrder) {IsUnique = true};
            var indexAnnotation = new IndexAnnotation(indexAttribute);

            return property.HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation);
        }
    }
}