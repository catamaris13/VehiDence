using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiDenceAPI.Migrations
{
    /// <inheritdoc />
    public partial class Andrei : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
 name: "ImageData",
 table: "Masina");

            // migrationBuilder.DropColumn(
            //name: "ImageData",
            //table: "Vigneta");

            migrationBuilder.DropColumn(
    name: "ImageData",
    table: "Casco");

            migrationBuilder.DropColumn(
    name: "ImageData",
    table: "Asigurare");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        

            migrationBuilder.AddColumn<byte[]>(
    name: "ImageData",
    table: "Masina",
    type: "varbinary(max)",
    nullable: false,
    defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
    name: "ImageData",
    table: "Vigneta",
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
    }
}
