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
    public class WorkAssignmentController : ApiController
    {
        public string[] GetAll()
        {

            //Employee emp = new Employee();
            //emp.EmployeePicture

            string[] assignmentNames = null;
            MobileDBEntities1 entities = new MobileDBEntities1();
            try
            {
                assignmentNames = (from e in entities.WorkAssignments
                                 where (e.Active == true)
                                 select e.Title).ToArray();
            }
            finally
            {

                entities.Dispose();
            }
            return assignmentNames;
        }
        [HttpPost]
        public bool PostStatus(WorkAssignmentOperationModel input)
        {
            MobileDBEntities1 entities = new MobileDBEntities1();
            try
            {
                WorkAssignment assignment = (from e in entities.WorkAssignments
                                             where (e.Active == true) &&
                                             (e.Title == input.AssignmentTitle)
                                             select e).FirstOrDefault();

                if (assignment == null)
                {
                    return false;
                }

            if (input.Operation == "Start")
            { 

                int assignmentId = assignment.Id_WorkAssignment;

                    Timesheet newEntry = new Timesheet()
                    {
                        Id_WorkAssignment = assignmentId,
                        StartTime = DateTime.Now,
                        WorkComplete = false,
                        Active = true,
                        CreatedAt = DateTime.Now
                };
                entities.Timesheets.Add(newEntry);

                    //assignment.InProgress = true;
                    //assignment.InProgressAt = DateTime.Now;
                    //assignment.LastModifiedAt = DateTime.Now;
            }
            else if (input.Operation == "Stop")
                {

                    int assignmentId = assignment.Id_WorkAssignment;

                    Timesheet existing = (from op in entities.Timesheets
                                          where (op.Id_WorkAssignment == assignmentId) &&
                                          (op.Active == true) &&(op.WorkComplete==false)
                                          orderby op.StartTime descending
                                          select op).FirstOrDefault();

                    if (existing != null)
                    {
                        existing.StopTime = DateTime.Now;
                        existing.WorkComplete = true;
                        existing.LastModifiedAt = DateTime.Now;

                        //assignment.InProgress = false;
                        //assignment.Completed = true;
                        //assignment.CompletedAt = DateTime.Now;
                        //assignment.LastModifiedAt = DateTime.Now;
                    }
                    else
                    {
                        return false;
                    }
                }
                entities.SaveChanges();
            }
            catch
            {
                return false;
            }
            finally
            {

                entities.Dispose();
            }

            return true;
        }
    }
}
