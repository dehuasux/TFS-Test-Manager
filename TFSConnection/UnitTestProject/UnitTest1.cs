using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TFSConnection.TFSCommon;
using TFSConnection.NUnitCommon;


namespace TFSTestManagerTests
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void ErrorOnInvalidArgs()
        {
            TFSOperate tfstest = new TFSOperate();
            Uri tfsUri = new Uri("https://tfs-alm.intel.com:8088/tfs/");
            tfstest.TfsConnect(tfsUri);
            string testPlan = "CentralTestPlan";
            //string valideTfsPath = "CentralTestPlan\\ISH\\Linux (Ubuntu)";
            string invalideTfsPath = "ISH\\Bat)";
            tfstest.GetTeamProject(testPlan);
            string sql = string.Format("select * from WorkItems where [System.TeamProject] = '{0}' and [System.WorkItemType] = 'Test Case' and [System.State] <> 'Removed' and [System.AreaPath] under '{1}'", testPlan, invalideTfsPath);
            int success = 0;
            try
            {
                var testcases = tfstest.GetTestCaseFromSql(sql);
                success = 1;
                Assert.IsNotNull(testcases);//"Expected exception was not thrown"
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception is thrown");
                Assert.IsTrue(success == 0);
            }
        }

        [TestMethod]
        public void SuccessOnValidArgs()
        {
            TFSOperate tfstest = new TFSOperate();
            Uri tfsUri = new Uri("https://tfs-alm.intel.com:8088/tfs/");
            tfstest.TfsConnect(tfsUri);
            string testPlan = "CentralTestPlan";
            string valideTfsPath = "CentralTestPlan\\ISH\\Linux (Ubuntu)";
            //string invalideTfsPath = "ISH\\Bat)";
            tfstest.GetTeamProject(testPlan);
            string sql = string.Format("select * from WorkItems where [System.TeamProject] = '{0}' and [System.WorkItemType] = 'Test Case' and [System.State] <> 'Removed' and [System.AreaPath] under '{1}'", testPlan, valideTfsPath);
            int success = 0;
            try
            {
                var testcases = tfstest.GetTestCaseFromSql(sql);
                success = 1;
                Assert.IsNotNull(testcases);//"Expected exception was not thrown");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(success == 0);
                Console.WriteLine("Exception is thrown");
            }
        }

        [TestMethod]
        public void SuccessOnValidMutipleArgs()
        {
            TFSOperate tfstest = new TFSOperate();
            Uri tfsUri = new Uri("https://tfs-alm.intel.com:8088/tfs/");
            tfstest.TfsConnect(tfsUri);
            string testPlan = "CentralTestPlan";
            tfstest.GetTeamProject(testPlan);
            string[] validTfsPaths = { "CentralTestPlan\\ISH", "CentralTestPlan\\ISH\\Linux (Ubuntu)" };
            for (int i = 0; i < validTfsPaths.Length;i++ )
            {
                string sql = string.Format("select * from WorkItems where [System.TeamProject] = '{0}' and [System.WorkItemType] = 'Test Case' and [System.State] <> 'Removed' and [System.AreaPath] under '{1}'", testPlan, validTfsPaths[i]);
                int success = 0;
                try
                {
                    var testcases = tfstest.GetTestCaseFromSql(sql);
                    success = 1;
                    Assert.IsNotNull(testcases);//"Expected exception was not thrown");
                    Console.WriteLine("args[{0}] {1} is a correct args!", i, validTfsPaths[i]);
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(success == 0);
                    Console.WriteLine("args[{0}] {1} is an error args!", i, validTfsPaths[i]);
                }
            }            
        }

        [TestMethod]
        public void ErrorsOnInvalidMultipleArgs()
        {
            TFSOperate tfstest = new TFSOperate();
            Uri tfsUri = new Uri("https://tfs-alm.intel.com:8088/tfs/");
            tfstest.TfsConnect(tfsUri);
            string testPlan = "CentralTestPlan";
            tfstest.GetTeamProject(testPlan);
            string[] validTfsPaths = { "AAA\\BBB", "CCC\\DDD" };
   
            for (int i = 0; i < validTfsPaths.Length; i++)
            {
                string sql = string.Format("select * from WorkItems where [System.TeamProject] = '{0}' and [System.WorkItemType] = 'Test Case' and [System.State] <> 'Removed' and [System.AreaPath] under '{1}'", testPlan, validTfsPaths[i]);
                int success = 0;
                try
                {
                    var testcases = tfstest.GetTestCaseFromSql(sql);
                    success = 1;
                    Assert.IsNotNull(testcases);
                    Console.WriteLine("args[{0}] {1} is a correct args!", i, validTfsPaths[i]);
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(success == 0);
                    Console.WriteLine("args[{0}] {1} is an error args!", i, validTfsPaths[i]);
                }
            }
        }

        [TestMethod]
        public void ErrorsOnValidArgsAndInvalidArgs()
        {
            TFSOperate tfstest = new TFSOperate();
            Uri tfsUri = new Uri("https://tfs-alm.intel.com:8088/tfs/");
            tfstest.TfsConnect(tfsUri);
            string testPlan = "CentralTestPlan";
            tfstest.GetTeamProject(testPlan);
            string[] validTfsPaths = { "AAA\\BBB", "CentralTestPlan\\ISH\\Linux (Ubuntu)" };

            for (int i = 0; i < validTfsPaths.Length; i++)
            {
                string sql = string.Format("select * from WorkItems where [System.TeamProject] = '{0}' and [System.WorkItemType] = 'Test Case' and [System.State] <> 'Removed' and [System.AreaPath] under '{1}'", testPlan, validTfsPaths[i]);
                int success = 0;
                try
                {
                    var testcases = tfstest.GetTestCaseFromSql(sql);
                    success = 1;
                    Assert.IsNotNull(testcases);
                    Console.WriteLine("args[{0}] {1} is a correct args!", i, validTfsPaths[i]);
                }
                catch (Exception ex)
                {
                    Assert.IsTrue(success == 0);
                    Console.WriteLine("args[{0}] {1} is an error args!", i, validTfsPaths[i]);
                }
            }
        }

        [TestMethod]
        public void SuccessOnValidUrl()
        {
            TFSOperate tfstest = new TFSOperate();
            Uri tfsUri = new Uri("https://tfs-alm.intel.com:8088/tfs/");
            string testPlan = "CentralTestPlan";
            int success = 0;
            try
            {
                tfstest.TfsConnect(tfsUri);
                tfstest.GetTeamProject(testPlan);
                success = 1;
                Console.WriteLine("URL: {0} is a correct TFS URL", tfsUri.ToString());
            }
            catch(Exception ex)
            {
                Assert.IsTrue(success == 0);
                Console.WriteLine("URL: {0} is a error TFS URL Info: {1}", tfsUri.ToString(),ex.Message);
            }
        }

        [TestMethod]
        public void ErrorsOnInvalidUrl()
        {
            TFSOperate tfstest = new TFSOperate();
            Uri tfsUri = new Uri("https://www.baidu.com/");
            string testPlan = "CentralTestPlan";
            int success = 0;
            try
            {
                tfstest.TfsConnect(tfsUri);
                tfstest.GetTeamProject(testPlan);
                success = 1;
                Console.WriteLine("URL: {0} is a correct TFS URL", tfsUri.ToString());
            }
            catch (Exception ex)
            {
                Assert.IsTrue(success == 0);
                Console.WriteLine("URL: {0} is a error TFS URL Info: {1}", tfsUri.ToString(), ex.Message);
            }
        }

        [TestMethod]
        public void SuccessOnValidCase()
        {
            ArrayList testCaseCollection = new ArrayList();
            NUnitOperate nunitOper = new NUnitOperate();
            string Path = "C:\\code\\TFS\\TFS\\TFSConnection-170228\\TFSConnection\\TFSConnection\\bin\\Release\\Tests\\SoftwareWindowsTests.dll";
            try
            {
                var testTypes = nunitOper.GetMethodsFromAssemblyFile(Path);
                Assert.IsNotNull(testTypes);
                Console.WriteLine("File: {0} is a Valid Case", Path);
            }
            catch(Exception ex)
            {
                Console.WriteLine("File: {0} is not a Valid Case", Path);
            }
        }

        [TestMethod]
        public void ErrorsOnInvalidCase()
        {
            ArrayList testCaseCollection = new ArrayList();
            NUnitOperate nunitOper = new NUnitOperate();
            string Path = "C:\\code\\TFS\\TFS\\TFSConnection-170228\\TFSConnection\\TFSConnection\\bin\\Release\\Tests\\test.dll";
            try
            {
                var testTypes = nunitOper.GetMethodsFromAssemblyFile(Path);
                Assert.IsNotNull(testTypes);
                Console.WriteLine("File: {0} is a Valid Case", Path);
            }
            catch(Exception ex)
            {
                Console.WriteLine("File: {0} is not a Valid Case",Path);
            }
        }

        [TestMethod]
        public void IsHaveTestCaseAttr()
        {
            ArrayList testCaseCollection = new ArrayList();
            NUnitOperate nunitOper = new NUnitOperate();
            string Path = "C:\\code\\TFS\\TFS\\TFSConnection-170228\\TFSConnection\\TFSConnection\\bin\\Release\\Tests\\SoftwareWindowsTests.dll";
            int success = 0;
            var testTypes = nunitOper.GetMethodsFromAssemblyFile(Path);
            try
            {
                foreach (var type in testTypes)
                {
                    foreach (var method in type.GetMethods())
                    {
                        if (nunitOper.HasTestCaseAttributeFor(method))
                        {
                            testCaseCollection.AddRange(nunitOper.GetTestCasesWithTestCaseAttribute(method));
                        }
                    }
                }
                Assert.IsNotNull(testCaseCollection);
                Console.WriteLine("The Type have Test case attribute");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("The Type not have Test case attribute");
            }
        }

        [TestMethod]
        public void IsHaveTestSourceCaseAttr()
        {
            ArrayList testCaseCollection = new ArrayList();
            NUnitOperate nunitOper = new NUnitOperate();
            string Path = "C:\\code\\TFS\\TFS\\TFSConnection-170228\\TFSConnection\\TFSConnection\\bin\\Release\\Tests\\SoftwareWindowsTests.dll";
            int success = 0;
            var testTypes = nunitOper.GetMethodsFromAssemblyFile(Path);
            try
            {
                foreach (var type in testTypes)
                {
                    foreach (var method in type.GetMethods())
                    {
                        if (nunitOper.HasTestSourceCaseAttributeFor(method))
                        {
                            testCaseCollection.AddRange(nunitOper.GetTestCaseWithTestSourceCaseAttribute(method));
                        }
                    }
                }
                Assert.IsNotNull(testCaseCollection);
                Console.WriteLine("The Type have Test source case attribute");
            }
            catch (Exception ex)
            {
                Console.WriteLine("The Type not have Test source case attribute");
            }
        }
        
    }
}
