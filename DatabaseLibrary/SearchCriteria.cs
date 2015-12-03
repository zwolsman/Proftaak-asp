using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseLibrary
{
    public class SearchCriteria : IEnumerable<KeyValuePair<string, object>>
    {
        readonly List<KeyValuePair<string, object>> mylist = new List<KeyValuePair<string, object>>();

        public KeyValuePair<string, object> this[int index]
        {
            get { return mylist[index]; }
            set { mylist.Insert(index, value); }
        }

        public void Add(string key, object value)
        {
            mylist.Add(new KeyValuePair<string, object>(key, value));
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return mylist.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
