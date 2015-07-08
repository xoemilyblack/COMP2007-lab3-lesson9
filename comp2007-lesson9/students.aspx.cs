using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

//reference Entity
using System.Web.ModelBinding;
using comp2007_lesson9.Models;
using System.Linq.Dynamic;

namespace comp2007_lesson9
{
    public partial class students : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if loading for first time populate student grid
            if (!IsPostBack)
            {
                Session["SortColumn"] = "StudentID";
                Session["SortDirection"] = "ASC";
                GetStudents();
            }
        }
        protected void GetStudents()
        {
            using (comp2007Entities db = new comp2007Entities())
            {

                String sortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

                var Students = from s in db.Students
                               select s;

                //bind results 
                grdStudents.DataSource = Students.AsQueryable().OrderBy(sortString).ToList();
                grdStudents.DataBind();
            }
        }

        protected void grdStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //store the row clicked
            Int32 selectedRow = e.RowIndex;

            //get the selected StudentID 
            Int32 StudentID = Convert.ToInt32(grdStudents.DataKeys[selectedRow].Values["StudentID"]);

            //using EF to remove selected student
            using (comp2007Entities db = new comp2007Entities())
            {
                Student s = (from objS in db.Students where objS.StudentID == StudentID select objS).FirstOrDefault();
                db.Students.Remove(s);
                db.SaveChanges();
            }
            //refresh grid
            GetStudents();
        }

        protected void grdStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdStudents.PageIndex = e.NewPageIndex;
            GetStudents();
        }

        protected void grdStudents_Sorting(object sender, GridViewSortEventArgs e)
        {
            Session["SortColumn"] = e.SortExpression;
            GetStudents();

            if (Session["SortDirection"].ToString() == "ASC")
            {
                Session["SortDirection"] = "DESC";
            }
            else
            {
                Session["SortDirection"] = "ASC";
            }
        }

        protected void grdStudents_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Image SortImage = new Image();

                    for (int i = 0; i <= grdStudents.Columns.Count - 1; i++)
                    {
                        if (grdStudents.Columns[i].SortExpression == Session["SortColumn"].ToString())
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

        protected void ddlPageSize3_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdStudents.PageSize = Convert.ToInt32(ddlPageSize3.SelectedValue);
            GetStudents();
        }

    }
}