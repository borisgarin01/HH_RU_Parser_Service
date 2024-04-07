using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HH_RU_ParserService.Migrations
{
    [Migration(1, "InitialMigration_07_04_2024_14_45")]
    public class InitialMigration : Migration
    {
        public override void Down()
        {
            Delete.Table("Addresses");
            Delete.Table("Areas");
            Delete.Table("Brandings");
            Delete.Table("Employers");
            Delete.Table("Employments");
            Delete.Table("Experiences");
            Delete.Table("InsidersInterviews");
            Delete.Table("Items");
            Delete.Table("Logo_URLs");
            Delete.Table("Metros");
            Delete.Table("Metro_Stations");
            Delete.Table("Professional_Roles");
            Delete.Table("Roots");
            Delete.Table("Salaries");
            Delete.Table("Schedules");
            Delete.Table("Snippets");
            Delete.Table("Types");
        }

        public override void Up()
        {
            Create.Table("Addresses")
                .WithColumn("City").AsString().Nullable()
                .WithColumn("Metro_Id").AsString().Nullable()
                .WithColumn("Street").AsString().Nullable()
                .WithColumn("Building").AsString().Nullable()
                .WithColumn("Lat").AsDouble().Nullable()
                .WithColumn("Lng").AsDouble().Nullable()
                .WithColumn("Description").AsString().Nullable()
                .WithColumn("Raw").AsString().Nullable()
                .WithColumn("Id").AsString().Nullable()
                .WithColumn("DB_Id").AsGuid().NotNullable().PrimaryKey().Identity();

            Create.Table("Areas")
                .WithColumn("DB_Id").AsGuid().NotNullable().PrimaryKey().Identity()
                .WithColumn("Id").AsString().Nullable()
                .WithColumn("Name").AsString().Nullable()
                .WithColumn("Url").AsString().Nullable();

            Create.Table("Brandings")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("Type").AsString().Nullable()
                .WithColumn("Tariff").AsString().Nullable();

            Create.Table("Employers")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("Id").AsString().Nullable()
                .WithColumn("Name").AsString().Nullable()
                .WithColumn("URL").AsString().Nullable()
                .WithColumn("Alternate_URL").AsString().Nullable()
                .WithColumn("Vacancies_URL").AsString().Nullable()
                .WithColumn("Accredited_It_Employer").AsBoolean().Nullable()
                .WithColumn("Logo_Urls_Id").AsBoolean().Nullable() // Original
                .WithColumn("Trusted").AsBoolean();

            Create.Table("Employments")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("Id").AsString().Nullable()
                .WithColumn("Name").AsString().Nullable();

            Create.Table("Experiences")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("Id").AsString().Nullable()
                .WithColumn("Name").AsString().Nullable();

            Create.Table("InsidersInterviews")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("Id").AsString().Nullable()
                .WithColumn("URL").AsString().Nullable();

            Create.Table("Items")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("Id").AsString().Nullable()
                .WithColumn("Premium").AsBoolean().Nullable()
                .WithColumn("Name").AsString().Nullable()
                .WithColumn("Has_Test").AsBoolean().Nullable()
                .WithColumn("Response_Letter_Required").AsBoolean().Nullable()
                .WithColumn("Created_At").AsString().Nullable()
                .WithColumn("Archived").AsBoolean().Nullable()
                .WithColumn("Apply_Alternate_URL").AsString().Nullable()
                .WithColumn("Show_Logo_In_Search").AsBoolean().Nullable()
                .WithColumn("Address_Id").AsString().Nullable()
                .WithColumn("Insider_Interview_Id").AsString().Nullable()
                .WithColumn("Employer_Id").AsString().Nullable()
                .WithColumn("Snippet_Id").AsString().Nullable()
                .WithColumn("Schedule_Id").AsString().Nullable()
                .WithColumn("Experience_Id").AsString().Nullable()
                .WithColumn("Employment_Id").AsString().Nullable()
                .WithColumn("Branding_Id").AsString().Nullable()
                .WithColumn("URL").AsString().Nullable()
                .WithColumn("Alternate_URL").AsString().Nullable()
                .WithColumn("Accept_Temporary").AsBoolean().Nullable()
                .WithColumn("Accept_Incomplete_Resumes").AsBoolean().Nullable()
                .WithColumn("Is_Adv_Vacancy").AsBoolean().Nullable();

            Create.Table("Logo_URLs")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("_240").AsString().Nullable()
                .WithColumn("_90").AsString().Nullable()
                .WithColumn("Original").AsString().Nullable();

            Create.Table("Metros")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("Station_Name").AsString().Nullable()
                .WithColumn("Line_Name").AsString().Nullable()
                .WithColumn("Station_Id").AsString().Nullable()
                .WithColumn("Line_Id").AsString().Nullable()
                .WithColumn("Lat").AsDouble().Nullable()
                .WithColumn("Lng").AsDouble().Nullable();

            Create.Table("Metro_Stations")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("Station_Name").AsString().Nullable()
                .WithColumn("Line_Name").AsString().Nullable()
                .WithColumn("Station_Id").AsString().Nullable().PrimaryKey()
                .WithColumn("Line_Id").AsString().Nullable()
                .WithColumn("Lat").AsDouble().Nullable()
                .WithColumn("Lng").AsDouble().Nullable();

            Create.Table("Professional_Roles")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("Id").AsString().Nullable()
                .WithColumn("Name").AsString().Nullable();

            Create.Table("Roots")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("Found").AsInt32().Nullable()
                .WithColumn("Pages").AsInt32().Nullable()
                .WithColumn("Per_Page").AsInt32().Nullable()
                .WithColumn("Alternate_URL").AsString().Nullable();

            Create.Table("Salaries")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("From").AsInt32().Nullable()
                .WithColumn("To").AsInt32().Nullable()
                .WithColumn("Currency").AsString().Nullable()
                .WithColumn("Gross").AsBoolean().Nullable();

            Create.Table("Schedules")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("Id").AsString().Nullable()
                .WithColumn("Name").AsString().Nullable();

            Create.Table("Snippets")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("Requirement").AsString().Nullable()
                .WithColumn("Responsibility").AsString().Nullable();

            Create.Table("Types")
                .WithColumn("DB_Id").AsGuid().NotNullable().Identity().PrimaryKey()
                .WithColumn("Id").AsString().Nullable()
                .WithColumn("Name").AsString().Nullable();
        }
    }
}
