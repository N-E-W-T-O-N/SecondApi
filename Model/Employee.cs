using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace SecondApi.Model
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Employee_id { get; set; }
        public string? First_name { get; set; }
        public string? Last_name { get; set; }


        public string? Email { get; set; }
        public string? Phone_number { get; set; }

        public int? Job_id { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateOnly? Hire_date { get; set; }
        // 
        //[DataType(DataType.Currency)]
        [Column(TypeName = "DECIMAL(8, 2)")]
        public float? Salary { get; set; }
    }
    public class Employee_To_Be_Added
    {

        public string? First_name { get; set; }
        public string? Last_name { get; set; }


        public string? Email { get; set; }
        public string? Phone_number { get; set; }

        public int? Job_id { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        public DateOnly? Hire_date { get; set; }
        // 
        //[DataType(DataType.Currency)]
        [Column(TypeName = "DECIMAL(8, 2)")]
        public float? Salary { get; set; }
    }
}