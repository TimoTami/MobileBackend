using MobileBackend.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MobileBackend.Controllers
{
    public class EmployeeController : ApiController
    {
        //public string[] GetAll()
        //{
        //    string[] employeeNames = null;
        //    //Employee juu;
        //    MobileDBEntities1 entities = new MobileDBEntities1();
        //    try
        //    {
        //        //juu = (from f in entities.Employees where (f.FirstName == "Hius") && (f.LastName == "Harja") select f ).FirstOrDefault();

        //        //juu.EmployeePicture = File.ReadAllBytes(@"C:\x\HiusHarja2.png");
        //        //entities.SaveChanges();

        //        //Employee newEmployee = new Employee()
        //        //{
        //        //    FirstName = "Hienolainen",
        //        //    LastName = "Harjalainen",
        //        //    Active = true,
        //        //    EmployeePicture = File.ReadAllBytes(@"C:\x\HiusHarja.png")
        //        //};
        //        //entities.Employees.Add(newEmployee);
        //        //entities.SaveChanges();

        //        employeeNames = (from e in entities.Employees
        //                                      where (e.Active == true)
        //                                      select e.FirstName + " " + e.LastName).ToArray();
        //    }
        //    finally
        //    {

        //        entities.Dispose();
        //    }
        //    return employeeNames;
        //}
        public string[] GetEmployee()
        {
            //string[] employeeName = null;
            string[] pekka = null;
            //string[] nn = null;
            
            MobileDBEntities1 entities = new MobileDBEntities1();
            try
            {
                pekka = (from e in entities.Employees
                             where (e.UserName == LoginController.nimi.UserName)
                             select e.FirstName + " " + e.LastName).ToArray();

                //employeeName = (from e in entities.Employees
                //                 where (e.UserName.ToString() == employeeUsername)
                //                 select e.FirstName + " " + e.LastName).ToArray();

                //nn = (LoginController.nimi.FirstName + " " + LoginController.nimi.LastName).ToArray();
            }
            finally
            {
                entities.Dispose();
            }

            return pekka;
            //return employeeName;
        }
        public byte[] GetEmployeeImage(string employeeName)
        {
            MobileDBEntities1 entities = new MobileDBEntities1();
            try
            {
                string[] nameParts = employeeName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                string first = nameParts[0];
                string last = nameParts[1];
                byte[] bytes = (from e in entities.Employees
                                where (e.Active == true) &&
                                (e.FirstName == first) &&
                                (e.LastName == last)
                                select e.EmployeePicture).FirstOrDefault();

                return bytes;
            }
            finally
            {
                entities.Dispose();
            }
        }
        //public string PutEmployeeImage()
        //{
        //    MobileDBEntities entities = new MobileDBEntities();
        //    try
        //    {
        //        Employee newEmployee = new Employee()
        //        {
        //            FirstName = "Hius",
        //            LastName = "Harja",
        //            EmployeePicture = File.ReadAllBytes(@"C:\x\HiusHarja.png")
        //        };
        //        entities.Employees.Add(newEmployee);
        //        entities.SaveChanges();

        //        return "OK!";
        //    }
        //    finally
        //    {
        //        entities.Dispose();
        //    }

        //    //return "Error";
        //}

    }
}
