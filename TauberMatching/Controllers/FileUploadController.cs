using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using LinqToExcel;
using System.Data.SqlServerCe;
using TauberMatching.Models;
using System.Collections.Generic;

namespace TauberMatching.Controllers
{
    public class FileUploadController : Controller
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ProjectController));

        MatchingDB db = new MatchingDB();

        private String InitializeConnectionString()
        {
            //string appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            string appPath = Server.MapPath("~/App_Data");
            return "Data Source=" + Path.Combine(appPath, "MatchingDB.sdf") + " ;pwd=;";
        }
        //
        // GET: /FileUpload/
        [HttpGet]
        public ActionResult UploadMatchingFile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadMatchingFile(HttpPostedFileBase file)//the argument name, file, is the same name as the name of the file input. This is important for the model binder to match up the uploaded file to the action method argument.
        {
            if (file!=null&&file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
                //linqtoExcel requires that application be compiled using x86 platform. Under project properties set platform target to x86 for both debug and release
                DeleteUploadTable();
                InsertIntoTempTableUsingEF(ReadUploadedExcelFile(path));
            }
            return View();
        }
        /// <summary>
        /// Reads uploaded Excel file mapping columns to object properties by reading the first row as the header line.
        /// The worksheet that will have the data to be uploaded should be names as ImportSheet by default
        /// </summary>
        /// <param name="fileNamAndPath">Fully qualified absolute file name with its path pointing to the uploaded Excel file </param>
        /// <returns>Enumerable collection of UploadEntity objects representing a row in Excel sheet.</returns>
        private IEnumerable<UploadEntity> ReadUploadedExcelFile(String fileNamAndPath)
        {
            var excel = new ExcelQueryFactory(fileNamAndPath);
            excel.AddMapping<UploadEntity>(ue => ue.ProjectName, "Project");
            excel.AddMapping<UploadEntity>(ue => ue.ContactFirst, "Contact_First");
            excel.AddMapping<UploadEntity>(ue => ue.ContactLast, "Contact_Last");
            excel.AddMapping<UploadEntity>(ue => ue.ContactEmail, "Contact_Email");
            excel.AddMapping<UploadEntity>(ue => ue.ContactPhone, "Contact_Phone");
            excel.AddMapping<UploadEntity>(ue => ue.UniqueName, "Unique_Name");
            excel.AddMapping<UploadEntity>(ue => ue.StudentFirst, "Student_First");
            excel.AddMapping<UploadEntity>(ue => ue.StudentLast, "Student_Last");
            excel.AddMapping<UploadEntity>(ue => ue.StudentDegree, "Student_Degree");
            // The excel worksheet should be named as Sheet1 as default if worksheet name is not specified.
            var entities = from row in excel.Worksheet<UploadEntity>("ImportSheet") select row;
            return entities;
        }
        /// <summary>
        /// Persists an enumerable collection of UploadEntity objects in the database using Entity framework.
        /// </summary>
        /// <param name="entities">Collection of Upload Entities to be persisted.</param>
        private void InsertIntoTempTableUsingEF(IEnumerable<UploadEntity> entities)
        {
            foreach (UploadEntity ue in entities)
            {
                db.UploadEntities.Add(ue);
                db.SaveChanges();
            }
        }
        /// <summary>
        /// Deletes all records in the temporary table that will store student/project matchings before they are migrated into actual production tables.
        /// Temporary table is being populated by the data that is coming from the uploaded Excel file that should have a very specific structure.
        /// </summary>
        private void DeleteUploadTable()
        {
            using (SqlCeConnection conn = new SqlCeConnection(InitializeConnectionString()))
            {
                conn.Open();
                string deleteTable = "delete from UploadEntities";
                SqlCeCommand cmdDelete = new SqlCeCommand(deleteTable, conn);
                cmdDelete.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Accepting LinqToExcel.Row object inserts data captured from upladed Excel spread sheet into UploadEntities table.
        /// </summary>
        /// <param name="row">LinqToExcel.Row object represnting a single row to be inserted.</param>
        /// <param name="conn">An open database connection to the target database.</param>
        private void InsertIntoTempTable(LinqToExcel.Row row, SqlCeConnection conn) 
        { 
            string insert = "insert into UploadEntities (ProjectName,ContactFirst,ContactLast,ContactEmail,ContactPhone,UniqueName,StudentFirst,StudentLast) values(@pName,@cFirst,@cLast,@cEmail,@cPhone,@uName,@sFirst,@sLast)";

            try
             {
                SqlCeCommand cmdInsert = new SqlCeCommand(insert,conn);
                cmdInsert.Parameters.AddWithValue("@pName",row["Project"].Value);
                cmdInsert.Parameters.AddWithValue("@cFirst",row["Contact_First"].Value);
                cmdInsert.Parameters.AddWithValue("@cLast",row["Contact_Last"].Value);
                cmdInsert.Parameters.AddWithValue("@cEmail",row["Contact_Email"].Value);
                cmdInsert.Parameters.AddWithValue("@cPhone",row["Contact_Phone"].Value);
                cmdInsert.Parameters.AddWithValue("@uName",row["Unique_Name"].Value);
                cmdInsert.Parameters.AddWithValue("@sFirst",row["Student_First"].Value);
                cmdInsert.Parameters.AddWithValue("@sLast",row["Student_Last"].Value);
                cmdInsert.ExecuteNonQuery();                  
             }
             catch (SqlCeException ex)
             {
                 throw ex;
             }          
       }
    }
}
