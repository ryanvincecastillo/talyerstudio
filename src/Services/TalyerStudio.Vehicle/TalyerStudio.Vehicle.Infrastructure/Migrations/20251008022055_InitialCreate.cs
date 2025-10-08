using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalyerStudio.Vehicle.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    make = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    model = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    year = table.Column<int>(type: "integer", nullable: false),
                    color = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    plate_number = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    engine_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    chassis_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    vehicle_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    vehicle_category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    displacement = table.Column<int>(type: "integer", nullable: true),
                    fuel_type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    transmission = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    current_odometer = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    or_cr_expiry_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    tire_size_front = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    tire_size_rear = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    images = table.Column<string[]>(type: "text[]", nullable: false),
                    qr_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vehicles", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "idx_vehicles_customer_id",
                table: "vehicles",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "idx_vehicles_plate_number",
                table: "vehicles",
                column: "plate_number");

            migrationBuilder.CreateIndex(
                name: "idx_vehicles_tenant_id",
                table: "vehicles",
                column: "tenant_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "vehicles");
        }
    }
}
