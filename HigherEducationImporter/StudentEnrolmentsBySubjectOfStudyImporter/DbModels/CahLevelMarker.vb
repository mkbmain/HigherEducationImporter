Imports ImportStudentDataVB.BaseDb
Imports ImportStudentDataVB.BaseDb.Models
Imports Mkb.DapperRepo.Attributes

Namespace StudentEnrolmentsBySubjectOfStudyImporter.DbModels
    <SqlTableName("CAHLevelMarker")>
    Public Class CahLevelMarker
        inherits BaseLookUpTable
    End Class
End NameSpace