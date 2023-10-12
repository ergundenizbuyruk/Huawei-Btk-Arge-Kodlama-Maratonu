using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Huawei.Btk.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddImagePathToAnalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "Analyses",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Analyses");
        }
    }
}
