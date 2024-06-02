using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VehiDenceAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PhoneNo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.UniqueConstraint("AK_Users_username", x => x.username);
                });

            migrationBuilder.CreateTable(
               name: "Masina",
               columns: table => new
               {
                   Id = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   SerieSasiu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                   NrInmatriclulare = table.Column<string>(type: "nvarchar(450)", nullable: false),
                   Marca = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Model = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   Username = table.Column<string>(type: "nvarchar(450)", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Masina", x => x.Id);
                   table.UniqueConstraint("AK_Masina_NrInmatriclulare", x => x.NrInmatriclulare);
                   table.UniqueConstraint("AK_Masina_SerieSasiu", x => x.SerieSasiu);
                   table.ForeignKey(
                      name: "FK_Masina_User",
                      column: x => x.Username,
                      principalTable: "Users",
                      principalColumn: "username",
                      onDelete: ReferentialAction.Cascade);
               });

            migrationBuilder.CreateTable(
                name: "Asigurare",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerieSasiu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NrInmatriculare = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataCreare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataExpirare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Asigurator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Asigurare", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Asigurare_Masina",
                        column: x => x.NrInmatriculare,
                        principalTable: "Masina",
                        principalColumn: "NrInmatriclulare",
                        onDelete:ReferentialAction.Cascade
                        );
                });

            migrationBuilder.CreateTable(
                name: "Casco",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerieSasiu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    NrInmatriculare = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataCreare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataExpirare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Asigurator = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Casco", x => x.Id);
                    table.ForeignKey(
                       name: "FK_Casco_Masina",
                       column: x => x.NrInmatriculare,
                       principalTable: "Masina",
                       principalColumn: "NrInmatriclulare",
                       onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ITP",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NrInmatriculare = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataCreare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataExpirare = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ITP", x => x.Id);
                    table.ForeignKey(
                       name: "FK_ITP_Masina",
                       column: x => x.NrInmatriculare,
                       principalTable: "Masina",
                       principalColumn: "NrInmatriclulare", 
                       onDelete: ReferentialAction.Cascade);
                });

           

            migrationBuilder.CreateTable(
                name: "PermisConducere",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nume = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataCreare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataExpirare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Categorie = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermisConducere", x => x.Id);
                    table.ForeignKey(
                       name: "FK_PermisConducere_User",
                       column: x => x.username,
                       principalTable: "Users",
                       principalColumn: "username",
                       onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RevizieService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SerieSasiu = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    KmUltim = table.Column<int>(type: "int", nullable: false),
                    KmExpirare = table.Column<int>(type: "int", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RevizieService", x => x.Id);
                    table.ForeignKey(
                       name: "FK_RevizieService_Masina",
                       column: x => x.SerieSasiu,
                       principalTable: "Masina",
                       principalColumn: "SerieSasiu",
                       onDelete: ReferentialAction.Cascade);
                });

            

            migrationBuilder.CreateTable(
                name: "Vigneta",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NrInmatriculare = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DataCreare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataExpirare = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Tara = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vigneta", x => x.Id);
                    table.ForeignKey(
                       name: "FK_Vigneta_Masina",
                       column: x => x.NrInmatriculare,
                       principalTable: "Masina",
                       principalColumn: "NrInmatriclulare",
                       onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Asigurare");

            migrationBuilder.DropTable(
                name: "Casco");

            migrationBuilder.DropTable(
                name: "ITP");

            migrationBuilder.DropTable(
                name: "Masina");

            migrationBuilder.DropTable(
                name: "PermisConducere");

            migrationBuilder.DropTable(
                name: "RevizieService");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Vigneta");
        }
    }
}
