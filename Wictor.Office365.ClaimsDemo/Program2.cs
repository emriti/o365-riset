using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Wictor.Office365.ClaimsDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.SharePoint.Client;
    using System.Net;
    using MSDN.Samples.ClaimsAuth;
    public class Program2
    {
        [STAThread]
        static void Main(string[] args)
        {
            //if (args.Length < 1) { Console.WriteLine("SP_Ctx <url>"); return; }
            //string targetSite = args[0];
            string targetSite = "https://sharepointOnlineURLRoot/sites/siteName";
            using (ClientContext ctx = ClaimClientContext.GetAuthenticatedContext(targetSite))
            {
                if (ctx != null)
                {
                    ctx.Load(ctx.Web); // Query for Web
                    ctx.ExecuteQuery(); // Execute
                    Console.WriteLine(ctx.Web.Title);
                }
            }
            Console.WriteLine("");
            Console.WriteLine("");
            CookieCollection authCookie =
                ClaimClientContext.GetAuthenticatedCookies(targetSite, 925, 525);
            listWS.Lists list = new listWS.Lists();
            list.Url = "https://sharepointOnlineURLRoot/sites/siteName/_vti_bin/Lists.asmx"
                //list.Timeout = 15000; //in milliseconds
            list.CookieContainer = new CookieContainer();
            list.CookieContainer.Add(authCookie);
            string listName = "Shared Documents";
            string viewName = "";
            //string listName = "{1A4A3C5D-360E-45EB-B9ED-E8653981CAC0}";
            //string viewName = "{5A4AF2C5-8A9F-427F-B8AA-BC59E3BE8AA0}";
            string rowLimit = "5";
            // Instantiate an XmlDocument object         
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            System.Xml.XmlElement query = xmlDoc.CreateElement("Query");
            System.Xml.XmlElement viewFields = xmlDoc.CreateElement("ViewFields");
            System.Xml.XmlElement queryOptions = xmlDoc.CreateElement("QueryOptions");

            //*Use CAML query*/        
            query.InnerXml = "<Where><Gt><FieldRef Name=\"ID\" />" +
                "<Value Type=\"Counter\">0</Value></Gt></Where>";
            viewFields.InnerXml = "<FieldRef Name=\"Title\" />";
            //queryOptions.InnerXml = "";
            queryOptions.InnerXml =
                "<IncludeMandatoryColumns>FALSE</IncludeMandatoryColumns>" +
                "<DateInUtc>TRUE</DateInUtc>";
            System.Xml.XmlNode nodes =
                list.GetListItems(
                    listName,
                    viewName,
                    query,
                    viewFields,
                    rowLimit,
                    null,
                    string.Empty);
            string ixml = list.GetList(listName).InnerXml;
            Console.WriteLine(
                "Retrieving title of all the items in SharePoint Online" +
                 "sites 'Shared Documents' using Lists webservice");
            Console.WriteLine(
                "===========================================" +
                "=============================================");
            foreach (System.Xml.XmlNode node in nodes)
            {
                if (node.Name == "rs:data")
                {
                    for (int i = 0; i < node.ChildNodes.Count; i++)
                    {
                        if (node.ChildNodes[i].Name == "z:row")
                        {
                            Console.WriteLine(
                                node.ChildNodes[i].Attributes["ows_Title"].Value);
                        }
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
