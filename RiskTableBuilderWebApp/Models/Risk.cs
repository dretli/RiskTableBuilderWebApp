using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public static class GlobalVariables
    {
        public static List<Risk> glob_risk_list = new List<Risk>();
        public static int glob_error_code = 0;
    }

    public class Risk
    {
        public string risk_name { get; set; }
        public string risk_category { get; set; }
        public short risk_probability { get; set; }
        public short risk_impact { get; set; }
        public string risk_RMMM { get; set; }
    }
}