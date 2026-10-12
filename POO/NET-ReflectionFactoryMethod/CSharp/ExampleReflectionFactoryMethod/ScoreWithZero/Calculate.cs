using Score.Plugin.Interface;

namespace Score.Plugin
{
    public class Calculate : ICalculate
    {
        public int GetScore(IList<int> values)
        {
            int output = 0;

            foreach(var item in values)
            {
                output += item;
            }

            return output;
        }
    }
}
