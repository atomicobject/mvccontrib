using System;
using System.Collections;
using System.Collections.Generic;

namespace MvcContrib.CommandProcessor
{
	public class GenericItemDictionary : IEnumerable
	{
		private readonly IDictionary<Type, object> _items = new Dictionary<Type, object>();

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return _items.GetEnumerator();
		}

		#endregion

		public void Add<TItem>(TItem item)
		{
			Add(typeof (TItem), item);
		}

		public void Add(Type type, object item)
		{
			if (_items.ContainsKey(type))
			{
				_items[type] = item;
			}
			else
			{
				_items.Add(type, item);
			}
		}

		public TItem Get<TItem>()
		{
			return (TItem) Get(typeof (TItem));
		}

		public object Get(Type itemType)
		{
			return _items[itemType];
		}
	}
}