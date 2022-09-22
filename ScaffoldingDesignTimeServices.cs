using System;
using EntityFrameworkCore.Scaffolding.Handlebars;
using HandlebarsDotNet;
using HandlebarsDotNet.IO;
using Microsoft.EntityFrameworkCore.Design;

namespace robot_controller_api
{
    public class ScaffoldingDesignTimeServices : IDesignTimeServices
    {
        public ScaffoldingDesignTimeServices()
        {
        }

        public void ConfigureDesignTimeServices(IServiceCollection services)
        {
            var options = ReverseEngineerOptions.DbContextAndEntities;
            services.AddHandlebarsScaffolding(options);

            services.AddHandlebarsHelpers(("to-lower-helper", (writer, context, parameters) =>
                writer.Write($"{context["property-name"].ToString()?.ToLower()}")
            ));
        }
    }
}
