﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class Client_cancletestdrive : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\CarDealDB.mdf;Integrated Security=True;User Instance=True");

    protected void Page_Load(object sender, EventArgs e)
    {
        con.Open();
        SqlCommand cmd = new SqlCommand("update tbl_TestDrive set TestDrive_Status='" + "Cancle" + "' where TestDrive_Id='" + Request.QueryString[0] + "'", con);
        cmd.ExecuteNonQuery();
        con.Close();

        Response.Redirect("viewtestdrive.aspx");
        Response.Write("<script>alert('Staff Deleted Successfully')</script>");
    }
}