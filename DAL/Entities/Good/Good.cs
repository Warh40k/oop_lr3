namespace DAL.Entities.Good;

public class Good
{
    public int? Id { get; set; }
    public string Name { get; set; }

    public decimal Price { get; set; }
    public int Quantity { get; set; }
}