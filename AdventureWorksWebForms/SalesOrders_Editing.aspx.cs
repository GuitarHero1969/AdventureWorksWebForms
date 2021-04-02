using System;
using System.Collections.Generic;
using System.Linq;
// using System.Web;
using System.Web.UI;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI.WebControls;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AdventureWorksWebForms.Helpers;
// using AdventureWorksWebForms.Models;

namespace AdventureWorksWebForms
{
    public partial class SalesOrders_Editing : Page
    {
        private AdventureContext salesContext = new AdventureContext();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string rawId = Request["salesOrderId"];
            long salesOrderId = 0;

            if (!IsPostBack)
            {
                var query = new object();

                if (!string.IsNullOrEmpty(rawId) && long.TryParse(rawId, out salesOrderId))
                {
                    if (salesOrderId != 0)
                    {
                        hiddenSalesOrderId.Value = salesOrderId.ToString();

                        using (var sc = new AdventureContext())
                        {
                            query = (from order in sc.SalesOrderHeaders
                                     join detail in sc.SalesOrderDetails on order.SalesOrderID equals detail.SalesOrderID
                                     join product in sc.Products
                                                  on detail.ProductID equals product.ProductID
                                     join productSubCat in sc.ProductSubcategories
                                                        on product.ProductSubcategoryID equals productSubCat.ProductSubcategoryID
                                     where order.SalesOrderID == 44157 // salesOrderId
                                     select new
                                     {
                                         ContactLastName = order.Customer.Person.LastName,
                                         ContactFirstName = order.Customer.Person.FirstName,
                                         StreetAddress = order.Address.AddressLine1,
                                         OrderNumber = order.SalesOrderNumber,
                                         Total = order.TotalDue,
                                         Product = order.SalesOrderDetails
                                     }).ToList();
                        }


                    }
                }
            }
        }

        /// <summary>
        /// Creates an existing sales order
        /// </summary>
        /// <param name="indx"></param>
        /// <param name="salesOrderDetail"></param>
        private void BuildSalesOrderLineHtml(int indx, SalesOrderDetail salesOrderDetail)
        {
            salesOrderLines.Text +=
            "<div class=\"row plcLine\" data-indx=" + indx + " data-salesorderlineid=" + salesOrderDetail.SalesOrderDetailID + ">" +
            "<div class=\"col-4\">" +
            "<div class=\"row\">" +
            "<div class=\"col\">" +
            "<select class=\"form-control clsCategories\" data-control=\"categories\" onchange=\"fieldValueChanged('" + indx + "', this)\">";

            var categories = salesContext.ProductCategories.ToList();

            foreach (var item in categories)
            {
                salesOrderLines.Text += "<option value=" + item.ProductCategoryID + "";

                if (item.ProductCategoryID == salesOrderDetail.ProductID)
                    salesOrderLines.Text += " selected";

                salesOrderLines.Text += ">" + item.Name + "</option>";
            }

            salesOrderLines.Text += "</select></div><div class=\"col\">" +
            "<select class=\"form-control clsSubcategories\" data-control=\"subCategories\" onchange=\"fieldValueChanged('" + indx + "', this)\">";

                // TODO get subcategories
            
             salesOrderLines.Text += "</select></div></div></div>" +
            "<div class=\"col\">" +
            "<div class=\"row\"><div class=\"col\">" +
            "<select class=\"form-control clsProducts\" data-control=\"products\" onchange=\"fieldValueChanged('" + indx + "', this)\"></select></div>" +
            "<div class=\"col\">" +
            "<input type=\"text\" class=\"form-control clsPrice\" data-control=\"price\" value=\"0.00\" onblur=\"fieldValueChanged('" + indx + "', this)\" /></div>" +
            "<div class=\"col-4\"><div class=\"row\">" +
            "<div class=\"col\">" +
            "<input type=\"text\" class=\"form-control clsQuantity\" data-control=\"quantity\" value=\"1\" onblur=\"fieldValueChanged('" + indx + "', this)\" /></div>" +
            "<div class=\"col\"><input type=\"text\" class=\"form-control clsDiscount\" data-control=\"discount\" value=\"0.00\" onblur=\"fieldValueChanged('" + indx + "', this)\" /></div>" +
            "<div class=\"col\">" +
            "<select class=\"form-control clsTaxCode\" data-control=\"tax\" onchange=\"fieldValueChanged('" + indx + "', this)\"></select></div></div></div>" +
            "<div class=\"col\"><div class=\"row\"><div class=\"col-8\">" +
            "<input type=\"text\" class=\"form-control clsTotal text-right\" readonly=\"true\" data-control=\"total\" value=\"0.00\" onchange=\"fieldValueChanged('" + indx + "', this)\" /></div>" +
            "<div class=\"col\">" +
            "<input type=\"button\" class=\"btn btn-sm btn-danger btn-block\" value=\"X\" data-toggle=\"tooltip\" title=\"Remove  Item\" onclick=\"removeSalesOrderLine('" + indx + "')\" />" +
            "</div></div></div></div></div></div>" +
            "<div data-loadspinner=" + indx + "><i class=\"fas fa-spinner fa-spin\"></i></div>";
        }

        #region Web Methods
        // Web Services are an integral part of the .NET framework that provide a cross-platform solution for
        // exchanging data between distributed systems.

        // Although Web Services are normally used to allow different operating systems, object models and programming languages
        // to send and receive data, they can also be used to dynamically inject data into an ASP.NET AJAX page or send data
        // from a page to a back-end system.

        // All of this can be done without resorting to postback operations.

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public static List<SalesOrderProductCategory> GetProductCategories()
        {
            using (var sc = new AdventureContext())
            {
                return (from prodCat in sc.ProductCategories
                        select new SalesOrderProductCategory
                        {
                            ProductCategoryID = prodCat.ProductCategoryID,
                            Name = prodCat.Name
                        }).ToList();
            }
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public static List<SalesOrderProductSubcategory> GetProductSubcategories(int productCategoryID)
        {
            using (var sc = new AdventureContext())
            {
                return (from psc in sc.ProductSubcategories
                        where psc.ProductCategoryID == productCategoryID
                        select new SalesOrderProductSubcategory
                        {
                            ProductSubcategoryID = psc.ProductSubcategoryID,
                            Name = psc.Name
                        }).ToList();
            }

        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public static List<SalesOrderProduct> GetProducts(int productSubcategoryID)
        {
            using (var sc = new AdventureContext())
            {
                return (from p in sc.Products
                        where p.ProductSubcategoryID == productSubcategoryID
                        select new SalesOrderProduct
                        {
                            ProductID = p.ProductID,
                            Name = p.Name,
                            ListPrice = p.ListPrice
                        }).ToList();
            }
        }

        [WebMethod]
        public static bool UpdateSalesOrder(string jsonisedVal)
        {
            // TODO: retrieve a sales order object here...          
            var deserialisedSalesOrder = JsonConvert.DeserializeObject(jsonisedVal) as JObject;
            int salesOrderId = 0;

            // assign to relevant model property here...
            var fieldVal = JSONParser.GetValue("deserialisedSalesOrder", "salesorderlineid");

            //foreach (JProperty jsonObj in deserialisedSalesOrder.Children())
            //{

            //}

            return true;
        }
        #endregion
    }

    public class SalesOrderProductCategory
    {
        public int ProductCategoryID { get; set; }
        public string Name { get; set; }
    }

    public class SalesOrderProductSubcategory
    {
        public int ProductSubcategoryID { get; set; }
        public string Name { get; set; }
    }

    public class SalesOrderProduct
    {
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal ListPrice { get; set; }
    }
}
