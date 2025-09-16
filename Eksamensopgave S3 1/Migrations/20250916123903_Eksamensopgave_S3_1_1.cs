using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eksamensopgave_S3_1.Migrations
{
    /// <inheritdoc />
    public partial class Eksamensopgave_S3_1_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BogTabel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Forfatter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Udgiver = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UdgivelsesDag = table.Column<int>(type: "int", nullable: false),
                    UdgivelsesMåned = table.Column<int>(type: "int", nullable: false),
                    UdgivelsesÅr = table.Column<int>(type: "int", nullable: false),
                    AntalEksemplarer = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BogTabel", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BogTabel");
        }
    }
}
