using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace DotsEcoCertificateSDK
{
    public static class WebUtilityDotsEco
    {
        public static string ParseHTMLString(string htmlString)
        {
            string parsedString = htmlString;

            parsedString = WebUtility.HtmlDecode(htmlString);

            parsedString = parsedString.Replace("<p>", string.Empty).Replace("</p>", string.Empty);

            parsedString = parsedString.Replace("<strong>", "<b>").Replace("</strong>", "</b>");

            parsedString = parsedString.Replace("<br>", "\n");

            return parsedString;
        }
    }

}