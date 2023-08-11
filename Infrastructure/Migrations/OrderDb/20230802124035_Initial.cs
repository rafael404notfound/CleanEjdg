using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations.OrderDb
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchasedLines_Capacity = table.Column<int>(type: "integer", nullable: false),
                    TotalPaymet = table.Column<int>(type: "integer", nullable: false),
                    ShippingAddress_City = table.Column<string>(type: "text", nullable: false),
                    ShippingAddress_Country = table.Column<string>(type: "text", nullable: false),
                    ShippingAddress_Line1 = table.Column<string>(type: "text", nullable: false),
                    ShippingAddress_Line2 = table.Column<string>(type: "text", nullable: false),
                    ShippingAddress_PostalCode = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    SuccessfulPayment = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
