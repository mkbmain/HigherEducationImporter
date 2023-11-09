Imports Mkb.DapperRepo.Attributes

Namespace StudentEnrolmentsByLevelOfStudyImporter.DbModels
    <SqlTableName("StudentEnrolments")>
    Public Class StudentEnrolmentsByLevel
        Public Property Id As Integer?
        Public Property LevelOfStudyId As Integer
        Public Property FirstYearMarkerId As Integer
        Public Property ModeOfStudyId As Integer
        Public Property CountryId As Integer
        Public Property SexId As Integer
        Public Property DomicileId As Integer
        Public Property AcademicYearId As Integer
        Public Property Number As Integer
        Public Property Percentage As decimal
    End Class
End NameSpace