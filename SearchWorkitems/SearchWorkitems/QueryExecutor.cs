using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;

public class QueryExecutor
{
    private readonly Uri uri;
    private readonly string personalAccessToken;

    
    public QueryExecutor(string orgName, string personalAccessToken)
    {
        this.uri = new Uri(orgName);
        this.personalAccessToken = personalAccessToken;
    }

    
    public async Task<IList<WorkItem>> QueryOpenBugs(string project)
    {
        var credentials = new VssBasicCredential(string.Empty, this.personalAccessToken);

        // create a wiql object and build our query
        var wiql = new Wiql()
        {            
            Query = "Select [Id] " +
                    "From WorkItems " +
                    "Where [Work Item Type] = 'Bug' " +
                    "And [System.TeamProject] = '" + project + "' " +
                    "And [System.State] <> 'Closed' " +
                    "Order By [State] Asc, [Changed Date] Desc",
        };

        // create instance of work item tracking http client
        using (var httpClient = new WorkItemTrackingHttpClient(this.uri, credentials))
        {
           
                // execute the query to get the list of work items in the results
                var result = await httpClient.QueryByWiqlAsync(wiql).ConfigureAwait(false);
                var ids = result.WorkItems.Select(item => item.Id).ToArray();

                
                if (ids.Length == 0)
                {
                    return Array.Empty<WorkItem>();
                }

                // build a list of the fields we want to see
                var fields = new[] { "System.Id", "System.AreaPath" };

                // get work items for the ids found in query
                return await httpClient.GetWorkItemsAsync(ids, fields, result.AsOf).ConfigureAwait(false);
           
        }
    }

    
    public async Task PrintOpenBugsAsync(string project, string searchString)
    {
        var workItems = await this.QueryOpenBugs(project).ConfigureAwait(false);
               
        int workitemCount = 0;
        
        // loop though work items 
        foreach (var workItem in workItems)
        {
            if (workItem.Fields["System.AreaPath"].ToString().Contains(searchString))
                workitemCount++;
            
        }

        Console.WriteLine(
                "Workitem Count: {0}", workitemCount);

    }
}