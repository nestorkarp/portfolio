using Score.Dynamic.Factory;
using Score.Dynamic.Factory.Interface;

namespace ScoreUnitTest
{
    [TestClass]
    public sealed class TestScore
    {
        [TestMethod]
        public void TestScoreCalculateWithZero()
        {
            // 1. Arrange (Preparar el escenario y variables)
            string _pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Plugins\\");
            string _dll = _pluginPath + "Score.Plugin.ScoreWithZero.dll";

            IList<int> values = new List<int>() { 1, 2, 0, 3 };

            string errorMsg = string.Empty;
            //..

            // 2. Act (Ejecutar la acción a probar)
            ICalculate _calculate = new Calculate();
            int result = _calculate.GetResult(_dll, values, ref errorMsg);
            //..
           
            // 3. Assert (Verificar el resultado mediante aserciones)
            if (errorMsg.Trim().Length == 0)
            {
                int resultOk = 6;
                Assert.AreEqual(resultOk, result, "Resultado esperado con cero");
            }
            else
            {
                Assert.Fail("Error al calcular con cero: " + errorMsg);
            }
            //..
        }


        [TestMethod]
        public void TestScoreCalculateWithOutZero()
        {
            // 1. Arrange (Preparar el escenario y variables)
            string _pluginPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "_Plugins\\");
            string _dll = _pluginPath + "Score.Plugin.ScoreWithOutZero.dll";

            IList<int> values = new List<int>() { 1, 2, 0, 3 };

            string errorMsg = string.Empty;
            //..


            // 2. Act (Ejecutar la acción a probar)
            ICalculate _calculate = new Calculate();
            int result = _calculate.GetResult(_dll, values, ref errorMsg);
            //..


            // 3. Assert (Verificar el resultado mediante aserciones)
            if (errorMsg.Trim().Length == 0)
            {
                int resultOk = 0;
                Assert.AreEqual(resultOk, result, "Resultado esperado  sin cero");
            }
            else
            {
                Assert.Fail("Error al calcular sin cero: " + errorMsg);
            }
            //..
        }
    }
}
