using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Byook.DataAccess.Migrations
{
    public partial class AddProductAndOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Sellers",
                table: "Sellers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Consumers",
                table: "Consumers");

            migrationBuilder.RenameTable(
                name: "Sellers",
                newName: "Seller");

            migrationBuilder.RenameTable(
                name: "Consumers",
                newName: "Consumer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Seller",
                table: "Seller",
                column: "SellerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Consumer",
                table: "Consumer",
                column: "ConsumerId");

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    ProductId = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false, comment: "상품번호"),
                    SellerId = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false, comment: "사업자등록번호"),
                    CreateDate = table.Column<DateTime>(type: "TEXT", nullable: false, comment: "등록날짜"),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false, comment: "제품명"),
                    Price = table.Column<int>(type: "INTEGER", nullable: false, comment: "가격"),
                    Weight = table.Column<int>(type: "INTEGER", nullable: false, comment: "무게")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Product_Seller_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Seller",
                        principalColumn: "SellerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order",
                columns: table => new
                {
                    OrderId = table.Column<string>(type: "TEXT", maxLength: 18, nullable: false, comment: "주문번호"),
                    ConsumerId = table.Column<string>(type: "TEXT", maxLength: 15, nullable: false, comment: "아이디"),
                    ProductId = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false, comment: "상품번호")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Order_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Order_ProductId",
                table: "Order",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_SellerId",
                table: "Product",
                column: "SellerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Seller",
                table: "Seller");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Consumer",
                table: "Consumer");

            migrationBuilder.RenameTable(
                name: "Seller",
                newName: "Sellers");

            migrationBuilder.RenameTable(
                name: "Consumer",
                newName: "Consumers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sellers",
                table: "Sellers",
                column: "SellerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Consumers",
                table: "Consumers",
                column: "ConsumerId");
        }
    }
}
