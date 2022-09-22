using System;
using System.Collections.Generic;

namespace robot_controller_api.Models
{
    public class Map
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public int Columns { get; set; }
        public int Rows { get; set; }
        public bool? IsSquare { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Map()
        {
        }

        public Map(int id, string name, string? description, int columns, int rows, bool? issquare, DateTime createddate, DateTime modifieddate)
        {
            Id = id;
            Name = name;
            Description = description;
            Columns = columns;
            Rows = rows;
            IsSquare = issquare;
            CreatedDate = createddate;
            ModifiedDate = modifieddate;
        }
    }
}
