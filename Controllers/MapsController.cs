using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/maps")]
public class MapsController : ControllerBase
{
    private readonly IMapDataAccess _mapRepo;
    public MapsController(IMapDataAccess mapRepo)
    {
        _mapRepo = mapRepo;
    }

    /// <summary>
    /// Gets all maps.
    /// </summary>
    /// <returns> A list of all maps. </returns>
    /// <response code="200"> Returns a list of all maps, or an empty
    /// list if there are currently none stored. </response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet()]
    public IEnumerable<Map> GetAllMaps() =>
        _mapRepo.GetMaps();

    /// <summary>
    /// Gets all square maps.
    /// </summary>
    /// <returns> A list of all square maps. </returns>
    /// <response code="200"> Returns a list of all square maps,
    /// or an empty list if there are currently none stored. </response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("square")]
    public IEnumerable<Map> GetSquareMapsOnly() =>
        _mapRepo.GetSquareMapsOnly();

    /// <summary>
    /// Gets a map with the specified id.
    /// </summary>
    /// <param name="id"> The id for the map we are looking to return (from the request URL). </param>
    /// <returns> A map with the specified id. </returns>
    /// <response code="200"> Returns the map with the specified id. </response>
    /// <response code="404"> If the map retrieved at the specified id is null,
    /// ie. the map does not exist. </response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}", Name = "GetMap")]
    public IActionResult GetMapById(int id)
    {
        Map? map = _mapRepo.GetMapById(id);
        if (map is null) return NotFound();
        else return Ok(map);
    }

    /// <summary>
    /// Creates a new map.
    /// </summary>
    /// <param name="newMap"> A new map (from the HTTP request body). </param>
    /// <returns> A newly created map. </returns>
    /// <remarks>
    /// Sample request body:
    ///
    ///     POST /api/maps
    ///     {
    ///         "name": "SMALL_MAP",
    ///         "columns": 4,
    ///         "rows": 3
    ///     }
    ///
    /// </remarks>
    /// <response code="201"> Returns a newly created map. </response>
    /// <response code="400"> If the new map is null. </response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [HttpPost()]
    public IActionResult AddMap(Map newMap)
    {
        if (newMap is not null)
        {
            _mapRepo.AddMap(newMap);

            return CreatedAtRoute("GetRobotCommand", new { id = newMap.Id }, newMap);
        }

        return BadRequest();
    }

    /// <summary>
    /// Updates an existing map.
    /// </summary>
    /// <param name="id"> The id for the map we are looking to update (from the request URL). </param>
    /// <param name="updatedMap"> Updated details for the map (from the HTTP request body). </param>
    /// <returns> An udpated map. </returns>
    /// <remarks>
    /// Sample request body:
    /// 
    ///     PUT /api/maps
    ///     {
    ///         "rows": 10
    ///     }
    ///
    /// </remarks>
    /// <response code="204"> If the map is updated successfully. </response>
    /// <response code="400"> If the request body is null. </response>
    /// <response code="404"> If the map to update is null. </response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public IActionResult UpdateMap(int id, Map updatedMap)
    {
        Map? map = _mapRepo.GetMapById(id);
        if (map is null) return NotFound();

        try
        {
            _mapRepo.UpdateMap(id, updatedMap);
        }
        catch (Exception er) { return BadRequest(er); }

        return NoContent();
    }

    /// <summary>
    /// Deletes a map with the specified id.
    /// </summary>
    /// <param name="id"> The id for the map we are looking to delete (from the request URL). </param>
    /// <returns> No content. </returns>
    /// <response code="204"> Returns the map with the specified id. </response>
    /// <response code="404"> If the command to delete is null. </response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public IActionResult DeleteMap(int id)
    {
        Map? map = _mapRepo.GetMapById(id);
        if (map is null) return NotFound();

        _mapRepo.DeleteMap(id);
        return NoContent();
    }

    /// <summary>
    /// Checks if the specified coordinate is present on a map with a specified id.
    /// </summary>
    /// <param name="id"> The id for the map we are looking to delete (from the request URL). </param>
    /// <param name="x"> The x coordinate we are looking to verify (from the request URL). </param>
    /// <param name="y"> The y coordinate we are looking to verify (from the request URL). </param>
    /// <returns> A boolean value. </returns>
    /// <response code="200"> If the coordinates are valid and can be checked against the maps size. </response>
    /// <response code="400"> If the coordinates to verify are less than zero. </response>
    /// <response code="404"> If the map with the specified id is null. </response>
    [HttpGet("{id}/{x}-{y}")]
    public IActionResult CheckCoordinate(int id, int x, int y)
    {
        Map? map = _mapRepo.GetMapById(id);
        bool isOnMap = false;

        if (map is null) return NotFound();
        if (x < 0 || y < 0) return BadRequest();

        if (x <= map.Columns && y <= map.Rows) return (Ok(!isOnMap));
        return Ok(isOnMap);
    }
}
