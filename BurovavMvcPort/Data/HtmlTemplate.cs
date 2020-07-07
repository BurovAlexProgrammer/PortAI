using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BurovavMvcPort.Data
{
    public static class HtmlTemplate
    {
        /// <summary>
        /// Generates HTML code snippet - link to download the archive.
        /// </summary>
        /// <param name="title">Title of the link</param>
        /// <param name="link">Link to the archive</param>
        /// <returns>HTML code snippet (string)</returns>
        public static string Archive(string title, string link)
        {
            string archiveIcon = "~/img/rar-flat.png";
            return "<a href = '" + link + "' >< img width = '32' height = '32' src = '" + archiveIcon + "' /> " + title + " </a>";
        }
    }
}
