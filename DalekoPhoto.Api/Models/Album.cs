namespace DalekoPhoto.Api;

public class Album
{
    public string Id { get; set; }

    public string Icon { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime UpdatedDate { get; set; }

    public Photo[] Photos { get; set; }
}
