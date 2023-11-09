Imports System.IO
Imports ImportStudentDataVB.Db.StudentBreakDownDb.Models
Imports Mkb.DapperRepo.Repo

Namespace Db.StudentBreakDownDb
    Public Class StudentBreakDownImporter
        Implements IImportDataToDb
        Private Property LookUp As Dictionary(of Type, Dictionary(Of String, BaseLookUpTable))
        Private ReadOnly Property Repo As SqlRepo

        Public Sub New(repo as SqlRepo)
            Me.Repo = repo
            LookUp = PreloadDbLookups(repo)
        End Sub

        Public readonly Property Name() As String Implements IImportDataToDb.Name
            Get
                Return NameOf(StudentBreakDownImporter)
            End Get
        End Property

        Public Sub ImportCsv_BuildDb() Implements IImportDataToDb.BuildDb
            repo.Execute(File.ReadAllText("Db/StudentBreakDownDb/SqlToCreateDb/Create.sql"))
        End Sub
        
        Public Sub ImportDataFromFile(location as String) Implements IImportDataToDb.ImportDataFromFile
            if File.Exists(location) = False Then
                Console.Write("input data does not exist")
                Return
            End If

            for Each item in _
                GetRowsOfT (of Raw)(location, function(line)  LineToRaw(line, repo, LookUp)).
                    Chunk(250)
                BulkInsertRaw(item, repo)
            Next
        End Sub

        private Shared Sub BulkInsertRaw(raws As IEnumerable(Of Raw), repo As SqlRepo)
            Const query =
                      "insert into Raw (LevelOfStudyId, FirstYearMarkerId, ModeOfStudyId, CountryId, SexId, DomicileId, AcademicYearId, Number, Percentage) values "
            dim sqlValues = raws.Select(Function(e) _
                                           $"({e.LevelOfStudyId},{e.FirstYearMarkerId},{e.ModeOfStudyId},{e.CountryId}," +
                                           $"{e.SexId},{e.DomicileId},{e.AcademicYearId},{e.Number},{e.Percentage})")

            repo.Execute(query + String.Join(",", sqlValues))
        End Sub

        private Shared Function PreloadDbLookups(repo as SqlRepo) _
            as Dictionary(Of Type,Dictionary(Of String,BaseLookUpTable))
            dim lookupDict = New Dictionary(Of Type,Dictionary(Of String,BaseLookUpTable))()
            lookupDict.Add(GetType(AcademicYear), repo.GetAllOfTAsLookup (of AcademicYear)())
            lookupDict.Add(GetType(Country), repo.GetAllOfTAsLookup (of Country)())
            lookupDict.Add(GetType(Domicile), repo.GetAllOfTAsLookup (of Domicile)())
            lookupDict.Add(GetType(FirstYearMarker), repo.GetAllOfTAsLookup (of FirstYearMarker)())
            lookupDict.Add(GetType(LevelOfStudy), repo.GetAllOfTAsLookup (of LevelOfStudy)())
            lookupDict.Add(GetType(Sex), repo.GetAllOfTAsLookup (of Sex)())
            lookupDict.Add(GetType(ModeOfStudy), repo.GetAllOfTAsLookup (of ModeOfStudy)())
            return lookupDict
        End Function

        private Shared Function LineToRaw(line as String, repo As SqlRepo,
                                          lookup As Dictionary(Of Type,Dictionary(Of String,BaseLookUpTable))) As Raw
            dim parts as String() = line.Split(",")
            Return new Raw With{
                .LevelOfStudyId = repo.FindOrCreate ( of LevelOfStudy)(parts(0), lookup),
                .FirstYearMarkerId =repo.FindOrCreate ( of FirstYearMarker)(parts(1), lookup),
                .ModeOfStudyId =repo.FindOrCreate ( of ModeOfStudy)(parts(2), lookup),
                .CountryId =repo.FindOrCreate ( of Country)(parts(3), lookup),
                .SexId = repo.FindOrCreate ( of Sex)(parts(4), lookup),
                .DomicileId = repo.FindOrCreate ( of Domicile)(parts(5), lookup),
                .AcademicYearId = repo.FindOrCreate ( of AcademicYear)(parts(6), lookup),
                .Number = Integer.Parse(parts(7)),
                .Percentage = Decimal.Parse(if (parts(8) = String.Empty, "0", parts(8).Replace("%", "")))}
        End Function
    End Class
End NameSpace