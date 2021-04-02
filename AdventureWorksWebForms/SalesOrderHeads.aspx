<%@ Page Title="Sales Orders" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesOrderHeads.aspx.cs" Inherits="AdventureWorksWebForms.SalesOrderHeads" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%: Title %>.</h2>
    <p>
        Demonstrates the asp:GridView and asp:DetailsView showing a very common pattern: the master: detail view
    </p>
    <p>
        Use this area to provide additional information.
    </p>
    <br />
    <div>
        <asp:GridView ID="gvSalesOrders" runat="server" AutoGenerateColumns="false" OnSelectedIndexChanged="OnSelectedIndexChanged">
            <Columns>
                <asp:BoundField DataField="ContactFirstName" HeaderText="First Name" />
                <asp:BoundField DataField="ContactLastName" HeaderText="Last Name" />
                <asp:BoundField DataField="StreetAddress" HeaderText="Address" />
                <asp:BoundField DataField="OrderNumber" HeaderText="Sales Order" />
                <asp:BoundField DataField="Total" HeaderText="Order Total" DataFormatString="{0:c}" />
                <asp:TemplateField HeaderText="Order Detail" Visible="false">
                    <ItemTemplate>
                        <asp:Label ID="lblOrderDescription" runat="server" Text='<%# Eval("Product") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ButtonField Text="Order Detail" CommandName="Select" />
            </Columns>
        </asp:GridView>
        <br />
        <u>Selected Row</u>
        <br />
        <asp:DetailsView ID="dvSalesOrderDetail" runat="server" AutoGenerateRows="false">
            <Fields>
                <asp:BoundField DataField="ContactFirstName" HeaderText="First Name" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="ContactLastName" HeaderText="Last Name" HeaderStyle-Font-Bold="true" />
                <asp:BoundField DataField="Product" HeaderText="Product" HeaderStyle-Font-Bold="true" />
            </Fields>
        </asp:DetailsView>
    </div>

</asp:Content>
