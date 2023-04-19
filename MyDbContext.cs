
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SecondApi.Model;

namespace SecondApi
{
    /*    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
        {
            public DateOnlyConverter() : base(
                v => v.ToDateTime(TimeZoneInfo.Local).ToLocalTime(),
                v => DateOnly.FromDateTime(v.ToUniversalTime()))
            { }
        }*/
    /*      public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
            {
                public DateOnlyConverter() : base(
                        d =>  d.ToDateTime(TimeOnly.MinValue),
                        d => DateOnly.FromDateTime(d))
                { }
            }*/
    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
   {    public DateOnlyConverter() : base(
                d => d.ToDateTime(default),
                d => DateOnly.FromDateTime(d))
        { }
    }
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) // able to send and retieve employee data
        { 
        }  // This will read our database
        public DbSet<Employee> Employees { get; set; } //use to retrieve tables 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasKey(e => e.Employee_id);

            modelBuilder.Entity<Employee>()
             .Property(e => e.Hire_date)
             .HasColumnName("hire_date")
             .HasConversion(new DateOnlyConverter());

            modelBuilder.Entity<Employee>()
                .Property(e => e.Job_id)
                .HasColumnName("job_id");


        }


    }
}