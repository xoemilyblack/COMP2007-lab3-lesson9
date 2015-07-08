<%@ Page Title="" Language="C#" MasterPageFile="~/site.Master" AutoEventWireup="true" CodeBehind="course.aspx.cs" Inherits="comp2007_lesson9.course" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>
        Course Details
    </h1>
    <h5>
        All fields are required.
    </h5>
    <fieldset>
        <label for="txtTitle" class="col-sm-2">Title:</label>
        <asp:TextBox ID="txtTitle" runat="server" required MaxLength="50" />
    </fieldset>
    <fieldset>
        <label for="txtCredits" class="col-sm-2">Credits:</label>
        <asp:TextBox ID="txtCredits" runat="server" required MaxLength="50" />
        <asp:RangeValidator ID="RangeValidator1" runat="server" ControlToValidate="txtCredits" ErrorMessage="Must be Between 0 - 100" MinimumValue="0" MaximumValue="100" Display="Dynamic" CssClass="label label-danger" Type="Integer"></asp:RangeValidator>
    </fieldset>
    <fieldset>
        <label for="ddlDepartments" class="col-sm-2">Department:</label>
        <asp:DropDownList ID="ddlDepartments" runat="server" AutoPostBack="true">
            
        </asp:DropDownList>
        
    </fieldset>

    <div  class="col-sm-offset-2">
        <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" />
    </div>

    <asp:Panel ID="pnlStudent" runat="server" Visible="false">
    <h2>Students</h2>
    <asp:GridView ID="grdStudent" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="false" OnRowDeleting="grdStudent_RowDeleting" DataKeyNames="StudentID">
        <Columns>
            <asp:BoundField DataField="FirstMidName" HeaderText="First Name" />
            <asp:BoundField DataField="LastName" HeaderText="LastName" />
            <asp:CommandField HeaderText="Delete" DeleteText="Delete" ShowDeleteButton="true" />
        </Columns>
    </asp:GridView>
</asp:Panel>
</asp:Content>
