#region Using Directives
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
#endregion

namespace XmlToBin {
	public interface IBinConverter {
		string	GetName { get; }
		bool	IsValid( string input );
		bool	Convert( string input, string output );
	}
}
