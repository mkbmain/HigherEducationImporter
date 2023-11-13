
Imports ImportStudentDataVB.BaseDb
Imports Microsoft.Data.Sqlite
Imports Microsoft.Extensions.DependencyInjection
Imports Mkb.DapperRepo.Repo

Module Program
    ''' Standard Args StudentBreakDownImporter "/home/mkb/CsvVB/StudentBreakDown2023.sqlite"  "/home/mkb/CsvVB/RawData.csv" 
    Private function BuildContainer(connection as String) as ServiceProvider
        dim serviceCollection as ServiceCollection = New ServiceCollection()
        dim singleInstance = new SqliteConnection("Data Source=" + connection)
        serviceCollection.AddSingleton(new SqlRepo(Function()singleInstance))
        serviceCollection.AddScoped (of IImporter,StudentEnrolmentsByLevelOfStudyImporter.StudentEnrolmentsByLevelOfStudyImporter )
        serviceCollection.AddScoped (of IImporter,StudentEnrolmentsBySubjectOfStudyImporter.StudentEnrolmentsBySubjectOfStudyImporter )
        Return serviceCollection.BuildServiceProvider()
    End function

    Sub Main(args As String())
        
        if args.Length <> 3 Then
            Console.Write("need 3 arguments first type to run , 2nd path for db, 3rd location of data")
            Return
        End If

        dim container as ServiceProvider = BuildContainer(args(1))
        dim services as IEnumerable(Of IImporter) = container.GetService (of IEnumerable(Of IImporter))
        dim importer as IImporter = services.First(function(e) e.Name = args.First())
        importer.BuildDb()
        importer.ImportDataFromFile(args.Last())
    End Sub
End Module
