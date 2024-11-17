using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infra.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    TradingName = table.Column<string>(type: "text", nullable: false),
                    Logo_ResourceName = table.Column<string>(type: "text", nullable: true),
                    Logo_ResourceKey = table.Column<string>(type: "text", nullable: true),
                    Logo_IsPrivate = table.Column<bool>(type: "boolean", nullable: true),
                    Customization_ButtonColor = table.Column<string>(type: "text", nullable: true),
                    Customization_PrimaryColor = table.Column<string>(type: "text", nullable: true),
                    Customization_TabBrowserTitle = table.Column<string>(type: "text", nullable: true),
                    Document = table.Column<string>(type: "text", nullable: false),
                    DisabledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    ForceChangePassword = table.Column<bool>(type: "boolean", nullable: false),
                    DisabledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FirstAccess = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastAccess = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Roles = table.Column<string[]>(type: "text[]", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TenantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Company_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Company",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Company_Document",
                table: "Company",
                column: "Document",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Company_Url",
                table: "Company",
                column: "Url",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email_TenantId",
                table: "User",
                columns: new[] { "Email", "TenantId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_TenantId",
                table: "User",
                column: "TenantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Company");
        }
    }
}
