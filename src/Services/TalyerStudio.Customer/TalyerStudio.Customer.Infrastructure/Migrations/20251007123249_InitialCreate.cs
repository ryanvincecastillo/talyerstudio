using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalyerStudio.Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    first_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    last_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    street = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    barangay = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    municipality = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    province = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    zip_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    birthday = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    loyalty_points = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    customer_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: "INDIVIDUAL"),
                    tags = table.Column<string[]>(type: "text[]", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_customers_email",
                table: "customers",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "idx_customers_phone",
                table: "customers",
                column: "phone_number");

            migrationBuilder.CreateIndex(
                name: "idx_customers_tenant_id",
                table: "customers",
                column: "tenant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customers");
        }
    }
}
