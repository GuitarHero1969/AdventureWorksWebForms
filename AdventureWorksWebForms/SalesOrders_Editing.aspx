<%@ Page Title="Add Sales Order" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesOrders_Editing.aspx.cs" Inherits="AdventureWorksWebForms.SalesOrders_Editing" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <h2><%: Title %>.</h2>
    <br />
    <div id="salesOrderLinesContainer" class="row bg-secondary text-white p-2">
        <div class="col">
            <strong>Category</strong>
        </div>
        <div class="col">
            <strong>Sub-Category</strong>
        </div>
        <div class="col">
            <strong>Item</strong>
        </div>
        <div class="col">
            <strong>List Price</strong>
        </div>
        <div class="col">
            <strong>Qty</strong>
        </div>
        <div class="col">
            <strong>Discount</strong>
        </div>
        <div class="col">
            <strong>Tax</strong>
        </div>
        <div class="col-8">
            <strong>Total</strong>
        </div>
        <div class="col-2">
            <button id="btnSalesOrderLine" type="button" class="btn btn-secondary" data-toggle="tooltip" title="Add Item">
                <i class="fas fa-plus-circle"></i>
            </button>
        </div>
        <asp:Literal ID="salesOrderLines" runat="server"></asp:Literal>
    </div>
    <div class="container container-no-left-padding">
        <div class="row">
            <div class="col-2 mt-2 p-2 bg-secondary text-white">
                <div class="panel-heading">Order Total</div>
                <hr />
                <div class="col">
                    Shipping
                    <input id="txtShipping" class="form-control" data-control="shipping" value="0.00" />
                </div>
                <div class="col">
                    Discount
                    <input id="txtDiscount" class="form-control" readonly="readonly" value="0.00" />
                </div>
                <div class="col">
                    VAT Total
                    <input id="txtVATTotal" class="form-control" readonly="readonly" value="0.00" />
                </div>
                <div class="col">
                    Total
                    <input id="txtOrderTotal" class="form-control" />
                </div>
            </div>
        </div>
    </div>
    <asp:HiddenField ID="hiddenSalesOrderId" runat="server" ClientIDMode="Static" />
    <script type="text/javascript" src="/Scripts/WebForms/SalesOrders.js"></script>

</asp:Content>
