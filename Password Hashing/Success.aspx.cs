using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace Password_Hashing
{
    public partial class Success : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static byte[] Key = null;
        static byte[] IV = null;
        static string userid = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoggedIn"] != null && Session["AuthToken"] != null && Request.Cookies["AuthToken"] != null)
            {
                if (!Session["AuthToken"].ToString().Equals(Request.Cookies["AuthToken"].Value))
                {
                    Response.Redirect("Login.aspx", false);
                }
                else
                {
                    btnLogOut2.Visible = true;
                    userid = Session["UserID"].ToString();
                    displayUserProfile(userid);
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }

        protected string decryptData(byte[] cipherText)
        {
            string plainText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt)) {
                            plainText = srDecrypt.ReadToEnd();
                        
                        }
                    }
                }
            }
            
            
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally { }
            return plainText;
        }

        protected void LogoutMe(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            if (Response.Cookies["ASP.NET_SessionId"] != null)
            {
                Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddMonths(-1);
            }
            if (Response.Cookies["AuthToken"] != null)
            {
                Response.Cookies["AuthToken"].Value = string.Empty;
                Response.Cookies["AuthToken"].Expires = DateTime.Now.AddMonths(-1);
            }
            Response.Redirect("Login.aspx", false);
        }
        protected void displayUserProfile(string userid)
        {
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select * FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["Email"] != DBNull.Value)
                        {
                            lbl_userID.Text = HttpUtility.HtmlEncode(reader["Email"].ToString());
                        }
                        if (reader["FirstName"] != DBNull.Value)
                        {
                            lbl_firstname.Text = HttpUtility.HtmlEncode(reader["FirstName"].ToString());
                        }
                        if (reader["IV"] != DBNull.Value)
                        {
                            lbl_lastname.Text = HttpUtility.HtmlEncode(reader["LastName"].ToString());
                        }
                        if (reader["Key"] != DBNull.Value)
                        {
                            lbl_dob.Text = HttpUtility.HtmlEncode(reader["DateOfBirth"].ToString());
                        }
                        //      zqif (reader["Key"] != DBNull.Value)
                        //{
                        //    lbl_photo.Text = reader["Photo"].ToString();
                        //}
                        if (reader["DateTimeRegistered"] != DBNull.Value)
                        {
                            lbl_registered.Text = reader["DateTimeRegistered"].ToString();
                        }
                        if (reader["Photo"] != DBNull.Value)
                        {
                            Image1.ImageUrl= reader["Photo"].ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally
            {
                connection.Close();
            }
        }

    }

    
}