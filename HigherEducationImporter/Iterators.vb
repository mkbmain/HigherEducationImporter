Imports System.IO

Public Module Iterators
    public Iterator Function GetRowsOfT (of T)(location as string,
                                               func as Func(Of string,T),
                                               Optional skipLine as integer = 1) As IEnumerable(Of T)
        ' ok maybe I am just being OTT here with a Func and a generic we could just return each line and let caller do whatever
        If skipLine < 0 Then
            skipLine = 0
        End If
        Using reader = new StreamReader(File.OpenRead(location))
            For i = 1 To skipLine
                reader.ReadLine()
            Next

            Do
                dim text = reader.ReadLine()
                If text = Nothing Then
                    Exit Do
                End If

                Yield func(text)
            Loop
        End Using
    End Function
End Module