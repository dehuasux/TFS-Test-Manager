using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.TestManagement.Common;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Common;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Common;
using log4net;
using log4net.Config;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.Server;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace TFSCreateCycle
{
    internal class Program
    {
        // The parentNodeUri string array
        private static string[] parentNodeUris;

        // The Uri string
        private static string strUri;

        // The iteratiion Node which find the start iteration node 
        private static Node iterNode;

        // Get the teamProjectCollection object
        private static TfsTeamProjectCollection teamProjectCollection;

        // Get WorkItemStore object
        private static WorkItemStore workItemStore;

        /// <summary>
        /// Create test cycle
        /// </summary>
        /// <param name="projName"></param>
        /// <param name="rootName"></param>
        /// <returns>if cycle was be create successfully, return true; or return false</returns>
        private static bool CreateCycle(string[] folder)
        {
            // we haven't permission to create project and root folder
            try
            {
                ICommonStructureService css = teamProjectCollection.GetService<ICommonStructureService>();
                Project proj = workItemStore.Projects[folder[0]];
                iterNode = proj.IterationRootNodes[folder[1]];
                NodeInfo ni = css.GetNode(iterNode.Uri.ToString());
                if (ni.StructureType.Equals("ProjectLifecycle"))
                {
                    for (int a = 2; a < folder.Length; a++)
                    {
                        if (!IsNodeExist(iterNode, folder[a]))
                        {
                            parentNodeUris = new string[folder.Length - a + 1];
                            parentNodeUris[0] = strUri;
                            for (int i = a; i < folder.Length; i++)
                            {
                                parentNodeUris[i - a + 1] = css.CreateNode(folder[i], parentNodeUris[i - a]);
                            }
                            break;
                        }
                        else
                        {
                            iterNode = iterNode.FindNodeInSubTree(folder[a]);
                        }
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// Get the Uri by Node
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static string GetNodeUri(Node node)
        {
            return node.Uri.ToString();
        }

        /// <summary>
        /// judge if the specfied by Node name exists
        /// </summary>
        /// <param name="node"></param>
        /// <param name="NodeName"></param>
        /// <returns>if node exists, return true, or false</returns>
        private static bool IsNodeExist(Node node, string NodeName)
        {
            try
            {
                foreach (Node childNode in node.ChildNodes)
                {
                    if (childNode.Name.Equals(NodeName))
                    {
                        strUri = GetNodeUri(childNode);
                        return true;
                    }
                }
                return false;

            }
            catch (Exception e)
            {
                return false;
            }
        }

        /// <summary>
        /// main function
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            //string path1 = "ISH3\\ISH3_KBL\\KBL_RS2\\Test";
            string logPath = Directory.GetCurrentDirectory();
            string logfile = Path.Combine(logPath, "log-file.txt");
            if (File.Exists(logfile))
                File.Delete(logfile);
            ILog log = LogManager.GetLogger("testApp.Logging");
            try
            {
                Uri tfsUri = new Uri(ConfigurationManager.AppSettings["TfsUri"]);
                //Uri tfsUri = new Uri("https://tfs-alm.intel.com:8088/tfs/"); 
                TfsConfigurationServer configurationServer =
                    TfsConfigurationServerFactory.GetConfigurationServer(tfsUri);
                // Get the catalog of team project collections
                ReadOnlyCollection<CatalogNode> collectionNodes = configurationServer.CatalogNode.QueryChildren(new[] { CatalogResourceTypes.ProjectCollection }, false, CatalogQueryOptions.None);
                var collectionNode = collectionNodes.Single();
                // Use the InstanceId property to get the team project collection
                Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);
                teamProjectCollection = configurationServer.GetTeamProjectCollection(collectionId);
                //Console.WriteLine("Collection: " + teamProjectCollection.Name);
                log.InfoFormat("Collecton: {0}", teamProjectCollection.Name);
                //log.Info("---------------------------------");
                log.Info("*********************************");
                workItemStore = teamProjectCollection.GetService<WorkItemStore>();
                //ICommonStructureService css = teamProjectCollection.GetService<ICommonStructureService>(); 
                //"ISH3\ISH3_KBL\KBL_RS2\Test"
                log.Info("path:" + args[0]);
                string path = args[0];
                string[] folder = path.Split('\\');
                for (int i = 0; i < folder.Length; i++)
                {
                    log.Info("folder[" + i + "]:" + folder[i]);
                }
                if (!CreateCycle(folder))
                    return;
                log.Info("Create test cycle:" + args[0] + " successfully!");
            }
            catch (Exception e)
            {
                log.Info(e.Message.ToString());
                return;
            }
        }
    }
}

