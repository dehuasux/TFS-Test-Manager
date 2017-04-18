using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NUnit.Core;
using NUnit.Core.Builders;
using NUnit.Core.Extensibility;

namespace TFSConnection.NUnitCommon
{
    public class NUnitOperate
    {
        /// <summary>
        /// thie NUnitTestCaseBuilder object
        /// </summary>
        public NUnitTestCaseBuilder TestBuilder;

        /// <summary>
        /// thie NUnitTestMethod object
        /// </summary>
        public NUnitTestMethod testMethods;

        /// <summary>
        /// thie TestCaseParameterProvider object
        /// </summary>
        public TestCaseParameterProvider ParamProvider;

        /// <summary>
        /// the TestCaseSourceProvider object
        /// </summary>
        public TestCaseSourceProvider SourceProvider;

        /// <summary>
        /// the InlinedataPointProvider object
        /// </summary>
        public InlineDataPointProvider dataPointProviders;

        /// <summary>
        /// the struct method for NunitCommon Class
        /// </summary>
        public NUnitOperate()
        {
            // initialize a NUnitTestCaseBuilder instance
            TestBuilder = new NUnitTestCaseBuilder();
            // initialize a TestCaseParameterProvider instance
            ParamProvider = new TestCaseParameterProvider();
            // initialize a TestCaseSourceProvider instance
            SourceProvider = new TestCaseSourceProvider();
            // initialize a InlineDataPointProvider instance
            dataPointProviders = new InlineDataPointProvider();
        }

        /// <summary>
        /// Get Testcase with Test case attributes 
        /// </summary>
        /// <param name="method">The MethodInfo object.</param>
        public ArrayList GetTestCasesWithTestCaseAttribute(MethodInfo method)
        {
            ArrayList testCaseCollection = new ArrayList();
            testMethods = Reflect.IsAsyncMethod(method) ? new NUnitAsyncTestMethod(method) : new NUnitTestMethod(method);
            ParameterizedMethodSuite methodSuite = new ParameterizedMethodSuite(method);
            NUnitFramework.ApplyCommonAttributes(method, methodSuite);
            IEnumerable ParameterList = ParamProvider.GetTestCasesFor(method);
            testCaseCollection.Add(testMethods.FixtureType.FullName + "." + method.Name);
            foreach (object source in ParameterList)
            {
                ParameterSet parms;
                if (source == null)
                {
                    parms = new ParameterSet();
                    parms.Arguments = new object[] { null };
                }
                else
                {
                    parms = source as ParameterSet;
                }
                if (parms == null)
                {
                    if (source.GetType().GetInterface("NUnit.Framework.ITestCaseData") != null)
                        parms = ParameterSet.FromDataSource(source);
                    else
                    {
                        parms = new ParameterSet();
                        ParameterInfo[] parameters = method.GetParameters();
                        Type sourceType = source.GetType();
                        if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(sourceType))
                            parms.Arguments = new object[] { source };
                        else if (source is object[])
                            parms.Arguments = (object[])source;
                        else if (source is Array)
                        {
                            Array array = (Array)source;
                            if (array.Rank == 1)
                            {
                                parms.Arguments = new object[array.Length];
                                for (int i = 0; i < array.Length; i++)
                                    parms.Arguments[i] = (object)array.GetValue(i);
                            }
                        }
                        else
                            parms.Arguments = new object[] { source };
                    }
                }
                TestMethod testMethod = NUnit.Core.Builders.NUnitTestCaseBuilder.BuildSingleTestMethod(method, null, parms);
                testCaseCollection.Add(testMethod.TestName.FullName);
            }
            return testCaseCollection;
        }

        /// <summary>
        /// Get Testcases with testsourcecase attribute
        /// </summary>
        /// <param name="method">The MethodInfo object.</param>
        public ArrayList GetTestCaseWithTestSourceCaseAttribute(MethodInfo method)
        {
            ArrayList testCaseCollection = new ArrayList();
            testMethods = Reflect.IsAsyncMethod(method) ? new NUnitAsyncTestMethod(method) : new NUnitTestMethod(method);
            ParameterizedMethodSuite methodSuite = new ParameterizedMethodSuite(method);
            NUnitFramework.ApplyCommonAttributes(method, methodSuite);
            IEnumerable ParameterList = SourceProvider.GetTestCasesFor(method);
            testCaseCollection.Add(testMethods.FixtureType.FullName + "." + method.Name);
            foreach (object source in ParameterList)
            {
                ParameterSet parms;
                if (source == null)
                {
                    parms = new ParameterSet();
                    parms.Arguments = new object[] { null };
                }
                else
                {
                    parms = source as ParameterSet;
                }
                if (parms == null)
                {
                    if (source.GetType().GetInterface("NUnit.Framework.ITestCaseData") != null)
                        parms = ParameterSet.FromDataSource(source);
                    else
                    {
                        parms = new ParameterSet();
                        ParameterInfo[] parameters = method.GetParameters();
                        Type sourceType = source.GetType();
                        if (parameters.Length == 1 && parameters[0].ParameterType.IsAssignableFrom(sourceType))
                            parms.Arguments = new object[] { source };
                        else if (source is object[])
                            parms.Arguments = (object[])source;
                        else if (source is Array)
                        {
                            Array array = (Array)source;
                            if (array.Rank == 1)
                            {
                                parms.Arguments = new object[array.Length];
                                for (int i = 0; i < array.Length; i++)
                                    parms.Arguments[i] = (object)array.GetValue(i);
                            }
                        }
                        else
                            parms.Arguments = new object[] { source };
                    }
                }
                TestMethod testMethod = NUnit.Core.Builders.NUnitTestCaseBuilder.BuildSingleTestMethod(method, null, parms);
                testCaseCollection.Add(testMethod.TestName.FullName);
            }
            return testCaseCollection;
        }

