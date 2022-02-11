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
    public partial class ChangePass : System.Web.UI.Page
    {
        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;
        byte[] Key;
        byte[] IV;
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public void twofac()
        {
            Random rnd = new Random();
            Code.code = rnd.Next(100000, 999999);
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    var client = new SmtpClient("smtp.gmail.com", 587)
                    {
                        Credentials = new NetworkCredential("bookhaven95@gmail.com", "bookHaven123"),
                        EnableSsl = true
                    };
                    client.Send("bookHaven95@gmail.com", tb_userid.Text.ToString().Trim(), "SITConnect Verification Code", "Your verification code is " + Code.code + ". It expires the moment you leave this site.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected void updateAccount()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPDATE Account SET PasswordHash = @PasswordHash, PasswordSalt = @PasswordSalt, PasswordSalt2 = @PasswordSalt2, PasswordHash2 = @PasswordHash2 WHERE Email = @Email"))
                    //using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Email, @Mobile,@Nric,@PasswordHash,@PasswordSalt,@DateTimeRegistered,@MobileVerified,@EmailVerified)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            string userid = tb_userid.Text.ToString().Trim();
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email", tb_userid.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@PasswordSalt2", (string.Join(" , ", (string.Join(" , ", getSaltList(userid))+" , "+ salt + " , ").Split(new string[] { " , " }, StringSplitOptions.None))) + " , ");
                            cmd.Parameters.AddWithValue("@PasswordHash2", (string.Join(" , ", (string.Join(" , ", getHashList(userid)) + " , " + finalHash + " , ").Split(new string[] { " , " }, StringSplitOptions.None))) + " , ");
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        protected void btn_change_onClick(object sender, EventArgs e)
        {
            //if (ValidateCaptcha())
            //{
            //Response.Write("<script>window.alert('before getDBHash.')</script>");         
            string pwd = tb_pwd.Text.ToString().Trim();
            string newpwd = tb_newpwd.Text.ToString().Trim();
            string userid = tb_userid.Text.ToString().Trim();

            SHA512Managed hashing = new SHA512Managed();
            string dbHash = getDBHash(userid);
            string dbSalt = getDBSalt(userid);
            List<string> newpwdsalt = new List<string>();
            List<string> salts = getSaltList(userid);
            foreach (var salt in salts)
            {
                string newpwdWithSalt = newpwd + salt;
                byte[] newhashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(newpwdWithSalt));
                string newuserHash = Convert.ToBase64String(newhashWithSalt);
                newpwdsalt.Add(newuserHash);
            }
            try
            {
                if (dbSalt != null && dbSalt.Length > 0 && dbHash != null && dbHash.Length > 0)
                {
                    string pwdWithSalt = pwd + dbSalt;
                    byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                    string userHash = Convert.ToBase64String(hashWithSalt);
                    List<string> Hlist= new List<string>();
                    if (getHashList(userid).Count>2)
                    {
                        Hlist.Add(getHashList(userid)[getHashList(userid).Count - 2]);
                        Hlist.Add(getHashList(userid)[getHashList(userid).Count - 1]);
                    }
                    else
                    {
                        Hlist = getHashList(userid);
                    }

                    if (userHash.Equals(dbHash))
                    {
                        foreach (var pass in newpwdsalt)
                        {
                            if (Hlist.Contains(pass))
                            {
                                lb_error.Text = "Password cannot be the same as your previous two";
                                break;
                            }
                            else
                            {
                                verifyTable.Visible = true;
                                lb_error.Text = "Enter Verification Code from Gmail";
                                btn_change.Text = "Resend Verification Code";
                                tb_cfmpwd.ReadOnly = true;
                                tb_newpwd.ReadOnly = true;
                                tb_pwd.ReadOnly = true;
                                tb_userid.ReadOnly = true;
                                twofac();
                            }
                        }
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

        protected List<string> getHashList(string userid)
        {
            List<string> s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordHash2 FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);

            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordHash2"] != null)
                        {
                            if (reader["PasswordHash2"] != DBNull.Value)
                            {
                                s = new List<string>(reader["PasswordHash2"].ToString().Split(new string[] { " , " }, StringSplitOptions.None));
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

        protected List<string> getSaltList(string userid)
        {
            List<string> s = null;
            SqlConnection connection = new SqlConnection(MYDBConnectionString);
            string sql = "select PasswordSalt2 FROM ACCOUNT WHERE Email=@USERID";
            SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@USERID", userid);
            try
            {
                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["PasswordSalt2"] != null)
                        {
                            if (reader["PasswordSalt2"] != DBNull.Value)
                            {
                                s = new List<string>(reader["PasswordSalt2"].ToString().Split(new string[] { " , " }, StringSplitOptions.None));
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
        protected void btn_Verify_click(object sender, EventArgs e)
        {
            if (lbl_verifycode.Text == Code.code.ToString())
            {

                string pwd = tb_newpwd.Text.ToString().Trim();
                RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
                byte[] saltByte = new byte[8];
                rng.GetBytes(saltByte);
                salt = Convert.ToBase64String(saltByte);
                SHA512Managed hashing = new SHA512Managed();
                string pwdWithSalt = pwd + salt;
                byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
                byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));
                finalHash = Convert.ToBase64String(hashWithSalt);
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.GenerateKey();
                Key = cipher.Key;
                IV = cipher.IV;
                updateAccount();
                Response.Redirect("Login.aspx", false);
            }
            else
            {
                lb_error.Text = "Invalid Code";
            }
            
            lbl_gScore.Text = Code.code.ToString();
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

        protected void HyperLink1_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}