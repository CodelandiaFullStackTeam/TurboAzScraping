using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HepsiBuradaScraping.Migrations
{
    public partial class mig_init1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EngineCapacity",
                table: "Cars",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EngineCapacity",
                table: "Cars");
        }
    }
}
