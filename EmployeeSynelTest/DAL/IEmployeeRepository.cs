using EmployeeSynelTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeSynelTest.DAL
{
    public interface IEmployeeRepository
    {
        // Retrieve a list of all employees
        IList<Employee> GetAll();

        // Filter employees based on surname and/or forenames
        // Returns a list of matching employees
        IList<Employee> Filter(string surname, string forenames);

        // Insert a new employee record
        void Insert(Employee emp);

        // Retrieve an employee by their unique ID
        Employee GetById(int id);

        // Update an existing employee record with new data
        void Update(Employee emp);

        // Delete an employee record by providing their unique ID
        void Delete(int id);

    }
}
