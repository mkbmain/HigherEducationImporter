Imports System.IO

Public Module Iterators
    public Iterator Function GetRowsOfT (of T)(location as string,
                                               func as Func(Of string,T),
                                               Optional skipFirstLine as Boolean = True) As IEnumerable(Of T)
        ' ok maybe I am just being OTT here with a Func and a generic we could just return each line and let caller do whatever

        Using reader = new StreamReader(File.OpenRead(location))
            if skipFirstLine AndAlso reader.ReadLine() = Nothing Then ' we skip first line as its headers
                Return ' file empty
            End If

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