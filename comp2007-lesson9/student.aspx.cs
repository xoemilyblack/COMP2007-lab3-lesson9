using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using comp2007_lesson9.Models;
using System.Web.ModelBinding;

namespace comp2007_lesson9
{
    public partial class Student1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if save wasn't clicked and we have a studentID in URL
            if ((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                GetStudent();
              
            }
        }

        protected void GetStudent()
        {
            Int32 StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);

            using (comp2007Entities db = new comp2007Entities())
            {
                Student s = (from ObjS in db.Students where ObjS.StudentID == StudentID select ObjS).FirstOrDefault();


                //map the student properties to form controls
                if (s != null)
                {
                    txtLastName.Text = s.LastName;
                    txtFirstName.Text = s.FirstMidName;
                    txtEnrollmentDate.Text = s.EnrollmentDate.ToString("yyyy-MM-dd");
                    pnlCourses.Visible = true;

                }
                var objE = (from en in db.Enrollments
                            join c in db.Courses on en.CourseID equals c.CourseID
                            join d in db.Departments on c.DepartmentID equals d.DepartmentID
                            where en.StudentID == s.StudentID
                            select new { en.EnrollmentID, d.Name, c.Title, en.Grade });

                grdStudent.DataSource = objE.ToList();
                grdStudent.DataBind();
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (comp2007Entities db = new comp2007Entities())
            {
                //use the student model to save record
                Student s = new Student();
                Enrollment en = new Enrollment();
                Int32 StudentID = 0;
                //check query string for an id so we can determine add or update
                if (Request.QueryString["StudentID"] != null)
                {
                    StudentID = Convert.ToInt32(Request.QueryString["StudentID"]);

                    //get the current student from EF
                    s = (from objS in db.Students where objS.StudentID == StudentID select objS).FirstOrDefault();
                }

                s.LastName = txtLastName.Text;
                s.FirstMidName = txtFirstName.Text;
                s.EnrollmentDate = Convert.ToDateTime(txtEnrollmentDate.Text);

                //add only if student has no ID
                if (StudentID == 0)
                {
                    //redirect to updated table of students
                    db.Students.Add(s);

                }
                db.SaveChanges();
                Response.Redirect("students.aspx");
            }
        }

        protected void grdStudent_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Int32 EnrollmentID = Convert.ToInt32(grdStudent.DataKeys[e.RowIndex].Values["EnrollmentID"]);

            using (comp2007Entities db = new comp2007Entities())
            {
                Enrollment objE = (from en in db.Enrollments
                                       where en.EnrollmentID == EnrollmentID
                                       select en).FirstOrDefault();
                db.Enrollments.Remove(objE);
                db.SaveChanges();

                GetStudent();
            }
        }
    }
}