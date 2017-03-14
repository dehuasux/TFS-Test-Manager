using System;
using System.IO;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using Microsoft.TeamFoundation.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.TestManagement.Client;
using Microsoft.TeamFoundation.Framework.Common;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Core.Extensibility;
using NUnit.Util;
using TFSConnection.NUnitCommon;
using TFSConnection.TFSCommon;
using log4net;
using log4net.Config;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace TFSTest
{
    internal class Program
    {
        public static bool Flag = true;
        // get test case name
        public static string GetTestCaseName(string auTestName)
        {
            string testCaseName = auTestName.Remove(0, auTestName.IndexOf("/run=") + 5);
            testCaseName.Trim();
            testCaseName = testCaseName.Substring(0, testCaseName.IndexOf(" "));
            testCaseName.Trim();
            return testCaseName;
        }

        // get Dll Name
        public static string GetDllName(ITestCase testCase)
        {
            string auTestName = testCase.CustomFields["Automation Test Name"].Value.ToString();
            string testDllName = auTestName.Substring(auTestName.IndexOf("\\") + 1, auTestName.IndexOf("/run=") - auTestName.IndexOf("\\") - 1);
            return testDllName;
        }
        
        private static void Main(string[] args)
        {
            string testDllName, testCaseName;
            ArrayList testCaseCollection = new ArrayList();
            string logpath = Directory.GetCurrentDirectory();
            // judges if current directory have old log file ,if hava ,delete it.
            string logfile = Path.Combine(logpath, "log-file.txt");
            if (File.Exists(logfile))
                File.Delete(logfile);
            ILog log = LogManager.GetLogger("testApp.Logging");    
            //Uri tfsUri = new Uri("https://tfs-alm.intel.com:8088/tfs/"); 
            Uri tfsUri = new Uri(ConfigurationManager.AppSettings["TfsUri"]);
            log.InfoFormat("Connecting to Team Foundation Server {0}...", tfsUri);
            TFSOperate tfsOper = new TFSOperate();
            // connected to TFS
            tfsOper.TfsConnect(tfsUri);
            // get teamproject from TFS
            tfsOper.GetTeamProject("CentralTestPlan");
            for (int index = 0; index < args.Length; index++)
            {
                try
                {
                    string tfsPath = args[index];
                    // print the case path in tfs
                    log.InfoFormat("PATH[[{0}]:{1}", index, tfsPath);
                    log.Info("************************************");
                    /*
                    string sql1
                        = "select * from WorkItems where [System.TeamProject] = 'CentralTestPlan' and [System.WorkItemType] = 'Test Case' and [System.State] <> 'Removed' and [System.AreaPath] under " + "'" + tfsPath + "'";*/
                    string sql 
                        = string.Format("select * from WorkItems where [System.TeamProject] = 'CentralTestPlan' and [System.WorkItemType] = 'Test Case' and [System.State] <> 'Removed' and [System.AreaPath] under '{0}'", tfsPath);
                    // Get testcase collection from sql
                    var testcases = tfsOper.GetTestCaseFromSql(sql);
                    int count = 0;
                    foreach (ITestCase testcase in testcases)
                    {
                        count++;
                        if ((testcase.CustomFields["Is Automated"].Value.ToString() == "Full" || testcase.CustomFields["Is Automated"].Value.ToString() == "Partial") && testcase.CustomFields["Automation Test Name"].Value.ToString() != null)
                        {
                            string autoTestName = testcase.CustomFields["Automation Test Name"].Value.ToString();
                            if (autoTestName.IndexOf("/run=") == -1)
                            {
                                Flag = false;
                                log.Info("ID: " + testcase.Id);
                                log.Info("Title: " + testcase.Title);
                                log.Info("Tfs Path: " + testcase.Area);
                                log.Info("Is Automated: " + testcase.CustomFields["Is Automated"].Value.ToString());
                                log.Info("Automation Test Name: " + testcase.CustomFields["Automation Test Name"].Value.ToString());
                                log.Info("FAIL: No Run Parameters.");
                                log.Info("");
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                        testDllName = GetDllName(testcase);
                        string path = ConfigurationManager.AppSettings["dllPath"] + @"\Tests\" + testDllName;
                        testCaseCollection.Clear();
                        if (File.Exists(path))
                        {
                            NUnitOperate nunitOper = new NUnitOperate();
                            var testTypes = nunitOper.GetMethodsFromAssemblyFile(path);
                            foreach (var type in testTypes)
                            {
                                // get test method in class.
                                foreach (var method in type.GetMethods())
                                {
                                    if (nunitOper.CanBulidForm(method))
                                    {
                                        // judge a test case have a certain attribute
                                        if (nunitOper.HasTestCaseAttributeFor(method))
                                        {
                                            testCaseCollection.AddRange(nunitOper.GetTestCasesWithTestCaseAttribute(method));
                                        }
                                        else if (nunitOper.HasTestSourceCaseAttributeFor(method))
                                        {
                                            testCaseCollection.AddRange(nunitOper.GetTestCaseWithTestSourceCaseAttribute(method));
                                        }
                                        else if (nunitOper.HasValueAttributeFor(method))
                                        {                                            
                                            testCaseCollection.AddRange(nunitOper.GetTestCasesWithParameter(method));
                                        }
                                        else
                                        {   
                                            testCaseCollection.Add(nunitOper.BuildTestMethod(method));
                                        }
                                    }
                                }
                            }
                            testCaseCollection.Sort();
                            // Get Testcase Name
                            testCaseName = GetTestCaseName(testcase.CustomFields["Automation Test Name"].Value.ToString());
                            // judge whether tests folder hava test case, if not , print these test case and its propety.
                            if (!testCaseCollection.Contains(testCaseName))
                            {
                                Flag = false;
                                log.Info("ID: " + testcase.Id);
                                log.Info("Title: " + testcase.Title);
                                log.Info("Tfs Path: " + testcase.Area);
                                log.Info("Is Automated: " + testcase.CustomFields["Is Automated"].Value.ToString());
                                log.Info("Automation Test Name: " + testcase.CustomFields["Automation Test Name"].Value.ToString());
                                log.Info("FAIL: Test Name is not correct.");
                                log.Info("");
                            }
                        }
                        else
                        {
                            Flag = false;
                            log.Info("ID: " + testcase.Id);
                            log.Info("Title: " + testcase.Title);
                            log.Info("Tfs Path: " + testcase.Area);
                            log.Info("Is Automated: " + testcase.CustomFields["Is Automated"].Value.ToString());
                            log.Info("Automation Test Name: " + testcase.CustomFields["Automation Test Name"].Value.ToString());
                            log.InfoFormat("Not find the Dll file name: {0}", testDllName);
                            log.Info("FAIL: dll not found");
                            log.Info("");
                        }
                    }
                    if(Flag)
                    {
                        log.Info("PASS: no errors.");
                        log.Info("");
                    }
                    log.Info("------------------------------------");
                }
                catch (Exception e)
                {
                    log.Info(e.Message);
                    log.Info("------------------------------------");
                    continue;
                }
            }
        }
    }
}
