<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WizardClass.aspx.cs" Inherits="AdventureWorksWebForms.WizardClass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Wizard ID="PalgweForms" runat="server" OnFinishButtonClick="PalgweForms_FinishButtonClick"
        BackColor="#EFF3FB"
        Font-Names="Verdana"
        Font-Size="0.8em"
        BorderWidth="1px"
        BorderColor="#B5C7DE"
        Style="font-size: medium; font-family: Verdana;"
        OnActiveStepChanged="OnActiveStepChanged">
        <StepStyle ForeColor="#333333" Font-Size="0.8em" />
        <WizardSteps>
            <asp:WizardStep ID="PalgweOne" Title="Palgwe One" AllowReturn="false" runat="server">
                Welcome to the Wizard example.  This step's AllowReturn property is set 
                to false, so after you leave this step you will not be able to return to it.
            </asp:WizardStep>
            <asp:WizardStep ID="PalgweTwo" Title="Palgwe Two" runat="server">
            </asp:WizardStep>
            <asp:WizardStep ID="PalgweThree" Title="Palgwe Three" runat="server">
            </asp:WizardStep>
            <asp:WizardStep ID="PalgweFour" Title="Palgwe Four" runat="server">
            </asp:WizardStep>
            <asp:WizardStep ID="PalgweFive" Title="Palgwe Five" runat="server">
            </asp:WizardStep>
            <asp:WizardStep ID="PalgweSix" Title="Palgwe Six" runat="server">
            </asp:WizardStep>
            <asp:WizardStep ID="PalgweSeven" Title="Palgwe Seven" runat="server">
            </asp:WizardStep>
            <asp:WizardStep ID="PalgweEight" Title="Palgwe Eight" runat="server">
            </asp:WizardStep>
        </WizardSteps>
        <NavigationButtonStyle ForeColor="#284E98"
            Font-Names="Verdana"
            Font-Size="1.0em"
            BorderStyle="Solid"
            BorderWidth="1px"
            BorderColor="#507CD1"
            BackColor="White" />
        <HeaderStyle ForeColor="White"
            HorizontalAlign="Center"
            Font-Size="0.9em"
            Font-Bold="True"
            BackColor="#284E98"
            BorderStyle="Solid"
            BorderColor="#EFF3FB"
            BorderWidth="2px" />
        <SideBarStyle VerticalAlign="Top"
            HorizontalAlign="Center"
            Font-Size="0.8em"
            ForeColor="#000099"
            BackColor="#EFF3FB"
            Width="45px" />
        <HeaderTemplate>
            <b>Wizard Example</b>
        </HeaderTemplate>
    </asp:Wizard>
</asp:Content>
