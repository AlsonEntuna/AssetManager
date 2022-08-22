using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AssetManager.Utils
{
    internal static class Utils
    {
        public static ObservableCollection<T> ToObservableCollection<T>(IEnumerable<T> list)
        {
            ObservableCollection<T> newList = new ObservableCollection<T>();
            foreach (T val in list)
            {
                newList.Add(val);
            }
            return newList;
        }
    }
}
