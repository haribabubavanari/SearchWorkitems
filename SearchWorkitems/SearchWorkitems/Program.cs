using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;


namespace SearchWorkitems
{
    class Program
    {
        static void Main(string[] args)
        {
            QueryExecutor qe = new global::QueryExecutor("https://MyPersonalTFS.visualstudio.com/", "aw4msdfasdfasdf2x4ha6gocw6yif3custsibq");
            

            qe.PrintOpenBugsAsync("HSABank","12345").GetAwaiter().OnCompleted(() =>
            {
                Console.WriteLine("finished");
            });
            Console.ReadKey();
        }


    }
}
