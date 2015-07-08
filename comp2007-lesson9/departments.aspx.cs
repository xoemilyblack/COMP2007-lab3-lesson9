using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq.Dynamic;

//reference Entity
using System.Web.ModelBinding;
using comp2007_lesson9.Models;

namespace comp2007_lesson9
{
    public partial class departments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["SortColumn"] = "DepartmentID";
                Session["SortDirection"] = "ASC";
                GetDepartments();
            }
        }
        protected void GetDepartments()
        {
            using (comp2007Entities db = new comp2007Entities())
            {
                String sortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();

                var Departments = from d in db.Departments
                                  select d;

                //bind results 
                grdDepartments.DataSource = Departments.AsQueryable().OrderBy(sortString).ToList();
                grdDepartments.DataBind();
            }
        }
        protected void grdDepartments_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //store the row clicked
            Int32 selectedRow = e.RowIndex;

            //get the selected StudentID 
            Int32 DepartmentID = Convert.ToInt32(grdDepartments.DataKeys[selectedRow].Values["DepartmentID"]);

            //using EF to remove selected student
            using (comp2007Entities db = new comp2007Entities())
            {
                Department d = (from objD in db.Departments where objD.DepartmentID == DepartmentID select objD).FirstOrDefault();
                db.Departments.Remove(d);
                db.SaveChanges();
            }
            //refresh grid
            GetDepartments();
        }

        protected void ddlPageSize2_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdDepartments.PageSize = Convert.ToInt32(ddlPageSize2.SelectedValue);
            GetDepartments();
        }

        protected void grdDepartments_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    Image SortImage = new Image();

                    for (int i = 0; i <= grdDepartments.Columns.Count - 1; i++)
                    {
                        if (grdDepartments.Columns[i].SortExpression == Session["SortColumn"].ToString())
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

        protected void grdDepartments_Sorting(object sender, GridViewSortEventArgs e)
        {
            Session["SortColumn"] = e.SortExpression;
            GetDepartments();

            if (Session["SortDirection"].ToString() == "ASC")
            {
                Session["SortDirection"] = "DESC";
            }
            else
            {
                Session["SortDirection"] = "ASC";
            }
        }

        protected void grdDepartments_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdDepartments.PageIndex = e.NewPageIndex;
            GetDepartments();
        }


    }
}
