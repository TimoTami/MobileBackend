using MobileBackend.DataAccess;
using MobileBackend.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MobileBackend.Controllers
{
    public class ReportsController : Controller
    {
        // GET: Reports
        public ActionResult HoursPerWorkAssignment()
        {
            MobileDBEntities1 entities = new MobileDBEntities1();
            try
            {
                DateTime today = DateTime.Today;
                DateTime tomorrow = DateTime.Today.AddDays(1);

                List<Timesheet> allTimesheetsToday = (from t in entities.Timesheets
                                                      where (t.StartTime > today) && (t.StartTime < tomorrow) && (t.WorkComplete == true)
                                                      select t).ToList();

                List<HoursPerWorkAssignmentModel> model = new List<HoursPerWorkAssignmentModel>();

                foreach (Timesheet timesheet in allTimesheetsToday)
                {
                    int assignmentId = timesheet.Id_WorkAssignment.Value;
                    HoursPerWorkAssignmentModel existing = model.Where
                        (m => m.WorkAssignmentId == assignmentId).FirstOrDefault();

                    if (existing!=null)
                    {
                        existing.TotalHours += (timesheet.StopTime.Value - timesheet.StartTime.Value).TotalHours;
                    }
                    else
                    {
                        existing = new HoursPerWorkAssignmentModel()
                        {
                            WorkAssignmentId = assignmentId,
                            WorkAssignmentName=timesheet.WorkAssignment.Title,
                            TotalHours = (timesheet.StopTime.Value - timesheet.StartTime.Value).TotalHours
                        };
                        model.Add(existing);
                    }
                }
                return View(model);
            }
            finally
            {
                entities.Dispose();
            }
            
        }
        public ActionResult HoursPerWorkAssignmentAsExcel()
        {
            StringBuilder csv = new StringBuilder();

            csv.AppendLine("Matti;55,3");
            csv.AppendLine("Maija;75,1");

            byte[] buffer = Encoding.UTF8.GetBytes(csv.ToString());
            return File(buffer,"text/csv","Työtunnit.csv");
        }
        public ActionResult HoursPerWorkAssignmentAsExcel2()
        {
            StringBuilder csv = new StringBuilder();

            MobileDBEntities1 entities = new MobileDBEntities1();

            try
            {
                DateTime today = DateTime.Today;
                DateTime tomorrow = DateTime.Today.AddDays(1);

                List<Timesheet> allTimesheetsToday = (from t in entities.Timesheets
                                                      where (t.StartTime > today) && (t.StartTime < tomorrow) && (t.WorkComplete == true)
                                                      select t).ToList();

                

                foreach (Timesheet timesheet in allTimesheetsToday)
                {
                    csv.AppendLine(timesheet.Id_Employee + ";" +
                        timesheet.StartTime + ";" + timesheet.StopTime + ";");
                }
                
            }
            finally
            {
                entities.Dispose();
            }
            byte[] buffer = Encoding.UTF8.GetBytes(csv.ToString());
            return File(buffer, "text/csv", "Työtunnit.csv");
        }
        public ActionResult GetTimesheetCounts(string onlyComplete)
        {
            ReportChartDataViewModel model = new ReportChartDataViewModel();

            MobileDBEntities1 entities = new MobileDBEntities1();
            try
            {
                model.Labels = (from wa in entities.WorkAssignments orderby
                                wa.Id_WorkAssignment select wa.Title).ToArray();

                if (onlyComplete=="1")
                {
                    model.Counts = (from ts in entities.Timesheets
                                    where (ts.WorkComplete==true)
                                    orderby ts.Id_WorkAssignment
                                    group ts by ts.Id_WorkAssignment into grp
                                    select grp.Count()).ToArray();
                }
                else
                {
                    model.Counts = (from ts in entities.Timesheets
                                    orderby ts.Id_WorkAssignment
                                    group ts by ts.Id_WorkAssignment into grp
                                    select grp.Count()).ToArray();
                }
            }
            finally
            {
                entities.Dispose();
            }

                return Json(model,JsonRequestBehavior.AllowGet);
        }
    }
}