using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EmployeeSynelTest.Models
{
    public class Employee
    {
        public int? ID { get; set; }

        [DisplayName("Payroll Number")]
        public string Payroll_Number { get; set; }
        public string Forenames { get; set; }
        public string Surname { get; set; }

        [DisplayName("Date of Birth")]
        public DateTime Date_of_Birth { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [DisplayName("Email Home")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string EMail_Home { get; set; }

        [Required(ErrorMessage = "Mobile number is required.")]
        [RegularExpression(@"^[0-9\-()+]*$", ErrorMessage = "Mobile number should not contain letters.")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Telephone number is required.")]
        [RegularExpression(@"^[0-9\-()+]*$", ErrorMessage = "Telephone number should not contain letters.")]
        public string Telephone { get; set; }

        public string Address { get; set; }

        [DisplayName("Address 2")]
        public string Address_2 { get; set; }
        public string Postcode { get; set; }

        [DisplayName("Start Date")]
        public DateTime Start_Date { get; set; }


        public string FormattedDateOfBirth
        {
            get { return Date_of_Birth.ToString("yyyy/MM/dd"); }
        }

        // Read-only property for formatted Start_Date
        public string FormattedStartDate
        {
            get { return Start_Date.ToString("yyyy/MM/dd"); }
        }
    }
}