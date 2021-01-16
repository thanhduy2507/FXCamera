namespace FXCamera.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Menu
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Link { get; set; }

        [StringLength(255)]
        public string Type { get; set; }

        public int? ParentId { get; set; }

        public int? Orders { get; set; }

        [StringLength(255)]
        public string Position { get; set; }

        public DateTime Created_At { get; set; }

        public int Created_By { get; set; }

        public DateTime? Updated_At { get; set; }

        public int? Updated_By { get; set; }

        public bool? Status { get; set; }
    }
}
