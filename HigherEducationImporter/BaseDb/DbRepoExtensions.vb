Imports System.Runtime.CompilerServices
Imports Mkb.DapperRepo.Repo

Namespace BaseDb
    Public Module DbRepoExtensions
        Private Property LookUp As Dictionary(of Type, Dictionary(Of String, BaseLookUpTable)) = new Dictionary(of Type, Dictionary(Of String, BaseLookUpTable))
        
        <Extension()>
        Private Function GetAllOfTAsLookup (of T as BaseLookUpTable)(repo as SqlRepo) _
            As Dictionary(Of String, BaseLookUpTable)
            Return repo.GetAll (of T).GroupBy(Function(e) e.Description) _
                .ToDictionary(Function(e) e.Key, Function(e) cType(e.First(), BaseLookUpTable))
        End Function

        <Extension()>
        public Function FindOrCreate (of T as {BaseLookUpTable,New} )(repo as SqlRepo, name as String)
                                                                      
            If (not LookUp.ContainsKey(GetType(T))) Then
                LookUp.Add(GetType(T), New Dictionary(Of String,BaseLookUpTable)())
            End If

            dim item as BaseLookUpTable
            if LookUp(GetType(T)).TryGetValue(name, item) Then
                Return item.Id
            End If

            ' might be quicker to preload all these but meh only should be a max of a dozen times per cel per file
            dim items = repo.GetAllOfTAsLookup (Of T)
            If (items.Any(function(pair)  pair.Value.Description = name))
                LookUp(GetType(T)) = repo.GetAllOfTAsLookup (Of T)
                Return LookUp(GetType(T))(name).Id
            End If

            repo.Add(new T With {.Description = name})
            LookUp(GetType(T)) = repo.GetAllOfTAsLookup (Of T)
            Return LookUp(GetType(T))(name).Id
        End Function
    End Module
End NameSpace