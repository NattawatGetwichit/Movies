using Microsoft.EntityFrameworkCore.Migrations;

namespace MoviesAPI.Data.Migrations
{
    public partial class add_AdminRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Insert into AspNetRoles(Id,[name],[NormalizedName]) values ('2900383b-04da-4e16-8257-3925157e9e5c','Admin','Admin')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"delete AspNetRoles where Id = '2900383b-04da-4e16-8257-3925157e9e5c'");
        }
    }
}