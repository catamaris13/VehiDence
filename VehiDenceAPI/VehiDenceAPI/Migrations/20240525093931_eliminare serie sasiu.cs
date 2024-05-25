using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiDenceAPI.Migrations
{
    /// <inheritdoc />
    public partial class eliminareseriesasiu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerieSasiu",
                table: "Casco");

            migrationBuilder.DropColumn(
                name: "SerieSasiu",
                table: "Asigurare");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerieSasiu",
                table: "Casco",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SerieSasiu",
                table: "Asigurare",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
