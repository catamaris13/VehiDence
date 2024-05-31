using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiDenceAPI.Migrations
{
    /// <inheritdoc />
    public partial class ceva : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Vigneta",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "PermisConducere",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Casco",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Asigurare",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Vigneta");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "PermisConducere");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Casco");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Asigurare");

            

            
        }
    }
}
