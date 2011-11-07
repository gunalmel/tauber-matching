using TauberMatching.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using TauberMatching.Models;

namespace TauberMatching.Tests
{
    
    
    /// <summary>
    ///This is a test class for ConfigurationServiceTest and is intended
    ///to contain all ConfigurationServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ConfigurationServiceTest
    {


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
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        [TestMethod()]
        public void GetConfigParametersTest()
        {
            AppConfiguration expected = null; // TODO: Initialize to an appropriate value
            AppConfiguration actual;
            actual = ConfigurationService.GetConfigParameters();
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }

        [TestMethod()]
        public void UpdateConfigParametersTest()
        {
            AppConfiguration config = null; // TODO: Initialize to an appropriate value
            ConfigurationService.UpdateConfigParameters(config);
            Assert.Inconclusive("A method that does not return a value cannot be verified.");
        }
    }
}
