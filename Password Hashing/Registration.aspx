<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="Password_Hashing.Registration" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function validate() {
            document.getElementById("btn_Submit").disabled = true;
            var str = document.getElementById('<%=tb_pwd.ClientID %>').value;
            if (str.length < 12) {
                document.getElementById('lbl_pwdchecker').innerHTML = "Password Lenght must be at Least 12 Characters";
                document.getElementById('lbl_pwdchecker').style.color = "Red";
                return ("too_short");
            }
            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 number";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_number");
            }
            else if (str.toUpperCase() == str) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 lowercase";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_lower")
            }
            else if (str.toLowerCase() == str) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 uppercase";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_upper")            }
            else if (str.search(/[^A-Za-z0-9]/) == -1) {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 special character";
                document.getElementById("lbl_pwdchecker").style.color = "Red";
                return ("no_specialchar")
            }
            document.getElementById('lbl_pwdchecker').innerHTML = "Excellent!";
            document.getElementById('lbl_pwdchecker').style.color = "Blue";
            document.getElementById("btn_Submit").disabled = false;

        }
        function cfmpass() {
            document.getElementById("btn_Submit").disabled = true;
            var pass = document.getElementById('<%=tb_pwd.ClientID %>').value;
            var confirm = document.getElementById('<%=tb_cfpwd.ClientID %>').value;
            if (pass != confirm) {
                document.getElementById("lbl_cfmpass").innerHTML = "Passwords does not match";
                document.getElementById("lbl_cfmpass").style.color = "Red";
                return ("no_match")
            } else {
                document.getElementById("lbl_cfmpass").innerHTML = "";
                document.getElementById("btn_Submit").disabled = false;
                return("match")
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>
                <asp:Label ID="Label1" runat="server" Text="Account Registration"></asp:Label>
                <br />
            </h2>
            <fieldset>
                <legend>Register</legend>
            <table class="style1">
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label7" runat="server" Text="First Name"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_firstname" runat="server" Height="36px" Width="280px" required></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label8" runat="server" Text="Last Name"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_lastname" runat="server" Height="36px" Width="280px" required></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label9" runat="server" Text="Credit Card Name"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_cardname" runat="server" Height="36px" Width="280px" required></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label10" runat="server" Text="Credit Card Number"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_cardnum" runat="server" Height="36px" Width="280px" TextMode="Number" min="1000000000000000" max="9999999999999999" required></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label11" runat="server" Text="Credit Card Exp date"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_cardexp" runat="server" Height="36px" Width="280px" TextMode="month" required></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label12" runat="server" Text="Credit Card CVV"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_cardcvv" runat="server" Height="36px" Width="280px" required TextMode="Number" min="100" max="999"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td class="style3">
                        <asp:Label ID="Label2" runat="server" Text="Email (UserID)"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_userid" runat="server" Height="36px" Width="280px" TextMode="Email" required></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_pwd" runat="server" TextMode="Password" Height="32px" Width="281px" onkeyup="javascript:validate()" required></asp:TextBox>
                        <asp:Label ID="lbl_pwdchecker" runat="server" Text=" "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label4" runat="server" Text="Confirm Password"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="tb_cfpwd" runat="server" TextMode="Password" Height="32px" Width="281px" onkeyup="javascript:cfmpass()" required></asp:TextBox>
                        <asp:Label ID="lbl_cfmpass" runat="server" Text=" "></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        <asp:Label ID="Label5" runat="server" Text="Date of Birth"></asp:Label>
                    </td>
                    <td class="style7">
                        <asp:TextBox ID="tb_dob" runat="server" Height="32px" Width="281px" required TextMode="Date"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label6" runat="server" Text="Photo"></asp:Label>
                    </td>
                    <td class="style2">                    
                        <asp:FileUpload ID="FileUpload1" runat="server" Width="211px"></asp:FileUpload>  
                    </td>
                    </tr>
                <tr>
                    <td></td><td>
                        <asp:Label ID="LabMsg" runat="server"></asp:Label>  
                        <asp:Button ID="Butupload" runat="server" Text="Upload" OnClick="Butupload_Click" />  
            <asp:Image ID="Image1" runat="server" ImageUrl="" />
                        </td>
                </tr>
                <tr>
                    <td class="style4"></td>
                    <td class="style5">
                        <asp:Button ID="btn_Submit" runat="server" Height="48px"
                            OnClick="btn_Submit_Click" Text="Submit" Width="288px" />
                    </td>
                </tr>
            </table>
                </fieldset>
                <asp:LinkButton  ID="LinkButton1" runat="server" Font-Underline style ="color:blue;" Text="<- Back to Login" OnClick="HyperLink2_Click"></asp:LinkButton>
            &nbsp;<br />
            <asp:Label ID="lb_error1" runat="server"></asp:Label>
            <br />
            <asp:Label ID="lb_error2" runat="server"></asp:Label>
            <br />
            <br />

        </div>
    </form>
</body>
</html>
