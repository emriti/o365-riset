using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.SharePoint.Client;

namespace Wictor.Office365.ClaimsDemo {
    class Program {
        static void Main(string[] args) {
            if (args.Count() != 3) {
                Console.WriteLine("Syntax: Wictor.Office365.ClaimsDemo.exe url username password");
            }
            MsOnlineClaimsHelper claimsHelper = new MsOnlineClaimsHelper(args[0], args[1], args[2]);
            using (ClientContext context = new ClientContext(args[0])) {
                context.ExecutingWebRequest += claimsHelper.clientContext_ExecutingWebRequest;

                /* get title */
                //context.Load(context.Web);
                //context.ExecuteQuery();
                //Console.WriteLine("Name of the web is: " + context.Web.Title);

                /* get lists */
                var results = context.LoadQuery(context.Web.Lists.Include(list => list.Title, list => list.Id));
                context.ExecuteQuery();
                results.ToList().ForEach(x =>
                {
                    Console.WriteLine(x.Title);
                });




                Console.ReadLine();
                
            }
        }
    }
}
