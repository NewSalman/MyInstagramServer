using Microsoft.EntityFrameworkCore.Migrations;

namespace MyInstagramApi.Migrations
{
    public partial class secondary_id_to_posts_table_003 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestID",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestID",
                table: "Post");
        }
    }
}
