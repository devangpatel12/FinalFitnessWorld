namespace FinalFitnessWorld.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ReservationModels : DbContext
    {
        public ReservationModels()
            : base("name=ReservationModels")
        {
        }

        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Trainer> Trainers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Branch>()
                .Property(e => e.latitude)
                .HasPrecision(10, 4);

            modelBuilder.Entity<Branch>()
                .Property(e => e.longitude)
                .HasPrecision(10, 4);

            modelBuilder.Entity<Branch>()
                .HasMany(e => e.Reservations)
                .WithRequired(e => e.Branch1)
                .HasForeignKey(e => e.Branch)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Branch>()
                .HasMany(e => e.Trainers)
                .WithRequired(e => e.Branch1)
                .HasForeignKey(e => e.Branch)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.latitude)
                .HasPrecision(10, 4);

            modelBuilder.Entity<Customer>()
                .Property(e => e.longitude)
                .HasPrecision(10, 4);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Reservations)
                .WithRequired(e => e.Customer1)
                .HasForeignKey(e => e.Customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Trainer>()
                .HasMany(e => e.Reservations)
                .WithRequired(e => e.Trainer1)
                .HasForeignKey(e => e.Trainer)
                .WillCascadeOnDelete(false);
        }
    }
}
