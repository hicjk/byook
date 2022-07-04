using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Byook.DataAccess.Migrations
{
    public partial class CreateTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Consumer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false, comment: "아이디"),
                    Password = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false, comment: "비밀번호"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false, comment: "성명"),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false, comment: "핸드폰 번호"),
                    Address = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false, comment: "구매자 주소")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Consumer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seller",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false, comment: "사업자등록번호"),
                    Password = table.Column<string>(type: "TEXT", maxLength: 512, nullable: false, comment: "비밀번호"),
                    Name = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false, comment: "대표자명"),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false, comment: "핸드폰번호"),
                    TradeName = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false, comment: "상호명"),
                    Address = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false, comment: "판매자 주소"),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seller", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false, comment: "상품번호")
                        .Annotation("Sqlite:Autoincrement", true),
                    SellerId = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false, comment: "사업자등록번호"),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "등록날짜"),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, comment: "제품명"),
                    Price = table.Column<int>(type: "INTEGER", nullable: false, comment: "가격"),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false, comment: "무게"),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    ProductId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_Seller_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Seller",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "TEXT", maxLength: 18, nullable: false, comment: "주문번호"),
                    ConsumerId = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false, comment: "소비자"),
                    ProductId = table.Column<int>(type: "INTEGER", nullable: false, comment: "상품번호")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_Consumer_ConsumerId",
                        column: x => x.ConsumerId,
                        principalTable: "Consumer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_ConsumerId",
                table: "Order",
                column: "ConsumerId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ProductId",
                table: "Order",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductId",
                table: "Product",
                column: "ProductId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Consumer");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Seller");
        }
    }
}
