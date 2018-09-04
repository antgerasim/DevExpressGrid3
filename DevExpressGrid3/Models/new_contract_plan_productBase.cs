namespace DevExpressGrid3.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class new_contract_plan_productBase
    {
        [Key]
        public Guid new_contract_plan_productId { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public Guid? ModifiedBy { get; set; }

        public Guid? CreatedOnBehalfBy { get; set; }

        public Guid? ModifiedOnBehalfBy { get; set; }

        public Guid OwnerId { get; set; }

        public int OwnerIdType { get; set; }

        public Guid? OwningBusinessUnit { get; set; }

        public int statecode { get; set; }

        public int? statuscode { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] VersionNumber { get; set; }

        public int? ImportSequenceNumber { get; set; }

        public DateTime? OverriddenCreatedOn { get; set; }

        public int? TimeZoneRuleVersionNumber { get; set; }

        public int? UTCConversionTimeZoneCode { get; set; }

        [StringLength(100)]
        public string new_name { get; set; }

        public Guid? new_link_product_id { get; set; }

        public Guid? new_link_contract_plan_year_id { get; set; }

        public decimal? new_consulting { get; set; }

        public decimal? new_service_1_quarter { get; set; }

        public decimal? new_service_2_quarter { get; set; }

        public decimal? new_service_3_quarter { get; set; }

        public decimal? new_service_4_quarter { get; set; }

        public decimal? new_service_year { get; set; }

        public decimal? new_consulting_1_quarter { get; set; }

        public decimal? new_consulting_2_quarter { get; set; }

        public decimal? new_consulting_3_quarter { get; set; }

        public decimal? new_consulting_4_quarter { get; set; }

        public decimal? new_consulting_year { get; set; }

        public Guid? new_link_product_group_id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? new_year_sum { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? new_product_sum_service { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public decimal? new_product_sum_consulting { get; set; }

        public virtual new_d_product_catalogBase new_d_product_catalogBase { get; set; }

        public virtual new_d_product_groupsBase new_d_product_groupsBase { get; set; }
    }
}
