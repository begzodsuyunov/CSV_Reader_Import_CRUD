using CsvHelper;
using CsvHelper.Configuration;
using EmployeeSynelTest.DAL;
using EmployeeSynelTest.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeSynelTest.Controllers
{
    public class EmployeeController : Controller
    {
        // GET: Employee
        public ActionResult Index(string surname, string forenames)
        {

            var rep = new EmployeeRepository();
            var employees = rep.Filter(surname, forenames);

            return View(employees);
        }

        // GET: Employee/Details/5
        public ActionResult Details(int id)
        {

            var rep = new EmployeeRepository();
            var emp = rep.GetById(id);
            return View(emp);
        }

        // GET: Employee/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            var rep = new EmployeeRepository();
            try
            {
                // TODO: Add insert logic here
                rep.Insert(emp);

                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: Employee/Edit/5
        public ActionResult Edit(int id)
        {
            var rep = new EmployeeRepository();
            var emp = rep.GetById(id);
            return View(emp);
        }

        // POST: Employee/Edit/5
        [HttpPost]
        public ActionResult Edit(Employee emp)
        {
            var rep = new EmployeeRepository();

            try
            {
                // TODO: Add update logic here
                rep.Update(emp);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        // GET: Employee/Delete/5
        public ActionResult Delete(int id)
        {
            var rep = new EmployeeRepository();

            var emp = rep.GetById(id);
            return View(emp);
        }

        // POST: Employee/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var rep = new EmployeeRepository();

            try
            {
                // TODO: Add delete logic here
                rep.Delete(id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        public ActionResult ImportCsv()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ImportCsv(HttpPostedFileBase csvFile)
        {
            int rowsAdded = 0; // Initialize the rowsAdded count

            var empList = new List<Employee>();
            if(csvFile?.ContentLength > 0)
            {
                using(var stream = new MemoryStream())
                {
                    csvFile.InputStream.CopyTo(stream);
                    byte[] dataBytes = stream.ToArray();

                    using (var byteStream = new MemoryStream(dataBytes))
                    using (var reader = new StreamReader(byteStream))
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        csv.Context.RegisterClassMap<EmployeeMap>();

                        var records = csv.GetRecords<Employee>();

                        if (records != null)
                        {
                            var repo = new EmployeeRepository();
                            foreach (var emp in records)
                            {
                                try
                                {
                                    repo.Insert(emp);
                                    empList.Add(emp);
                                    rowsAdded++; // Increment rowsAdded for each successful insert

                                }
                                catch (Exception ex)
                                {
                                    ModelState.AddModelError("", ex.Message + " in Import Employees");
                                }
                            }
                        }
                    }
                
                }
                TempData["RowsAdded"] = rowsAdded; // Store rowsAdded in TempData
                return RedirectToAction("Index");
            } 
            else
            {
                ModelState.AddModelError("", "Empty File");
            }
            return View(empList);
        }

    }
}
