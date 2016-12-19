using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Xml.Linq;

namespace Vidly.Controllers
{

    public class RiskController : Controller
    {

        public ActionResult AddRisk()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult AddRisk(Risk risk)
        {
            //int error_code = 0;
            //error_code = RiskValid(risk);

            GlobalVariables.glob_error_code = RiskValid(risk);
            if (GlobalVariables.glob_error_code == 0) //no error
            {
                GlobalVariables.glob_risk_list.Add(risk);
                return RedirectToAction("BuildRiskTable", "Risk");
            }
            else //error
            {
                //return RedirectToAction("BuildRiskTableError", "Risk", new { id = error_code });
                //TempData["EC"] = error_code; //Temp data stores the error code
                return RedirectToAction("BuildRiskTableError", "Risk");
            }
        }

        public ActionResult DeleteRisk(int id)
        {
            GlobalVariables.glob_risk_list.RemoveAt(id);
            //return RedirectToAction("AddRisk", "Risk");
            return RedirectToAction("BuildRiskTable", "Risk"); //need to fix this
        }

        public ActionResult DisplayRiskTable()     //returns action result, in this case its a ViewResult
        {
            //Sorts the list by impact then by probability
            GlobalVariables.glob_risk_list = GlobalVariables.glob_risk_list.OrderBy(s => s.risk_impact).ThenByDescending(s => s.risk_probability).ToList();

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

        public ActionResult BuildRiskTableError()
        {
            //int error_code = (int)TempData["EC"]; //retrieves the error code saved in temp data
            return View(GlobalVariables.glob_error_code);
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

        public void ExportRiskTableToXLS()
        {
            var grid = new System.Web.UI.WebControls.GridView();

            grid.DataSource = /*from d in dbContext.diners
                              where d.user_diners.All(m => m.user_id == userID) && d.active == true */
                              from line in GlobalVariables.glob_risk_list
                              select new
                              {
                                  RiskName = line.risk_name,
                                  RiskCategory = line.risk_category,
                                  RiskProbability = line.risk_probability,
                                  RiskImpact = line.risk_impact,
                                  RiskRMMM = line.risk_RMMM
                              };

            grid.DataBind();

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment; filename=RiskTable.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);

            grid.RenderControl(htw);

            Response.Write(sw.ToString());

            Response.End();
        }

        public ActionResult ClearRiskTable()
        {
            GlobalVariables.glob_risk_list.Clear();
            return RedirectToAction("BuildRiskTable", "Risk"); //need to fix this
        }

        //Returns error code if error, or 0 if no error
        public int RiskValid(Risk risk)
        {
            //Check Risk Name
                //Required input
                //length must be less than 50
            if (risk.risk_name == null)
                return 1;
            if (risk.risk_name.Length > 50)
                return 1;


            //Check Risk Category
                //Required input
                //Alpha only
                //length less than 5?
            if (risk.risk_category == null)
                return 2;
            if (!risk.risk_category.All(Char.IsLetter))
                return 2;
            if (risk.risk_category.Length > 5)
                return 2;



            //Check Risk Probability'
                //Required input
                //numeric only
                //must be number between 1 and 100
            //if (risk.risk_probability
            //    return false;
            //if (!risk.risk_probability.All(Char.IsNumber))
            //    return false;
           if (!(risk.risk_probability >= 1 && risk.risk_probability <= 100))
                return 3;



            //Check Risk Impact
                //must be int 1,2,3,4
            if (!(risk.risk_impact >= 1 && risk.risk_impact <= 4))
                return 4;


            //Check RMMM
            //Length max is 5
            if(risk.risk_RMMM != null)
                if (risk.risk_RMMM.Length > 5)
                    return 5;


            return 0;
        }
    }
}
