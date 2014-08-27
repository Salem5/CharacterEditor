using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlToBin {
	public class CharacterConverter : IBinConverter {
		public string GetName {
			get { return "Character Converter"; }
		}

		public bool IsValid( string input ) {
			var result = Directory.GetFiles( input, "ProjBCharacterProject.xml", SearchOption.TopDirectoryOnly );
			return result != null && result.Length > 0;
		}

		public bool Convert( string input, string output ) {
			var dirs = Directory.GetDirectories( input );
			if ( dirs.Length == 0 ) {
				return false;
			}
			foreach ( var dir in dirs ) {
				// Save character binary filess
			}
			return true;
		}
	}
}
