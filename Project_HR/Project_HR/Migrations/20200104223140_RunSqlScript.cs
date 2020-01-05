using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.IO;

namespace Project_HR.Migrations
{
    public partial class RunSqlScript : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //var sqlFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"SQLQuery2.sql");
            //migrationBuilder.Sql(File.ReadAllText(sqlFile));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
