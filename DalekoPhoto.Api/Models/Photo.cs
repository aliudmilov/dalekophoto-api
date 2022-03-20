namespace DalekoPhoto.Api;

public class Photo
{
    public string Id { get; set; }

    public string LargeImageUrl { get; set; }

    public string SmallImageUrl { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime? DateTaken { get; set; }

    public ulong? NumberOfLikes { get; set; }
}
