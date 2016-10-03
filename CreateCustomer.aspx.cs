using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BO;
using DAL;
using System.Data.SqlClient;
using System.Data;

namespace Retail_Banking
{
    public partial class CreateCustomer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string ssnid = txtSSNId.Text;
            string name = txtName.Text;
            string address = txtAd1.Text + ',' + txtAd2.Text + ',' + txtCity.Text + ',' + txtState.Text;
            string age = txtAge.Text;
            string custmerId = "NULL";
            CustomerBoRequest.Customer cu = new CustomerBoRequest.Customer(ssnid, name, address, age);
            DAL.CustomerDal obj = new DAL.CustomerDal();
            BO.CustomerBoResponce.RootObjectout root = obj.getdetails(cu);
            if ((root != null) && (root.Envelope.Body.TF000001OperationResponse.ws_sta_flg == "Y"))
            {
                string constr = "Data Source=inchnilpdb02\\mssqlserver1;Initial Catalog=CHN16_MMS98_TEST;User ID=mms98user;Password=mms98user";
                SqlConnection cn = new SqlConnection(constr);
                cn.Open();
                SqlCommand cmd;
                cmd =new SqlCommand("group7",cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@SSNID", root.Envelope.Body.TF000001OperationResponse.ws_id));
                cmd.Parameters.Add(new SqlParameter("@CustomerID",custmerId ));
                cmd.Parameters.Add(new SqlParameter("@CustomerName", name));
                cmd.Parameters.Add(new SqlParameter("@CustomerAddress",address));
                cmd.Parameters.Add(new SqlParameter("Age",age));
                cmd.Parameters.Add(new SqlParameter("@Status", root.Envelope.Body.TF000001OperationResponse.ws_sta_flg));
               int i= cmd.ExecuteNonQuery();
                if (i > 0)
                {
                    Response.Write("Create Successfully");
                    lblNotify.Text = "ssnid:" + root.Envelope.Body.TF000001OperationResponse.ws_id;
                }

            }
            else
            {
                Response.Write("Creation Fail");
            }


        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

        }

        protected void txtSSNId_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtName_TextChanged(object sender, EventArgs e)
        {

        }
    }
}