using System;
using Microsoft.EntityFrameworkCore;
using robot_controller_api.Contexts;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class RobotCommandEF : RobotContext, IRobotCommandDataAccess
    {
        public RobotCommandEF(DbContextOptions<RobotContext> options) : base(options)
        {
        }

        private RobotContext _robotContext = new();

        public List<RobotCommand> GetRobotCommands()
        {
            return _robotContext.RobotCommands.OrderBy(x => x.Id).ToList();
        }

        public List<RobotCommand> GetMoveCommands()
        {
            var commands = _robotContext.RobotCommands
                .Where(x => x.IsMoveCommand == true)
                .ToList();

            return commands;
        }

        public RobotCommand? GetRobotCommandById(int id)
        {
            var command = _robotContext.RobotCommands
                .FirstOrDefault(x => x.Id == id);

            return command;
        }

        public bool UpdateRobotCommand(int id, RobotCommand updatedCommand)
        {
            var command = GetRobotCommandById(id);

            if (command is not null)
            {
                command.Name = updatedCommand.Name;
                command.Description = updatedCommand.Description;
                command.IsMoveCommand = updatedCommand.IsMoveCommand;
                command.CreatedDate = DateTime.Now;
                command.ModifiedDate = DateTime.Now;
            }

            _robotContext.SaveChanges();

            return true;
        }

        public RobotCommand AddRobotCommand(RobotCommand newCommand)
        {
            RobotCommand command = new(default, newCommand.Name, newCommand.Description,
                newCommand.IsMoveCommand, DateTime.Now, DateTime.Now);

            _robotContext.RobotCommands.Add(command);
            _robotContext.SaveChanges();

            return command;
        }

        public bool DeleteRobotCommand(int id)
        {
            _robotContext.Remove(_robotContext.RobotCommands
                .Single(x => x.Id == id));

            _robotContext.SaveChanges();

            return true;
        }
    }
}

