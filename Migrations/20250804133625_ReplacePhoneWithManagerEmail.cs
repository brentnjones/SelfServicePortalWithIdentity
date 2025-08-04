using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SelfServicePortalWithIdentity.Migrations
{
    /// <inheritdoc />
    public partial class ReplacePhoneWithManagerEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "UserProfiles",
                newName: "ManagerEmailAddress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ManagerEmailAddress",
                table: "UserProfiles",
                newName: "PhoneNumber");
        }
    }
}
