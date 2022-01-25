using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class add_new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_M_Education_TB_M_University_University_Id",
                table: "TB_M_Education");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_M_Profiling_TB_M_Education_Education_Id",
                table: "TB_M_Profiling");

            migrationBuilder.DropIndex(
                name: "IX_TB_M_Education_University_Id",
                table: "TB_M_Education");

            migrationBuilder.RenameColumn(
                name: "Education_Id",
                table: "TB_M_Profiling",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_TB_M_Profiling_Education_Id",
                table: "TB_M_Profiling",
                newName: "IX_TB_M_Profiling_Id");

            migrationBuilder.AddColumn<int>(
                name: "UniversityId",
                table: "TB_M_Education",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Education_UniversityId",
                table: "TB_M_Education",
                column: "UniversityId");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_M_Education_TB_M_University_UniversityId",
                table: "TB_M_Education",
                column: "UniversityId",
                principalTable: "TB_M_University",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TB_M_Profiling_TB_M_Education_Id",
                table: "TB_M_Profiling",
                column: "Id",
                principalTable: "TB_M_Education",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TB_M_Education_TB_M_University_UniversityId",
                table: "TB_M_Education");

            migrationBuilder.DropForeignKey(
                name: "FK_TB_M_Profiling_TB_M_Education_Id",
                table: "TB_M_Profiling");

            migrationBuilder.DropIndex(
                name: "IX_TB_M_Education_UniversityId",
                table: "TB_M_Education");

            migrationBuilder.DropColumn(
                name: "UniversityId",
                table: "TB_M_Education");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "TB_M_Profiling",
                newName: "Education_Id");

            migrationBuilder.RenameIndex(
                name: "IX_TB_M_Profiling_Id",
                table: "TB_M_Profiling",
                newName: "IX_TB_M_Profiling_Education_Id");

            migrationBuilder.CreateIndex(
                name: "IX_TB_M_Education_University_Id",
                table: "TB_M_Education",
                column: "University_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TB_M_Education_TB_M_University_University_Id",
                table: "TB_M_Education",
                column: "University_Id",
                principalTable: "TB_M_University",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TB_M_Profiling_TB_M_Education_Education_Id",
                table: "TB_M_Profiling",
                column: "Education_Id",
                principalTable: "TB_M_Education",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
