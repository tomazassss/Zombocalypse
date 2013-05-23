using System;
using System.Collections.Generic;
using System.Text;

namespace XRpgLibrary.DialogueSystem
{
	public class HashMap<TKey, TValue> : Dictionary<TKey, TValue>
	{
		private TValue nullValue = default(TValue);

		public new TValue this[TKey key]
		{
			get
			{
				if (key == null) 
				{
					return nullValue;
				}
				return base[key];
			}
			set
			{
				if (key.Equals(default(TKey)))
				{
					nullValue = value;
				}
				else 
				{
					base[key] = value;
				}
			}
		}
	}
}
