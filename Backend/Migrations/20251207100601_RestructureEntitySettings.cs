using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fork.Migrations
{
    /// <inheritdoc />
    public partial class RestructureEntitySettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutomationTime_ServerSet_ServerId",
                table: "AutomationTime");

            migrationBuilder.DropForeignKey(
                name: "FK_ServerSet_JavaSettings_JavaSettingsId",
                table: "ServerSet");

            migrationBuilder.DropForeignKey(
                name: "FK_ServerSet_ServerVersion_VersionId",
                table: "ServerSet");

            migrationBuilder.DropIndex(
                name: "IX_ServerSet_JavaSettingsId",
                table: "ServerSet");

            migrationBuilder.DropIndex(
                name: "IX_ServerSet_VersionId",
                table: "ServerSet");

            migrationBuilder.DropIndex(
                name: "IX_AutomationTime_ServerId",
                table: "AutomationTime");

            migrationBuilder.DropColumn(
                name: "JavaSettingsId",
                table: "ServerSet");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ServerSet");

            migrationBuilder.DropColumn(
                name: "ServerIconId",
                table: "ServerSet");

            migrationBuilder.DropColumn(
                name: "VersionId",
                table: "ServerSet");

            migrationBuilder.DropColumn(
                name: "ServerId",
                table: "AutomationTime");

            migrationBuilder.RenameColumn(
                name: "StartWithFork",
                table: "ServerSet",
                newName: "EntitySettingsId");

            migrationBuilder.AddColumn<ulong>(
                name: "EntitySettingsId",
                table: "AutomationTime",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EntitySettings",
                columns: table => new
                {
                    Id = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServerVersionId = table.Column<ulong>(type: "INTEGER", nullable: true),
                    JavaSettingsId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    ServerIconId = table.Column<int>(type: "INTEGER", nullable: false),
                    EntityName = table.Column<string>(type: "TEXT", nullable: true),
                    StartWithFork = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntitySettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntitySettings_JavaSettings_JavaSettingsId",
                        column: x => x.JavaSettingsId,
                        principalTable: "JavaSettings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EntitySettings_ServerVersion_ServerVersionId",
                        column: x => x.ServerVersionId,
                        principalTable: "ServerVersion",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServerSet_EntitySettingsId",
                table: "ServerSet",
                column: "EntitySettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomationTime_EntitySettingsId",
                table: "AutomationTime",
                column: "EntitySettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_EntitySettings_JavaSettingsId",
                table: "EntitySettings",
                column: "JavaSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_EntitySettings_ServerVersionId",
                table: "EntitySettings",
                column: "ServerVersionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AutomationTime_EntitySettings_EntitySettingsId",
                table: "AutomationTime",
                column: "EntitySettingsId",
                principalTable: "EntitySettings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerSet_EntitySettings_EntitySettingsId",
                table: "ServerSet",
                column: "EntitySettingsId",
                principalTable: "EntitySettings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AutomationTime_EntitySettings_EntitySettingsId",
                table: "AutomationTime");

            migrationBuilder.DropForeignKey(
                name: "FK_ServerSet_EntitySettings_EntitySettingsId",
                table: "ServerSet");

            migrationBuilder.DropTable(
                name: "EntitySettings");

            migrationBuilder.DropIndex(
                name: "IX_ServerSet_EntitySettingsId",
                table: "ServerSet");

            migrationBuilder.DropIndex(
                name: "IX_AutomationTime_EntitySettingsId",
                table: "AutomationTime");

            migrationBuilder.DropColumn(
                name: "EntitySettingsId",
                table: "AutomationTime");

            migrationBuilder.RenameColumn(
                name: "EntitySettingsId",
                table: "ServerSet",
                newName: "StartWithFork");

            migrationBuilder.AddColumn<ulong>(
                name: "JavaSettingsId",
                table: "ServerSet",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ServerSet",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServerIconId",
                table: "ServerSet",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<ulong>(
                name: "VersionId",
                table: "ServerSet",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<ulong>(
                name: "ServerId",
                table: "AutomationTime",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.CreateIndex(
                name: "IX_ServerSet_JavaSettingsId",
                table: "ServerSet",
                column: "JavaSettingsId");

            migrationBuilder.CreateIndex(
                name: "IX_ServerSet_VersionId",
                table: "ServerSet",
                column: "VersionId");

            migrationBuilder.CreateIndex(
                name: "IX_AutomationTime_ServerId",
                table: "AutomationTime",
                column: "ServerId");

            migrationBuilder.AddForeignKey(
                name: "FK_AutomationTime_ServerSet_ServerId",
                table: "AutomationTime",
                column: "ServerId",
                principalTable: "ServerSet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServerSet_JavaSettings_JavaSettingsId",
                table: "ServerSet",
                column: "JavaSettingsId",
                principalTable: "JavaSettings",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ServerSet_ServerVersion_VersionId",
                table: "ServerSet",
                column: "VersionId",
                principalTable: "ServerVersion",
                principalColumn: "Id");
        }
    }
}
