using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserAuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class YeniAlanEklendi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountNumber",
                table: "SavedTransactions",
                newName: "SenderAccountNumber");

            migrationBuilder.AddColumn<string>(
                name: "RecieverAccountNumber",
                table: "SavedTransactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecieverAccountNumber",
                table: "SavedTransactions");

            migrationBuilder.RenameColumn(
                name: "SenderAccountNumber",
                table: "SavedTransactions",
                newName: "AccountNumber");
        }
    }
}
