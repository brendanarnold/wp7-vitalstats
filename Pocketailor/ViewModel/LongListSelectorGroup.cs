using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pocketailor.ViewModel
{
    // Taken from the WP Toolkit samples 'PeopleInGroup'
    public class LongListSelectorGroup<TItems> : List<TItems>
    {
        #region IGrouping<TKey,TElement> Members
        public string Key { get; set; }
        #endregion


        public LongListSelectorGroup(string key, IEnumerable<TItems> items) : base(items)
        {
            this.Key = key;
        }

        public bool HasItems { get { return this.Count > 0; } }

    }
}
