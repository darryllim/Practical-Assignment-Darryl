<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePass.aspx.cs" Inherits="Password_Hashing.ChangePass" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script type="text/javascript">
        function validate() {
            document.getElementById("btn_change").disabled = true;
            var str = document.getElementById('<%=tb_newpwd.ClientID %>').value;
            var old = document.getElementById('<%=tb_pwd.ClientID %>').value;
            if (str != old) {
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
                    return ("no_upper")
                }
                else if (str.search(/[^A-Za-z0-9]/) == -1) {
                    document.getElementById("lbl_pwdchecker").innerHTML = "Password require at least 1 special character";
                    document.getElementById("lbl_pwdchecker").style.color = "Red";
                    return ("no_specialchar")
                }
                document.getElementById('lbl_pwdchecker').innerHTML = "Excellent!";
                document.getElementById('lbl_pwdchecker').style.color = "Blue";
                document.getElementById("btn_Submit").disabled = false;

            } else {
                document.getElementById("lbl_pwdchecker").innerHTML = "Password cannot be same as old";
            }

        }
        function cfmpass() {
            document.getElementById("btn_change").disabled = true;
            var pass = document.getElementById('<%=tb_newpwd.ClientID %>').value;
            var confirm = document.getElementById('<%=tb_cfmpwd.ClientID %>').value;
            var old = document.getElementById('<%=tb_pwd.ClientID %>').value;
            if (old != pass) {
                if (pass != confirm) {
                    document.getElementById("lbl_cfmpass").innerHTML = "Passwords does not match";
                    document.getElementById("lbl_cfmpass").style.color = "Red";
                    return ("no_match")
                } else {
                    document.getElementById("lbl_cfmpass").innerHTML = "";
                    document.getElementById("btn_change").disabled = false;
                    return ("match")
                }
            }
        }
    </script>
</head>
<body>
       <form id="form1" runat="server">
    <h2>
        Change Password</h2>
           
            <fieldset>
                <legend>Change Pass</legend>
        <table class="style1">
            <tr>
                <td class="style3">
        <asp:Label ID="Label2" runat="server" Text="User ID/Email"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_userid" runat="server" Height="16px" Width="280px">test@WADS.COM</asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3">
        <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_pwd" runat="server" Height="16px" Width="281px">Password1!12</asp:TextBox>
        <asp:Label ID="lb_error" runat="server" style ="color:red;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style3">
        <asp:Label ID="Label1" runat="server" Text="New Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_newpwd" runat="server" Height="16px" Width="281px" onkeyup="javascript:validate()" required></asp:TextBox>
                        <asp:Label ID="lbl_pwdchecker" runat="server" Text=" " style ="color:red;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="style3">
        <asp:Label ID="Label6" runat="server" Text="Confirm Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_cfmpwd" runat="server" Height="16px" Width="281px" onkeyup="javascript:cfmpass()" required></asp:TextBox>
                        <asp:Label ID="lbl_cfmpass" runat="server" Text=" "></asp:Label>
                </td>
            </tr>
                        <tr>
                <td class="style3">
                </td>
                <td class="style2">
                    <asp:Button ID="btn_change" runat="server" Text="Change" OnClick="btn_change_onClick" Width="288" height="48"/>

                </td>
            </tr>
    </table>
&nbsp;&nbsp;&nbsp;
    <br />
           
        <table class="style1" runat="server" visible=false id="verifyTable">
            <tr>
                <td class="style3">
        <asp:Label ID="Label4" runat="server" Text="Verification Code"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="lbl_verifycode" runat="server" Height="16px" Width="280px" TextMode="Number" min="100000" max="999999"></asp:TextBox>
            <asp:Label ID="lbl_gScore" style ="color:red;" runat="server"></asp:Label>
                </td>
            </tr>
                        <tr>
                <td class="style3">
       
                </td>
                <td class="style2">
    <asp:Button ID="verify" runat="server" Height="48px" 
        onclick="btn_Verify_click" Text="Verify" Width="288px" />
                </td>
            </tr>
    </table>
                </fieldset>
                               <asp:LinkButton  ID="HyperLink1" runat="server" Font-Underline style ="color:blue;" Text="<- Back to Login" OnClick="HyperLink1_Click"></asp:LinkButton>

           <br />
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
           <br />
           <br />
           <br />
        <br />
        <br />
   
    <div>
    
    </div>
    </form>
</body>
</html>
