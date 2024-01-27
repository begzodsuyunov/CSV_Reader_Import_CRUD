using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EmployeeSynelTest.Models
{

    public class EmployeeMap : ClassMap<Employee>
    {
        //Faced some error while importing csv file. Thus, create seperate 
        //class to Map properly. In addition, used DateConverter to solve issue with conversion
        public EmployeeMap()
        {
            Map(m => m.Payroll_Number).Index(0);
            Map(m => m.Forenames).Index(1);
            Map(m => m.Surname).Index(2);

            // Use TypeConverter to handle date formats
            Map(m => m.Date_of_Birth).Index(3).TypeConverter<DateConverter>();
            Map(m => m.Telephone).Index(4);
            Map(m => m.Mobile).Index(5);
            Map(m => m.Address).Index(6);
            Map(m => m.Address_2).Index(7);
            Map(m => m.Postcode).Index(8);
            Map(m => m.EMail_Home).Index(9);

            // Use TypeConverter to handle date formats
            Map(m => m.Start_Date).Index(10).TypeConverter<DateConverter>();
        }
    }

    public class DateConverter : DefaultTypeConverter
    {
        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            // Specify the date formats to attempt
            string[] dateFormats = new string[] { "dd/MM/yyyy", "M/d/yyyy" };

            DateTime parsedDate;
            if (DateTime.TryParseExact(text, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate;
            }

            return base.ConvertFromString(text, row, memberMapData);
        }
    }
}