using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiDenceAPI.Migrations
{
    /// <inheritdoc />
    public partial class IsValid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsValid",
                table: "Vigneta",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsValid",
                table: "RevizieService",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsValid",
                table: "PermisConducere",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsValid",
                table: "ITP",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsValid",
                table: "Casco",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "IsValid",
                table: "Asigurare",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Vigneta");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "RevizieService");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "PermisConducere");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "ITP");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Casco");

            migrationBuilder.DropColumn(
                name: "IsValid",
                table: "Asigurare");
        }
    }
}