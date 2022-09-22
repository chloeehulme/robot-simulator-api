using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using robot_controller_api.Models;

namespace robot_controller_api.Contexts
{
    public partial class RobotContext
    {
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Map>(entity => { 
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<RobotCommand>(entity => {
                entity.HasKey(x => x.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });
        }
    }
}

