using System;
using System.ComponentModel;

namespace MvcContrib.CommandProcessor.Interfaces
{
	/// <summary>
	/// A trick to assist with API fluency - this will try to hide object overrides from Intellisense
	/// </summary>
	public interface IGrammar
	{
		[EditorBrowsable(EditorBrowsableState.Never)]
		bool Equals(object obj);

		[EditorBrowsable(EditorBrowsableState.Never)]
		int GetHashCode();

		[EditorBrowsable(EditorBrowsableState.Never)]
		Type GetType();

		[EditorBrowsable(EditorBrowsableState.Never)]
		string ToString();
	}
}