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

create table IF NOT EXISTS CAHLevelMarker
(
    Id   integer
    constraint CahLevelMarker_pk
    primary key autoincrement,
    Name Varchar(50)
    );

create table IF NOT EXISTS CAHLevelSubject
(
    Id   integer
    constraint CahLevelSubject_pk
    primary key autoincrement,
    Name Varchar(50)
    );


create table IF NOT EXISTS Category
(
    Id   integer
    constraint Category_pk
    primary key autoincrement,
    Name Varchar(50)
    );


create table IF NOT EXISTS CategoryMarker
(
    Id   integer
    constraint CategoryMarker_pk
    primary key autoincrement,
    Name Varchar(50)
    );



create table IF NOT EXISTS StudentEnrolmentsBySubjectOfStudy
(
    Id                integer
    constraint StudentEnrolmentsBySubjectOfStudy_pk
    primary key autoincrement,
    CahLevelMarkerId    integer not null
    constraint StudentEnrolmentsBySubjectOfStudy_CahLevelMArker_FK
    references CAHLevelMarker,
    CAHLevelSubjectId integer not null
    constraint StudentEnrolmentsBySubjectOfStudy_CAHLevelSubject_FK
    references CAHLevelSubject,
    LevelOfStudyId     integer not null
    constraint StudentEnrolmentsBySubjectOfStudy_LevelOfStudy_FK
    references LevelOfStudy,
    ModeOfStudyId         integer not null
    constraint StudentEnrolmentsBySubjectOfStudy_ModeOfStudy_FK
    references ModeOfStudy,
    AcademicYearId             integer not null
    constraint StudentEnrolmentsBySubjectOfStudy_AcademicYear_FK
    references AcademicYear,
    CategoryMarkerId        integer not null
    constraint StudentEnrolmentsBySubjectOfStudy_CategoryMarker_FK
    references CategoryMarker,
    CategoryId    integer not null
    constraint StudentEnrolmentsBySubjectOfStudy_Category_FK
    references Category,
    Number     integer not null
);


create view IF NOT EXISTS ReadableStudentEnrolmentsBySubjectOfStudy as

select CLM.Name as CAHLevelMarker,
       CLS.Name as CAHLevelSubject,
       LOS.Name as LevelOfStudy,
       MOS.Name as ModeOfStudy,
       AY.Name as AcademicYear,
       CM.Name as CategoryMarker,
       C.Name as Category,
       Number from StudentEnrolmentsBySubjectOfStudy t
                       inner join CAHLevelMarker clm on clm.Id = t.CahLevelMarkerId
                       inner join CAHLevelSubject CLS on CLS.Id = t.CAHLevelSubjectId
                       inner join LevelOfStudy LOS on LOS.Id = t.LevelOfStudyId
                       inner join ModeOfStudy MOS on t.ModeOfStudyId = MOS.Id
                       inner join AcademicYear AY on AY.Id = t.AcademicYearId
                       inner join CategoryMarker CM on CM.Id = t.CategoryMarkerId
                       inner join Category C on t.CategoryId = C.Id
