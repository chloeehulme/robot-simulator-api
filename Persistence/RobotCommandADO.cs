using System;
using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class RobotCommandADO : IRobotCommandDataAccess
    {
        private const string CONNECTION_STRING =
            "Host=localhost;Username=postgres;Password=chl123;Database=SIT331";

        // Gets all commands from DB
        public List<RobotCommand> GetRobotCommands()
        {
            var robotCommands = new List<RobotCommand>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);

            using var cmd = new NpgsqlCommand("SELECT * FROM robot_command", conn);
            {
                conn.Open();
                using var dr = cmd.ExecuteReader();
                {
                    while (dr.Read())
                    {
                        var id = (int)dr["id"];
                        var name = (string)dr["name"];
                        string? descr = dr["description"] as string;
                        var isMoveCommand = (bool)dr["is_move_command"];
                        var createdDate = (DateTime)dr["created_date"];
                        var modifiedDate = (DateTime)dr["modified_date"];

                        RobotCommand command = new(id, name, descr, isMoveCommand, createdDate, modifiedDate);
                        robotCommands.Add(command);
                    }
                    return robotCommands;
                }
            }
        }

        // Gets all 'move' commands from DB
        public List<RobotCommand> GetMoveCommands()
        {
            var robotCommands = new List<RobotCommand>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);

            using var cmd = new NpgsqlCommand("SELECT * FROM robot_command WHERE is_move_command=TRUE", conn);
            {
                conn.Open();
                using var dr = cmd.ExecuteReader();
                {
                    while (dr.Read())
                    {
                        var isMoveCommand = (bool)dr["is_move_command"];
                        var id = (int)dr["id"];
                        var name = (string)dr["name"];
                        string? descr = dr["description"] as string;
                        var createdDate = (DateTime)dr["created_date"];
                        var modifiedDate = (DateTime)dr["modified_date"];

                        RobotCommand command = new(id, name, descr, isMoveCommand, createdDate, modifiedDate);
                        robotCommands.Add(command);
                    }

                    return robotCommands;
                }
            }
        }

        // Gets command by ID from DB
        public RobotCommand? GetRobotCommandById(int id)
        {
            RobotCommand command;
            using var conn = new NpgsqlConnection(CONNECTION_STRING);

            using var cmd = new NpgsqlCommand("SELECT * FROM robot_command WHERE id=@id", conn);
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@id", id);
                using var dr = cmd.ExecuteReader();
                {
                    while (dr.Read())
                    {
                        var name = (string)dr["name"];
                        string? descr = dr["description"] as string;
                        var isMoveCommand = (bool)dr["is_move_command"];
                        var createdDate = (DateTime)dr["created_date"];
                        var modifiedDate = (DateTime)dr["modified_date"];

                        command = new(id, name, descr, isMoveCommand, createdDate, modifiedDate);
                        return command;
                    }
                    return null;
                }
            }
        }

        // Updates command in DB
        public bool UpdateRobotCommand(int id, RobotCommand updatedCommand)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            {
                conn.Open();

                using var cmd = new NpgsqlCommand("UPDATE robot_command SET name=@name, " +
                    "description=@description, is_move_command=@ismovecommand, modified_date=@modifieddate, created_date=@createddate" +
                    " WHERE id=@id", conn);
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", updatedCommand.Name);
                    cmd.Parameters.AddWithValue("@description", updatedCommand.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ismovecommand", updatedCommand.IsMoveCommand);
                    cmd.Parameters.AddWithValue("@modifieddate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@createddate", DateTime.Now);
                }

                var result = cmd.ExecuteNonQuery();

                return result == 1;
            }
        }

        // Adds command to DB
        public RobotCommand AddRobotCommand(RobotCommand newCommand)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            {
                conn.Open();

                using var cmd = new NpgsqlCommand("INSERT INTO robot_command VALUES(DEFAULT, @name, @description, " +
                    "@ismovecommand, @createddate, @modifieddate)", conn);
                {
                    cmd.Parameters.AddWithValue("@name", newCommand.Name);
                    cmd.Parameters.AddWithValue("@description", newCommand.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ismovecommand", newCommand.IsMoveCommand);
                    cmd.Parameters.AddWithValue("@createddate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@modifieddate", DateTime.Now);
                }

                var result = cmd.ExecuteNonQuery();

                return newCommand;
            }
        }

        // Deletes command from DB
        public bool DeleteRobotCommand(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            {
                conn.Open();

                using var cmd = new NpgsqlCommand("DELETE FROM robot_command WHERE id=@id", conn);
                {
                    cmd.Parameters.AddWithValue("@id", id);
                }

                var result = cmd.ExecuteNonQuery();

                return result == 1;
            }
        }
    }
}

