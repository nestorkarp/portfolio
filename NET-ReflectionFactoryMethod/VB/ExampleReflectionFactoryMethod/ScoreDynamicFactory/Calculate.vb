Imports System.IO
Imports System.Reflection
Imports System.Linq

Namespace Score.Dynamic.Factory
    Public Class Calculate
        Implements ICalculate

        Public Function GetResult(assemblyFile As String, values As IList(Of Integer), ByRef errorMsg As String) As Integer Implements ICalculate.GetResult
            Dim output As Integer = 0
            errorMsg = String.Empty

            Const _NamespaceScore As String = "Score.Plugin"
            Const _ClassName As String = "Calculate"
            Const _MethodName As String = "GetScore"

            Try
                If String.IsNullOrWhiteSpace(assemblyFile) Then
                    errorMsg = "assemblyFile no puede estar vacío."
                    Return output
                End If

                If values Is Nothing Then
                    errorMsg = "values no puede ser null."
                    Return output
                End If

                If Not File.Exists(assemblyFile) Then
                    errorMsg = $"Archivo no encontrado: {assemblyFile}"
                    Return output
                End If

                Dim assembly As Assembly = Assembly.LoadFrom(assemblyFile)

                ' Intento 1: tipo totalmente calificado con el namespace esperado
                Dim fullName As String = $"{_NamespaceScore}.{_ClassName}"
                Dim _scoreType As Type = assembly.GetType(fullName, False)

                ' Intento 2: buscar por nombre simple o por coincidencia en FullName
                If _scoreType Is Nothing Then
                    For Each t As Type In assembly.GetTypes()
                        If String.Equals(t.Name, _ClassName, StringComparison.OrdinalIgnoreCase) OrElse
                           t.FullName.EndsWith("." & _ClassName, StringComparison.OrdinalIgnoreCase) Then
                            _scoreType = t
                            Exit For
                        End If
                    Next
                End If

                ' Intento 3: buscar un tipo que implemente la interfaz ICalculate (por nombre)
                If _scoreType Is Nothing Then
                    For Each t As Type In assembly.GetTypes()
                        Dim interfaces = t.GetInterfaces()
                        If interfaces.Any(Function(i) String.Equals(i.Name, "ICalculate", StringComparison.OrdinalIgnoreCase)) Then
                            _scoreType = t
                            Exit For
                        End If
                    Next
                End If

                If _scoreType Is Nothing Then
                    ' Para depuración, devolver algunos tipos disponibles
                    Dim available = String.Join(", ", assembly.GetTypes().Take(10).Select(Function(t) t.FullName))
                    errorMsg = $"No se pudo resolver la clase Type especificada. Tipos disponibles (ejemplos): {available}"
                    Return output
                End If

                If Not _scoreType.IsPublic Then
                    errorMsg = "El tipo encontrado no es público y no se puede instanciar."
                    Return output
                End If

                Dim _scoreInstance As Object = Activator.CreateInstance(_scoreType)
                Dim methodInfo As MethodInfo = _scoreType.GetMethod(_MethodName, BindingFlags.Public Or BindingFlags.Instance)

                If methodInfo Is Nothing Then
                    errorMsg = "Método no encontrado dentro de la clase especificada."
                    Return output
                End If

                Dim result As Object = methodInfo.Invoke(_scoreInstance, New Object() {values})
                output = Convert.ToInt32(result)

            Catch fnf As FileNotFoundException
                errorMsg = $"Archivo no encontrado: {fnf.Message}"
            Catch bife As BadImageFormatException
                errorMsg = $"Formato de ensamblado inválido: {bife.Message}"
            Catch ex As Exception
                errorMsg = $"{ex.GetType().FullName}: {ex.Message}"
            End Try

            Return output
        End Function
    End Class
End Namespace