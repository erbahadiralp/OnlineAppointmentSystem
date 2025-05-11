using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineAppointmentSystem.Entity.Concrete;

namespace OnlineAppointmentSystem.DataAccess.Concrete.EntityFramework
{
    public class OnlineAppointmentSystemDbContext : IdentityDbContext<AppUser>
    {
        public OnlineAppointmentSystemDbContext(DbContextOptions<OnlineAppointmentSystemDbContext> options) : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<EmployeeService> EmployeeServices { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<WorkingHours> WorkingHours { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure composite key for EmployeeService
            modelBuilder.Entity<EmployeeService>()
                .HasKey(es => new { es.EmployeeId, es.ServiceId });

            // Configure relationships with Restrict on delete to avoid cascade delete conflicts
            modelBuilder.Entity<Employee>()
                .HasOne(e => e.User)
                .WithOne(u => u.Employee)
                .HasForeignKey<Employee>(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            modelBuilder.Entity<Customer>()
                .HasOne(c => c.User)
                .WithOne(u => u.Customer)
                .HasForeignKey<Customer>(c => c.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Customer)
                .WithMany(c => c.Appointments)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Employee)
                .WithMany(e => e.Appointments)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Service)
                .WithMany(s => s.Appointments)
                .HasForeignKey(a => a.ServiceId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            modelBuilder.Entity<EmployeeService>()
                .HasOne(es => es.Employee)
                .WithMany(e => e.EmployeeServices)
                .HasForeignKey(es => es.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            modelBuilder.Entity<EmployeeService>()
                .HasOne(es => es.Service)
                .WithMany(s => s.EmployeeServices)
                .HasForeignKey(es => es.ServiceId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            modelBuilder.Entity<WorkingHours>()
                .HasOne(wh => wh.Employee)
                .WithMany(e => e.WorkingHours)
                .HasForeignKey(wh => wh.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.Appointment)
                .WithMany()
                .HasForeignKey(n => n.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict); // Avoid cascade delete

            // Configure AppointmentStatus as enum
            modelBuilder.Entity<Appointment>()
                .Property(a => a.Status)
                .HasConversion<int>();
        }
    }
}
