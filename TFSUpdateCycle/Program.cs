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
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.Framework.Common;
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

        // Get the iteration path of test execution
        private static string iterationPath;
        // Get the projectName of project
        private static string projectName;
        // Get the test execution state 
        private static string testExcutionState;
        // Get the test execution name
        private static string testExcutionName;
        // Get Kit-Rev
        private static string strKitRev;
        // Get BugDB
        private static string strBugDB;
        // Get BugID
        private static string strBugID;
        // Get returned string from command line
        private static string rdr;
        // log instance
        private static ILog log;

        /// <summary>
        /// Update Test exectuion state
        /// </summary>
        /// <param name="workItem">specified test execution</param>
        /// <returns>TRUE OR FALSE</returns>
        private static bool UpdateWorkItemState(WorkItem workItem)
        {
            workItem.Open();
            switch (testExcutionState)
            {
                case "Blocked":
                case "Fail":
                    workItem.Fields["Kit - Rev"].Value = strKitRev;
                    workItem.Fields["Bug DB"].Value = strBugDB;
                    workItem.Fields["Bug ID"].Value = strBugID;
                    workItem.State = testExcutionState;
                    workItem.Save();
                    break;
                case "No Run Due To Automation Bug":
                    workItem.Fields["Bug DB"].Value = strBugDB;
                    workItem.Fields["Bug ID"].Value = strBugID;
                    workItem.State = testExcutionState;
                    workItem.Save();
                    break;
                case "Pass":
                    workItem.Fields["Kit - Rev"].Value = strKitRev;
                    //workItem.Fields["Bug DB"].Value = "";
                    //workItem.Fields["Bug ID"].Value = "";
                    workItem.State = testExcutionState;
                    workItem.Save();
                    break;
                case "Blocked Internal":
                case "No Run":
                case "Not Planned":
                case "Removed":
                case "Running":
                case "Under Debug":
                    workItem.State = testExcutionState;
                    workItem.Save();
                    break;
                default:
                    workItem.Close();
                    return false;
            }
            //workItem.Reset();
            workItem.Close();
            return true;
        }

        private static bool GetKitRevString()
        {
            //
            for (; ; )
            {
                Console.Write("Please input [Kit - Rev] value:");
                rdr = Console.ReadLine();
                if (!string.IsNullOrEmpty(rdr))
                {
                    strKitRev = rdr;
                    return true;
                }
                else
                    continue;
            }
            return false;
        }

        private static bool GetBugDBString()
        {
            //
            for (; ; )
            {
                Console.Write("Please input [Bug DB] value:");
                rdr = Console.ReadLine();
                if (!string.IsNullOrEmpty(rdr))
                {
                    strBugDB = rdr;
                    return true;
                }
                else
                    continue;
            }
            return false;
        }

        private static bool GetBugIDString()
        {
            //
            for (; ; )
            {
                Console.Write("Please input [Bug ID] value:");
                rdr = Console.ReadLine();
                if (!string.IsNullOrEmpty(rdr))
                {
                    strBugID = rdr;
                    return true;
                }
                else
                    continue;
            }
            return false;
        }

        private static bool GetCmdStringResult(string State)
        {
            switch (State)
            {
                case "Blocked":
                case "Fail":
                    if (!GetKitRevString())
                        return false;
                    if (!GetBugDBString())
                        return false;
                    if (!GetBugIDString())
                        return false;
                    break;
                case "No Run Due To Automation Bug":
                    if (!GetBugDBString())
                        return false;
                    if (!GetBugIDString())
                        return false;
                    break;
                case "Pass":
                    if (!GetKitRevString())
                        return false;
                    break;
                case "Blocked Internal":
                case "No Run":
                case "Not Planned":
                case "Removed":
                case "Running":
                case "Under Debug":
                    break;
                default:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Get Project Name
        /// </summary>
        /// <param name="Path">Iteration or Area Path</param>
        /// <returns>string</returns>
        public static string GetProjectName(string Path)
        {
            return Path.Substring(0, Path.IndexOf("\\"));
        }

        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args">The Parameters of Main</param>  
        private static void Main(string[] args)
        {
            // ISH3\ISH3_KBL\KBL_RS2\Test
            // [0] Check_Power_D0i1
            // FAIL
            string logPath = Directory.GetCurrentDirectory();
            string logfile = Path.Combine(logPath, "log-file.txt");
            if (File.Exists(logfile))
                File.Delete(logfile);
            log = LogManager.GetLogger("testApp.Logging");

            try
            {
                Uri tfsUri = new Uri(ConfigurationManager.AppSettings["TfsUri"]);
                //Uri tfsUri = new Uri("https://tfs-alm.intel.com:8088/tfs/"); 
                TfsConfigurationServer configurationServer = TfsConfigurationServerFactory.GetConfigurationServer(tfsUri);

                // Get the catalog of team project collections
                ReadOnlyCollection<CatalogNode> collectionNodes
                    = configurationServer.CatalogNode.QueryChildren(new[] { CatalogResourceTypes.ProjectCollection }, false, CatalogQueryOptions.None);

                var collectionNode = collectionNodes.Single();
                // Use the InstanceId property to get the team project collection
                Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);

                TfsTeamProjectCollection teamProjectCollection = configurationServer.GetTeamProjectCollection(collectionId);

                log.InfoFormat("Collecton: {0}", teamProjectCollection.Name);
                //log.Info("---------------------------------");
                log.Info("*********************************");
                WorkItemStore workItemStore = teamProjectCollection.GetService<WorkItemStore>();
                if (args[0] != null && args[1] != null && args[2] != null)
                {
                    iterationPath = args[0];
                    testExcutionName = args[1];
                    testExcutionState = args[2];
                    projectName = GetProjectName(iterationPath);
                }
                else
                {
                    throw new Exception("Parameter errors!");
                }

                if (!GetCmdStringResult(testExcutionState))
                {
                    throw new Exception(string.Format("The state {0} you input is not correct!", testExcutionState));
                }
                string sql
                    = string.Format("select [System.Id] from WorkItems where [System.TeamProject] = '{0}' and [System.WorkItemType] = 'Test Execution' and [System.State] <> 'Removed' and [System.Title] = '{1}' and  [System.IterationPath] under '{2}'", projectName, testExcutionName, iterationPath);
                WorkItemCollection workItemCollection = workItemStore.Query(sql);
                if (workItemCollection.Count > 0)
                {
                    foreach (WorkItem workItem in workItemCollection)
                    {
                        if (workItem.IsValid())
                        {
                            if (string.Compare(workItem.Title, testExcutionName, true) == 0)
                            {
                                if (!UpdateWorkItemState(workItem))
                                {
                                    throw new Exception(string.Format("The state {0} you input is not correct!", testExcutionState));
                                }
                                else
                                {
                                    workItem.Open();
                                    //log.InfoFormat("Update ID:{0} Title:{1} IterationPath:{2} State:{3} successfully!", workItem.Id, workItem.Title, workItem.IterationPath, workItem.State); 
                                    log.Info("Update Spcificed Test Execution Successfully!");
                                    log.Info("ID:" + workItem.Id);
                                    log.Info("Title:" + workItem.Title);
                                    log.Info("IterationPath:" + workItem.IterationPath);
                                    log.Info("State:" + workItem.State);
                                    //log.Info("Bug ID:" + workItem.Fields["Bug ID"].Value);
                                    //log.Info("Bug DB:" + workItem.Fields["Bug DB"].Value);
                                    //log.Info("Kit - Rev:" + workItem.Fields["Kit - Rev"].Value);
                                    log.Info("--------------------------------------------");
                                    workItem.Close();
                                }
                            }
                        }
                        else
                        {
                            throw new Exception("This Test execution is not a valid workitem!");
                        }
                    }
                }
                else
                {
                    throw new Exception("Not find the specified test execution!");
                }
            }
            catch (Exception e)
            {
                log.Info(e.Message.ToString());
            }
        }
    }
}
