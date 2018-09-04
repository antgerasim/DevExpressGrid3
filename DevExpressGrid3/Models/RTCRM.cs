namespace DevExpressGrid3.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class RTCRM : DbContext
    {
        public RTCRM()
            : base("name=RTCRM")
        {
        }

        public virtual DbSet<new_contract_plan_productBase> new_contract_plan_productBase { get; set; }
        public virtual DbSet<new_d_product_catalogBase> new_d_product_catalogBase { get; set; }
        public virtual DbSet<new_d_product_groupsBase> new_d_product_groupsBase { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.VersionNumber)
                .IsFixedLength();

            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.new_consulting)
                .HasPrecision(23, 10);

            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.new_service_1_quarter)
                .HasPrecision(23, 10);

            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.new_service_2_quarter)
                .HasPrecision(23, 10);

            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.new_service_3_quarter)
                .HasPrecision(23, 10);

            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.new_service_4_quarter)
                .HasPrecision(23, 10);

            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.new_service_year)
                .HasPrecision(23, 10);

            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.new_consulting_1_quarter)
                .HasPrecision(23, 10);

            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.new_consulting_2_quarter)
                .HasPrecision(23, 10);

            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.new_consulting_3_quarter)
                .HasPrecision(23, 10);

            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.new_consulting_4_quarter)
                .HasPrecision(23, 10);

            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.new_consulting_year)
                .HasPrecision(23, 10);

            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.new_year_sum)
                .HasPrecision(23, 10);

            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.new_product_sum_service)
                .HasPrecision(23, 10);

            modelBuilder.Entity<new_contract_plan_productBase>()
                .Property(e => e.new_product_sum_consulting)
                .HasPrecision(23, 10);

            modelBuilder.Entity<new_d_product_catalogBase>()
                .Property(e => e.VersionNumber)
                .IsFixedLength();

            modelBuilder.Entity<new_d_product_catalogBase>()
                .HasMany(e => e.new_contract_plan_productBase)
                .WithOptional(e => e.new_d_product_catalogBase)
                .HasForeignKey(e => e.new_link_product_id);

            modelBuilder.Entity<new_d_product_groupsBase>()
                .Property(e => e.VersionNumber)
                .IsFixedLength();

            modelBuilder.Entity<new_d_product_groupsBase>()
                .HasMany(e => e.new_contract_plan_productBase)
                .WithOptional(e => e.new_d_product_groupsBase)
                .HasForeignKey(e => e.new_link_product_group_id);

            modelBuilder.Entity<new_d_product_groupsBase>()
                .HasMany(e => e.new_d_product_catalogBase)
                .WithOptional(e => e.new_d_product_groupsBase)
                .HasForeignKey(e => e.new_link_product_group_id);
        }
    }
}
