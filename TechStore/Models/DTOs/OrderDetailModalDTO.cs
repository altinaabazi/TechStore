namespace TechStore.Models.DTOs;

public class OrderDetailModalDTO
{
    public string DivId { get; set; }
    public List<OrderDetailModal> OrderDetail { get; set; }
}
//public class OrderDetailModalDTO
//{
//    public List<OrderDetailModal> OrderDetails { get; set; }
//}
public class OrderDetailModal
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string ProductName { get; set; }
    public string BrandName { get; set; }
    public string CategoryName { get; set; }
    public double UnitPrice { get; set; }
    public int Quantity { get; set; }
    public double TotalPrice { get; set; }
}