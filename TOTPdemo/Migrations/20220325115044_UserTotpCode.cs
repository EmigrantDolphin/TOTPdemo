using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TOTPdemo.Migrations
{
    public partial class UserTotpCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TotpSecretCode",
                table: "Users",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotpSecretCode",
                table: "Users");
        }
    }
}
