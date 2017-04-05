using System;

namespace SQLite
{
	[Flags]
	public enum CreateFlags
	{
		None,
		ImplicitPK,
		ImplicitIndex,
		AllImplicit,
		AutoIncPK
	}
}