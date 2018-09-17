using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VehicleManagementApp.Models.Models;

namespace VehicleManagementApp.Repository.DatabaseContext
{
    public class VehicleDatabaseContext : DbContext
    {
        public DbSet<Organaization> Organaizations { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<VehicleType> VehicleTypes { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Role> Roles { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<Requsition> Requsitions { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<RequsitionStatus> RequsitionStatuses { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Thana> Thanas { get; set; }
        public DbSet<Manager> Managers { get; set; }



        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //    public class Student
            //{
            //    public int StudentId { get; set; }
            //    public string StudentName { get; set; }

            //    public virtual StudentAddress Address { get; set; }
            //}

            //public class StudentAddress
            //{
            //    [ForeignKey("Student")]
            //    public int StudentAddressId { get; set; }

            //    public string Address1 { get; set; }
            //    public string Address2 { get; set; }
            //    public string City { get; set; }
            //    public int Zipcode { get; set; }
            //    public string State { get; set; }
            //    public string Country { get; set; }

            //    public virtual Student Student { get; set; }
            //}

            //modelBuilder.Entity<Manager>()
            //    .HasOptional(s => s.Employee)
            //    .WithMany()
            //    .HasForeignKey(t => t.EmployeeId);
            // Address property optional in Student entity
            //modelBuilder.Entity<Comment>()
            //   .HasOptional(s => s.Employee)
            //   .WithMany()
            //   .HasForeignKey(t => t.EmployeeId);

        }



}
  
}
