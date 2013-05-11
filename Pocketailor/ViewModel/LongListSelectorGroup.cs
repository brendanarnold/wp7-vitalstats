using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.ViewModel
{
    public class LongListSelectorGroup<T> : IEnumerable<T>
    {
        public LongListSelectorGroup(string key, IEnumerable<T> items)
        {
            this.Key = key;
            this.Items = new List<T>(items);
        }

        public override bool Equals(object obj)
        {
            LongListSelectorGroup<T> that = obj as LongListSelectorGroup<T>;

            return (that != null) && (this.Key.Equals(that.Key));
        }

        public string Key
        {
            get;
            set;
        }

        public IList<T> Items
        {
            get;
            set;
        }

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.Items.GetEnumerator();
        }

        #endregion
    }
}
