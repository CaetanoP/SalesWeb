using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesWebMVc.Migrations
{
    /// <inheritdoc />
    public partial class RenameBaseSalaryColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BaseSalay",
                table: "Seller",
                newName: "BaseSalary");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BaseSalary",
                table: "Seller",
                newName: "BaseSalay");
        }
    }
}
