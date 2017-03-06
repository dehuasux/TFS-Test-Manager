using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using log4net;
using log4net.Config;

namespace TFSConnection.TFSCommon
{
    public class TFSOperate
    {
        public ITestManagementTeamProject testProject;

        /// <summary>
        /// the TfsConfiguraitonServer object
        /// </summary>
        public TfsConfigurationServer configurationServer;

        /// <summary>
        /// the TfsTeamProjectCollection object
        /// </summary>
        public TfsTeamProjectCollection teamProjectCollection;

        /// <summary>
        /// Connected to TFS
        /// </summary>
        /// <param name="tfsUri">The Uri object.</param>
        public void TfsConnect(Uri tfsUri)
        {
           configurationServer 
               = TfsConfigurationServerFactory.GetConfigurationServer(tfsUri);
        }

        /// <summary>
        /// Get TeamProject from TFS
        /// </summary>
        /// <param name="teamProjecName">The teamProjectName object.</param>
        public void GetTeamProject(string teamProjecName)
        {            
            ReadOnlyCollection<CatalogNode> collectionNodes 
                = configurationServer.CatalogNode.QueryChildren(new[] { CatalogResourceTypes.ProjectCollection }, false, CatalogQueryOptions.None);

            var collectionNode = collectionNodes.Single();
            Guid collectionId = new Guid(collectionNode.Resource.Properties["InstanceId"]);
            teamProjectCollection = configurationServer.GetTeamProjectCollection(collectionId);

            ITestManagementService testManagementService = teamProjectCollection.GetService<ITestManagementService>();
            testProject = testManagementService.GetTeamProject(teamProjecName);
        }

        /// <summary>
        /// Get Provided Testcases from sql
        /// </summary>
        public IEnumerable<ITestCase> GetTestCaseFromSql(string sql)
        {
            return testProject.TestCases.Query(sql);
        }
    }
}
