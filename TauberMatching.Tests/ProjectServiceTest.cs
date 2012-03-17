using TauberMatching.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using TauberMatching.Models;
using System.Linq;
using System.Collections.Generic;

namespace TauberMatching.Tests
{
    
    
    /// <summary>
    ///This is a test class for ProjectServiceTest and is intended
    ///to contain all ProjectServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ProjectServiceTest
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
        public void GetJsConfigParametersTest()
        {
            Student a = new Student() { Id = 1 };
            Student b = new Student() { Id = 2 };
            Student c = new Student() { Id = 3 };
            Student d = new Student() { Id = 4 };

            Project p = new Project() { Id = 10 };

            Matching m1 = new Matching() { Id = 1, Project = p, ProjectScore = "A", Student = a, StudentScore = "1" };
            Matching m2 = new Matching() { Id = 1, Project = p, ProjectScore = "C", Student = c, StudentScore = "4" };
            Matching m3 = new Matching() { Id = 0, Project = p, ProjectScore = "D", Student = b, StudentScore = "2" };
            Matching m4 = new Matching() { Id = 0, Project = p, ProjectScore = "B", Student = d, StudentScore = "3" };

            p.Matchings = new List<Matching>();
            p.Matchings.Add(m1);
            p.Matchings.Add(m4);

            int[] st = { 1, 2, 3 };
            
            IList<Matching> existingProjectMatchings = p.Matchings.ToList();
            IList<Matching> matchingsToRemove = existingProjectMatchings.Where(m => !st.Contains(m.Student.Id)).ToList();
            int[] studentIdsToRemove = matchingsToRemove.Select(m => m.Student.Id).ToArray();
            int[] studentIdsToAdd = st.Where(sId => !existingProjectMatchings.Select(m => m.Student.Id).Contains(sId)).ToArray();

            //Remove students that do not appear in th list of students to add
            foreach (var m in matchingsToRemove)
            {
                p.Matchings.Remove(m);
            } 
            // Add students that did not exist before
            foreach (var sId in studentIdsToAdd)
            {
                Matching m = new Matching() { Project = p, Student = new Student() { Id = sId } };
                p.Matchings.Add(m);
            }
           // Matching m = new Matching() { Project = p, Student = st, ProjectScore = ProjectScore.NoScore.ToString(), StudentScore = StudentScore.NoScore.ToString() };

            Console.Out.Write(p.Matchings.Count);
            //string expected = "var MinABusStudents = 1;" + System.Environment.NewLine + "var MinAEngStudents = 1;" + System.Environment.NewLine + "var MinAStudents = 2;" + System.Environment.NewLine + "var MaxRejectedBusStudents = 1;" + System.Environment.NewLine + "var MaxRejectedEngStudents = 1;" + System.Environment.NewLine + "var MaxRejectedStudents = 2;" + System.Environment.NewLine + "var RejectedStudentThreshold = 5;" + System.Environment.NewLine + "var EnforceContinuousStudentRanking = true;" + System.Environment.NewLine + "var NoScore_Bucket = $(\"#NoScore_Bucket\");" + System.Environment.NewLine + "var hf_NoScore_Ids = $(\"#hf_NoScore_Ids\");" + System.Environment.NewLine + "var hf_Bus_Total = $(\"#hf_Bus_Total\");" + System.Environment.NewLine + "var hf_NoScore_Bus_Count = $(\"#hf_NoScore_Bus_Count\");" + System.Environment.NewLine + "var hf_Eng_Total = $(\"#hf_Eng_Total\");" + System.Environment.NewLine + "var hf_NoScore_Eng_Count = $(\"#hf_NoScore_Eng_Count\");" + System.Environment.NewLine + "var A_Bucket = $(\"#A_Bucket\");" + System.Environment.NewLine + "var hf_A_Ids = $(\"#hf_A_Ids\");" + System.Environment.NewLine + "var hf_A_Bus_Count = $(\"#hf_A_Bus_Count\");" + System.Environment.NewLine + "var hf_A_Eng_Count = $(\"#hf_A_Eng_Count\");" + System.Environment.NewLine + "var B_Bucket = $(\"#B_Bucket\");" + System.Environment.NewLine + "var hf_B_Ids = $(\"#hf_B_Ids\");" + System.Environment.NewLine + "var hf_B_Bus_Count = $(\"#hf_B_Bus_Count\");" + System.Environment.NewLine + "var hf_B_Eng_Count = $(\"#hf_B_Eng_Count\");" + System.Environment.NewLine + "var C_Bucket = $(\"#C_Bucket\");" + System.Environment.NewLine + "var hf_C_Ids = $(\"#hf_C_Ids\");" + System.Environment.NewLine + "var hf_C_Bus_Count = $(\"#hf_C_Bus_Count\");" + System.Environment.NewLine + "var hf_C_Eng_Count = $(\"#hf_C_Eng_Count\");" + System.Environment.NewLine + "var X_Bucket = $(\"#X_Bucket\");" + System.Environment.NewLine + "var hf_X_Ids = $(\"#hf_X_Ids\");" + System.Environment.NewLine + "var hf_X_Bus_Count = $(\"#hf_X_Bus_Count\");" + System.Environment.NewLine + "var hf_X_Eng_Count = $(\"#hf_X_Eng_Count\");" + System.Environment.NewLine + "var hfProjectId = $(\"#hfProjectId\");";
            //Project p = ProjectService.GetProjectWithFullDetailsByGuid(new Guid("931b3f50-ff86-4a6c-a407-aafc5bbd0750"));
            //string actual = ProjectService.GetJsVariablesForElementsAndUIRules(ProjectService.GetStudentsForProjectGroupedByScore(p));
            //Assert.AreEqual(1, 1);
        }
    }
}
