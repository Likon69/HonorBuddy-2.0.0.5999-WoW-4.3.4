using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace PvPRogue.Utils
{
    public static class Stats
    {
        private static readonly string WebsiteURL = "http://swiny.net/pvprogue/stats.php";

        public static void Init()
        {
            WebBrowser WBrowser= new WebBrowser();

            WBrowser.Navigate(WebsiteURL);
        }
    }
}
