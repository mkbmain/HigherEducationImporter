Imports ImportStudentDataVB.BaseDb
Imports Mkb.DapperRepo.Attributes

Namespace StudentEnrolmentsBySubjectOfStudyImporter.DbModels
    <SqlTableName("CAHLevelMarker")>
    Public Class CahLevelMarker
        inherits BaseLookUpTable
    End Class
End NameSpace