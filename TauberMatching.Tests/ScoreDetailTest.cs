using TauberMatching.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;

namespace TauberMatching.Tests
{
    
    
    /// <summary>
    ///This is a test class for ScoreDetailTest and is intended
    ///to contain all ScoreDetailTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ScoreDetailTest
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
        public void EqualsTest()
        {
            ScoreDetail target = new ScoreDetail();
            target.Score = "A";
            target.ScoreFor = "Project";
            ScoreDetail target2 = new ScoreDetail();
            target2.Score = "A";
            target2.ScoreFor = "Project";
            
            Assert.IsTrue(target.Equals(target2));
            target2.Score = "B";
            Assert.IsFalse(target.Equals(target2));
            target2.ScoreFor = "Student";
            Assert.IsFalse(target.Equals(target2));
            target2.Score = "A";
            Assert.IsFalse(target.Equals(target2));

            target.Id = 1;
            target2.ScoreFor = "Project";
            Assert.IsFalse(target.Equals(target2));
            target2.Id = 1;
            Assert.IsTrue(target.Equals(target2));
        }
     
        [TestMethod()]
        public void GetHashCodeTest()
        {
            ScoreDetail target = new ScoreDetail();
            target.Score = "A";
            target.ScoreFor = "Project";
            ScoreDetail target2 = new ScoreDetail();
            target2.Score = "A";
            target2.ScoreFor = "Project";

            Assert.AreEqual(target2.GetHashCode(),target.GetHashCode());
            target2.Score = "B";
            Assert.AreNotEqual(target2.GetHashCode(), target.GetHashCode());
            target2.ScoreFor = "Student";
            Assert.AreNotEqual(target2.GetHashCode(), target.GetHashCode());
            target2.Score = "A";
            Assert.AreNotEqual(target2.GetHashCode(), target.GetHashCode());

            target.Id = 1;
            target2.ScoreFor = "Project";
            Assert.AreNotEqual(target2.GetHashCode(), target.GetHashCode());
            target2.Id = 1;
            Assert.AreEqual(target2.GetHashCode(), target.GetHashCode());
        }
    }
}
