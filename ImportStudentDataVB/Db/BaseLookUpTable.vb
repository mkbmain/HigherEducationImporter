Imports Mkb.DapperRepo.Attributes

Namespace Db
    Public MustInherit Class BaseLookUpTable
         Public Property Id As Integer?

         <SqlColumnName("Name")>
         Public Property Description As String
    End Class
End NameSpace