        /// <summary>
        /// Get Testcases with 
        /// </summary>
        /// <param name="method">The MethodInfo object.</param>
        public ArrayList GetTestCasesWithParameter(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            IEnumerable[] Sources = new IEnumerable[parameters.Length];
            NUnit.Core.Builders.InlineDataPointProvider dataPointProviders = new NUnit.Core.Builders.InlineDataPointProvider();
            for (int i = 0; i < parameters.Length; i++)
                Sources[i] = dataPointProviders.GetDataFor(parameters[i]);
            IEnumerator[] enumerators = new IEnumerator[Sources.Length];
            int index = -1;
#if CLR_2_0 || CLR_4_0
            List<ParameterSet> testCases = new List<ParameterSet>();
#else
            ArrayList testCases = new ArrayList();
#endif
            for (; ; )
            {
                while (++index < Sources.Length)
                {
                    enumerators[index] = Sources[index].GetEnumerator();
                    if (!enumerators[index].MoveNext())
                        ;
                }
                object[] testdata = new object[Sources.Length];
                for (int i = 0; i < Sources.Length; i++)
                    testdata[i] = enumerators[i].Current;
                ParameterSet testCase = new ParameterSet();
                testCase.Arguments = testdata;
                //testCases.Add(testCase);
                testCases.Add(NUnit.Core.Builders.NUnitTestCaseBuilder.BuildSingleTestMethod(method, null, testCase).TestName.FullName);
                index = Sources.Length;
                while (--index >= 0 && !enumerators[index].MoveNext()) ;
                if (index < 0) break;
            }
            return testCases;
        }

        /// <summary>
        /// Use instance of MethodInfo to construct a Test Method
        /// </summary>
        /// <param name="method">The MethodInfo object.</param>
        public string BuildTestMethod(MethodInfo method)
        {
            return NUnit.Core.Builders.NUnitTestCaseBuilder.BuildSingleTestMethod(method, null, null).TestName.FullName;
        }

        /// <summary>
        /// judge whether a testcase have testcase attribute
        /// </summary>
        /// <param name="method">The MethodInfo object.</param>
        public bool HasTestCaseAttributeFor(MethodInfo method)
        {
            return ParamProvider.HasTestCasesFor(method);
        }

        public bool HasValueAttributeFor(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            ArrayList testCases = new ArrayList();
            for (int i = 0; i < parameters.Length; i++)
            {
                if (Reflect.HasAttribute(parameters[i], "NUnit.Framework.ValuesAttribute", false))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// judge whether a testcase have testsourcecase attribute
        /// </summary>
        /// <param name="method">The MethodInfo object.</param>
        public bool HasTestSourceCaseAttributeFor(MethodInfo method)
        {
            return SourceProvider.HasTestCasesFor(method);
        }

        /// <summary>
        /// judge whether a testcase have Combinatorial attribute
        /// </summary>
        /// <param name="method">The MethodInfo object.</param>
        public bool HasParamAttributeFor(MethodInfo method)
        {
            ParameterInfo[] parameters = method.GetParameters();
            IEnumerable[] Sources = new IEnumerable[parameters.Length];
            return dataPointProviders.HasDataFor(parameters[0]);
        }

        /// <summary>
        /// judge if a method can build a testcase
        /// </summary>
        /// <param name="method">The MethodInfo object.</param>
        public bool CanBulidForm(MethodInfo method)
        {
            return TestBuilder.CanBuildFrom(method);
        }

        /// <summary>
        /// GET Methods From Assembly file
        /// </summary>
        /// <param name="Path">the file Path</param>       
        public IEnumerable<Type> GetMethodsFromAssemblyFile(string Path)
        {
            // Load the test case Dll file
            var assembly = Assembly.LoadFrom(Path);
            // get testfixture classes in assembly.
            var testTypes = from t in assembly.GetTypes()
                            let attributes = t.GetCustomAttributes(typeof(NUnit.Framework.TestFixtureAttribute), true)
                            where attributes != null && attributes.Length > 0
                            orderby t.Name
                            select t;
            return testTypes;
        }
    }
}
