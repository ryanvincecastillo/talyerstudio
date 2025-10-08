using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalyerStudio.JobOrder.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "job_orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_order_number = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    tenant_id = table.Column<Guid>(type: "uuid", nullable: false),
                    branch_id = table.Column<Guid>(type: "uuid", nullable: true),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    vehicle_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    priority = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    odometer_reading = table.Column<int>(type: "integer", nullable: false),
                    customer_complaints = table.Column<string>(type: "text", nullable: false),
                    inspection_notes = table.Column<string>(type: "text", nullable: false),
                    before_photos = table.Column<string[]>(type: "text[]", nullable: false),
                    after_photos = table.Column<string[]>(type: "text[]", nullable: false),
                    assigned_mechanic_ids = table.Column<Guid[]>(type: "uuid[]", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    estimated_completion_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    total_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    discount_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    tax_amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false, defaultValue: 0m),
                    grand_total = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_orders", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "job_order_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_id = table.Column<Guid>(type: "uuid", nullable: false),
                    service_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_order_items", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_order_items_job_orders_job_order_id",
                        column: x => x.job_order_id,
                        principalTable: "job_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "job_order_parts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    job_order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    product_sku = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    subtotal = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    notes = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    deleted_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_order_parts", x => x.id);
                    table.ForeignKey(
                        name: "FK_job_order_parts_job_orders_job_order_id",
                        column: x => x.job_order_id,
                        principalTable: "job_orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "idx_job_order_items_job_order_id",
                table: "job_order_items",
                column: "job_order_id");

            migrationBuilder.CreateIndex(
                name: "idx_job_order_items_service_id",
                table: "job_order_items",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "idx_job_order_parts_job_order_id",
                table: "job_order_parts",
                column: "job_order_id");

            migrationBuilder.CreateIndex(
                name: "idx_job_order_parts_product_id",
                table: "job_order_parts",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "idx_job_orders_customer_id",
                table: "job_orders",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "idx_job_orders_number",
                table: "job_orders",
                column: "job_order_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_job_orders_status",
                table: "job_orders",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_job_orders_tenant_id",
                table: "job_orders",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "idx_job_orders_tenant_status",
                table: "job_orders",
                columns: new[] { "tenant_id", "status" });

            migrationBuilder.CreateIndex(
                name: "idx_job_orders_vehicle_id",
                table: "job_orders",
                column: "vehicle_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "job_order_items");

            migrationBuilder.DropTable(
                name: "job_order_parts");

            migrationBuilder.DropTable(
                name: "job_orders");
        }
    }
}
