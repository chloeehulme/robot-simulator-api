using System;
using System.Globalization;
using System.Text;
using System.Text.Json;
using FastMember;
using Npgsql;

namespace robot_controller_api.Persistence
{
    public static class ExtensionMethods
    {
        public static void MapTo<T>(this NpgsqlDataReader dr, T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var fastMember = TypeAccessor.Create(entity.GetType());
            var props = fastMember.GetMembers().Select(x => x.Name).ToHashSet();

            for (int i = 0; i < dr.FieldCount; i++)
            {
                var dbColumnName = dr.GetName(i);

                var splits = dbColumnName.Split('_');
                var columnName = new StringBuilder("");
                foreach (var split in splits)
                {
                    columnName.Append(CultureInfo.InvariantCulture.TextInfo.ToTitleCase(split.ToLower()));
                }

                var prop = props.FirstOrDefault(x => x.Equals(columnName.ToString()));
                if (!string.IsNullOrEmpty(prop)) fastMember[entity, prop] = dr.IsDBNull(i) ? null : dr.GetValue(i);
            }
        }
    }
}

