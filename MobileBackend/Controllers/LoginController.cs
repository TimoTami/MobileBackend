using MobileBackend.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TimesheetMobile1.Models;

namespace MobileBackend.Controllers
{
    [RoutePrefix("/api/login")]
    public class LoginController : ApiController
    {
       
        MobileDBEntities1 entities = new MobileDBEntities1();


        [HttpPost]
        [ActionName("XAMARIN_REG")]
        // POST: api/Login  
        public HttpResponseMessage Xamarin_reg(NewUserModel input)/*(string firstname, string lastname, string phonenumber, string email, string username, string password)*/
        {
            

            try
            {
                //var sama = from e in entities.Employees
                //                 where e.UserName == input.username
                //                 select e.UserName;

                var sama = entities.Employees.Any(u => u.UserName == input.username);

                if (sama == true)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Username is in use. Please try to make up another one."
                        + sama.ToString());
                }
                else
                {

                    Employee employee = new Employee();
                    employee.FirstName = input.firstname;
                    employee.LastName = input.lastname;
                    employee.PhoneNumber = input.phonenumber;
                    employee.EmailAddress = input.email;
                    employee.UserName = input.username;
                    employee.Password = input.password;
                    entities.Employees.Add(employee);
                    entities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Accepted, "Successfully Created");
                }
            }

            finally
            {

                entities.Dispose();
            }
        }
        [HttpGet]
        [ActionName("XAMARIN_Login")]
        // GET: api/Login/5  
        //public HttpResponseMessage Xamarin_login(NewUserModel output)
            public HttpResponseMessage Xamarin_login(string username, string password)
        {
            
            try
            {
                var user = entities.Employees.Where(x => x.UserName == username && x.Password == password).FirstOrDefault();
                if (user == null)
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Please Enter valid UserName and Password");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Accepted, "Success");
                }
            }
            finally
            {

                entities.Dispose();
            }
        }
    }
}
