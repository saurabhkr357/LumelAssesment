using CsvHelper.Configuration;
using LumelAssesment.Models;

namespace LumelAssessment.Models
{
    public class CsvRecordMap : ClassMap<CsvRecord>
    {
        public CsvRecordMap()
        {
            Map(m => m.OrderID).Name("Order ID");
            Map(m => m.ProductID).Name("Product ID");
            Map(m => m.CustomerID).Name("Customer ID");
            Map(m => m.ProductName).Name("Product Name");
            Map(m => m.Category).Name("Category");
            Map(m => m.Region).Name("Region");
            Map(m => m.DateOfSale).Name("Date of Sale");
            Map(m => m.QuantitySold).Name("Quantity Sold");
            Map(m => m.UnitPrice).Name("Unit Price");
            Map(m => m.Discount).Name("Discount");
            Map(m => m.ShippingCost).Name("Shipping Cost");
            Map(m => m.PaymentMethod).Name("Payment Method");
            Map(m => m.CustomerName).Name("Customer Name");
            Map(m => m.CustomerEmail).Name("Customer Email");
            Map(m => m.CustomerAddress).Name("Customer Address");
        }
    }
}