namespace JPWP.Scripts;

public class Event
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<int> Effect1 { get; set; } = new List<int>();
    public List<int> Effect2 { get; set; } = new List<int>(); 
    public string Effect1T { get; set; } =  string.Empty;
    public string Effect2T { get; set; } =  string.Empty;
}