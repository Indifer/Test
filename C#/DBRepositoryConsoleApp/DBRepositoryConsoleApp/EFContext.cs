using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBRepositoryConsoleApp
{
    public class EFContext : DbContext
    {
        public EFContext()
            : base("name=MySQLContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            //modelBuilder.Conventions.Remove<IncludeMetadataConvention>();

            Configuration.AutoDetectChangesEnabled = false;
            Configuration.ValidateOnSaveEnabled = false;
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
            //商品规格
            modelBuilder.Configurations.Add(new ProductSpecConfig());


            //默认约定
            modelBuilder.Types().Configure(e =>
            {
                e.HasKey("ID");
                if (e.ClrType.GetInterfaces().Contains(typeof(global::Repository.IEntity.IAutoIncr)))
                {
                    e.Property("ID").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity);
                }
                else
                {
                    e.Property("ID").HasDatabaseGeneratedOption(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None);
                }
            });

            Database.SetInitializer<EFContext>(null);
        }
    }


    public class ProductSpecConfig : EntityTypeConfiguration<ProductSpec>
    {
        public ProductSpecConfig()
        {
            this.ToTable("ProductSpec");
            this.Property(r => r.Code).HasMaxLength(40);
            this.Property(r => r.Creator).HasMaxLength(50);
            this.Property(r => r.Updator).HasMaxLength(50);

        }
    }
}
