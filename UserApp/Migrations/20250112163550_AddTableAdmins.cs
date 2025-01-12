using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Migrations;
using Resturant.Models;
using System.Text;

#nullable disable

namespace Resturant.Migrations
{
    /// <inheritdoc />
    public partial class AddTableAdmins : Migration
    {
        const string ADMIN_USER_GUID = "e43fd0db-a940-4a99-a810-d40e280f178a";
        const string ADMIN_ROLE_GUID = "59dfa40e-4e36-4ee5-bc7a-0a7772eff1de";
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetAdmins",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetAdmins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRolesAdmins",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRolesAdmins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRolesAdmins",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRolesAdmins", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRolesAdmins_AspNetAdmins_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRolesAdmins_AspNetRolesAdmins_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRolesAdmins",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            var hasher = new PasswordHasher<Admins>();
            var passwordHash = hasher.HashPassword(null, "Password100!");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO AspNetAdmins(Id, UserName, NormalizedUserName, Email, EmailConfirmed, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled, AccessFailedCount, NormalizedEmail, PasswordHash, SecurityStamp, FirstName)");
            sb.AppendLine("VALUES (");
            sb.AppendLine($"'{ADMIN_USER_GUID}',");
            sb.AppendLine("'admin@gmail.com',");
            sb.AppendLine("'ADMIN@GMAIL.COM',");
            sb.AppendLine("'admin@gmail.com',");
            sb.AppendLine("0,");
            sb.AppendLine("0,");
            sb.AppendLine("0,");
            sb.AppendLine("0,");
            sb.AppendLine("0,");
            sb.AppendLine("'ADMIN@GMAIL.COM',");
            sb.AppendLine($"'{passwordHash}',");
            sb.AppendLine("'',");
            sb.AppendLine("'Admin'");
            sb.AppendLine(")");

            migrationBuilder.Sql(sb.ToString());

            migrationBuilder.Sql($"INSERT INTO AspNetRolesAdmins (Id, Name, NormalizedName) VALUES ('{ADMIN_ROLE_GUID}', 'Admin', 'ADMIN')");
            migrationBuilder.Sql($"INSERT INTO AspNetUserRolesAdmins (UserId, RoleId) VALUES ('{ADMIN_USER_GUID}', '{ADMIN_ROLE_GUID}')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"DELETE FROM AspNetUserRolesAdmins WHERE UserId = '{ADMIN_USER_GUID}' AND RoleId = '{ADMIN_ROLE_GUID}'");
            migrationBuilder.Sql($"DELETE FROM AspNetAdmins WHERE Id = '{ADMIN_USER_GUID}'");
            migrationBuilder.Sql($"DELETE FROM AspNetRolesAdmins WHERE Id = '{ADMIN_ROLE_GUID}'");

            migrationBuilder.DropTable(
                name: "AspNetUserRolesAdmins");

            migrationBuilder.DropTable(
                name: "AspNetRolesAdmins");

            migrationBuilder.DropTable(
                name: "AspNetAdmins");
        }
    }
}
