using System.ComponentModel.DataAnnotations;

namespace DalekoPhoto.Api;

public class Photo
{
    [Required]
    public string Id { get; set; }

    [Required]
    public string SmallImageUrl { get; set; }

    [Required]
    public string SmallImageDefaultUrl { get; set; }

    [Required]
    public string MediumImageUrl { get; set; }

    [Required]
    public string MediumImageDefaultUrl { get; set; }

    [Required]
    public string LargeImageUrl { get; set; }

    [Required]
    public string LargeImageDefaultUrl { get; set; }

    [Required]
    public bool IsFeatured { get; set; }

    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime? DateTaken { get; set; }

    public ulong? NumberOfLikes { get; set; }
}
