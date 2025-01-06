namespace JPWP.Scripts;

public class Action
{
    public Action(string description, int id, string name, int[] costs, int[] revenues)
    {
        Description = description;
        Id = id;
        Name = name;
        Costs = costs;
        Revenues = revenues;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public int[] Costs { get; set; }
    public int[] Revenues { get; set; }
    public string Description { get; set; }
}