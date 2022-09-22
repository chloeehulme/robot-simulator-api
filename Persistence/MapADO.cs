using System;
using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class MapADO : IMapDataAccess
    {
        private const string CONNECTION_STRING =
            "Host=localhost;Username=postgres;Password=chl123;Database=SIT331";

        // Gets all maps from DB
        public List<Map> GetMaps()
        {
            var maps = new List<Map>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);

            using var cmd = new NpgsqlCommand("SELECT * FROM map", conn);
            {
                conn.Open();
                using var dr = cmd.ExecuteReader();
                {
                    while (dr.Read())
                    {
                        var id = (int)dr["id"];
                        var name = (string)dr["name"];
                        string? descr = dr["description"] as string;
                        var columns = (int)dr["columns"];
                        var rows = (int)dr["rows"];
                        var isSquare = (bool)dr["is_square"];
                        var createdDate = (DateTime)dr["created_date"];
                        var modifiedDate = (DateTime)dr["modified_date"];

                        Map map = new(id, name, descr, columns, rows, isSquare, createdDate, modifiedDate);
                        maps.Add(map);
                    }
                    return maps;
                }
            }
        }

        // Gets square maps from DB
        public List<Map> GetSquareMapsOnly()
        {
            var maps = new List<Map>();
            using var conn = new NpgsqlConnection(CONNECTION_STRING);

            using var cmd = new NpgsqlCommand("SELECT * FROM map WHERE is_square=TRUE", conn);
            {
                conn.Open();
                using var dr = cmd.ExecuteReader();
                {
                    while (dr.Read())
                    {
                        var id = (int)dr["id"];
                        var name = (string)dr["name"];
                        string? descr = dr["description"] as string;
                        var columns = (int)dr["columns"];
                        var rows = (int)dr["rows"];
                        var isSquare = (bool)dr["is_square"];
                        var createdDate = (DateTime)dr["created_date"];
                        var modifiedDate = (DateTime)dr["modified_date"];

                        Map map = new(id, name, descr, columns, rows, isSquare, createdDate, modifiedDate);
                        maps.Add(map);
                    }
                    return maps;
                }
            }
        }

        // Gets map by ID from DB
        public Map? GetMapById(int id)
        {
            Map map;
            using var conn = new NpgsqlConnection(CONNECTION_STRING);

            using var cmd = new NpgsqlCommand("SELECT * FROM map WHERE id=@id", conn);
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@id", id);
                using var dr = cmd.ExecuteReader();
                {
                    while (dr.Read())
                    {
                        var name = (string)dr["name"];
                        string? descr = dr["description"] as string;
                        var columns = (int)dr["columns"];
                        var rows = (int)dr["rows"];
                        var isSquare = (bool)dr["is_square"];
                        var createdDate = (DateTime)dr["created_date"];
                        var modifiedDate = (DateTime)dr["modified_date"];

                        map = new(id, name, descr, columns, rows, isSquare, createdDate, modifiedDate);
                        return map;
                    }
                    return null;
                }
            }
        }

        // Updates map in DB
        public bool UpdateMap(int id, Map updatedMap)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            {
                conn.Open();

                using var cmd = new NpgsqlCommand("UPDATE map SET name=@name, " +
                    "description=@description, columns=@columns, rows=@rows, modified_date=@modifieddate, created_date=@createddate" +
                    " WHERE id=@id", conn);
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@name", updatedMap.Name);
                    cmd.Parameters.AddWithValue("@description", updatedMap.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@columns", updatedMap.Columns);
                    cmd.Parameters.AddWithValue("@rows", updatedMap.Rows);
                    cmd.Parameters.AddWithValue("@modifieddate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@createddate", DateTime.Now);
                }

                var result = cmd.ExecuteNonQuery();

                return result == 1;
            }
        }

        // Adds map to DB
        public Map AddMap(Map newMap)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            {
                conn.Open();

                using var cmd = new NpgsqlCommand("INSERT INTO map VALUES(DEFAULT, @name, @description, " +
                    "@columns, @rows, DEFAULT, @createddate, @modifieddate)", conn);
                {
                    cmd.Parameters.AddWithValue("@name", newMap.Name);
                    cmd.Parameters.AddWithValue("@description", newMap.Description ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@columns", newMap.Columns);
                    cmd.Parameters.AddWithValue("@rows", newMap.Rows);
                    cmd.Parameters.AddWithValue("@createddate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@modifieddate", DateTime.Now);
                }

                var result = cmd.ExecuteNonQuery();

                return newMap;
            }
        }

        // Deletes map from DB
        public bool DeleteMap(int id)
        {
            using var conn = new NpgsqlConnection(CONNECTION_STRING);
            {
                conn.Open();

                using var cmd = new NpgsqlCommand("DELETE FROM map WHERE id=@id", conn);
                {
                    cmd.Parameters.AddWithValue("@id", id);
                }

                var result = cmd.ExecuteNonQuery();

                return result == 1;
            }
        }
    }
}

