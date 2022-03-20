using System.ComponentModel.DataAnnotations;

namespace DalekoPhoto.Api;

public class Album
{
    [Required]
    public string Id { get; set; }

    public string Icon { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public string Location { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateUpdated { get; set; }

    public Photo[] Photos { get; set; }
}
