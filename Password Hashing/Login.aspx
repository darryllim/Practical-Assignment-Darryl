<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Password_Hashing.Login" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src ="https://www.google.com/recaptcha/api.js?render=6LfdnmUeAAAAAGLuqxsGrr3evEHt3ChAdbQF65Nf"></script>
</head>
<body>
       <form id="form1" runat="server">
    <h2>
        <asp:Label ID="Label1" runat="server" Text="Login"></asp:Label>
   </h2>
           
            <fieldset>
                <legend>Login</legend>
        <table class="style1">
            <tr>
                <td class="style3">
        <asp:Label ID="Label2" runat="server" Text="User ID/Email"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_userid" runat="server" Height="16px" Width="280px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="style3">
        <asp:Label ID="Label3" runat="server" Text="Password"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="tb_pwd" runat="server" Height="16px" Width="281px"></asp:TextBox>
                </td>
            </tr>
                        <tr>
                <td class="style3">
                    <asp:LinkButton  ID="HyperLink1" runat="server" Font-Underline style ="color:blue;" Text="Change Password" OnClick="HyperLink1_Click"></asp:LinkButton>
                </td>
                <td class="style2">
    <asp:Button ID="btn_Submit" runat="server" Height="48px" 
        onclick="btn_Submit_Click" Text="Submit" Width="288px" />
                </td>
            </tr>
    </table>
    <asp:LinkButton  ID="LinkButton1" runat="server" Font-Underline style ="color:blue;" Text="Register Account" OnClick="HyperLink2_Click"></asp:LinkButton>
&nbsp;&nbsp;&nbsp;
    <br />
           
        <table class="style1" runat="server" visible=false id="verifyTable">
            <tr>
                <td class="style3">
        <asp:Label ID="Label4" runat="server" Text="Verification Code"></asp:Label>
                </td>
                <td class="style2">
                    <asp:TextBox ID="lbl_verifycode" runat="server" Height="16px" Width="280px" TextMode="Number" min="100000" max="999999"></asp:TextBox>
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
           <br />
        <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
        <asp:Label ID="lb_error" runat="server" style ="color:red;"></asp:Label>
           <br />
            <asp:Label ID="lbl_gScore" runat="server"></asp:Label>
           <br />
           <br />
        <br />
        <br />
   
    <div>
    
    </div>
    </form>
</body>
</html>
