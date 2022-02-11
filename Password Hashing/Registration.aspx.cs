using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Password_Hashing
{
    static class Img
    {
        public static string img;
    }
    public partial class Registration : System.Web.UI.Page
    {

        string MYDBConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MYDBConnection"].ConnectionString;
        static string finalHash;
        static string salt;

        byte[] Key;
        byte[] IV;

        static string line = "\r";

        //static string isDebug = ConfigurationManager.AppSettings["isDebug"].ToString();


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Submit_Click(object sender, EventArgs e)
        {
            string pwd = tb_pwd.Text.ToString().Trim(); ;

            //Generate random "salt" 
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] saltByte = new byte[8];

            //Fills array of bytes with a cryptographically strong sequence of random values.
            rng.GetBytes(saltByte);
            salt = Convert.ToBase64String(saltByte);

            SHA512Managed hashing = new SHA512Managed();

            string pwdWithSalt = pwd + salt;
            byte[] plainHash = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwd));
            byte[] hashWithSalt = hashing.ComputeHash(Encoding.UTF8.GetBytes(pwdWithSalt));

            finalHash = Convert.ToBase64String(hashWithSalt);

            lb_error1.Text = "Salt:" + salt;
            lb_error2.Text = "Hash with salt:" + finalHash;

            RijndaelManaged cipher = new RijndaelManaged();
            cipher.GenerateKey();
            Key = cipher.Key;
            IV = cipher.IV;


            createAccount();

            if (Response.IsClientConnected)
            {
                Response.Redirect("Login.aspx", false);
            }
            else
            {
                Response.End();
            }
        }


        protected void createAccount()
        {

            try
            {
                using (SqlConnection con = new SqlConnection(MYDBConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Email, @FirstName, @LastName,@PasswordHash,@PasswordSalt,@PasswordSalt2,@PasswordHash2,@CardName,@CardNum,@CardExp,@CardCVV,@DateTimeRegistered,@MobileVerified,@EmailVerified,@Photo,@DateOfBirth,@IV,@Key)"))
                    //using (SqlCommand cmd = new SqlCommand("INSERT INTO Account VALUES(@Email, @Mobile,@Nric,@PasswordHash,@PasswordSalt,@DateTimeRegistered,@MobileVerified,@EmailVerified)"))
                    {
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Email", tb_userid.Text.Trim());
                            cmd.Parameters.AddWithValue("@FirstName", tb_firstname.Text.Trim());
                            cmd.Parameters.AddWithValue("@LastName", tb_lastname.Text.Trim());
                            cmd.Parameters.AddWithValue("@PasswordHash", finalHash);
                            cmd.Parameters.AddWithValue("@PasswordSalt", salt);
                            cmd.Parameters.AddWithValue("@PasswordSalt2", salt + " , ");
                            cmd.Parameters.AddWithValue("@PasswordHash2", finalHash+ " , ");
                            cmd.Parameters.AddWithValue("@CardName", Convert.ToBase64String(encryptData(tb_cardname.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CardNum", Convert.ToBase64String(encryptData(tb_cardnum.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CardExp", Convert.ToBase64String(encryptData(tb_cardexp.Text.Trim())));
                            cmd.Parameters.AddWithValue("@CardCVV", Convert.ToBase64String(encryptData(tb_cardcvv.Text.Trim())));
                            cmd.Parameters.AddWithValue("@DateTimeRegistered", DateTime.Now);
                            cmd.Parameters.AddWithValue("@MobileVerified", DBNull.Value);
                            cmd.Parameters.AddWithValue("@EmailVerified", DBNull.Value);
                            cmd.Parameters.AddWithValue("@Photo", Img.img);
                            cmd.Parameters.AddWithValue("@DateOfBirth", tb_dob.Text.Trim());
                            cmd.Parameters.AddWithValue("@IV", Convert.ToBase64String(IV));
                            cmd.Parameters.AddWithValue("@Key", Convert.ToBase64String(Key));
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

        protected byte[] encryptData(string data)
        {
            byte[] cipherText = null;
            try
            {
                RijndaelManaged cipher = new RijndaelManaged();
                cipher.IV = IV;
                cipher.Key = Key;
                ICryptoTransform encryptTransform = cipher.CreateEncryptor();
                //ICryptoTransform decryptTransform = cipher.CreateDecryptor();
                byte[] plainText = Encoding.UTF8.GetBytes(data);
                cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);


                //Encrypt
                //cipherText = encryptTransform.TransformFinalBlock(plainText, 0, plainText.Length);
                //cipherString = Convert.ToBase64String(cipherText);
                //Console.WriteLine("Encrypted Text: " + cipherString);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

            finally { }
            return cipherText;
        }

        protected void HyperLink2_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
            protected void Butupload_Click(object sender, EventArgs e)
        {
            string filepath = Path.GetExtension(FileUpload1.FileName);
            if (filepath !=".jpg" && filepath!=".gif" && filepath!=".png")
            {
                LabMsg.ForeColor = Color.Red;
                LabMsg.Text = "Only .jpg .gif & .png file allowed";
            }
            else
            {
                foreach (HttpPostedFile postedFile in FileUpload1.PostedFiles)
                {
                    string filename = Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(Server.MapPath("~/images/") + filename);
                    Image1.ImageUrl = "~/images/" + filename;
                    Img.img = "~/images/" + filename;
                }
                LabMsg.ForeColor = Color.Green;
                LabMsg.Text = "";


            }
        }
    }

}