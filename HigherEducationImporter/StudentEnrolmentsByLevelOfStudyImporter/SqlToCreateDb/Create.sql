create table IF NOT EXISTS LevelOfStudy
(
    Id   integer
        constraint LevelOfStudy_pk
            primary key autoincrement,
    Name Varchar(50)
);

create table IF NOT EXISTS ModeOfStudy
(
    Id   integer
        constraint ModeOfStudy_pk
            primary key autoincrement,
    Name Varchar(50)
);

create table IF NOT EXISTS AcademicYear
(
    Id   integer
        constraint AcademicYear_pk
            primary key autoincrement,
    Name Varchar(50)
);

create table IF NOT EXISTS FirstYearMarker
(
    Id   integer
        constraint FirstYearMarker_pk
            primary key autoincrement,
    Name Varchar(50)
);


create table IF NOT EXISTS Country
(
    Id   integer
        constraint Country_pk
            primary key autoincrement,
    Name Varchar(50)
);

create table IF NOT EXISTS Sex
(
    Id   integer
        constraint Sex_pk
            primary key autoincrement,
    Name Varchar(50)
);

create table IF NOT EXISTS Domicile
(
    Id   integer
        constraint Domicile_pk
            primary key autoincrement,
    Name Varchar(50)
);

create table IF NOT EXISTS StudentEnrolmentsByLevelOfStudy
(
    Id                integer
        constraint StudentEnrolmentsByLevelOfStudy_pk
            primary key autoincrement,
    LevelOfStudyId    integer not null
        constraint StudentEnrolmentsByLevelOfStudy_LevelOfStudy_fk
            references LevelOfStudy,
    FirstYearMarkerId integer not null
        constraint StudentEnrolmentsByLevelOfStudy_FirstYearMarker_fk
            references FirstYearMarker,
    ModeOfStudyId     integer not null
        constraint StudentEnrolmentsByLevelOfStudy_ModeOfStudy_fk
            references ModeOfStudy,
    CountryId         integer not null
        constraint StudentEnrolmentsByLevelOfStudy_Country_fk
            references Country,
    SexId             integer not null
        constraint StudentEnrolmentsByLevelOfStudy_Sex_fk
            references Sex,
    DomicileId        integer not null
        constraint StudentEnrolmentsByLevelOfStudy_Domicile_fk
            references Domicile,
    AcademicYearId    integer not null
        constraint StudentEnrolmentsByLevelOfStudy_AcademicYear_fk
            references AcademicYear,
    Number            integer not null,
    Percentage        decimal not null
);


create view IF NOT EXISTS ReadableStudentEnrolmentsByLevel as
select LOS.Name as LevelOfStudy,
       FYM.Name as FirstYearMarker,
       MOS.Name as ModeOfStudy,
       C.Name   as Country,
       S.Name   as Sex,
       D.Name   as Domicile,
       AY.Name  as AcademicYear,
       Number,
       Percentage
from StudentEnrolmentsByLevelOfStudy r
         inner join AcademicYear AY on AY.Id = r.AcademicYearId
         inner join Country C on r.CountryId = C.Id
         inner join Domicile D on r.DomicileId = D.Id
         inner join FirstYearMarker FYM on r.FirstYearMarkerId = FYM.Id
         inner join LevelOfStudy LOS on r.LevelOfStudyId = LOS.Id
         inner join ModeOfStudy MOS on r.ModeOfStudyId = MOS.Id
         inner join Sex S on r.SexId = S.Id