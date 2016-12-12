using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;



namespace Vidly.Controllers
{

    public class RiskController : Controller
    {

        public ActionResult AddRisk()
        {
            //fdsafds
            return PartialView();
        }
    
        // GET: url/Risk/AddRisk
        [HttpPost]
        public ActionResult AddRisk(Risk risk)
        {
            GlobalVariables.glob_risk_list.Add(risk);
            //return View();
            return RedirectToAction("BuildRiskTable", "Risk");
        }

        public ActionResult DeleteRisk(int id)
        {
            GlobalVariables.glob_risk_list.RemoveAt(id);
            //return RedirectToAction("AddRisk", "Risk");
            return RedirectToAction("BuildRiskTable", "Risk");
        }

        // GET: url/Risk/DisplayRiskTable
        public ActionResult DisplayRiskTable()     //returns action result, in this case its a ViewResult
        {
            //Sorts the list by impact then by probability
            GlobalVariables.glob_risk_list = GlobalVariables.glob_risk_list.OrderBy(s => s.risk_impact).ThenBy(s => s.risk_probability).ToList();
            GlobalVariables.glob_risk_list.Reverse();

            return PartialView(GlobalVariables.glob_risk_list);

            //this method is returing a view, but under the views->movies, we dont have one called random. so add one.
            //return View(movie); // is returning the Random view. movie object is given as a param so that we can render the object
            //Now the random view can use that movie object 

            //Below are some examples of other action results instead of return view
            //return Content("Hello World!");
            //return HttpNotFound();
            //return new EmptyResult();
            //return RedirectToAction("Index", "Home", new { page = 1, sortBy = "name" }); //(name of action, controller, arguments as an anon obj.)
        }

        public ActionResult BuildRiskTable()
        {
            return View();
        }

        public ActionResult Export()
        {
            return View();
        }

        public void ExportRiskTableToCSV()
        {

            StringWriter sw = new StringWriter();

            sw.WriteLine("\"Risk\",\"Category\",\"Probability\",\"Impact\"");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=RiskTable.csv");
            Response.ContentType = "text/csv";

            foreach (var line in GlobalVariables.glob_risk_list)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\"\"{4}\"",
                                           line.risk_name,
                                           line.risk_category,
                                           line.risk_probability,
                                           line.risk_impact,
                                           line.risk_RMMM));
            }

            Response.Write(sw.ToString());

            Response.End();

        }


    }
}
