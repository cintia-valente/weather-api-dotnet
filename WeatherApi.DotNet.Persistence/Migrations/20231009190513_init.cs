using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherApi.DotNet.Persistence.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CityData",
                columns: table => new
                {
                    IdCity = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CityData", x => x.IdCity);
                });

            migrationBuilder.CreateTable(
                name: "WeatherData",
                columns: table => new
                {
                    IdWeather = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaxTemperature = table.Column<int>(type: "integer", nullable: false),
                    MinTemperature = table.Column<int>(type: "integer", nullable: false),
                    Precipitation = table.Column<double>(type: "double precision", nullable: false),
                    Humidity = table.Column<double>(type: "double precision", nullable: false),
                    WindSpeed = table.Column<double>(type: "double precision", nullable: false),
                    DayTime = table.Column<int>(type: "integer", nullable: false),
                    NightTime = table.Column<int>(type: "integer", nullable: false),
                    IdCity = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherData", x => x.IdWeather);
                    table.ForeignKey(
                        name: "FK_WeatherData_CityData_IdCity",
                        column: x => x.IdCity,
                        principalTable: "CityData",
                        principalColumn: "IdCity",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CityData_Name",
                table: "CityData",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeatherData_IdCity",
                table: "WeatherData",
                column: "IdCity");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherData");

            migrationBuilder.DropTable(
                name: "CityData");
        }
    }
}
