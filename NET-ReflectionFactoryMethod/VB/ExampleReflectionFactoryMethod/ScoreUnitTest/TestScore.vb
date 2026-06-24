Imports System.IO
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Score.Dynamic.Factory.Score.Dynamic.Factory

Namespace ScoreUnitTest
    <TestClass()>
    Public NotInheritable Class TestScore

        <TestMethod()>
        Public Sub TestScoreCalculateWithZero()
            ' 1. Arrange
            Dim _pluginPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Plugins\")
            Dim _dll As String = _pluginPath & "Score.Plugin.ScoreWithZero.dll"

            Dim values As IList(Of Integer) = New List(Of Integer) From {1, 2, 0, 3}

            Dim errorMsg As String = String.Empty

            ' 2. Act
            Dim _calculate As ICalculate = New Calculate()
            Dim result As Integer = _calculate.GetResult(_dll, values, errorMsg)

            ' 3. Assert
            If errorMsg.Trim().Length = 0 Then
                Dim resultOk As Integer = 6
                Assert.AreEqual(resultOk, result, "Resultado esperado con cero")
            Else
                Assert.Fail("Error al calcular con cero: " & errorMsg)
            End If
        End Sub

        <TestMethod()>
        Public Sub TestScoreCalculateWithOutZero()
            ' 1. Arrange
            Dim _pluginPath As String = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Plugins\")
            Dim _dll As String = _pluginPath & "Score.Plugin.ScoreWithOutZero.dll"

            Dim values As IList(Of Integer) = New List(Of Integer) From {1, 2, 0, 3}

            Dim errorMsg As String = String.Empty

            ' 2. Act
            Dim _calculate As ICalculate = New Calculate()
            Dim result As Integer = _calculate.GetResult(_dll, values, errorMsg)

            ' 3. Assert
            If errorMsg.Trim().Length = 0 Then
                Dim resultOk As Integer = 0
                Assert.AreEqual(resultOk, result, "Resultado esperado  sin cero")
            Else
                Assert.Fail("Error al calcular sin cero: " & errorMsg)
            End If
        End Sub

    End Class
End Namespace