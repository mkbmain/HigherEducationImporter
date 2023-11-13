Imports System.IO
Imports ImportStudentDataVB.BaseDb
Imports ImportStudentDataVB.StudentEnrolmentsByLevelOfStudyImporter.DbModels
Imports ImportStudentDataVB.StudentEnrolmentsBySubjectOfStudyImporter.DbModels
Imports Mkb.DapperRepo.Repo

Namespace StudentEnrolmentsBySubjectOfStudyImporter
    Public Class StudentEnrolmentsBySubjectOfStudyImporter
        Implements IImporter
        Private ReadOnly Property Repo As SqlRepo

        Public Sub New(repo as SqlRepo)
            Me.Repo = repo
        End Sub

        Public readonly Property Name() As String Implements IImporter.Name
            Get
                Return NameOf(StudentEnrolmentsBySubjectOfStudyImporter)
            End Get
        End Property

        Public Sub ImportCsv_BuildDb() Implements IImporter.BuildDb
            repo.Execute(File.ReadAllText("StudentEnrolmentsBySubjectOfStudyImporter/SqlToCreateDb/Create.sql"))
        End Sub

        Public Sub ImportDataFromFile(location as String) Implements IImporter.ImportDataFromFile

            if File.Exists(location) = False Then
                Console.Write("input data does not exist")
                Return
            End If

            for Each item in _
                GetRowsOfT (of StudentEnrolmentsBySubjectOfStudy)(location, function(line)  LineToStudentEnrolmentsBySubjectOfStudy(line, repo),15) _
                .Where(Function(e) e isnot nothing).Chunk(250)
                BulkInsertRaw(item, repo)
            Next
        End Sub

        private Shared Sub BulkInsertRaw(raws As IEnumerable(Of StudentEnrolmentsBySubjectOfStudy), repo As SqlRepo)
            Const query =
                      "insert into StudentEnrolmentsBySubjectOfStudy (CahLevelMarkerId, CAHLevelSubjectId, LevelOfStudyId, ModeOfStudyId, AcademicYearId, CategoryMarkerId, CategoryId, Number) values "
            dim sqlValues = raws.Select(Function(e) _
                                           $"({e.CahLevelMarkerId},{e.CAHLevelSubjectId},{e.LevelOfStudyId _
                                               },{e.ModeOfStudyId}," +
                                           $"{e.AcademicYearId},{e.CategoryMarkerId},{e.CategoryId},{e.Number})")

            repo.Execute(query + String.Join(",", sqlValues))
        End Sub

        private Shared Function LineToStudentEnrolmentsBySubjectOfStudy(line as String, repo As SqlRepo) As StudentEnrolmentsBySubjectOfStudy
            dim parts as String() = line.Split(",")
            if parts.Length <> 8 Then
                Return Nothing
            End If
            Return new StudentEnrolmentsBySubjectOfStudy With{
                .CahLeveLMarkerId = repo.FindOrCreate ( of CahLevelMarker)(parts(0)),
                .CAHLevelSubjectId =repo.FindOrCreate ( of CahLevelSubject)(parts(1)),
                .LevelOfStudyId =repo.FindOrCreate ( of LevelOfStudy)(parts(2)),
                .ModeOfStudyId =repo.FindOrCreate ( of ModeOfStudy)(parts(3)),
                .AcademicYearId = repo.FindOrCreate ( of AcademicYear)(parts(4)),
                .CategoryMarkerId = repo.FindOrCreate ( of CategoryMarker)(parts(5)),
                .CategoryId = repo.FindOrCreate ( of Category)(parts(6)),
                .Number = Integer.Parse(parts(7))}
        End Function
    End Class
End NameSpace