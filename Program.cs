using System.Reflection;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.OpenApi.Models;
using robot_controller_api.Contexts;
using robot_controller_api.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Robot Controller API",
        Description = "New backend service that provides " +
        "resources for the Moon Robot Simulator.",
        Contact = new OpenApiContact
        {
            Name = "Chloe Hulme",
            Email = "chulme@deakin.edu.au"
        },
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandADO>();
//builder.Services.AddScoped<IMapDataAccess, MapADO>();
//builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandRepository>();
//builder.Services.AddScoped<IMapDataAccess, MapRepository>();
builder.Services.AddScoped<IRobotCommandDataAccess, RobotCommandEF>();
builder.Services.AddScoped<IMapDataAccess, MapEF>();
builder.Services.AddDbContext<RobotContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(setup => setup.InjectStylesheet("/styles/robot.css"));
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.MapControllers();

app.Run();