Imports System.Collections.Generic
Imports IScore.Plugin

Namespace Score.Plugin
    Public Class Calculate
        Implements ICalculate

        Public Function GetScore(values As IList(Of Integer)) As Integer Implements ICalculate.GetScore
            If values Is Nothing OrElse values.Count = 0 Then
                Return 0
            End If

            Dim output As Integer = 0

            For Each item As Integer In values
                output += item
            Next

            Return output
        End Function
    End Class
End Namespace