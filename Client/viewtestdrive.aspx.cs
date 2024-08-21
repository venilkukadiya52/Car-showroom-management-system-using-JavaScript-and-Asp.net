﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;

public partial class Client_viewtestdrive : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hp\OneDrive\Desktop\CarDealProject\App_Data\CarDealDB.mdf;Integrated Security=True;Connect Timeout=30");

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["userid"] == null)
        {
            Response.Redirect("Login.aspx");
        }

        if (Session["username"] != null)
        {
            namelbl.Text = "" + Session["username"];
            loginlbl.Visible = false;
            logoutlbl.Visible = true;
            namelbl.Visible = true;
            A4.Visible = true;
            A5.Visible = true;
            A6.Visible = true;




        }
        else
        {
            loginlbl.Visible = true;
            logoutlbl.Visible = false;
            namelbl.Visible = false;
            A4.Visible = false;
            A5.Visible = false;
            A6.Visible = false;


        }
        con.Open();
        SqlCommand cmd = new SqlCommand("select c.*, t.* from tbl_TestDrive AS t INNER JOIN tbl_CarDetails AS c ON t.Car_Id=c.Car_Id where t.TestDrive_Status='" + "Approve" + "' and t.User_Id='" + Session["userid"] + "'", con);
        SqlDataAdapter adt = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adt.Fill(dt);
        con.Close();
        Repeater1.Visible = true;
        Repeater2.Visible = false;
        Repeater1.DataSource = dt;
        Repeater1.DataBind();
        lblError.Visible = false;
        Button2.Visible = false;
        Button1.Visible = true;
        
       
    }

    protected void Cancle_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        int id = Convert.ToInt32(btn.CommandArgument);
        con.Open();
        SqlCommand cmd = new SqlCommand("update tbl_TestDrive set TestDrive_Status='" + "Cancle" + "' where TestDrive_Id='" + id + "'", con);
        cmd.ExecuteNonQuery();
        con.Close();

        Response.Redirect("viewtestdrive.aspx");
        Response.Write("<script>alert('Staff Deleted Successfully')</script>");
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        con.Open();
        SqlCommand cmd = new SqlCommand("select c.*, t.* from tbl_TestDrive AS t INNER JOIN tbl_CarDetails AS c ON t.Car_Id=c.Car_Id where t.TestDrive_Status='" + "Done" + "' and t.User_Id='" + Session["userid"] + "'", con);
        SqlDataAdapter adt = new SqlDataAdapter(cmd);
        DataTable dt = new DataTable();
        adt.Fill(dt);
        con.Close();
        if (dt.Rows.Count == 0)
        {
            lblError.Visible = true;
            lblError.Text = "No Record Found!";
            Repeater1.Visible = false;
            Repeater2.Visible = true;
            Button2.Visible = true;
            Button1.Visible = false ;
        }
        else
        {
            Repeater1.Visible = false;
            Repeater2.Visible = true;
            Repeater2.DataSource = dt;
            Repeater2.DataBind();
            Button2.Visible = true;
            Button1.Visible = false;
        
        }
    }
}