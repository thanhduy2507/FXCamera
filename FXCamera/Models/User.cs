namespace FXCamera.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Web;

    public partial class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string UserName { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(60)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(60)]
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string Phone { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public bool? Gender { get; set; }
        [NotMapped]
        public HttpPostedFileBase Files { get; set; }

        public string Avatar { get; set; }

        public DateTime Created_At { get; set; }

        public int Created_By { get; set; }

        public DateTime? Updated_At { get; set; }

        public int? Updated_By { get; set; }

        public bool Status { get; set; }

        public int? RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
