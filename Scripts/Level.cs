namespace JPWP;

public class Level
{
    public string? Name { get; set; }
    public int Time { get; set; }
    public int[]? Parameters { get; set; }
    public int[]? Resources { get; set; }
    public int[]? StartIncome { get; set; }
    public int[]? Actions { get; set; }
    public List<List<int>> EventPositions { get; set; }
}