using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Management;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.TestManagement.Common;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Common;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Framework.Client;
using log4net;
using log4net.Config;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace TFSCycleQuery
{
    class Program
    {
        /// <summary>
        /// Project Name
        /// </summary>
        public static string ProjectName;

        /// <summary>
        /// the iterationPath 
        /// </summary>
        public static string iterationPath;

        /// <summary>
        /// the MyExcel Object
        /// </summary>
        public static MyExcel myExcel;

        /// <summary>
        /// the Row Number
        /// </summary>
        public static int Row = 2;

        public const int COLUMNCOUNT = 23;

        public static string FilePath;

        public static string ReportFileName;

        private static readonly string Fmt_DateTime = "yyyy-MM-dd HH:mm:ss.fff";

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
        /// Find Specified TeamProjectCollection
        /// </summary>
        /// <param name="CollectionName"></param>
        /// <returns></returns>
        public static CatalogNode FindTeamProjectCollection(string CollectionName, ReadOnlyCollection<CatalogNode> Nodes)
        {
            foreach (var collectionNode in Nodes)
            {
                if (collectionNode.Resource.DisplayName == CollectionName)
                {
                    return collectionNode;
                }
            }
            return null;
        }

        /// <summary>
        /// Create Sheet Head
        /// </summary>
        /// <param name="ColumnCount">The Column Count</param>
        public static void CreateSheetHead(int ColumnCount)
        {
            string[] strHead = new string[COLUMNCOUNT] 
            {
                "Node Name",     "Team Project",    "ID",                  "Work Item Type",       "Grouping", 
                "Title",         "State",           "Assigned To",         "Iteration Path",       "Component",
                "Sub Component", "Kit-Rev",         "Bug ID",              "Bug DB",               "Authorized As",
                "Changed By",    "Changed Date",    "State Change Date",   "Automation Test Name", "Comments",
                "Created By",    "Created Date",    "Test Case Revision"
            };
            for (int i = 1; i <= ColumnCount; i++)
            {
                myExcel.SetCellValue(myExcel.ws,1,i,strHead[i-1]);
            }
        }

        /// <summary>
        /// Main function
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            string logPath = Directory.GetCurrentDirectory();
            string logfile = Path.Combine(logPath, "log-file.txt");
            if (File.Exists(logfile))
                File.Delete(logfile);
            ILog log = LogManager.GetLogger("testApp.Logging");
            string reportfile = Path.Combine(logPath, "CycleReport.xlsx");
            if (File.Exists(reportfile))
                File.Delete(reportfile);
            myExcel = new MyExcel();
            myExcel.Create();
            CreateSheetHead(COLUMNCOUNT);
            try
            {
                if (args.Length < 1)
                {
                    log.Info("[Error:] Please input the Area Path or Iteration Path!");
                    throw new Exception("Not input the area and iteraton path!");
                }
                Uri tfsUri = new Uri(ConfigurationManager.AppSettings["TfsUri"]);
                //Uri tfsUri = new Uri("https://tfs-alm.intel.com:8088/tfs/"); 

                TfsConfigurationServer configurationServer = TfsConfigurationServerFactory.GetConfigurationServer(tfsUri);

                // Get the catalog of team project collections
                ReadOnlyCollection<CatalogNode> collectionNodes 
                    = configurationServer.CatalogNode.QueryChildren(
                        new[] { CatalogResourceTypes.ProjectCollection }, false, CatalogQueryOptions.None);

                var collectionNode = FindTeamProjectCollection("SOC_FW", collectionNodes);
                if (collectionNode == null)
                {
                    throw new Exception("Not Find The Specified Team Project Collection!");
                }

                //var collectionNode = collectionNodes.Single();
                // Use the InstanceId property to get the team project collection
                Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);
                TfsTeamProjectCollection teamProjectCollection = configurationServer.GetTeamProjectCollection(collectionId);

                // Print the name of the team project collection
                log.InfoFormat("Collecton: {0}", teamProjectCollection.Name);
                //log.Info("---------------------------------");
                log.Info("*********************************");
                WorkItemStore workItemStore = teamProjectCollection.GetService<WorkItemStore>();

                // ISH3\\ISH3_KBL\\KBL_RS2
                for (int i = 0; i < args.Length;i++)
                {
                    //string areaPath = args[i];
                    iterationPath = args[i];
                    ProjectName = GetProjectName(iterationPath);

                    log.InfoFormat("Project[{0}]:{1}",i,iterationPath);
                    string sql = string.Format(
                            "select * from WorkItems " +
                            "where [System.TeamProject] = '{0}' " +
                            "and [System.State] <> 'Removed' " +
                            "and [System.WorkItemType] = 'Test Execution' " +
                            "and [System.IterationPath] under '{1}'", 
                            ProjectName,iterationPath);

                    WorkItemCollection workItemCollection = workItemStore.Query(sql);
                    foreach (WorkItem testItem in workItemCollection)
                    {
                        myExcel.SetCellValue(myExcel.ws, Row, 1, testItem.Fields["Node Name"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 2, testItem.Fields["Team Project"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 3, testItem.Id);
                        myExcel.SetCellValue(myExcel.ws, Row, 4, testItem.Fields["Work Item Type"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 5, testItem.Fields["Grouping"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 6, testItem.Title);
                        myExcel.SetCellValue(myExcel.ws, Row, 7, testItem.State);
                        myExcel.SetCellValue(myExcel.ws, Row, 8, testItem.Fields["Assigned To"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 9, testItem.IterationPath);
                        myExcel.SetCellValue(myExcel.ws, Row, 10, testItem.Fields["Grouping"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 11, testItem.Fields["Sub Component"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 12, testItem.Fields["Kit - Rev"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 13, testItem.Fields["Bug ID"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 14, testItem.Fields["Bug DB"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 15, testItem.Fields["Authorized As"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 16, testItem.Fields["Changed By"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 17, testItem.Fields["Changed Date"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 18, testItem.Fields["State Change Date"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 19, testItem.Fields["Automation Test Name"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 20, testItem.Fields["Comments"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 21, testItem.Fields["Created By"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 22, testItem.Fields["Created Date"].Value);
                        myExcel.SetCellValue(myExcel.ws, Row, 23, testItem.Fields["Test Case Revision"].Value);
                        Console.Write(">");
/*                      Console.Clear();
                        Console.Write(string.Format("{0}",Row++));*/
                        Row++;
                    }
                    log.Info("\r\n");
                    log.Info("-------------------------------------------------------------------------------");
                }
                myExcel.SaveAs(logPath + "\\CycleReport");
                myExcel.Close();
                myExcel.KillSpecialExcel();
            }
            catch (Exception e)
            {
                log.Info(e.Message.ToString());
                myExcel.KillSpecialExcel();
                return;
            }
        }
    }
}
