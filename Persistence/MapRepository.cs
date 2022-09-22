using System;
using Npgsql;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class MapRepository : IMapDataAccess, IRepository
    {
        private IRepository _repo => this;

        // Gets all maps from DB
        public List<Map> GetMaps()
        {
            var maps = _repo.ExecuteReader<Map>("SELECT * FROM map");
            return maps;
        }

        // Gets square maps from DB
        public List<Map> GetSquareMapsOnly()
        {
            var maps = _repo.ExecuteReader<Map>("SELECT * FROM map WHERE is_square=TRUE");
            return maps;
        }

        // Gets map by ID from DB
        public Map? GetMapById(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("id", id)
            };

            var map = _repo.ExecuteReader<Map>("SELECT * FROM map WHERE id=@id", sqlParams).SingleOrDefault();
            return map;
        }

        // Updates map in DB
        public bool UpdateMap(int id, Map updatedMap)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("id", id),
                new("name", updatedMap.Name),
                new("description", updatedMap.Description ?? (object)DBNull.Value),
                new("issquare", updatedMap.IsSquare),
                new("rows", updatedMap.Rows),
                new("columns", updatedMap.Columns),
                new("createddate", DateTime.Now),
                new("modifieddate", DateTime.Now)
            };

            _repo.ExecuteReader<Map>("UPDATE map SET name=@name, description=@description, " +
                "columns=@columns, rows=@rows, modified_date=@modifieddate, created_date=@createddate WHERE id=@id " +
                "RETURNING *", sqlParams).SingleOrDefault();

            return true;
        }

        // Adds map to DB
        public Map AddMap(Map newMap)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("id", newMap.Id),
                new("name", newMap.Name),
                new("description", newMap.Description ?? (object)DBNull.Value),
                new("ismovecommand", newMap.IsSquare),
                new("rows", newMap.Rows),
                new("columns", newMap.Columns),
                new("createddate", DateTime.Now),
                new("modifieddate", DateTime.Now)
            };

            var result = _repo.ExecuteReader<Map>("INSERT INTO map VALUES(DEFAULT, @name, @description, " +
                    "@columns, @rows, DEFAULT, @createddate, @modifieddate) RETURNING *", sqlParams).Single();

            return result;
        }

        // Deletes map from DB
        public bool DeleteMap(int id)
        {
            var sqlParams = new NpgsqlParameter[]
            {
                new("id", id)
            };

            _repo.ExecuteReader<Map>("DELETE FROM map WHERE id=@id", sqlParams).SingleOrDefault();
            return true;
        }
    }
}

