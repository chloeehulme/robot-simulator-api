using Microsoft.AspNetCore.Mvc;
using robot_controller_api.Models;
using robot_controller_api.Persistence;

namespace robot_controller_api.Controllers;

[ApiController]
[Route("api/robot-commands")]
public class RobotCommandsController : ControllerBase
{
    private readonly IRobotCommandDataAccess _robotCommandsRepo;
    public RobotCommandsController(IRobotCommandDataAccess robotCommandsRepo)
    {
        _robotCommandsRepo = robotCommandsRepo;
    }

    /// <summary>
    /// Gets all robot commands.
    /// </summary>
    /// <returns> A list of all robot commands. </returns>
    /// <response code="200"> Returns a list of all robot commands, or an empty
    /// list if there are currently none stored. </response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet()]
    public IEnumerable<RobotCommand> GetAllRobotCommands() =>
        _robotCommandsRepo.GetRobotCommands();

    /// <summary>
    /// Gets all 'move' based commands.
    /// </summary>
    /// <returns> A list of all 'move' based commands. </returns>
    /// <response code="200"> Returns a list of all 'move' based robot commands,
    /// or an empty list if there are currently none stored. </response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [HttpGet("move")]
    public IEnumerable<RobotCommand> GetMoveCommandsOnly() =>
        _robotCommandsRepo.GetMoveCommands();

    /// <summary>
    /// Gets a robot command with the specified id.
    /// </summary>
    /// <param name="id"> The id for the command we are looking to return (from the request URL). </param>
    /// <returns> A robot command with the specified id. </returns>
    /// <response code="200"> Returns the robot command with the specified id. </response>
    /// <response code="404"> If the robot command retrieved at the specified id is null,
    /// ie. the command does not exist. </response>
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpGet("{id}", Name = "GetRobotCommand")]
    public IActionResult GetRobotCommandById(int id)
    {
        RobotCommand? command = _robotCommandsRepo.GetRobotCommandById(id);
        if (command is null) return NotFound();
        else return Ok(command);
    }

    /// <summary>
    /// Creates a new robot command.
    /// </summary>
    /// <param name="newCommand"> A new robot command (from the HTTP request body). </param>
    /// <returns> A newly created robot command. </returns>
    /// <remarks>
    /// Sample request body:
    ///
    ///     POST /api/robot-commands
    ///     {
    ///         "name": "LEAP",
    ///         "isMoveCommand": true,
    ///         "description": One giant leap for robot-kind.
    ///     }
    ///
    /// </remarks>
    /// <response code="201"> Returns a newly created robot command. </response>
    /// <response code="400"> If the new command is null. </response>
    /// <response code="409"> If a robot command with the same name already exists. </response>
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [HttpPost()]
    public IActionResult AddRobotCommand(RobotCommand newCommand)
    {
        if (newCommand is null) return BadRequest();

        if (_robotCommandsRepo.GetRobotCommands().Exists(x => x.Name == newCommand.Name)) return Conflict();

        _robotCommandsRepo.AddRobotCommand(newCommand);

        return CreatedAtRoute("GetRobotCommand", new { id = newCommand.Id }, newCommand);
    }

    /// <summary>
    /// Updates an existing robot command.
    /// </summary>
    /// <param name="id"> The id for the command we are looking to update (from the request URL). </param>
    /// <param name="updatedCommand"> Updated details for the robot command (from the HTTP request body). </param>
    /// <returns> No content. </returns>
    /// <remarks>
    /// Sample request body:
    ///
    ///     PUT /api/robot-commands
    ///     {
    ///         "name": "BLINK",
    ///         "isMoveCommand": false
    ///     }
    ///
    /// </remarks>
    /// <response code="204"> If the command is udpated successfully. </response>
    /// <response code="400"> If the request body is null. </response>
    /// <response code="404"> If the command to update is null. </response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpPut("{id}")]
    public IActionResult UpdateRobotCommand(int id, RobotCommand updatedCommand)
    {
        RobotCommand? command = _robotCommandsRepo.GetRobotCommandById(id);
        if (command is null) return NotFound();

        try
        {
            _robotCommandsRepo.UpdateRobotCommand(id, updatedCommand);
        }
        catch (Exception er) { return BadRequest(er); }

        return NoContent();
    }

    /// <summary>
    /// Deletes a robot command with the specified id.
    /// </summary>
    /// <param name="id"> The id for the command we are looking to delete (from the request URL). </param>
    /// <returns> No content. </returns>
    /// <response code="204"> Returns the robot command with the specified id. </response>
    /// <response code="404"> If the command to delete is null. </response>
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [HttpDelete("{id}")]
    public IActionResult DeleteRobotCommand(int id)
    {
        RobotCommand? command = _robotCommandsRepo.GetRobotCommandById(id);
        if (command is null) return NotFound();

        _robotCommandsRepo.DeleteRobotCommand(id);
        return NoContent();
    }
}
