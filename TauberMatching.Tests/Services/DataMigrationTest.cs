using TauberMatching.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TauberMatching.Services;

namespace TauberMatching.Tests
{
    
    
    /// <summary>
    ///This is a test class for DataMigrationTest and is intended
    ///to contain all DataMigrationTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DataMigrationTest
    {

        DataMigrationService m = new DataMigrationService();
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
        public void ExtractProjectsTest()
        {
            Assert.AreEqual(18,m.ExtractProjects().Count);
            
        }
        [TestMethod()]
        public void ExtractStudentsTest()
        {
            Assert.AreEqual(83, m.ExtractStudents().Count());
        }
        [TestMethod()]
        public void CreateProjectsTest()
        {
            m.CreateProjects(m.ExtractProjects());
            Assert.AreEqual(18, new MatchingDB().Projects.Select(p=>p.Id).Distinct().Count());
            Assert.AreEqual(0, new MatchingDB().Projects.Where(p => p.Id==0).Count());
        }
        [TestMethod()]
        public void CreateStudentsTest()
        {
            m.CreateStudents(m.ExtractStudents());
            Assert.AreEqual(83, new MatchingDB().Students.Select(s => s.Id).Distinct().Count());
            Assert.AreEqual(0, new MatchingDB().Students.Where(s => s.Id == 0).Count());
        }
        [TestMethod()]
        public void CreateMatchings()
        {
            m.CreateProjects(m.ExtractProjects());
            m.CreateStudents(m.ExtractStudents());
            m.CreateMatchings(m.ExtractMatchings());
            Assert.AreEqual(189,new MatchingDB().Matchings.Where(mat=>mat.Id!=0).Count());
            var db = new MatchingDB();
            Assert.AreEqual(0, db.Projects.Where(p => p.Matchings.Count() == 0).Count());
            Assert.AreEqual(0, db.Students.Where(s => s.Matchings.Count() == 0).Count());
        }
    }
}
