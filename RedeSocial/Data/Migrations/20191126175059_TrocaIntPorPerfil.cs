using Microsoft.EntityFrameworkCore.Migrations;

namespace RedeSocial.Data.Migrations
{
    public partial class TrocaIntPorPerfil : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PerfilId",
                table: "Mensagem");

            migrationBuilder.DropColumn(
                name: "Convidado",
                table: "Convite");

            migrationBuilder.DropColumn(
                name: "Convidante",
                table: "Convite");

            migrationBuilder.AddColumn<int>(
                name: "AutorId",
                table: "Mensagem",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConvidadoId",
                table: "Convite",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ConvidanteId",
                table: "Convite",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Mensagem_AutorId",
                table: "Mensagem",
                column: "AutorId");

            migrationBuilder.CreateIndex(
                name: "IX_Convite_ConvidadoId",
                table: "Convite",
                column: "ConvidadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Convite_ConvidanteId",
                table: "Convite",
                column: "ConvidanteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Convite_Perfil_ConvidadoId",
                table: "Convite",
                column: "ConvidadoId",
                principalTable: "Perfil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Convite_Perfil_ConvidanteId",
                table: "Convite",
                column: "ConvidanteId",
                principalTable: "Perfil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mensagem_Perfil_AutorId",
                table: "Mensagem",
                column: "AutorId",
                principalTable: "Perfil",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Convite_Perfil_ConvidadoId",
                table: "Convite");

            migrationBuilder.DropForeignKey(
                name: "FK_Convite_Perfil_ConvidanteId",
                table: "Convite");

            migrationBuilder.DropForeignKey(
                name: "FK_Mensagem_Perfil_AutorId",
                table: "Mensagem");

            migrationBuilder.DropIndex(
                name: "IX_Mensagem_AutorId",
                table: "Mensagem");

            migrationBuilder.DropIndex(
                name: "IX_Convite_ConvidadoId",
                table: "Convite");

            migrationBuilder.DropIndex(
                name: "IX_Convite_ConvidanteId",
                table: "Convite");

            migrationBuilder.DropColumn(
                name: "AutorId",
                table: "Mensagem");

            migrationBuilder.DropColumn(
                name: "ConvidadoId",
                table: "Convite");

            migrationBuilder.DropColumn(
                name: "ConvidanteId",
                table: "Convite");

            migrationBuilder.AddColumn<int>(
                name: "PerfilId",
                table: "Mensagem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Convidado",
                table: "Convite",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Convidante",
                table: "Convite",
                nullable: false,
                defaultValue: 0);
        }
    }
}
