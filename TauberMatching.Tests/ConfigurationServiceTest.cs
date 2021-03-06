﻿using TauberMatching.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TauberMatching.Services;
using System;

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
            AppConfiguration expected = null; 
            AppConfiguration actual;
            actual = ConfigurationService.GetConfigParameters();
            Assert.AreNotEqual(expected, actual);
        }

        [TestMethod()]
        public void UpdateConfigParametersTest()
        {
            AppConfiguration config = new AppConfiguration() { EnforceContinuousProjectRanking = true, MaxRejectedBusStudents = 10 };
            ConfigurationService.UpdateConfigParameters(config);
            AppConfiguration updated = ConfigurationService.GetConfigParameters();
            Assert.AreEqual(true,updated.EnforceContinuousProjectRanking);
            Assert.AreEqual(10, updated.MaxRejectedBusStudents);
        }

        [TestMethod()]
        public void GetEmailConfigParametersTest()
        {
            EmailConfiguration expected = null;
            EmailConfiguration actual;
            actual = ConfigurationService.GetEmailConfigParameters();
            Assert.AreNotEqual(expected, actual);
            var enumerator = actual.GetConfigParameters().GetEnumerator();
            while (enumerator.MoveNext())
            {
                Console.WriteLine(enumerator.Current.Value);
            }
        }
    }
}
