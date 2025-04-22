namespace LumelAssesment.Models
{
    // Models/Customer.cs
    public class Customer
    {
        public string CustomerID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    // Models/Product.cs
    public class Product
    {
        public string ProductID { get; set; }
        public string Name { get; set; }
        public int CategoryID { get; set; }
        public Category Category { get; set; }
        public decimal UnitPrice { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }

    // Models/Category.cs
    public class Category
    {
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public ICollection<Product> Products { get; set; }
    }

    // Models/Region.cs
    public class Region
    {
        public int RegionID { get; set; }
        public string Name { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    // Models/PaymentMethod.cs
    public class PaymentMethod
    {
        public int PaymentMethodID { get; set; }
        public string MethodName { get; set; }
        public ICollection<Order> Orders { get; set; }
    }

    // Models/Order.cs
    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public Customer Customer { get; set; }
        public DateTime DateOfSale { get; set; }
        public int RegionID { get; set; }
        public Region Region { get; set; }
        public int PaymentMethodID { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal ShippingCost { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }

    // Models/OrderDetail.cs
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public Order Order { get; set; }
        public string ProductID { get; set; }
        public Product Product { get; set; }
        public int QuantitySold { get; set; }
        public decimal Discount { get; set; }
    }

}
