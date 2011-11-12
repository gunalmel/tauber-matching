using TauberMatching.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting.Web;
using TauberMatching.Models;
using System.Collections.Generic;
using System.Linq;

namespace TauberMatching.Tests
{


    /// <summary>
    ///This is a test class for EmailQueueServiceTest and is intended
    ///to contain all EmailQueueServiceTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EmailQueueServiceTest
    {
        private TestContext testContextInstance;
        private MatchingDB _db = new MatchingDB();
        private Project _p;
        private Student _s;
        private EmailQueueMessage _emq1;
        private EmailQueueMessage _emq2;

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
            _p = new Project() { ContactFirst = "Mel", ContactLast = "Gunal", Guid = Guid.NewGuid(), ContactEmail = "gunalmel@yahoo.com", Name="Test Project" };
            _s = new Student() { FirstName = "John", LastName = "Doe", Degree = "Bus", Guid = Guid.NewGuid(), Email = "gunalmel@gmail.com", UniqueName="gunalmel" };
            _db.Projects.Add(_p);
            _db.Students.Add(_s);
            _db.SaveChanges();
            _emq1 = new EmailQueueMessage() { ContactId = _p.Id, ContactType = "Project", Guid = _p.Guid, LastName = _p.ContactLast, FirstName = _p.ContactFirst, To = _p.ContactEmail, Subject = "Test Project Email", Body = "See if project with id "+_p.Id.ToString()+" has been emailed." };
            _emq2 = new EmailQueueMessage() { ContactId = _s.Id, ContactType = "Student", Guid = _s.Guid, LastName = _s.LastName, FirstName = _s.FirstName, To = _s.Email, Subject = "Test Student Email", Body = "See if student with id "+_s.Id.ToString()+" has been emailed." };
            _db.EmailQueueMessages.Add(_emq1);
            _db.EmailQueueMessages.Add(_emq2);
            _db.SaveChanges();
        }
        //
        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {

            var x = _db.Projects.Include("EmailLogs").First().EmailLogs.FirstOrDefault();
            var y = _db.Students.Include("EmailLogs").First().EmailLogs.FirstOrDefault();
            _db.Projects.Remove(_p);
            _db.Students.Remove(_s);
            foreach (var el in _p.EmailLogs)
                _db.EmailLogs.Remove(el);
            foreach (var el in _s.EmailLogs)
                _db.EmailLogs.Remove(el);
            _db.SaveChanges();
            _db.Database.ExecuteSqlCommand("delete from EmailQueueMessages", new object[0]);
        }
        //
        #endregion


        [TestMethod()]
        public void SendMailsInTheQueueTest()
        {
            EmailQueueService.SendMailsInTheQueue();
            Assert.AreEqual(null, _db.EmailQueueMessages.FirstOrDefault());        
        }

        [TestMethod()]
        public void QueueMessageTest()
        {
            EmailQueueMessage message = new EmailQueueMessage() { ContactId = 11000, ContactType = "Project", Guid = _p.Guid, LastName = _p.ContactLast, FirstName = _p.ContactFirst, To = _p.ContactEmail, Subject = "Test Project Email2", Body = "See if project with id 11000 has been emailed." };
            EmailQueueService.QueueMessage(message);
            var emq = _db.EmailQueueMessages.First(m => m.Id == message.Id);
            Assert.AreNotEqual(null, emq);
            _db.EmailQueueMessages.Remove(emq);
            _db.SaveChanges();
        }

        [TestMethod()]
        public void LogMessageTest()
        {
            EmailQueueService_Accessor.LogMessage(_db, _emq1, EmailStatus.Success.ToString());
            var pr = _db.Projects.First(p => p.Id == _emq1.ContactId);
            Assert.IsTrue(pr.Emailed); //Test if project emailed is updated.
            var eLog = pr.EmailLogs.First();
            Assert.AreEqual(_emq1.Guid, eLog.Guid);
            Assert.AreEqual(_emq1.Subject, eLog.Subject);
            Assert.AreEqual(_emq1.Body, eLog.Message);
            Assert.AreEqual(EmailStatus.Success.ToString(), eLog.Status);
            Assert.AreNotEqual(DateTime.MinValue, eLog.Date);
            Assert.AreNotEqual(DateTime.MaxValue, eLog.Date);
            Assert.AreNotEqual(null, eLog.Date);

            EmailQueueService_Accessor.LogMessage(_db, _emq2, EmailStatus.Failed.ToString());
            var st = _db.Students.First(s => s.Id == _emq2.ContactId);
            Assert.IsFalse(st.Emailed); //Test if student emailed is updated.
            var eLog2 = st.EmailLogs.First();
            Assert.AreEqual(_emq2.Guid, eLog2.Guid);
            Assert.AreEqual(_emq2.Subject, eLog2.Subject);
            Assert.AreEqual(_emq2.Body, eLog2.Message);
            Assert.AreEqual(EmailStatus.Success.ToString(), eLog.Status);
            Assert.AreNotEqual(DateTime.MinValue, eLog2.Date);
            Assert.AreNotEqual(DateTime.MaxValue, eLog2.Date);
            Assert.AreNotEqual(null, eLog2.Date);
        }

        [TestMethod()]
        public void GetMessagesTest()
        {
            IList<EmailQueueMessage> actual = EmailQueueService_Accessor.GetMessages(_db); ;
            Assert.AreEqual(2, actual.Count);
        }
    }
}
