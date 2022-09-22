
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public interface IRobotCommandDataAccess
    {
        RobotCommand AddRobotCommand(RobotCommand newCommand);

        bool DeleteRobotCommand(int id);

        List<RobotCommand> GetMoveCommands();

        RobotCommand? GetRobotCommandById(int id);

        List<RobotCommand> GetRobotCommands();

        bool UpdateRobotCommand(int id, RobotCommand updatedCommand);
    }
}