namespace JPWP;

public class Level
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public int Time { get; set; }
    public int[]? Parameters { get; set; }
    public int[]? Resources { get; set; }
    public int[]? StartIncome { get; set; }
}