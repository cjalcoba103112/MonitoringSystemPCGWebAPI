using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MonitoringSystemPCGWebAPI.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActivityType",
                columns: table => new
                {
                    ActivityTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityTypeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxCredits = table.Column<int>(type: "int", nullable: true),
                    ResetMonths = table.Column<int>(type: "int", nullable: true),
                    IsMandatoryLeave = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityType", x => x.ActivityTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    DepartmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DepartmentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Department", x => x.DepartmentId);
                });

            migrationBuilder.CreateTable(
                name: "LeaveTypes",
                columns: table => new
                {
                    LeaveTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeaveName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccrualPerMonth = table.Column<int>(type: "int", nullable: true),
                    MaxPerPeriod = table.Column<int>(type: "int", nullable: true),
                    ResetType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Accumulation = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveTypes", x => x.LeaveTypeID);
                });

            migrationBuilder.CreateTable(
                name: "OtpVerifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OtpCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsUsed = table.Column<bool>(type: "bit", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtpVerifications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RankCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Casing = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RankCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "Sidebar",
                columns: table => new
                {
                    SidebarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SidebarName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sidebar", x => x.SidebarId);
                });

            migrationBuilder.CreateTable(
                name: "SidebarRoleMapping",
                columns: table => new
                {
                    SidebarRoleMappingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: true),
                    SidebarId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SidebarRoleMapping", x => x.SidebarRoleMappingId);
                });

            migrationBuilder.CreateTable(
                name: "Usertbl",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usertbl", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Rank",
                columns: table => new
                {
                    RankId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RankCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankLevel = table.Column<int>(type: "int", nullable: true),
                    RankCategoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rank", x => x.RankId);
                    table.ForeignKey(
                        name: "FK_Rank_RankCategory_RankCategoryId",
                        column: x => x.RankCategoryId,
                        principalTable: "RankCategory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Personnel",
                columns: table => new
                {
                    PersonnelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Profile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SerialNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RankId = table.Column<int>(type: "int", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    EmploymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateEnlisted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateEnteredService = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfLastPromotion = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Personnel", x => x.PersonnelId);
                    table.ForeignKey(
                        name: "FK_Personnel_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "DepartmentId");
                    table.ForeignKey(
                        name: "FK_Personnel_Rank_RankId",
                        column: x => x.RankId,
                        principalTable: "Rank",
                        principalColumn: "RankId");
                    table.ForeignKey(
                        name: "FK_Personnel_Usertbl_UserId",
                        column: x => x.UserId,
                        principalTable: "Usertbl",
                        principalColumn: "UserId");
                });

            migrationBuilder.CreateTable(
                name: "EmailEteCommunication",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonnelId = table.Column<int>(type: "int", nullable: true),
                    EmailCategory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NextEte = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RemainingDays = table.Column<int>(type: "int", nullable: true),
                    CommunicationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsAccessed = table.Column<bool>(type: "bit", nullable: true),
                    AccessedDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Response = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportingDocument = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailEteCommunication", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailEteCommunication_Personnel_PersonnelId",
                        column: x => x.PersonnelId,
                        principalTable: "Personnel",
                        principalColumn: "PersonnelId");
                });

            migrationBuilder.CreateTable(
                name: "EnlistmentRecord",
                columns: table => new
                {
                    EnlistmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonnelId = table.Column<int>(type: "int", nullable: true),
                    EnlistmentStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EnlistmentEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContractYears = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReenlistmentFlag = table.Column<bool>(type: "bit", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnlistmentRecord", x => x.EnlistmentId);
                    table.ForeignKey(
                        name: "FK_EnlistmentRecord_Personnel_PersonnelId",
                        column: x => x.PersonnelId,
                        principalTable: "Personnel",
                        principalColumn: "PersonnelId");
                });

            migrationBuilder.CreateTable(
                name: "PersonnelActivity",
                columns: table => new
                {
                    PersonnelActivityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonnelId = table.Column<int>(type: "int", nullable: true),
                    ActivityTypeId = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Days = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsWarningSent = table.Column<bool>(type: "bit", nullable: true),
                    IsFullyApproved = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonnelActivity", x => x.PersonnelActivityId);
                    table.ForeignKey(
                        name: "FK_PersonnelActivity_ActivityType_ActivityTypeId",
                        column: x => x.ActivityTypeId,
                        principalTable: "ActivityType",
                        principalColumn: "ActivityTypeId");
                    table.ForeignKey(
                        name: "FK_PersonnelActivity_Personnel_PersonnelId",
                        column: x => x.PersonnelId,
                        principalTable: "Personnel",
                        principalColumn: "PersonnelId");
                });

            migrationBuilder.CreateTable(
                name: "PersonnelDutyLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonnelId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonnelDutyLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonnelDutyLogs_Personnel_PersonnelId",
                        column: x => x.PersonnelId,
                        principalTable: "Personnel",
                        principalColumn: "PersonnelId");
                });

            migrationBuilder.CreateTable(
                name: "PersonnelPromotion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PersonnelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonnelPromotion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonnelPromotion_Personnel_PersonnelId",
                        column: x => x.PersonnelId,
                        principalTable: "Personnel",
                        principalColumn: "PersonnelId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailEteCommunication_PersonnelId",
                table: "EmailEteCommunication",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_EnlistmentRecord_PersonnelId",
                table: "EnlistmentRecord",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_Personnel_DepartmentId",
                table: "Personnel",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Personnel_RankId",
                table: "Personnel",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_Personnel_UserId",
                table: "Personnel",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelActivity_ActivityTypeId",
                table: "PersonnelActivity",
                column: "ActivityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelActivity_PersonnelId",
                table: "PersonnelActivity",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelDutyLogs_PersonnelId",
                table: "PersonnelDutyLogs",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonnelPromotion_PersonnelId",
                table: "PersonnelPromotion",
                column: "PersonnelId");

            migrationBuilder.CreateIndex(
                name: "IX_Rank_RankCategoryId",
                table: "Rank",
                column: "RankCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Rank_RankLevel",
                table: "Rank",
                column: "RankLevel",
                unique: true,
                filter: "[RankLevel] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailEteCommunication");

            migrationBuilder.DropTable(
                name: "EnlistmentRecord");

            migrationBuilder.DropTable(
                name: "LeaveTypes");

            migrationBuilder.DropTable(
                name: "OtpVerifications");

            migrationBuilder.DropTable(
                name: "PersonnelActivity");

            migrationBuilder.DropTable(
                name: "PersonnelDutyLogs");

            migrationBuilder.DropTable(
                name: "PersonnelPromotion");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Sidebar");

            migrationBuilder.DropTable(
                name: "SidebarRoleMapping");

            migrationBuilder.DropTable(
                name: "ActivityType");

            migrationBuilder.DropTable(
                name: "Personnel");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Rank");

            migrationBuilder.DropTable(
                name: "Usertbl");

            migrationBuilder.DropTable(
                name: "RankCategory");
        }
    }
}
