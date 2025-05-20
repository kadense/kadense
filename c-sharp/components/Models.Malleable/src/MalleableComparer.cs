namespace Kadense.Models.Malleable
{
    public class MalleableComparer<T> : IComparer<T>
    {
        public Func<T?, T?, int> CompareFunc { get; set; }

        public MalleableComparer(Func<T?, T?, int> compareFunc)
        {
            CompareFunc = compareFunc;
        }
        
        public int Compare(T? x, T? y)
        {
            return CompareFunc(x, y);
        }
    }
}
