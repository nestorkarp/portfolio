namespace Score.Dynamic.Factory.Interface
{
    public interface ICalculate
    {
        int GetResult(string assemblyFile, IList<int> values, ref string errorMsg);
    }
}