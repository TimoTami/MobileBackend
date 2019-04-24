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
        public static Employee nimi;
        MobileDBEntities1 entities = new MobileDBEntities1();

        [HttpPost]
        [ActionName("XAMARIN_REG")]
        // POST: api/Login  
        public HttpResponseMessage Xamarin_reg(NewUserModel input)/*(string firstname, string lastname, string phonenumber, string email, string username, string password)*/
        {
            
            try
            {
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
                    nimi = user;
                    user.Active = true;
                    entities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.Accepted, "Success");
                }
            }
            finally
            {
                entities.Dispose();
            }
        }
        //[HttpPut]
        //[ActionName("XAMARIN_Logout")]
        //public HttpResponseMessage Xamarin_logout(NewUserModel input)
        //{
        //    ////ei tarvi parametrejä, voi vaan pistää kaikki falseksi

        //    //var kaikkiEmployeet = entities.Employees.Where(k => k.Active == true);
        //    //var employeeLogout = entities.Employees.Where(u => u.UserName == input.username).FirstOrDefault();

        //    //if (employeeLogout == null)
        //    //{
        //    //    return Request.CreateResponse(HttpStatusCode.Unauthorized, "Vika");
        //    //}

        //    //else
        //    //{
        //    //    foreach (var employee in kaikkiEmployeet)
        //    //    {
        //    //        employee.Active = false;
        //    //    }
        //    //    //employeeLogout.Active = false;
        //    //    entities.SaveChanges();

        //    //    return Request.CreateResponse(HttpStatusCode.Accepted, "Successfully LoggedOut. ");
                
        //    //    //+ "Username: "+employeeLogout.UserName.ToString()+
        //    //        //" Etunimi: "+employeeLogout.FirstName.ToString()+" Aktiivisuus: "+employeeLogout.Active.Value.ToString());
        //    //}
        //}
    }
}
