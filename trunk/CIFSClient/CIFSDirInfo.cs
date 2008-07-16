// DirectoryInfo.cs
// 
// Copyright (C) 2008 Jordi Martín Cardona
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections;

namespace CIFSClient
{
	
	/// <summary>
	/// Classe que conté informaciò i funcions referents a un directori
	/// </summary>
	public class CIFSDirInfo
	{
		/// <summary>
		/// Array de fitxers que conté una ruta 
		/// </summary>
		private ArrayList files;
		/// <summary>
		/// Array de directoris que conté una ruta 
		/// </summary>
		private ArrayList dirs;
	
		/// <summary>
		/// Constructor
		/// </summary>
		public CIFSDirInfo()
		{
			this.files= new ArrayList();
			this.dirs= new ArrayList();
		}
		
		/// <summary>
		/// Afegeix un nom de directori (s'utilitza internament per emplenar les matrius).
		/// </summary>
		/// <param name="dir">
		/// Nom del directori <see cref="System.String"/>
		/// </param>
		public void addDir(string dir){
			this.dirs.Add(dir);
		}
		
		/// <summary>
		/// Afegeix un nom de fitxer (s'utilitza internament per emplenar les matrius).
		/// </summary>
		/// <param name="file">
		/// Nom del fitxer <see cref="System.String"/>
		/// </param>
		public void addFile(string file){
			this.files.Add(file);
		}
		

		/// <summary>
		/// Obte els directoris de la ruta
		/// </summary>
		/// <returns>
		/// Llistat de directoris <see cref="System.String"/>
		/// </returns>
		public string[] GetDirectories(){
			string[] dirs = new string[this.dirs.Count]; 
			dirs = (string[]) this.dirs.ToArray(typeof(string));
			return dirs;
		}
		
		/// <summary>
		/// Obte els fitxers de la ruta
		/// </summary>
		/// <returns>
		/// Llistat de fitxers<see cref="System.String"/>
		/// </returns>
		
		public string[] GetFiles(){
			string[] files = new string[this.files.Count]; 
			files = (string[]) this.files.ToArray(typeof(string));
			return files;
		}
		
		
	}
}
