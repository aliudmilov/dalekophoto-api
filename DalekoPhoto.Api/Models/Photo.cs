namespace DalekoPhoto.Api;

public class Photo
{
    public string Id { get; set; }

    public string Url { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime TakenDate { get; set; }

    public ulong NumberOfLikes { get; set; }
}
