using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Omega.Infrastructure.Migrations
{
    public partial class AddedServicesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddedDate = table.Column<DateTime>(nullable: false),
                    ModifiedDate = table.Column<DateTime>(nullable: true),
                    Deleted = table.Column<bool>(nullable: false),
                    ServiceId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Price = table.Column<int>(nullable: false),
                    IsDefault = table.Column<bool>(nullable: false),
                    Qty = table.Column<int>(nullable: false),
                    MinQty = table.Column<int>(nullable: false),
                    MaxQty = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    ContractorSharePercent = table.Column<int>(nullable: false),
                    ContractorId = table.Column<int>(nullable: false),
                    ContractorName = table.Column<string>(nullable: true),
                    UnitMeasureId = table.Column<int>(nullable: false),
                    UnitMeasureName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Services");
        }
    }
}
