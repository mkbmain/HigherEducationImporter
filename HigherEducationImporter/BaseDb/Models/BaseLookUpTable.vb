Imports Mkb.DapperRepo.Attributes

Namespace BaseDb.Models
    Public MustInherit Class BaseLookUpTable
         Public Property Id As Integer?

         <SqlColumnName("Name")>
         Public Property Description As String
    End Class
End NameSpace