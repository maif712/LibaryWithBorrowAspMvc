using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibaryWithBorrowAspMvc.Migrations
{
    /// <inheritdoc />
    public partial class AddingNewPropIntoBookTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBorrowed",
                table: "Books",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBorrowed",
                table: "Books");
        }
    }
}
