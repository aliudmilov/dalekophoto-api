namespace DalekoPhoto.Api;

using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

[ApiController]
[Route("api/v1/albums")]
[Produces("application/json")]
public class AlbumsController : ControllerBase
{
    private readonly ILogger<AlbumsController> _logger;
    private readonly IAlbumRepository _albumRepository;

    public AlbumsController(ILogger<AlbumsController> logger, IAlbumRepository albumRepository)
    {
        _logger = logger;
        _albumRepository = albumRepository;
    }

    /// <summary>
    /// Gets all the photo albums
    /// </summary>
    /// <response code="200">Returns all the photo albums</response>
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
        catch (OperationCanceledException)
        {
            albums = Array.Empty<Album>();
            _logger.LogInformation("The get all albums operation was cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occured while fetching all albums");
            throw;
        }

        sw.Stop();

        _logger.LogInformation($"All albums fetched for {sw.ElapsedMilliseconds}ms");

        return Ok(albums);
    }

    /// <summary>
    /// Gets the photo album that has the specified ID
    /// </summary>
    /// <response code="200">Returns the photo album that has the specified ID</response>
    /// <response code="400">The request does not contain a valid photo album ID</response>
    [HttpGet("{id}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> Get(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            return BadRequest();

        _logger.LogInformation($"Received get album with ID = {id} request...");

        Stopwatch sw = Stopwatch.StartNew();
        Album album = null;
        try
        {
            album = (await _albumRepository.ReadAsync(x => x.Id == id.Trim().ToLowerInvariant()))?.SingleOrDefault();
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation($"Get album with ID = {id} operation was cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occured while fetching album with ID = {id}");
            throw;
        }

        sw.Stop();

        _logger.LogInformation($"Album with ID = {id} fetched for {sw.ElapsedMilliseconds}ms");

        return Ok(album);
    }

    /// <summary>
    /// Gets the portfolio photo album
    /// </summary>
    /// <response code="200">Returns the portfolio photo album</response>
    [HttpGet("/portoflio")]
    [ProducesResponseType(200)]
    public async Task<IActionResult> GetPortfolio()
    {
        _logger.LogInformation($"Received get portfolio album request...");

        Stopwatch sw = Stopwatch.StartNew();
        Album album = null;
        try
        {
            album = await _albumRepository.ReadPortfolioAsync();
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation($"Get portoflio album was cancelled");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"An error occured while fetching portoflio album");
            throw;
        }

        sw.Stop();

        _logger.LogInformation($"Portfolio album fetched for {sw.ElapsedMilliseconds}ms");

        return Ok(album);
    }
}
