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
    public partial class course : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if save wasn't clicked and we have a studentID in URL
            if ((!IsPostBack))
            {
                using(comp2007Entities db = new comp2007Entities())
                {
                    foreach(Department d in db.Departments)
                    {
                        ddlDepartments.Items.Add(new ListItem(d.Name, d.DepartmentID.ToString()));
                    }
                }
              
                if (Request.QueryString.Count > 0)
                {
                     GetCourse();
                }              
            }

            
        }
        protected void GetCourse()
        {
            Int32 CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);

            using (comp2007Entities db = new comp2007Entities())
            {
                Course d = (from ObjD in db.Courses where ObjD.CourseID == CourseID select ObjD).FirstOrDefault();

                if (d != null)
                {
                    //map the student properties to form controls
                    txtTitle.Text = d.Title;
                    txtCredits.Text = d.Credits.ToString();
                    ddlDepartments.SelectedValue = d.DepartmentID.ToString();

                    pnlStudent.Visible = true;
                }

                var objE = (from s in db.Students
                            join en in db.Enrollments on s.StudentID equals en.StudentID
                            where en.CourseID == d.CourseID
                            select new { s.StudentID, s.FirstMidName, s.LastName });

                grdStudent.DataSource = objE.ToList();
                grdStudent.DataBind();
                
            }

        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            using (comp2007Entities db = new comp2007Entities())
            {
                //use the student model to save record
                Course d = new Course();

                Int32 CourseID = 0;

                //check query string for an id so we can determine add or update
                if (Request.QueryString["CourseID"] != null)
                {
                    CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);

                    //get the current student from EF
                    d = (from objD in db.Courses where objD.CourseID == CourseID select objD).FirstOrDefault();
                }
                d.Title = txtTitle.Text;
                d.Credits = Convert.ToInt32(txtCredits.Text);
                d.DepartmentID = Convert.ToInt32(ddlDepartments.SelectedValue);


                //redirect to updated table of students
                if (CourseID == 0)
                {
                    db.Courses.Add(d);
                }
                db.SaveChanges();
                Response.Redirect("courses.aspx");
            }
        }

        protected void grdStudent_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}