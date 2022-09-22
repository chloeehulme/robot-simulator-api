using System;
using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class RobotCommandRepository : IRobotCommandDataAccess, IRepository
    {
        private IRepository _repo => this;

        // Gets all commands from DB
        public List<RobotCommand> GetRobotCommands()
        {
            var commands = _repo.ExecuteReader<RobotCommand>("SELECT * FROM robot_command");
            return commands;
        }

        // Gets all 'move' commands from DB
        public List<RobotCommand> GetMoveCommands()
        {
            var commands = _repo.ExecuteReader<RobotCommand>("SELECT * FROM robot_command WHERE is_move_command=TRUE");

            return commands;
        }

        // Gets command by ID from DB
        public RobotCommand? GetRobotCommandById(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("id", id)
            };

            var command = _repo.ExecuteReader<RobotCommand>("SELECT * FROM robot_command WHERE id=@id", sqlParams).SingleOrDefault();
            return command;
        }

        // Updates command in DB
        public bool UpdateRobotCommand(int id, RobotCommand updatedCommand)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("id", id),
                new("name", updatedCommand.Name),
                new("description", updatedCommand.Description ?? (object)DBNull.Value),
                new("ismovecommand", updatedCommand.IsMoveCommand),
                new("createddate", DateTime.Now),
                new("modifieddate", DateTime.Now)
            };

                _repo.ExecuteReader<RobotCommand>("UPDATE robot_command SET name=@name, description=@description," +
                "is_move_command=@ismovecommand, modified_date=@modifieddate, created_date=@createddate WHERE id=@id " +
                "RETURNING *", sqlParams).SingleOrDefault();

            return true;
        }

        // Adds command to DB
        public RobotCommand AddRobotCommand(RobotCommand newCommand)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("id", newCommand.Id),
                new("name", newCommand.Name),
                new("description", newCommand.Description ?? (object)DBNull.Value),
                new("ismovecommand", newCommand.IsMoveCommand),
                new("createddate", DateTime.Now),
                new("modifieddate", DateTime.Now)
            };

            var result = _repo.ExecuteReader<RobotCommand>("INSERT INTO robot_command VALUES(DEFAULT, @name, @description, " +
                    "@ismovecommand, @createddate, @modifieddate) RETURNING *", sqlParams).Single();

            return result;
        }

        // Deletes command from DB
        public bool DeleteRobotCommand(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("id", id)
            };

            _repo.ExecuteReader<RobotCommand>("DELETE FROM robot_command WHERE id=@id", sqlParams).SingleOrDefault();
            return true;
        }
    }
}

