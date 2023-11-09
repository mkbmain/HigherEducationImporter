Imports ImportStudentDataVB.Db
Imports ImportStudentDataVB.Db.StudentBreakDownDb
Imports Microsoft.Data.Sqlite
Imports Microsoft.Extensions.DependencyInjection
Imports Mkb.DapperRepo.Repo

Module Program
    ''' Standard Args StudentBreakDownImporter "/home/mkb/CsvVB/StudentBreakDown2023.sqlite"  "/home/mkb/CsvVB/RawData.csv" 
    Private function BuildContainer(connection as String) as ServiceProvider
        dim serviceCollection as ServiceCollection = New ServiceCollection()
        serviceCollection.AddSingleton(new SqlRepo(Function() new SqliteConnection("Data Source=" + connection)))
        serviceCollection.AddScoped (of IImportDataToDb,StudentBreakDownImporter )
        Return serviceCollection.BuildServiceProvider()
    End function

    Sub Main(args As String())

        if args.Length <> 3 Then
            Console.Write("need 3 arguments first type to run , 2nd path for db, 3rd location of data")
            Return
        End If

        dim container as ServiceProvider = BuildContainer(args(1))
        dim services as IEnumerable(Of IImportDataToDb) = container.GetService (of IEnumerable(Of IImportDataToDb))
        dim importer as IImportDataToDb = services.First(function(e) e.Name = args.First())
        importer.BuildDb()
        importer.ImportDataFromFile(args.Last())
    End Sub
End Module
