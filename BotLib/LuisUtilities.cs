/*******************************************
 * 
 * Marcos Tito de Pardo Marques
 * https://www.linkedin.com/in/mrctito/
 * 
 ********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace BotLib
{
    public static class LuisUtilities
    {
        private static double _acceptableScore = 0;
        public static double AcceptableScore
        {
            get
            {
                if (_acceptableScore == 0)
                {
                    _acceptableScore = Convert.ToDouble(WebConfigurationManager.AppSettings["AcceptableScore"]);
                    if(_acceptableScore > 1)
                    {
                        _acceptableScore = _acceptableScore / 100;
                    }
                }
                return _acceptableScore;
            }
        }
    }
}