using TauberMatching.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using System.Collections.Generic;

namespace TauberMatching.Tests
{
    
    
    /// <summary>
    ///This is a test class for AppConfigurationTest and is intended
    ///to contain all AppConfigurationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class AppConfigurationTest
    {
        IList<ConfigParameter> configParameters = new List<ConfigParameter>();

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
            foreach (ConfigEnum e in Enum.GetValues(typeof(ConfigEnum)))
            {
                ConfigParameter p = new ConfigParameter(e);
                switch (e)
                {
                    case ConfigEnum.EnforceContinuousProjectRanking:
                        p.Value = true.ToString();
                        break;
                    case ConfigEnum.EnforceContinuousStudentRanking:
                        p.Value = false.ToString();
                        break;
                    case ConfigEnum.SiteMasterEmail:
                        p.Value = "gunalmel@yahoo.com";
                        break;
                    case ConfigEnum.SiteMasterFirstName:
                        p.Value = "Melih";
                        break;
                    case ConfigEnum.SiteMasterLastName:
                        p.Value = "Gunal";
                        break;
                    case ConfigEnum.SiteMasterPhone:
                        p.Value = "7349986145";
                        break;
                    case ConfigEnum.RejectedProjectThreshold:
                         p.Value = "10";
                        break;
                    case ConfigEnum.RejectedStudentThreshold:
                        p.Value = "20";
                        break;
                    case ConfigEnum.MinAStudents:
                        p.Value = "2";
                        break;
                    case ConfigEnum.MinAEngStudents:
                        p.Value = "1";
                        break;
                    case ConfigEnum.MinABusStudents:
                        p.Value = "1";
                        break;
                    case ConfigEnum.MaxRejectedStudents:
                        p.Value = "4";
                        break;
                    case ConfigEnum.MaxRejectedEngStudents:
                        p.Value = "2";
                        break;
                    case ConfigEnum.MaxRejectedBusStudents:
                        p.Value = "2";
                        break;
                    case ConfigEnum.MaxRejectedProjects:
                        p.Value = "2";
                        break;
                    default:
                        break;
                }
                configParameters.Add(p);
            }
        }
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        [TestMethod()]
        public void AppConfigurationConstructorTest()
        {
            AppConfiguration target = new AppConfiguration(configParameters);
            Assert.AreEqual(true, target.EnforceContinuousProjectRanking);
            Assert.AreEqual(false, target.EnforceContinuousStudentRanking);
        }

        [TestMethod()]
        public void GetConfigParameterTest()
        {
            AppConfiguration target = new AppConfiguration(configParameters);
            Assert.AreEqual("4",target[ConfigEnum.MaxRejectedStudents].Value);
        }

        [TestMethod()]
        public void SetConfigParameterTest()
        {
            AppConfiguration target = new AppConfiguration();
            target[ConfigEnum.MinABusStudents]=new ConfigParameter(ConfigEnum.MinABusStudents,"100");
            Assert.AreEqual("100", target[ConfigEnum.MinABusStudents].Value);
        }

        [TestMethod()]
        public void GetConfigParametersTest()
        {
            IEnumerable<ConfigParameter> target = new AppConfiguration(configParameters).GetConfigParameters();
            foreach (ConfigParameter param in target)
            {
                Assert.IsTrue(configParameters.Contains(param));
            }
        }
    }
}
