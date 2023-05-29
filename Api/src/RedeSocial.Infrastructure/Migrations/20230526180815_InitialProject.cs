using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RedeSocial.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialProject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Discriminator = table.Column<string>(type: "TEXT", nullable: false),
                    UsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
                    AmigoId = table.Column<int>(type: "INTEGER", nullable: true),
                    StatusAmizade = table.Column<int>(type: "INTEGER", nullable: true),
                    UsuarioId1 = table.Column<int>(type: "INTEGER", nullable: true),
                    UsuarioId2 = table.Column<int>(type: "INTEGER", nullable: true),
                    Conteudo = table.Column<string>(type: "TEXT", nullable: true),
                    PostId = table.Column<int>(type: "INTEGER", nullable: true),
                    AutorId = table.Column<int>(type: "INTEGER", nullable: true),
                    Autor = table.Column<string>(type: "TEXT", nullable: true),
                    DataPublicacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Curtida_PostId = table.Column<int>(type: "INTEGER", nullable: true),
                    Curtida_UsuarioId = table.Column<int>(type: "INTEGER", nullable: true),
                    Post_AutorId = table.Column<int>(type: "INTEGER", nullable: true),
                    Post_Conteudo = table.Column<string>(type: "TEXT", nullable: true),
                    Criacao = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Curtidas = table.Column<int>(type: "INTEGER", nullable: true),
                    PermissaoVisualizar = table.Column<int>(type: "INTEGER", nullable: true),
                    Nome = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    Apelido = table.Column<string>(type: "TEXT", nullable: true),
                    DataNascimento = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Cep = table.Column<string>(type: "TEXT", nullable: true),
                    SenhaHash = table.Column<string>(type: "TEXT", nullable: true),
                    ImagemPerfil = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bases_Bases_AmigoId",
                        column: x => x.AmigoId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bases_Bases_Curtida_PostId",
                        column: x => x.Curtida_PostId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bases_Bases_Curtida_UsuarioId",
                        column: x => x.Curtida_UsuarioId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bases_Bases_PostId",
                        column: x => x.PostId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bases_Bases_Post_AutorId",
                        column: x => x.Post_AutorId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bases_Bases_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Bases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bases_Bases_UsuarioId1",
                        column: x => x.UsuarioId1,
                        principalTable: "Bases",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Bases_Bases_UsuarioId2",
                        column: x => x.UsuarioId2,
                        principalTable: "Bases",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bases_AmigoId",
                table: "Bases",
                column: "AmigoId");

            migrationBuilder.CreateIndex(
                name: "IX_Bases_Curtida_PostId",
                table: "Bases",
                column: "Curtida_PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Bases_Curtida_UsuarioId",
                table: "Bases",
                column: "Curtida_UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Bases_Post_AutorId",
                table: "Bases",
                column: "Post_AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Bases_PostId",
                table: "Bases",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Bases_UsuarioId",
                table: "Bases",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Bases_UsuarioId1",
                table: "Bases",
                column: "UsuarioId1");

            migrationBuilder.CreateIndex(
                name: "IX_Bases_UsuarioId2",
                table: "Bases",
                column: "UsuarioId2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bases");
        }
    }
}
