

Public Interface IImporter
    readonly Property Name() As String
        
    sub BuildDb()

    Sub ImportDataFromFile(location as String)
End Interface