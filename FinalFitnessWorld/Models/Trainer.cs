namespace FinalFitnessWorld.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Trainer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Trainer()
        {
            Reservations = new HashSet<Reservation>();
        }

        [Required]
        public string Id { get; set; }

        //[Required]
        [Display(Name = "Trainer Name")]
        public string Name { get; set; }

        //[Required]
        [StringLength(256)]
        public string Email { get; set; }

        public int? PhoneNo { get; set; }

        public int Branch { get; set; }

        public virtual Branch Branch1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Reservation> Reservations { get; set; }
    }
}
