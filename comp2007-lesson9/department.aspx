<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="department.aspx.cs" Inherits="comp2007_lesson9.department" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <h1>
        Department Details
    </h1>
    <h5>
        All fields are required.
    </h5>
    <fieldset>
        <label for="txtName" class="col-sm-2">Name:</label>
        <asp:TextBox ID="txtName" runat="server" required MaxLength="50" />
    </fieldset>
    <fieldset>
        <label for="txtBudget" class="col-sm-2">Budget:</label>
        <asp:TextBox ID="txtBudget" runat="server" required MaxLength="50"/>
        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtBudget" ErrorMessage="Must be Between 0 - 20 million" MinimumValue="0" MaximumValue="200000000" Display="Dynamic" CssClass="label label-danger" Type="Double"></asp:RangeValidator>
    </fieldset>

    <div  class="col-sm-offset-2">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
    </div>
    
    <asp:Panel ID="pnlCourse" runat="server" Visible="false">
    <h2>Courses</h2>
    <asp:GridView ID="grdCourse" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="false" OnRowDeleting="grdCourse_RowDeleting" DataKeyNames="CourseID">
        <Columns>
            <asp:BoundField DataField="Title" HeaderText="Title" />
            <asp:BoundField DataField="Credits" HeaderText="Credits" />
            <asp:CommandField HeaderText="Delete" DeleteText="Delete" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>
</asp:Panel>
</asp:Content>
