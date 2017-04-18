using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.TestManagement.Common;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using log4net;
using log4net.Config;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace TFSTest
{
    internal class Program
    {
        //"CentralTestPlan\ISH\BAT" 
        //"CentralTestPlan\ISH\SW Drivers" 
        //"ISH3\ISH3_KBL\KBL_RS2"
        private static void Main(string[] args)
        {
            /*
            string iterationPath = args[args.Length - 1];
            string projectName = iterationPath.Substring(0, iterationPath.IndexOf("\\"));*/
            string logPath = Directory.GetCurrentDirectory();
            string logfile = Path.Combine(logPath, "log-file.txt");
            if (File.Exists(logfile))
                File.Delete(logfile);
            ILog log = LogManager.GetLogger("testApp.Logging");

            try
            {
                if (args.Length < 1)
                {
                    log.Info("[Error:] Please input the Area Path or Iteration Path!");
                    throw new Exception("Not input the area and iteraton path!");
                }
                Uri tfsUri = new Uri(ConfigurationManager.AppSettings["TfsUri"]);
                //Uri tfsUri = new Uri("https://tfs-alm.intel.com:8088/tfs/"); 

                TfsConfigurationServer configurationServer =
                    TfsConfigurationServerFactory.GetConfigurationServer(tfsUri);

                // Get the catalog of team project collections
                ReadOnlyCollection<CatalogNode> collectionNodes = configurationServer.CatalogNode.QueryChildren(new[] { CatalogResourceTypes.ProjectCollection }, false, CatalogQueryOptions.None);

                var collectionNode = collectionNodes.Single();

                // Use the InstanceId property to get the team project collection
                Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);
                TfsTeamProjectCollection teamProjectCollection = configurationServer.GetTeamProjectCollection(collectionId);

                // Print the name of the team project collection
                //Console.WriteLine("Collection: " + teamProjectCollection.Name);
                log.InfoFormat("Collecton: {0}", teamProjectCollection.Name);
                //log.Info("---------------------------------");
                log.Info("*********************************");
                WorkItemStore workItemStore = teamProjectCollection.GetService<WorkItemStore>();
                // ISH3\\ISH3_KBL\\KBL_RS2
                string iterationPath = args[args.Length - 1];
                string projectName = iterationPath.Substring(0, iterationPath.IndexOf("\\"));
                for (int i = 0; i < args.Length - 1; i++)
                {
                    string areaPath = args[i];
                    log.InfoFormat("Area Path[{0}]:{1}", i, areaPath);
                    log.InfoFormat("Iteration Path:{0}", iterationPath);
                    log.Info("[WorkItemType]\t[ID]\t[Title]");
                    log.Info("---------------------------------");
                    string sql
                        = string.Format("select * from WorkItems where [System.TeamProject] = 'CentralTestPlan' and [System.WorkItemType] = 'Test Case' and [System.State] <> 'Removed' and [System.AreaPath] under '{0}'", areaPath);
                    WorkItemCollection workItemCollection = workItemStore.Query(sql);
                    foreach (WorkItem testItem in workItemCollection)
                    {
                        // create Test Cycle
                        //Project teamProject = workItemStore.Projects["ISH3"];
                        Project teamProject = workItemStore.Projects[projectName];
                        WorkItemType workItemType = teamProject.WorkItemTypes["Test Execution"];
                        WorkItem newWorkItem = new WorkItem(workItemType);
                        string workItemTitle = string.Format("[0] {0}", testItem.Title);
                        newWorkItem.Title = workItemTitle;
                        newWorkItem.State = "No Run";
                        newWorkItem.IterationPath = iterationPath;
                        newWorkItem.Fields["Automation Test Name"].Value = testItem.Fields["Automation Test Name"].Value;
                        newWorkItem.Fields["Test Domain"].Value = testItem.Fields["Test Domain"].Value;
                        newWorkItem.Fields["Component"].Value = testItem.Fields["Component"].Value;
                        // the link Type must be "Parent Test Case", add test case
                        WorkItemLinkTypeEnd linkTypeEnd = workItemStore.WorkItemLinkTypes.LinkTypeEnds["Parent Test Case"];
                        newWorkItem.Links.Add(new RelatedLink(linkTypeEnd, testItem.Id));
                        //WorkItemLink newWorkItemLink = new WorkItemLink(workItemStore.WorkItemLinkTypes["Test Execution"], newWorkItem.Id, testItem.Id);
                        //newWorkItem.WorkItemLinks.Add(newWorkItemLink);
                        // save the workitem
                        newWorkItem.Save();
                        //log.InfoFormat("Test excutiion [{0}] [{1}] IterationPath:{2} create finished!", newWorkItem.Id, newWorkItem.Title, newWorkItem.IterationPath);
                        log.InfoFormat("Test excutiion\t[{0}]\t[{1}]\thas been created finished!", newWorkItem.Id, newWorkItem.Title);
                    }
                    log.Info("");
                }
            }
            catch (Exception e)
            {
                log.Info(e.Message.ToString());
                return;
            }
        }
    }
}

