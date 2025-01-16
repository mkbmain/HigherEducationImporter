Imports System.IO
Imports ImportStudentDataVB.BaseDb
Imports ImportStudentDataVB.BaseDb.Models
Imports ImportStudentDataVB.StudentEnrolmentsBySubjectOfStudyImporter.DbModels
Imports Mkb.DapperRepo.Repo

Namespace StudentEnrolmentsBySubjectOfStudyImporter
    Public Class StudentEnrolmentsBySubjectOfStudyImporter
        Implements IImporter
        Private ReadOnly Property Repo As SqlRepo

        Public Sub New(repo As SqlRepo)
            Me.Repo = repo
        End Sub

        Public ReadOnly Property Name As String Implements IImporter.Name
            Get
                Return NameOf(StudentEnrolmentsBySubjectOfStudyImporter)
            End Get
        End Property

        Public Sub ImportCsv_BuildDb() Implements IImporter.BuildDb
            Repo.Execute(File.ReadAllText("StudentEnrolmentsBySubjectOfStudyImporter/SqlToCreateDb/Create.sql"))
        End Sub

        Public Sub ImportDataFromFile(location As String) Implements IImporter.ImportDataFromFile

            If File.Exists(location) = False Then
                Console.Write("input data does not exist")
                Return
            End If

            For Each item In
                GetRowsOfT(location, Function(line) LineToStudentEnrolmentsBySubjectOfStudy(line, Repo), 15) _
                .Where(Function(e) e IsNot Nothing).Chunk(250)
                BulkInsertRaw(item, Repo)
            Next
        End Sub

        Private Shared Sub BulkInsertRaw(raws As IEnumerable(Of StudentEnrolmentsBySubjectOfStudy), repo As SqlRepo)
            Const query =
                      "insert into StudentEnrolmentsBySubjectOfStudy (CahLevelMarkerId, CAHLevelSubjectId, LevelOfStudyId, ModeOfStudyId, AcademicYearId, CategoryMarkerId, CategoryId, Number) values "
            Dim sqlValues = raws.Select(Function(e) _
                                           $"({e.CahLevelMarkerId},{e.CAHLevelSubjectId},{e.LevelOfStudyId _
                                               },{e.ModeOfStudyId}," +
                                           $"{e.AcademicYearId},{e.CategoryMarkerId},{e.CategoryId},{e.Number})")

            repo.Execute(query + String.Join(",", sqlValues))
        End Sub

        Private Shared Function LineToStudentEnrolmentsBySubjectOfStudy(line As String, repo As SqlRepo) As StudentEnrolmentsBySubjectOfStudy
            Dim parts As String() = line.Split(",")
            If parts.Length <> 8 Then
                Return Nothing
            End If
            Return New StudentEnrolmentsBySubjectOfStudy With {
                .CahLevelMarkerId = repo.FindOrCreate(Of CahLevelMarker)(parts(0)),
                .CAHLevelSubjectId = repo.FindOrCreate(Of CahLevelSubject)(parts(1)),
                .LevelOfStudyId = repo.FindOrCreate(Of LevelOfStudy)(parts(2)),
                .ModeOfStudyId = repo.FindOrCreate(Of ModeOfStudy)(parts(3)),
                .AcademicYearId = repo.FindOrCreate(Of AcademicYear)(parts(4)),
                .CategoryMarkerId = repo.FindOrCreate(Of CategoryMarker)(parts(5)),
                .CategoryId = repo.FindOrCreate(Of Category)(parts(6)),
                .Number = Integer.Parse(parts(7))}
        End Function
    End Class
End NameSpace