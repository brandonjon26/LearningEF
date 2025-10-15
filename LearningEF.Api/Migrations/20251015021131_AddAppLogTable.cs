using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningEF.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAppLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "AppLog",
            //    columns: table => new
            //    {
            //        AppLogId = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        TimeStamp = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
            //        SeverityLevel = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
            //        Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        Exception = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Properties = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AppLog", x => x.AppLogId);
            //    });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppLog");
        }
    }
}
