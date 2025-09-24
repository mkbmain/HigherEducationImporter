Imports System.IO
Imports ImportStudentDataVB.BaseDb
Imports ImportStudentDataVB.BaseDb.Models
Imports ImportStudentDataVB.StudentEnrolmentsByLevelOfStudyImporter.DbModels
Imports Mkb.DapperRepo.Repo

Namespace StudentEnrolmentsByLevelOfStudyImporter
    Public Class StudentEnrolmentsByLevelOfStudyImporter
        Implements IImporter
        Private ReadOnly Property Repo As SqlRepo

        Public Sub New(repo as SqlRepo)
            Me.Repo = repo
        End Sub

        Public ReadOnly Property Name As String Implements IImporter.Name
            Get
                Return NameOf(StudentEnrolmentsByLevelOfStudyImporter)
            End Get
        End Property

        Public Sub ImportCsv_BuildDb() Implements IImporter.BuildDb
            repo.Execute(File.ReadAllText("StudentEnrolmentsByLevelOfStudyImporter/SqlToCreateDb/Create.sql"))
        End Sub

        Public Sub ImportDataFromFile(location as String) Implements IImporter.ImportDataFromFile

            if File.Exists(location) = False Then
                Console.Write("input data does not exist")
                Return
            End If

            For Each item In _
                GetRowsOfT(location, Function(line) LineToStudentEnrolments(line, Repo), 18) _
                    .Where(Function(e) e IsNot Nothing).Chunk(250)
                BulkInsertRaw(item, Repo)
            Next
        End Sub

        private Shared Sub BulkInsertRaw(raws As IEnumerable(Of StudentEnrolmentsByLevel), repo As SqlRepo)
            Const query =
                      "insert into StudentEnrolmentsByLevelOfStudy (LevelOfStudyId, FirstYearMarkerId, ModeOfStudyId, CountryId, SexId, DomicileId, AcademicYearId, Number, Percentage) values "
            dim sqlValues = raws.Select(Function(e) _
                                           $"({e.LevelOfStudyId},{e.FirstYearMarkerId},{e.ModeOfStudyId},{e.CountryId}," +
                                           $"{e.SexId},{e.DomicileId},{e.AcademicYearId},{e.Number},{e.Percentage})")

            repo.Execute(query + String.Join(",", sqlValues))
        End Sub

        private Shared Function LineToStudentEnrolments(line as String, repo As SqlRepo) As StudentEnrolmentsByLevel
            dim parts as String() = line.Split(",")
            if parts.Length <> 9 Then
                Return Nothing
            End If
            Return new StudentEnrolmentsByLevel With{
                .LevelOfStudyId = repo.FindOrCreate ( of LevelOfStudy)(parts(0)),
                .FirstYearMarkerId =repo.FindOrCreate ( of FirstYearMarker)(parts(1)),
                .ModeOfStudyId =repo.FindOrCreate ( of ModeOfStudy)(parts(2)),
                .CountryId =repo.FindOrCreate ( of Country)(parts(3)),
                .SexId = repo.FindOrCreate ( of Sex)(parts(4)),
                .DomicileId = repo.FindOrCreate ( of Domicile)(parts(5)),
                .AcademicYearId = repo.FindOrCreate ( of AcademicYear)(parts(6)),
                .Number = Integer.Parse(parts(7)),
                .Percentage = Decimal.Parse(if (parts(8) = String.Empty, "0", parts(8).Replace("%", "")))}
        End Function
    End Class
End NameSpace