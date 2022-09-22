
using robot_controller_api.Models;

namespace robot_controller_api.Persistence
{
    public interface IMapDataAccess
    {
        Map AddMap(Map newMap);

        bool DeleteMap(int id);

        Map? GetMapById(int id);

        List<Map> GetMaps();

        List<Map> GetSquareMapsOnly();

        bool UpdateMap(int id, Map updatedMap);
    }
}