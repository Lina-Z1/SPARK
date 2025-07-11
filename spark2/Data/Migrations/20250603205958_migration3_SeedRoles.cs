﻿using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;

#nullable disable

namespace spark2.Data.Migrations
{
    /// <inheritdoc />
    public partial class migration3_SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] {"Id","Name", "NormalizedName", "ConcurrencyStamp" },
                values:new object[] {Guid.NewGuid().ToString(),"EndUser", "EndUser".ToUpper(),Guid.NewGuid().ToString() }
                
                );

            migrationBuilder.InsertData(
               table: "AspNetRoles",
               columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
               values: new object[] { Guid.NewGuid().ToString(), "Admin", "Admin".ToUpper(), Guid.NewGuid().ToString() }

               );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [AspNetRoles] WHERE [Name] IN ('Admin', 'EndUser')");

        }
    }
}
