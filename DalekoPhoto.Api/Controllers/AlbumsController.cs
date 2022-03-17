namespace DalekoPhoto.Api;

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

[ApiController]
[Route("api/v1/albums")]
[Produces("application/json")]
public class AlbumController : ControllerBase
{
    private readonly ILogger<AlbumController> _logger;
    private readonly IAlbumRepository _albumRepository;

    public AlbumController(ILogger<AlbumController> logger, IAlbumRepository albumRepository)
    {
        _logger = logger;
        _albumRepository = albumRepository;
    }

    /// <summary>
    /// Gets all the photo albums
    /// </summary>
    /// <response code="200">Returns all the albums</response>
    [HttpGet(Name = "GetAlbums")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("Received get all albums request. Fetching all albums...");

        Stopwatch sw = Stopwatch.StartNew();
        IReadOnlyList<Album> albums;
        try
        {
            albums = await _albumRepository.ReadAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(new EventId(), ex, "An error occured while fetching all albums");
            throw;
        }

        sw.Stop();

        _logger.LogInformation($"All albums fetched for {sw.ElapsedMilliseconds}ms");

        return Ok(albums);
    }
}
