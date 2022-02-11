using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Password_Hashing
{
    static class Code
    {
        public static int code;
    }
    public partial class Login : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string errorMsg = "";

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public class MyObject
        {
            public string success { get; set; }
            public List<string> ErrorMessage { get; set; }
        }

        public void twofac()
        {
            Random rnd = new Random();
            Code.code = rnd.Next(100000, 999999);
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("bookhaven95@gmail.com");
                    mail.To.Add("3dsssgaming@gmail.com");
                    mail.Subject = "Test Sending Mail";
                    mail.Body = "<h1>This is body</h1>";
                    mail.IsBodyHtml = true;
                    var client = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("bookhaven95@gmail.com", "bookHaven123"),
                        EnableSsl = true
                    };
                    client.Send("bookHaven95@gmail.com", "3dsssgaming@gmail.com", "SITConnect Verification Code", "Your verification code is " + Code.code + ". It expires the moment you leave this site.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected void btn_Verify_click(object sender, EventArgs e)
        {
            string pwd = tb_pwd.Text.ToString().Trim();
            string userid = tb_userid.Text.ToString().Trim();
            if (lbl_verifycode.Text == Code.code.ToString())
            {
                Session["LoggedIn"] = tb_userid.Text.Trim();
                string guid = Guid.NewGuid().ToString();
                Session["AuthToken"] = guid;
                Response.Cookies.Add(new HttpCookie("AuthToken", guid));
                Session["UserID"] = userid;
                Response.Redirect("Success.aspx", false);
            }
            else
            {
                lb_error.Text = "Invalid Code";
            }
            
            lbl_gScore.Text = Code.code.ToString();
        }
        public bool ValidateCaptcha()
        {
            bool result = true;

            //When user submits the recaptcha form, the user gets a response POST parameter. 
            //captchaResponse consist of the user click pattern. Behaviour analytics! AI :) 
            string captchaResponse = Request.Form["g-recaptcha-response"];

            //To send a GET request to Google along with the response and Secret key.
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create
           (" https://www.google.com/recaptcha/api/siteverify?secret=SecretCode &response=" + captchaResponse);


            try
            {

                //Codes to receive the Response in JSON format from Google Server
                using (WebResponse wResponse = req.GetResponse())
                {
                    using (StreamReader readStream = new StreamReader(wResponse.GetResponseStream()))
                    {
                        //The response in JSON format
                        string jsonResponse = readStream.ReadToEnd();

                        //To show the JSON response string for learning purpose
                        lbl_gScore.Text = jsonResponse.ToString();

                        JavaScriptSerializer js = new JavaScriptSerializer();

                        //Create jsonObject to handle the response e.g success or Error
                        //Deserialize Json
                        MyObject jsonObject = js.Deserialize<MyObject>(jsonResponse);

                        //Convert the string "False" to bool false or "True" to bool true
                        result = Convert.ToBoolean(jsonObject.success);//

                    }
                }

                return result;
            }
            catch (WebException ex)
            {
                throw ex;
            }
        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            //if (ValidateCaptcha())
            //{
            //Response.Write("<script>window.alert('before getDBHash.')</script>");         
            string pwd = tb_pwd.Text.ToString().Trim();
            string userid = tb_userid.Text.ToString().Trim();

            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);

            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = pwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);

                    if (userHash.Equals(dbHash))
                    {
                        verifyTable.Visible = true;
                        lb_error.Text = "Enter Verification Code from Gmail";
                        btn_Submit.Text = "Resend Verification Code";
                        tb_userid.ReadOnly = true;
                        tb_pwd.ReadOnly = true;
                        twofac();
                    }
                    else
                    {
                        lb_error.Text = "Userid or password is not valid. Please try again.";
                    }
                }
                else
                {
                    lb_error.Text = "Userid or password is not valid. Please try again.";
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            //}
            /*else
            {
                lb_error.Text = "Validate captcha to prove that you are a human.";
            }*/

        }


        protected string getDBSalt(string userid)
        {

            string s = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PASSWORDSALT FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PASSWORDSALT"] != null)
                        {
                            if (reader["PASSWORDSALT"] != DBNull.Value)
                            {
                                s = reader["PASSWORDSALT"].ToString();
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return s;

        }

        protected string getDBHash(string userid)
        {

            string h = null;

            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash FROM Account WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        if (reader["PasswordHash"] != null)
                        {
                            if (reader["PasswordHash"] != DBNull.Value)
                            {
                                h = reader["PasswordHash"].ToString();
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { connection.Close(); }
            return h;
        }





        protected string decryptData(byte[] cipherText)
        {

            string decryptedString = null;
            //byte[] cipherText = Convert.FromBase64String(cipherString);

            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                ICryptoTransform decryptTransform = cipher.CreateDecryptor();

                //Decrypt
                //byte[] decryptedText = decryptTransform.TransformFinalBlock(cipherText, 0, cipherText.Length);
                //decryptedString = Encoding.UTF8.GetString(decryptedText);


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return decryptedString;
        }

        protected void HyperLink1_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePass.aspx", false);
        }

        protected void HyperLink2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Registration.aspx", false);
        }

    }
}