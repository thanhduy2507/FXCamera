namespace FXCamera.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }

        public int Code { get; set; }

        public int UserID { get; set; }

        public DateTime CreatedDay { get; set; }

        public DateTime? ExportDay { get; set; }

        [Required]
        [StringLength(255)]
        public string DeliveryAddress { get; set; }

        [Required]
        [StringLength(255)]
        public string DeliveryName { get; set; }

        [StringLength(255)]
        public string DeliveryEmail { get; set; }

        public bool? Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual User Users { get; set; }
    }
}
