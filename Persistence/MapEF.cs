using System;
using Microsoft.EntityFrameworkCore;
using robot_controller_api.Contexts;
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public class MapEF : RobotContext, IMapDataAccess
    {
        public MapEF(DbContextOptions<RobotContext> options) : base(options)
        {
        }

        private RobotContext _robotContext = new();

        public List<Map> GetMaps()
        {
            return _robotContext.Maps.OrderBy(x => x.Id).ToList();
        }

        public List<Map> GetSquareMapsOnly()
        {
            var maps = _robotContext.Maps
                .Where(x => x.IsSquare == true)
                .ToList();

            return maps;
        }

        public Map? GetMapById(int id)
        {
            var map = _robotContext.Maps
                .FirstOrDefault(x => x.Id == id);

            return map;
        }

        public bool UpdateMap(int id, Map updatedMap)
        {
            var map = GetMapById(id);

            if (map is not null)
            {
                map.Name = updatedMap.Name;
                map.Description = updatedMap.Description;
                map.Columns = updatedMap.Columns;
                map.Rows = updatedMap.Rows;
                map.CreatedDate = DateTime.Now;
                map.ModifiedDate = DateTime.Now;
            }

            _robotContext.SaveChanges();

            return true;
        }

        public Map AddMap(Map newMap)
        {
            var map = new Map {
                Name = newMap.Name,
                Description = newMap.Description,
                Columns = newMap.Columns,
                Rows = newMap.Rows,
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            _robotContext.Maps.Add(map);
            _robotContext.SaveChanges();

            return map;
        }

        public bool DeleteMap(int id)
        {
            _robotContext.Remove(_robotContext.Maps.Single(x => x.Id == id));
            _robotContext.SaveChanges();

            return true;
        }
    }
}

