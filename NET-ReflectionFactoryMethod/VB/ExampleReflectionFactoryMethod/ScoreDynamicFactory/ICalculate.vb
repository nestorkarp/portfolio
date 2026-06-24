Imports System.Collections.Generic

Namespace Score.Dynamic.Factory
    Public Interface ICalculate
        Function GetResult(assemblyFile As String, values As IList(Of Integer), ByRef errorMsg As String) As Integer
    End Interface
End Namespace
