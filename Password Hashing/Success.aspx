<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Success.aspx.cs" Inherits="Password_Hashing.Success" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>
                <asp:Label ID="Label1" runat="server" Text="Home Page"></asp:Label>
                <br />
            </h2>
            <fieldset>
                <legend>Home Page</legend>
            <table class="style1">
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label7" runat="server" Text="First Name"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:Label ID="lbl_firstname" runat="server" Text="">&nbsp;</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label8" runat="server" Text="Last Name"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:Label ID="lbl_lastname" runat="server" Text="">&nbsp;</asp:Label>
                    </td>
                </tr>
                

                <tr>
                    <td class="style3">
                        <asp:Label ID="Label2" runat="server" Text="Email (UserID)"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:Label ID="lbl_userID" runat="server" Text="">&nbsp;</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style6">
                        <asp:Label ID="Label5" runat="server" Text="Date of Birth"></asp:Label>
                    </td>
                    <td class="style7">
                        <asp:Label ID="lbl_dob" runat="server" Text="">&nbsp;</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label6" runat="server" Text="Photo"></asp:Label>
                    </td>
                    <td class="style2">
            <asp:Image ID="Image1" runat="server" ImageUrl="" />                    </td>
                </tr>
                <tr>
                    <td class="style3">
                        <asp:Label ID="Label3" runat="server" Text="Date and Time Registered"></asp:Label>
                    </td>
                    <td class="style2">
                        <asp:Label ID="lbl_registered" runat="server" Text="">&nbsp;</asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="style4"></td>
                    <td class="style5">
        <asp:Button ID="btnLogOut2" runat ="server" Text="Logout" OnClick="LogoutMe" Visible="false" height ="48px" Width ="288px"/>
                    </td>
                </tr>
            </table>
                </fieldset>

        </div>
    </form>
</body>
</html>

