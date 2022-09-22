namespace robot_controller_api;

public class Map
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Columns { get; set; }
    public int Rows { get; set; }
    public bool IsSquare { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }
    public string? Description { get; set; }

    public Map()
    {

    }
}
