#region Using Directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace XmlToBin {
	class Program {
		static void Main( string[] args ) {
			var defaultColor = Console.ForegroundColor;

			// Check Arguments
			if ( args.Length < 2 ) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine( "> ERROR: You need to specify and input and output path for this application to work properly." );
				Console.WriteLine( "> ERROR: Use it like this: XmlToBin.exe \"inputPath\" \"outputPath\"" );
				Console.ForegroundColor = defaultColor;
				return;
			}

			string inputPath	= args[ 0 ];
			string outputPath	= args[ 1 ];

			// Check directories
			if ( !Directory.Exists( inputPath ) ) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.WriteLine( "> ERROR: Input Path does not exist: \"" + inputPath + "\"." );
				Console.WriteLine( "> ERROR: You need to specify a valid input path for this application to work." );
				Console.ForegroundColor = defaultColor;
				return;
			} else if ( !Directory.Exists( outputPath ) ) {
				try {
					Directory.CreateDirectory( outputPath );
				} catch {
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine( "> ERROR: Output Path does not exist: \"" + outputPath + "\"." );
					Console.WriteLine( "> ERROR: You need to specify a valid output path for this application to work." );
					Console.ForegroundColor = defaultColor;
					return;
				}
			}

			// Add converter
			var converters = new List<IBinConverter>();
			converters.Add( new CharacterConverter() );
			foreach ( var converter in converters ) {
				if ( converter.IsValid( inputPath ) ) {
					Console.WriteLine( "> Starting " + converter.GetName );
					if ( !converter.Convert( inputPath, outputPath ) ) {
						Console.ForegroundColor = ConsoleColor.Red;
						Console.WriteLine( "> ERROR: " + converter.GetName + " couldn't finish the process!" );
						Console.ForegroundColor = defaultColor;
						return;
					} else {
						Console.WriteLine( "> ALL DONE!" );
						return;
					}
				}
			}
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine( "> ERROR: no valid converter found for the given files in the input folder!" );
			Console.ForegroundColor = defaultColor;
			return;
		}
	}
}
