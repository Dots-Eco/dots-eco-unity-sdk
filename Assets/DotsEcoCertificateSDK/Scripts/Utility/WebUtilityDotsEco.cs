using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public static class WebUtilityDotsEco
{
    public static string ParseHTMLString(string htmlString)
    {
        string cleanedString = htmlString;
        
        cleanedString = WebUtility.HtmlDecode(htmlString);
            
        cleanedString = cleanedString.Replace("<p>", string.Empty).Replace("</p>", string.Empty);
            
        cleanedString = cleanedString.Replace("<strong>", "<b>").Replace("</strong>", "</b>");
        return cleanedString;
    }
}
