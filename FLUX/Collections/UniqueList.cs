using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FLUX.Collections
{
    /// <summary>
    /// Defines a list that has type-uniqueness (only one item of a certain type is allowed.)
    /// </summary>
    /// <typeparam name="T">Base type of the list</typeparam>
    public class UniqueList<T> : IEnumerable<T>
    {
        public List<T> Items { get; set; }

        public bool Contains<LT>() where LT : T
        {
            if (Items.Exists(e => e.GetType() == typeof(LT))) return true;
            return false;
        }

        public void Add<LT>(LT item) where LT : T
        {
            if (!Contains<LT>())
            {
                Items.Add(item);
            }
            else
            {
                throw new InvalidOperationException("Type already exists in UniqueList");
            }
        }

        public void Remove<LT>() where LT : T
        {
            if (Contains<LT>())
            {
                Items.Remove(Items.First(e => e.GetType() == typeof(LT)));
            }
            else
            {
                throw new InvalidOperationException("Type does not exist in UniqueList");
            }
        }

        public T Get<LT>() where LT : T
        {
            if (Contains<LT>())
            {
                return Items.First(e => e.GetType() == typeof(LT));
            }
            else
            {
                throw new InvalidOperationException("Type does not exist in UniqueList");
            }
        }

        public T this[int key]
        {
            get
            {
                return Items[key];
            }
            set
            {
                Items[key] = value;
            }
        }

        public UniqueList() { }
        #region Implementation of IEnumerable
        public IEnumerator<T> GetEnumerator()
        {
            return Items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}
