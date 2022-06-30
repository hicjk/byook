using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Byook.DataAccess.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consumers",
                columns: table => new
                {
                    ConsumerId = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false, comment: "아이디"),
                    Password = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false, comment: "비밀번호"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false, comment: "성명"),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false, comment: "핸드폰 번호"),
                    Address = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false, comment: "구매자 주소")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumers", x => x.ConsumerId);
                });

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    SellerId = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false, comment: "사업자등록번호"),
                    Password = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false, comment: "비밀번호"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false, comment: "대표자명"),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false, comment: "핸드폰번호"),
                    TradeName = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false, comment: "상호명"),
                    Address = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false, comment: "판매자 주소")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.SellerId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Consumers");

            migrationBuilder.DropTable(
                name: "Sellers");
        }
    }
}
