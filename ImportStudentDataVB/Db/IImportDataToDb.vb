
Namespace Db
    Public Interface IImportDataToDb
        readonly Property Name() As String
        
        sub BuildDb()

        Sub ImportDataFromFile(location as String)
    End Interface
End NameSpace