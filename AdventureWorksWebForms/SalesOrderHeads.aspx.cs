using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Reflection;

namespace AdventureWorksWebForms
{
    public partial class SalesOrderHeads : Page
    {
        private AdventureContext salesContext = new AdventureContext();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var salesOrders = new object();

                using (salesContext)
                {
                    salesOrders = (from order in salesContext.SalesOrderHeaders
                             join detail in salesContext.SalesOrderDetails on order.SalesOrderID equals detail.SalesOrderID
                             join product in salesContext.Products
                                          on detail.ProductID equals product.ProductID
                             join productSubCat in salesContext.ProductSubcategories
                                                on product.ProductSubcategoryID equals productSubCat.ProductSubcategoryID
                             where order.SalesOrderID == 44157 // salesOrderId
                                    || order.SalesOrderID == 44158
                                    || order.SalesOrderID == 44159
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

                if (salesOrders != null)
                {
                    BindGridView(salesOrders);
                    // extract order details...


                    gvSalesOrders.DataSource = salesOrders;
                    gvSalesOrders.DataBind();

                }
            }
        }

        private void BindGridView(object salesOrders)
        {
            var orders = salesOrders.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetField);

            foreach (var item in orders)
            {
                
            }
            
        }

        protected void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            string firstName = gvSalesOrders.SelectedRow.Cells[0].Text;
            string lastName = gvSalesOrders.SelectedRow.Cells[1].Text;
            string orderDescription = (gvSalesOrders.SelectedRow.FindControl("lblOrderDescription") as Label).Text;
            var test = gvSalesOrders.SelectedRow.FindControl("lblOrderDescription");

            DataTable dt = new DataTable();

            dt.Columns.AddRange(new DataColumn[2] { new DataColumn("OrderNumber", typeof(string)),
                                                    new DataColumn("Product", typeof(string)) });
            dt.Rows.Add(firstName, lastName, orderDescription);

            dvSalesOrderDetail.DataSource = dt;
            dvSalesOrderDetail.DataBind();
        }
    }
}