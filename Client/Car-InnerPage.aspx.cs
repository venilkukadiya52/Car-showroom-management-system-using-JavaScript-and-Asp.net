﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;

public partial class Client_Car_InnerPage : System.Web.UI.Page
{
    SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\hp\OneDrive\Desktop\CarDealProject\App_Data\CarDealDB.mdf;Integrated Security=True;Connect Timeout=30");
    int n, total;
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
        if (!IsPostBack)
        {
            coupan.Visible = false;
            apply.Visible = false;

        }


    }
    protected void apply_Click(object sender, EventArgs e)
    {
        con.Open();
        SqlDataAdapter adt = new SqlDataAdapter("select * from tbl_Booking where Booking_Status='" + "Buy" + "' and coupan_code='" + coupan.Text + "' and User_Id='" + Session["userid"] + "' and Car_Id='" + Session["cid"] + "'", con);
        DataTable dt5 = new DataTable();
        adt.Fill(dt5);

        if (dt5.Rows.Count > 0)
        {

            int n = Convert.ToInt32(dt5.Rows[0]["count"]) + 1;

            if (n > 3)
            {

                Response.Write("<script>alert('You Can't Use This Code')</script>");
                Session["booking"] = null;
                Response.Write("<Script>window.location.assign('paymentgetway/payment.aspx')</script>");
            }
            else
            {
                
                
                SqlCommand cm = new SqlCommand("insert into tbl_Service1 (User_Id,Service_Type,Car_Id,total,Date,Payment_Mode) values('" + Session["userid"] + "','" + Session["service"] + "','" + Convert.ToInt32(Session["cid"]) + "','" + Session["tprice"] + "','" + Session["datebook"] + "','" + "Online" + "')", con);
                cm.ExecuteNonQuery();
                
                Response.Write("<script>alert('Service Booked Successfully')</script>");
                SqlCommand cmd = new SqlCommand("update tbl_Booking set count='" + n + "' where Booking_Id='" + dt5.Rows[0]["Booking_Id"] + "' ", con);
                cmd.ExecuteNonQuery();
                coupan.Visible = true;
                apply.Visible = true;
                lbl.Visible = false;
                Button1.Visible = false;
                Button2.Visible = false;

                string companyName = "Car Deal";
                int orderNo = 1;
                DataTable dt = new DataTable();
                dt.Columns.AddRange(new DataColumn[2] {
                           
                          
                           
                            new DataColumn("Service_Name", typeof(string)),
                             new DataColumn("Service_Price", typeof(string))
    });


                SqlCommand cmd6 = new SqlCommand("select s.*,c.* from tbl_Service1 AS s INNER JOIN tbl_CarDetails AS c ON s.Car_Id=c.Car_Id where s.User_Id='" + Session["userid"] + "' and s.Car_Id='" + Session["cid"] + "' and s.Service_Type='" + Session["service"] + "'", con);
                SqlCommand cmd1 = new SqlCommand("select * from tbl_ServiceCetagory where Service_Type='" + Session["service"] + "'", con);

                SqlDataAdapter ad = new SqlDataAdapter(cmd6);
                DataTable d = new DataTable();
                ad.Fill(d);
                int i = d.Rows.Count;

                SqlDataAdapter ad1 = new SqlDataAdapter(cmd1);
                DataTable d1 = new DataTable();
                ad1.Fill(d1);
                int i1 = d.Rows.Count;

                string image = "D:/6 sem/CarDealProject/Images/superbmain.jpg";
                string user = Session["username"] + "";
                string cname = d.Rows[0]["Car_Name"] + "";
                string cprice = "" + d.Rows[0]["Car_Price"];
                string stype = "" + Session["service"];

                int s = d1.Rows.Count;
                while (s > 0)
                {
                    dt.Rows.Add(d1.Rows[s - 1][1], d1.Rows[s - 1][2]);
                    total += Convert.ToInt32(d1.Rows[s - 1][2]);
                    s = s - 1;
                }
                con.Close();

                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter hw = new HtmlTextWriter(sw))
                    {
                        StringBuilder sb = new StringBuilder();

                        //Generate Invoice (Bill) Header.
                        sb.Append("<table width='100%' cellspacing='0' cellpadding='2'>");
                        sb.Append("<tr><h2><td align='center' style='background-color: #18B5F0' colspan = '2'><b>Car Service Bill</b></td></h2></tr>");
                        sb.Append("<tr><td colspan = '2'></td></tr>");
                        //sb.Append("<tr><td><img src='");
                        //sb.Append(image);
                        //sb.Append("' style='height:100px;' /></td></tr>");
                        //sb.Append("<div class=col-sm-4 col-6> <div><img src='");
                        //sb.Append(image);
                        //    sb.Append("' alt='' class='img-fluid'></div><br></div>");
                        sb.Append("<td><b>Order No: </b>");
                        sb.Append(orderNo);
                        sb.Append("</td><td align = 'right'><b>Date: </b>");
                        sb.Append(DateTime.Now);
                        sb.Append(" </td></tr>");
                        sb.Append("<tr><td colspan = '2'><b>Company Name: </b>");
                        sb.Append(companyName);
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td colspan = '2'><b>User Name: </b>");
                        sb.Append(user);
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td colspan = '2'><b>Car Name: </b>");
                        sb.Append(cname);
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td colspan = '2'><b>Car Price: </b>");
                        sb.Append(cprice);
                        sb.Append("</td></tr>");
                        sb.Append("<tr><td colspan = '2'><b>Service Type: </b>");
                        sb.Append(stype);
                        sb.Append("</td></tr>");
                        sb.Append("</table>");
                        sb.Append("<br />");

                        //Generate Invoice (Bill) Items Grid.
                        sb.Append("<table border = '1'>");
                        sb.Append("<tr>");
                        foreach (DataColumn column in dt.Columns)
                        {
                            sb.Append("<b><th style = 'background-color: #f06f06;color:black'></b>");
                            sb.Append(column.ColumnName);
                            sb.Append("</th>");
                        }
                        sb.Append("</tr>");
                        foreach (DataRow row in dt.Rows)
                        {
                            sb.Append("<tr>");
                            foreach (DataColumn column in dt.Columns)
                            {
                                sb.Append("<td>");
                                sb.Append(row[column]);
                                sb.Append("</td>");
                            }
                            sb.Append("</tr>");
                        }
                        sb.Append("<tr><td align = 'right' colspan = '");
                        sb.Append(dt.Columns.Count - 1);
                        sb.Append("'>Total Amount</td>");
                        sb.Append("<td>");
                        sb.Append(total);
                        sb.Append("</td>");
                        sb.Append("</tr></table>");

                        //Export HTML String as PDF.
                        StringReader sr = new StringReader(sb.ToString());
                        Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                        HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
                        pdfDoc.Open();
                        htmlparser.Parse(sr);
                        pdfDoc.Close();
                        Response.ContentType = "application/pdf";
                        Response.AddHeader("content-disposition", "attachment;filename=Service_" + d.Rows[0]["Car_Name"] + ".pdf");
                        Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        Response.Write(pdfDoc);
                        Response.End();
                    }

                }
               

            }

        }
        else
        {
            Response.Write("<script>alert('Invalid Coupan Code!')</script>");
            coupan.Visible = true;
            apply.Visible = true;
            lbl.Visible = false;
            Button1.Visible = false;
            Button2.Visible = false;
        }
        con.Close();
        Response.Write("<script>alert('Service Booked Successfully')</script>");
        Response.Write("<Script>window.location.assign('Cars.aspx')</script>");
    }


    protected void yes_Click(object sender, EventArgs e)
    {
        coupan.Visible = true;
        apply.Visible = true;
        lbl.Visible = false;
        Button1.Visible = false;
        Button2.Visible = false;

    }


    protected void no_Click(object sender, EventArgs e)
    {
        Session["booking"] = null;
        Response.Redirect("paymentgetway/payment.aspx");
    }
}