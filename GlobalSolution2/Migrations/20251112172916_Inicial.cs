using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GlobalSolution2.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "COMPETENCIA",
                columns: table => new
                {
                    ID_COMPETENCIA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME_COMPETENCIA = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    CATEGORIA_COMPETENCIA = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    DESCRICAO_COMPETENCIA = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("COMPETENCIA_PK", x => x.ID_COMPETENCIA);
                });

            migrationBuilder.CreateTable(
                name: "USUARIO",
                columns: table => new
                {
                    ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    NOME_USUARIO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    SENHA_USUARIO = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    AREA_ATUAL = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    AREA_INTERESSE = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    OBJETIVO_CARREIRA = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false),
                    NIVEL_EXPERIENCIA = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("USUARIO_PK", x => x.ID_USUARIO);
                });
            
            migrationBuilder.Sql("ALTER TABLE USUARIO ADD CONSTRAINT CHK_NIVEL_EXPERIENCIA CHECK (NIVEL_EXPERIENCIA IN ('Nenhuma', 'Estagiário', 'Júnior', 'Sênior', 'Pleno'))");

            migrationBuilder.CreateTable(
                name: "RECOMENDACAO",
                columns: table => new
                {
                    ID_RECOMENDACAO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DATA_RECOMENDACAO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    DESCRICAO_RECOMENDACAO = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: false),
                    PROMPT_USADO = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: false),
                    TITULO_RECOMENDACAO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    USUARIO_ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RECOMENDACAO_PK", x => x.ID_RECOMENDACAO);
                    table.ForeignKey(
                        name: "RECOMENDACAO_USUARIO_FK",
                        column: x => x.USUARIO_ID_USUARIO,
                        principalTable: "USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "REGISTRO_BEM_ESTAR",
                columns: table => new
                {
                    ID_REGISTRO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    DATA_REGISTRO = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false),
                    HUMOR_REGISTRO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: false),
                    HORAS_SONO = table.Column<int>(type: "NUMBER(2,0)", precision: 2, scale: 0, nullable: false),
                    HORAS_TRABALHO = table.Column<int>(type: "NUMBER(2,0)", precision: 2, scale: 0, nullable: false),
                    NIVEL_ENERGIA = table.Column<int>(type: "NUMBER(2,0)", precision: 2, scale: 0, nullable: false),
                    NIVEL_ESTRESSE = table.Column<int>(type: "NUMBER(2,0)", precision: 2, scale: 0, nullable: false),
                    OBSERVACAO_REGISTRO = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true),
                    USUARIO_ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("REGISTRO_BEM_ESTAR_PK", x => x.ID_REGISTRO);
                    table.ForeignKey(
                        name: "REGISTRO_BEM_ESTAR_USUARIO_FK",
                        column: x => x.USUARIO_ID_USUARIO,
                        principalTable: "USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });
            
            migrationBuilder.Sql("ALTER TABLE REGISTRO_BEM_ESTAR ADD CONSTRAINT CHK_HUMOR_REGISTRO CHECK (HUMOR_REGISTRO IN ('Feliz', 'Triste', 'Estressado', 'Bravo', 'Calmo'))");

            migrationBuilder.CreateTable(
                name: "USUARIO_COMPETENCIA",
                columns: table => new
                {
                    USUARIO_ID_USUARIO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    COMPETENCIA_ID_COMPETENCIA = table.Column<int>(type: "NUMBER(10)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("USUARIO_COMPETENCIA_PK", x => new { x.USUARIO_ID_USUARIO, x.COMPETENCIA_ID_COMPETENCIA });
                    table.ForeignKey(
                        name: "USUARIO_COMP_COMPETENCIA_FK",
                        column: x => x.COMPETENCIA_ID_COMPETENCIA,
                        principalTable: "COMPETENCIA",
                        principalColumn: "ID_COMPETENCIA",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "USUARIO_COMP_USUARIO_FK",
                        column: x => x.USUARIO_ID_USUARIO,
                        principalTable: "USUARIO",
                        principalColumn: "ID_USUARIO",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RECOMENDACAO_PROFISSIONAL",
                columns: table => new
                {
                    ID_RECOMENDACAO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    CATEGORIA_RECOMENDACAO = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    AREA_RECOMENDACAO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false),
                    FONTE_RECOMENDACAO = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RECOMENDACAO_PROFISSIONAL_PK", x => x.ID_RECOMENDACAO);
                    table.ForeignKey(
                        name: "RECOMENDACAO_PROFISSIONAL_FK",
                        column: x => x.ID_RECOMENDACAO,
                        principalTable: "RECOMENDACAO",
                        principalColumn: "ID_RECOMENDACAO",
                        onDelete: ReferentialAction.Cascade);
                });

                migrationBuilder.Sql("ALTER TABLE RECOMENDACAO_PROFISSIONAL ADD CONSTRAINT CHK_CATEGORIA_RECOMENDACAO CHECK (CATEGORIA_RECOMENDACAO IN ('Vaga', 'Curso'))");

                migrationBuilder.Sql("ALTER TABLE RECOMENDACAO_PROFISSIONAL ADD CONSTRAINT CHK_AREA_RECOMENDACAO CHECK (AREA_RECOMENDACAO IN ('Front-end', 'Back-end', 'DevOps', 'UX/UI', 'Data Science', 'Banco de Dados', 'Governança de TI', 'IA'))");

            migrationBuilder.CreateTable(
                name: "RECOMENDACAO_SAUDE",
                columns: table => new
                {
                    ID_RECOMENDACAO = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TIPO_SAUDE = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    NIVEL_ALERTA = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    MENSAGEM_SAUDE = table.Column<string>(type: "NVARCHAR2(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("RECOMENDACAO_SAUDE_PK", x => x.ID_RECOMENDACAO);
                    table.ForeignKey(
                        name: "RECOMENDACAO_SAUDE_FK",
                        column: x => x.ID_RECOMENDACAO,
                        principalTable: "RECOMENDACAO",
                        principalColumn: "ID_RECOMENDACAO",
                        onDelete: ReferentialAction.Cascade);
                });

                migrationBuilder.Sql("ALTER TABLE RECOMENDACAO_SAUDE ADD CONSTRAINT CHK_TIPO_SAUDE CHECK (TIPO_SAUDE IN ('Sono', 'Produtividade', 'Saúde Mental'))");

                migrationBuilder.Sql("ALTER TABLE RECOMENDACAO_SAUDE ADD CONSTRAINT CHK_NIVEL_ALERTA CHECK (NIVEL_ALERTA IN ('Baixo', 'Moderado', 'Alto'))");

            migrationBuilder.CreateIndex(
                name: "IX_RECOMENDACAO_USUARIO_ID_USUARIO",
                table: "RECOMENDACAO",
                column: "USUARIO_ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "IX_REGISTRO_BEM_ESTAR_USUARIO_ID_USUARIO",
                table: "REGISTRO_BEM_ESTAR",
                column: "USUARIO_ID_USUARIO");

            migrationBuilder.CreateIndex(
                name: "NOME_UNIQUE",
                table: "USUARIO",
                column: "NOME_USUARIO",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_USUARIO_COMPETENCIA_COMPETENCIA_ID_COMPETENCIA",
                table: "USUARIO_COMPETENCIA",
                column: "COMPETENCIA_ID_COMPETENCIA");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RECOMENDACAO_PROFISSIONAL");

            migrationBuilder.DropTable(
                name: "RECOMENDACAO_SAUDE");

            migrationBuilder.DropTable(
                name: "REGISTRO_BEM_ESTAR");

            migrationBuilder.DropTable(
                name: "USUARIO_COMPETENCIA");

            migrationBuilder.DropTable(
                name: "RECOMENDACAO");

            migrationBuilder.DropTable(
                name: "COMPETENCIA");

            migrationBuilder.DropTable(
                name: "USUARIO");
        }
    }
}
