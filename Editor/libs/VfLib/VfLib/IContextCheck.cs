using System;
using System.Collections.Generic;
using System.Text;

namespace vflibcs
{
	interface IContextCheck
	{
		bool FCompatible(IContextCheck icc);
	}
}
