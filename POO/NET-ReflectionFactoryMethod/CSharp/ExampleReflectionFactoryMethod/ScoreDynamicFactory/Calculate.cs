using System.Reflection;
using Score.Dynamic.Factory.Interface;

namespace Score.Dynamic.Factory
{
    public class Calculate : ICalculate
    {
        public int GetResult(string assemblyFile, IList<int> values, ref string errorMsg)
        {
            int output = 0;
            errorMsg = string.Empty;

            const string _NamespaceScore = "Score.Plugin";
            const string _ClassName = "Calculate";
            const string _MethodName = "GetScore";

            try
            {
                if (string.IsNullOrWhiteSpace(assemblyFile))
                {
                    errorMsg = "assemblyFile no puede estar vacío.";
                    return output;
                }

                if (values == null)
                {
                    errorMsg = "values no puede ser null.";
                    return output;
                }


                //1. Cargar el ensamblado en el contexto de la aplicación
                Assembly assembly = Assembly.LoadFrom(assemblyFile);

                //2. Extraer el tipo de clase usando su nombre completo (Namespace.ClassName)            
                string _assemblyName = _NamespaceScore + "." + _ClassName;
                Type _scoreType = assembly.GetType(_assemblyName);
                if (_scoreType == null)
                {
                    errorMsg = "No se pudo resolver la clase Type especificada.";
                    return output;
                }

                //3. Crear una instancia del tipo de clase extraída
                object _scoreInstance = Activator.CreateInstance(_scoreType);

                //4. Localizar la definición del método objetivo mediante su nombre de cadena (o string)
                MethodInfo methodInfo = _scoreType.GetMethod(_MethodName);
                if (methodInfo == null)
                {
                    errorMsg = "Método no encontrado dentro de la clase especificada.";
                    return output;
                }

                //5. Invoca el método en nuestra instancia dinámica y captura el objeto devuelto
                object result = methodInfo.Invoke(_scoreInstance, new object[] { values });
                output = (int)result;
               
            }
            catch (FileNotFoundException fnf)
            {
                errorMsg = $"Archivo no encontrado: {fnf.Message}";
            }
            catch (BadImageFormatException bife)
            {
                errorMsg = $"Formato de ensamblado inválido: {bife.Message}";
            }
            catch (Exception e)
            {
                errorMsg = $"{e.GetType().FullName}: {e.Message}";
            }

            return output;
        }
    }
}
