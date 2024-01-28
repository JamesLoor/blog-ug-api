using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace blog_ug_api.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePostCategoriesToCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriaPost");

            migrationBuilder.AddColumn<string>(
                name: "Categoria",
                table: "Posts",
                type: "longtext",
                nullable: true,
                collation: "utf8mb4_0900_ai_ci")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "CategoriaId",
                table: "Posts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CategoriaId",
                table: "Posts",
                column: "CategoriaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Categories_CategoriaId",
                table: "Posts",
                column: "CategoriaId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Categories_CategoriaId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CategoriaId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CategoriaId",
                table: "Posts");

            migrationBuilder.CreateTable(
                name: "CategoriaPost",
                columns: table => new
                {
                    CategoriasId = table.Column<int>(type: "int", nullable: false),
                    PostsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriaPost", x => new { x.CategoriasId, x.PostsId });
                    table.ForeignKey(
                        name: "FK_CategoriaPost_Categories_CategoriasId",
                        column: x => x.CategoriasId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoriaPost_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "IX_CategoriaPost_PostsId",
                table: "CategoriaPost",
                column: "PostsId");
        }
    }
}
