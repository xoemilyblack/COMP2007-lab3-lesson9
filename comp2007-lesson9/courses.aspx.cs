using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.ModelBinding;
using comp2007_lesson9.Models;
using System.Linq.Dynamic;


namespace comp2007_lesson9
{
    public partial class courses : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["SortColumn"] = "CourseID";
                Session["SortDirection"] = "ASC";
                GetCourses();
            }
        }
        protected void GetCourses()
        {
            using (comp2007Entities db = new comp2007Entities())
            {
                String sortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

                var Courses = from c in db.Courses
                              select new { c.CourseID, c.Title, c.Credits, c.Department.Name };

                

                //bind results 
                grdCourses.DataSource = Courses.AsQueryable().OrderBy(sortString).ToList();
                grdCourses.DataBind();
            }
        }

        protected void grdCourses_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //store the row clicked
            Int32 selectedRow = e.RowIndex;

            //get the selected StudentID 
            Int32 CourseID = Convert.ToInt32(grdCourses.DataKeys[selectedRow].Values["CourseID"]);

            //using EF to remove selected student
            using (comp2007Entities db = new comp2007Entities())
            {
                Course c = (from objC in db.Courses where objC.CourseID == CourseID select objC).FirstOrDefault();
                db.Courses.Remove(c);
                db.SaveChanges();
            }
            //refresh grid
            GetCourses();
        }

        protected void grdCourses_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCourses.PageIndex = e.NewPageIndex;
            GetCourses();
        }

        protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdCourses.PageSize = Convert.ToInt32(ddlPageSize.SelectedValue);
            GetCourses();
        }

        protected void grdCourses_Sorting(object sender, GridViewSortEventArgs e)
        {
            Session["SortColumn"] = e.SortExpression;
            GetCourses();

            if(Session["SortDirection"].ToString() == "ASC")
            {
                Session["SortDirection"] = "DESC";
            }
            else
            {
                Session["SortDirection"] = "ASC";
            }
        }

        protected void grdCourses_RowDataBound(object sender, GridViewRowEventArgs e)
        {
             if (IsPostBack) { 
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Image SortImage = new Image();
                
                    for (int i = 0; i <= grdCourses.Columns.Count -1; i++) {
                        if (grdCourses.Columns[i].SortExpression == Session["SortColumn"].ToString())
                        {
                            if (Session["SortDirection"].ToString() == "DESC")
                            {
                                SortImage.ImageUrl = "images/desc.jpg";
                                SortImage.AlternateText = "Sort desc";
                            }
                            else
                            {
                                SortImage.ImageUrl = "images/asc.jpg";
                                SortImage.AlternateText = "Sort asc";
                            }
                        
                            e.Row.Cells[i].Controls.Add(SortImage);
                            
                        }
                    }
                }
               
            }
        }
    }

    
}