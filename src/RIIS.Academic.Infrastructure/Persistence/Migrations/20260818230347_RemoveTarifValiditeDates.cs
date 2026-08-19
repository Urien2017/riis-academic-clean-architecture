using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RIIS.Academic.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTarifValiditeDates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateDebutValidite",
                table: "TarifsScolarite");

            migrationBuilder.DropColumn(
                name: "DateFinValidite",
                table: "TarifsScolarite");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DateDebutValidite",
                table: "TarifsScolarite",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateFinValidite",
                table: "TarifsScolarite",
                type: "date",
                nullable: true);
        }
    }
}
