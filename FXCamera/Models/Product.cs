namespace FXCamera.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public int CategoryId { get; set; }

        public int ManufacturerId { get; set; }

        [StringLength(255)]
        public string Slug { get; set; }

        [StringLength(255)]
        public string Images { get; set; }
        [NotMapped]
        public HttpPostedFileBase Files { get; set; }

        public string Detail { get; set; }

        public int? Number { get; set; }

        public decimal? Price { get; set; }

        public decimal? PriceSale { get; set; }

        [Required]
        [StringLength(255)]
        public string MetaKey { get; set; }

        [Required]
        [StringLength(255)]
        public string MetaDescription { get; set; }

        public DateTime Created_At { get; set; }

        public int Created_By { get; set; }

        public DateTime? Updated_At { get; set; }

        public int? Updated_By { get; set; }

        public bool? Status { get; set; }

        public virtual Category Category { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
