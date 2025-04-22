using CsvHelper;
using CsvHelper.Configuration;
using LumelAssesment.Data;
using LumelAssesment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LumelAssessment.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DataController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DataController> _logger;

        public DataController(AppDbContext context, ILogger<DataController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshData()
        {
            try
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "New-Text-Document.csv");

                using (var reader = new StreamReader(filePath))
                {
                    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        Delimiter = ",",
                        HeaderValidated = null, // Disable header validation for debugging
                        MissingFieldFound = null // Ignore missing fields
                    };

                    using (var csv = new CsvReader(reader, config))
                    {
                        csv.Context.RegisterClassMap<Models.CsvRecordMap>();
                        var records = csv.GetRecords<CsvRecord>().ToList();

                        foreach (var record in records)
                        {
                            // Handle Customers
                            var customer = await _context.Customers.FindAsync(record.CustomerID);
                            if (customer == null)
                            {
                                customer = new Customer
                                {
                                    CustomerID = record.CustomerID,
                                    Name = record.CustomerName,
                                    Email = record.CustomerEmail,
                                    Address = record.CustomerAddress
                                };
                                _context.Customers.Add(customer);
                            }

                            // Handle Categories
                            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == record.Category);
                            if (category == null)
                            {
                                category = new Category { Name = record.Category };
                                _context.Categories.Add(category);
                                await _context.SaveChangesAsync();
                            }

                            // Handle Products
                            var product = await _context.Products.FindAsync(record.ProductID);
                            if (product == null)
                            {
                                product = new Product
                                {
                                    ProductID = record.ProductID,
                                    Name = record.ProductName,
                                    UnitPrice = record.UnitPrice,
                                    CategoryID = category.CategoryID
                                };
                                _context.Products.Add(product);
                            }

                            // Handle Regions
                            var region = await _context.Regions.FirstOrDefaultAsync(r => r.Name == record.Region);
                            if (region == null)
                            {
                                region = new Region { Name = record.Region };
                                _context.Regions.Add(region);
                                await _context.SaveChangesAsync();
                            }

                            // Handle Payment Methods
                            var paymentMethod = await _context.PaymentMethods.FirstOrDefaultAsync(pm => pm.MethodName == record.PaymentMethod);
                            if (paymentMethod == null)
                            {
                                paymentMethod = new PaymentMethod { MethodName = record.PaymentMethod };
                                _context.PaymentMethods.Add(paymentMethod);
                                await _context.SaveChangesAsync();
                            }

                            // Handle Orders
                            var order = await _context.Orders.FindAsync(record.OrderID);
                            if (order == null)
                            {
                                order = new Order
                                {
                                    OrderID = record.OrderID,
                                    CustomerID = record.CustomerID,
                                    DateOfSale = record.DateOfSale,
                                    RegionID = region.RegionID,
                                    PaymentMethodID = paymentMethod.PaymentMethodID,
                                    ShippingCost = record.ShippingCost
                                };
                                _context.Orders.Add(order);
                            }

                            // Handle Order Details
                            var orderDetail = new OrderDetail
                            {
                                OrderID = record.OrderID,
                                ProductID = record.ProductID,
                                QuantitySold = record.QuantitySold,
                                Discount = record.Discount
                            };
                            _context.OrderDetails.Add(orderDetail);
                        }

                        await _context.SaveChangesAsync();
                    }
                }

                _logger.LogInformation("Data refresh completed at: {Time}", DateTime.UtcNow);
                return Ok(new { message = "Data refreshed successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Refresh failed");
                return StatusCode(500, "Error occurred during refresh");
            }
        }
        [HttpGet("revenue/total")]
        public async Task<IActionResult> GetTotalRevenue(DateTime startDate, DateTime endDate)
        {
            var totalRevenue = await _context.OrderDetails
                .Where(od => od.Order.DateOfSale >= startDate && od.Order.DateOfSale <= endDate)
                .SumAsync(od => od.QuantitySold * od.Product.UnitPrice * (1 - od.Discount));

            return Ok(new { totalRevenue });
        }
    }
}