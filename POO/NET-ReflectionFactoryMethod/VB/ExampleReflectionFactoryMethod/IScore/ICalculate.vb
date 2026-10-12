Imports System.Collections.Generic

Namespace Plugin
    Public Interface ICalculate
        Function GetScore(values As IList(Of Integer)) As Integer
    End Interface
End Namespace
