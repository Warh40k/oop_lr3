namespace DTOs;

public class ShopGoodDto
{
    public int? Id { get; set; }
    public int? GoodId { get; set; }
    public int? ShopId { get; set; }
    public decimal Price { get; set; }
    public int InStock { get; set; }
}