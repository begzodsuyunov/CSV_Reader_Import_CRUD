using EmployeeSynelTest.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace EmployeeSynelTest.DAL
{
    public class EmployeeRepository : IEmployeeRepository
    {
        // SQL queries
        private const string SQL_SELECT = @"SELECT ID,
                                            Payroll_Number,
                                            Forenames,
                                            Surname,
                                            Date_of_Birth,
                                            Telephone,
                                            Mobile,
                                            Address,
                                            Address_2,
                                            Postcode,
                                            EMail_Home,
                                            Start_Date
                                        FROM Personnel_Records
                                        ";
        private const string SQL_INSERT =@"INSERT INTO [dbo].[Personnel_Records]
                                                   ([Payroll_Number]
                                                   ,[Forenames]
                                                   ,[Surname]
                                                   ,[Date_of_Birth]
                                                   ,[Telephone]
                                                   ,[Mobile]
                                                   ,[Address]
                                                   ,[Address_2]
                                                   ,[Postcode]
                                                   ,[EMail_Home]
                                                   ,[Start_Date])
                                             VALUES
                                                   (
		                                           @Payroll_Number,
		                                           @Forenames,
		                                           @Surname,
		                                           @Date_of_Birth,
		                                           @Telephone,
		                                           @Mobile,
		                                           @Address,
		                                           @Address_2,
		                                           @Postcode,
		                                           @EMail_Home,
		                                           @Start_Date)";
        private const string SQL_UPDATE = @"UPDATE [dbo].[Personnel_Records]
                                               SET [Payroll_Number]	   = @Payroll_Number	
                                                  ,[Forenames] 		   = @Forenames 		
                                                  ,[Surname] 		   = @Surname 		
                                                  ,[Date_of_Birth] 	   = @Date_of_Birth 	
                                                  ,[Telephone] 		   = @Telephone 		
                                                  ,[Mobile] 		   = @Mobile 		
                                                  ,[Address]  		   = @Address  		
                                                  ,[Address_2] 		   = @Address_2		
                                                  ,[Postcode] 		   = @Postcode 		
                                                  ,[EMail_Home]		   = @EMail_Home		
                                                  ,[Start_Date]		   = @Start_Date
                                             WHERE ID = @ID";
        private const string SQL_DELETE =@"DELETE FROM [dbo].[Personnel_Records]
                                                  WHERE ID = @ID";
        private const string SQL_GET_BY_ID = @"
                                        SELECT ID,
                                            Payroll_Number,
                                            Forenames,
                                            Surname,
                                            Date_of_Birth,
                                            Telephone,
                                            Mobile,
                                            Address,
                                            Address_2,
                                            Postcode,
                                            EMail_Home,
                                            Start_Date
                                        FROM Personnel_Records
                                        WHERE ID = @ID";
        private const string SQL_FILTER = @"SELECT ID,
                                            Payroll_Number,
                                            Forenames,
                                            Surname,
                                            Date_of_Birth,
                                            Telephone,
                                            Mobile,
                                            Address,
                                            Address_2,
                                            Postcode,
                                            EMail_Home,
                                            Start_Date
                                        FROM Personnel_Records";
        
        private string ConnStr
        {
            get
            {
                return WebConfigurationManager.ConnectionStrings["EmployeeConnStr"].ConnectionString;
            }
        }

        // Delete an employee record by ID
        public void Delete(int id)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                using (var command = conn.CreateCommand())
                {
                    command.CommandText = SQL_DELETE;

                    command.Parameters.AddWithValue("@ID", id);

                    conn.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

        // Filter employee records by surname and/or forenames
        public IList<Employee> Filter(string surname, string forenames)
        {
            IList<Employee> employees = new List<Employee>();
            using (var conn = new SqlConnection(ConnStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    string whereSql = "";
                    cmd.CommandText = SQL_FILTER;

                    //WHERE clause based on surname and forenames 
                    if (!string.IsNullOrWhiteSpace(forenames))
                    {
                        whereSql += (whereSql.Length == 0 ? "" : " AND ") + " Forenames like @Forenames + '%' ";
                        cmd.Parameters.AddWithValue("@Forenames", forenames);
                    }
                    if (!string.IsNullOrWhiteSpace(surname))
                    {
                        whereSql += (whereSql.Length == 0 ? "" : " AND ") + " Surname like @Surname + '%' ";
                        cmd.Parameters.AddWithValue("@Surname", surname);
                    }

                    if(!string.IsNullOrWhiteSpace(whereSql))
                    {
                        whereSql = " WHERE " + whereSql;
                    }
                    cmd.CommandText = SQL_FILTER + whereSql; 


                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var emp = new Employee();
                            // Map the database columns to the corresponding properties of the 'Employee' object
                            emp.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                            emp.Payroll_Number = reader.GetString(reader.GetOrdinal("Payroll_Number"));
                            emp.Forenames = reader.GetString(reader.GetOrdinal("Forenames"));
                            emp.Surname = reader.GetString(reader.GetOrdinal("Surname"));
                            emp.Date_of_Birth = reader.GetDateTime(reader.GetOrdinal("Date_of_Birth"));
                            emp.Telephone = reader.GetString(reader.GetOrdinal("Telephone"));
                            emp.Mobile = reader.GetString(reader.GetOrdinal("Mobile"));
                            emp.Address = reader.GetString(reader.GetOrdinal("Address"));
                            emp.Address_2 = reader.GetString(reader.GetOrdinal("Address_2"));
                            emp.Postcode = reader.GetString(reader.GetOrdinal("Postcode"));
                            emp.EMail_Home = reader.GetString(reader.GetOrdinal("EMail_Home"));
                            emp.Start_Date = reader.GetDateTime(reader.GetOrdinal("Start_Date"));

                            employees.Add(emp);
                        }
                    }
                }
            }

            return employees;
        }

        // Retrieve all employee records
        public IList<Employee> GetAll()
        {
            IList<Employee> employees = new List<Employee>();
            using (var conn = new SqlConnection(ConnStr))
            {
                using(var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = SQL_SELECT;

                    conn.Open();

                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var emp = new Employee();
                            // Map the database columns to the corresponding properties of the 'Employee' object
                            emp.ID = reader.GetInt32(reader.GetOrdinal("ID"));
                            emp.Payroll_Number = reader.GetString(reader.GetOrdinal("Payroll_Number"));
                            emp.Forenames = reader.GetString(reader.GetOrdinal("Forenames"));
                            emp.Surname = reader.GetString(reader.GetOrdinal("Surname"));
                            emp.Date_of_Birth = reader.GetDateTime(reader.GetOrdinal("Date_of_Birth"));
                            emp.Telephone = reader.GetString(reader.GetOrdinal("Telephone"));
                            emp.Mobile = reader.GetString(reader.GetOrdinal("Mobile"));
                            emp.Address = reader.GetString(reader.GetOrdinal("Address"));
                            emp.Address_2 = reader.GetString(reader.GetOrdinal("Address_2"));
                            emp.Postcode = reader.GetString(reader.GetOrdinal("Postcode"));
                            emp.EMail_Home = reader.GetString(reader.GetOrdinal("EMail_Home"));
                            emp.Start_Date = reader.GetDateTime(reader.GetOrdinal("Start_Date"));

                            employees.Add(emp);
                        }
                    }
                }
            }

            return employees;
        }

        // Retrieve an employee record by ID
        public Employee GetById(int id)
        {
            Employee emp = null;

            using (var conn = new SqlConnection(ConnStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = SQL_GET_BY_ID;

                    cmd.Parameters.AddWithValue("@ID", id);

                    conn.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            emp = new Employee()
                            {
                                // Map the database columns to the corresponding properties of the 'Employee' object
                                ID = id,
                                Payroll_Number = reader.GetString(reader.GetOrdinal("Payroll_Number")),
                                Forenames = reader.GetString(reader.GetOrdinal("Forenames")),
                                Surname = reader.GetString(reader.GetOrdinal("Surname")),
                                Date_of_Birth = reader.GetDateTime(reader.GetOrdinal("Date_of_Birth")),
                                Telephone = reader.GetString(reader.GetOrdinal("Telephone")),
                                Mobile = reader.GetString(reader.GetOrdinal("Mobile")),
                                Address = reader.GetString(reader.GetOrdinal("Address")),
                                Address_2 = reader.GetString(reader.GetOrdinal("Address_2")),
                                Postcode = reader.GetString(reader.GetOrdinal("Postcode")),
                                EMail_Home = reader.GetString(reader.GetOrdinal("EMail_Home")),
                                Start_Date = reader.GetDateTime(reader.GetOrdinal("Start_Date"))
                            };
                        }
                    }
                }
            }
            return emp;
        }

        // Insert a new employee record
        public void Insert(Employee emp)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = SQL_INSERT;

                    // Set parameters for the INSERT operation
                    cmd.Parameters.AddWithValue("Payroll_Number", emp.Payroll_Number);
                    cmd.Parameters.AddWithValue("Forenames", emp.Forenames);
                    cmd.Parameters.AddWithValue("Surname", emp.Surname);
                    cmd.Parameters.AddWithValue("Date_of_Birth", emp.Date_of_Birth);
                    cmd.Parameters.AddWithValue("Telephone", emp.Telephone);
                    cmd.Parameters.AddWithValue("Mobile", emp.Mobile);
                    cmd.Parameters.AddWithValue("Address", emp.Address);
                    cmd.Parameters.AddWithValue("Address_2", emp.Address_2);
                    cmd.Parameters.AddWithValue("Postcode", emp.Postcode);
                    cmd.Parameters.AddWithValue("EMail_Home", emp.EMail_Home);
                    cmd.Parameters.AddWithValue("Start_Date", emp.Start_Date);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Update an existing employee record
        public void Update(Employee emp)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                using ( var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = SQL_UPDATE;

                    // Set parameters for the UPDATE operation
                    cmd.Parameters.AddWithValue("Payroll_Number", emp.Payroll_Number);
                    cmd.Parameters.AddWithValue("Forenames", emp.Forenames);
                    cmd.Parameters.AddWithValue("Surname", emp.Surname);
                    cmd.Parameters.AddWithValue("Date_of_Birth", emp.Date_of_Birth.Date);
                    cmd.Parameters.AddWithValue("Telephone", emp.Telephone);
                    cmd.Parameters.AddWithValue("Mobile", emp.Mobile);
                    cmd.Parameters.AddWithValue("Address", emp.Address);
                    cmd.Parameters.AddWithValue("Address_2", emp.Address_2);
                    cmd.Parameters.AddWithValue("Postcode", emp.Postcode);
                    cmd.Parameters.AddWithValue("EMail_Home", emp.EMail_Home);
                    cmd.Parameters.AddWithValue("Start_Date", emp.Start_Date);

                    // Specify the employee ID for the WHERE clause
                    cmd.Parameters.AddWithValue("@ID", emp.ID);

                    conn.Open();

                    cmd.ExecuteNonQuery();


                }
            }

            
        }
    }
}