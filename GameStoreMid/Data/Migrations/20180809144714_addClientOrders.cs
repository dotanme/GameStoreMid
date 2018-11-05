using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace GameStoreMid.Migrations
{
    public partial class addClientOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderClient_AspNetUsers_ApplicationUserID",
                table: "OrderClient");

            migrationBuilder.DropForeignKey(
                name: "FK_Product_OrderClient_ClientOrderOrderID",
                table: "Product");

            migrationBuilder.DropIndex(
                name: "IX_Product_ClientOrderOrderID",
                table: "Product");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderClient",
                table: "OrderClient");

            migrationBuilder.DropColumn(
                name: "ClientOrderOrderID",
                table: "Product");

            migrationBuilder.RenameTable(
                name: "OrderClient",
                newName: "Order");

            migrationBuilder.RenameIndex(
                name: "IX_OrderClient_ApplicationUserID",
                table: "Order",
                newName: "IX_Order_ApplicationUserID");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Order",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Order",
                table: "Order",
                column: "OrderID");

            migrationBuilder.CreateTable(
                name: "ProductOrder",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false),
                    OrderID = table.Column<int>(nullable: false),
                    OrderID1 = table.Column<int>(nullable: true),
                    ProductID1 = table.Column<int>(nullable: true),
                    Quantity = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductOrder", x => new { x.ProductID, x.OrderID });
                    table.ForeignKey(
                        name: "FK_ProductOrder_Order_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOrder_Order_OrderID1",
                        column: x => x.OrderID1,
                        principalTable: "Order",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductOrder_Product_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductOrder_Product_ProductID1",
                        column: x => x.ProductID1,
                        principalTable: "Product",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrder_OrderID",
                table: "ProductOrder",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrder_OrderID1",
                table: "ProductOrder",
                column: "OrderID1");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrder_ProductID1",
                table: "ProductOrder",
                column: "ProductID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_ApplicationUserID",
                table: "Order",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_ApplicationUserID",
                table: "Order");

            migrationBuilder.DropTable(
                name: "ProductOrder");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Order",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Order");

            migrationBuilder.RenameTable(
                name: "Order",
                newName: "OrderClient");

            migrationBuilder.RenameIndex(
                name: "IX_Order_ApplicationUserID",
                table: "OrderClient",
                newName: "IX_OrderClient_ApplicationUserID");

            migrationBuilder.AddColumn<int>(
                name: "ClientOrderOrderID",
                table: "Product",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderClient",
                table: "OrderClient",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_ClientOrderOrderID",
                table: "Product",
                column: "ClientOrderOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderClient_AspNetUsers_ApplicationUserID",
                table: "OrderClient",
                column: "ApplicationUserID",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Product_OrderClient_ClientOrderOrderID",
                table: "Product",
                column: "ClientOrderOrderID",
                principalTable: "OrderClient",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
