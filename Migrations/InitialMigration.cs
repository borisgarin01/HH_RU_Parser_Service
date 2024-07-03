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
            Execute.Sql(@"
                CREATE TABLE Metros(
                    StationName varchar(255) not null unique,
                    LineName varchar(255) not null unique,
                    StationId varchar(255) not null unique,
                    LineId varchar(255) not null unique,
                    Lat double precision null unique,
                    Lng double precision null unique);

                CREATE TABLE Addresses(City varchar(255) not null,
                    Street varchar(255) null,
                    Building varchar(255) null,
                    Lat double precision null,
                    Lng double precision null,
                    Raw varchar(8191) null,
                    MetroId varchar(255) null,
                    FOREIGN KEY (MetroId) 
                    REFERENCES Metros (StationId),
                    Id varchar(255) primary key not null unique);

                CREATE TABLE MetroStations(
                    StationName varchar(255) not null unique,
                    LineName varchar(255) not null unique,
                    StationId varchar(255) not null unique,
                    LineId varchar(255) not null unique,
                    Lat double precision null unique,
                    Lng double precision null unique,
                    AddressId varchar(255) null,
                    FOREIGN KEY (AddressId) REFERENCES Addresses(Id)
                    );

                CREATE TABLE Areas(Id varchar(255) not null unique,
                    Name varchar(255) not null unique,
                    Url varchar(255) not null unique);

                CREATE TABLE Salaries(Id varchar(255) not null unique,
                    SalaryFrom int null,
                    SalaryTo int null,
                    Currency varchar(255) not null,
                    Gross boolean null);

                CREATE TABLE Types(Id varchar(63) not null primary key unique,
                    Name varchar(63) not null unique);

                CREATE TABLE LogosUrls(_240 varchar(255) null unique,
                    _90 varchar(255) null unique,
                    Original varchar(255) not null primary key unique);

                CREATE TABLE Employers(Id varchar(255) not null primary key unique,
                    Name varchar(255) not null,
                    Url varchar(255) not null unique,
                    AlternateUrl varchar(255) not null unique,
                    LogosUrlsOriginal varchar(255) null unique,
                    constraint FK_LogosUrlsOriginal
                    FOREIGN KEY (LogosUrlsOriginal) 
                    REFERENCES LogosUrls (Original),
                    VacanciesUrl varchar(255) not null unique,
                    AccreditedItEmployer boolean,
                    Trusted boolean);
    
                
                CREATE TABLE Schedules(Id varchar(63) not null primary key, Name varchar(63) not null unique);

                CREATE TABLE ProfessionalRoles(Id varchar(255) not null primary key, Name varchar(255) not null);

                CREATE TABLE Experiences(Id varchar(63) not null primary key, Name varchar(63) not null unique);

                CREATE TABLE Employments(Id varchar(63) not null primary key, Name varchar(63) not null unique);

                CREATE TABLE Brandings(Id varchar(255) not null primary key, 
                    Type not null unique, Tariff null unique);

                CREATE TABLE InsidersInterviews(Id varchar(255) not null primary key,
                    Url varchar(255) not null);

                CREATE TABLE Items(Id varchar(255) not null unique primary key,
                    Premium boolean null,
                    Name varchar(255) not null,
                    HasTest boolean null,
                    ResponseLetterRequired boolean null,
                    AreaId varchar(255) not null,
                    FOREIGN KEY(AreaId) REFERENCES Areas(Id),
                    TypeId varchar(63) not null,
                    FOREIGN KEY(TypeId) REFERENCES Types(Id),
                    AddressId varchar(255) null,
                    FOREIGN KEY (AddressId) REFERENCES Addresses(Id),
                    PublishedAt varchar(255) null,
                    CreatedAt varchar(255) null,
                    Archived boolean null,
                    ApplyAlternateUrl varchar(255) null,
                    ShowLogoInSearch boolean null,
                    InsiderInterviewId varchar(255) null,
                    FOREIGN KEY (InsiderInterviewId) 
                    REFERENCES InsidersInterviews (Id),
                    Url varchar(255) not null unique,
                    AlternateUrl varchar(255) not null unique,
                    EmployerId varchar(255) not null,
                    FOREIGN KEY (EmployerId) 
                    REFERENCES Employers(Id),
                    ScheduleId varchar(63) not null,
                    FOREIGN KEY (ScheduleId) 
                    REFERENCES Schedules(Id),
                    AcceptTemporary boolean null,
                    ExperienceId varchar(63) not null,
                    FOREIGN KEY (ExperienceId) 
                    REFERENCES Experiences(Id),
                    EmploymentId varchar(63) not null,
                    FOREIGN KEY (EmploymentId)
                    REFERENCES Employments(Id),
                    IsAdvVacancy boolean null,
                    AcceptIncompleteResumes boolean null,
                    BrandingId varchar(255) not null,
                    FOREIGN KEY (BrandingId)
                    REFERENCES Brandings(Id));

                CREATE TABLE Snippets(Id varchar(255) not null unique primary key,
                    FOREIGN KEY (Id) REFERENCES Items(Id),
                    Requirement varchar(8191) default 'Not set',
                    Responsibility varchar(8191) default 'Not set');
            ");
        }
    }
}
