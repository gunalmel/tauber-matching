using TauberMatching.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TauberMatching.Services;

namespace TauberMatching.Tests
{
    
    
    /// <summary>
    ///This is a test class for ProjectScoreStudentMatrixTest and is intended
    ///to contain all ProjectScoreStudentMatrixTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProjectScoreStudentMatrixTest
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
        public void ItemTest()
        {
            ProjectScoreStudentCountMatrix target = new ProjectScoreStudentCountMatrix();
            target["A", StudentDegree.Bus] = 3;
            target["A", StudentDegree.Eng] = 2;
            target["B", StudentDegree.Bus] = 3;
            Assert.AreEqual(5, target["A", StudentDegree.Bus] + target["A", StudentDegree.Eng]);
            Assert.AreEqual(0, target["C", StudentDegree.Bus]);
            Assert.AreEqual(0, target["NoScore", StudentDegree.Bus]);
        }

        [TestMethod()]
        public void ScoreStudentDegreeCountDictionaryTest()
        {
            ProjectScoreStudentCountMatrix target = new ProjectScoreStudentCountMatrix();
            Assert.AreNotEqual(null, target.ScoreStudentDegreeCountDictionary);
            foreach (var score in ProjectService.ProjectScoreDetails)
            {
                foreach(StudentDegree degree in Enum.GetValues(typeof(StudentDegree)))
                    Assert.AreEqual(0,target.ScoreStudentDegreeCountDictionary[score.Score][degree]);
            }
        }
    }
}
