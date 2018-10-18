namespace FinalFitnessWorld.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Reservation
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string Customer { get; set; }

        public int Branch { get; set; }

        [Required]
        [StringLength(128)]
        public string Trainer { get; set; }

        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        [Required]
        [StringLength(50)]
        public string ReservationStatus { get; set; }

        public virtual Branch Branch1 { get; set; }

        public virtual Customer Customer1 { get; set; }

        public virtual Trainer Trainer1 { get; set; }
    }
}
