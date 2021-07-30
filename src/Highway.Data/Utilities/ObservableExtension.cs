using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace System.Linq
{
    public static class ObservableExtension
    {
        public static ObservableCollection<T> ToObservableList<T>(this IEnumerable<T> data)
        {
            ObservableCollection<T> dataToReturn = new ObservableCollection<T>();

            foreach (T t in data)
            {
                dataToReturn.Add(t);
            }

            return dataToReturn;
        }
    }
}