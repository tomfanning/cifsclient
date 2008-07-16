// Iclient.cs
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
	/// Interfície de funcions mínimes d'una classe client de CIFSClient
	/// </summary>
	public  interface IClient
	{
		
		
		/// <summary>
		/// Comprovació de l'existencia d'un fitxer
		/// </summary>
		/// <param name="path">
		/// Ruta del fitxer a comprovar <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// Existeix = True , No existeix = False <see cref="System.Boolean"/>
		/// </returns>
		bool Exists(string path);
		
		
		/// <summary>
		/// Copia un fitxer d'una ubicació a una altra
		/// </summary>
		/// <param name="sourceFileName">
		/// Ruta del fitxer origen <see cref="System.String"/>
		/// </param>
		/// <param name="destFileName">
		/// Ruta del fitxer destinació <see cref="System.String"/>
		/// </param>
		/// <param name="overwrite">
		/// Permet la reescriutra d'un fitxer ja existent? aaa<see cref="System.Boolean"/>
		/// </param>
		void Copy(
		string sourceFileName,
        string destFileName,
        bool overwrite
        );
		
		
		/// <summary>
		/// Elimina un fitxer
		/// </summary>
		/// <param name="path">
		/// Ruta del fitxer que es vol eliminar <see cref="System.String"/>
		/// </param>		
		void Delete(
		string path
        );
		
		/// <summary>
		/// Creació d'un directori
		/// </summary>
		/// <param name="path">
		/// Ruta del directori que es vol crear <see cref="System.String"/>
		/// </param>
		void CreateDirectory(
		    string path
        );
		
		/// <summary>
		/// Elimina un directori
		/// </summary>
		/// <param name="path">
		/// Ruta del directri que es vol eliminar <see cref="System.String"/>
		/// </param>
		void DeleteDirectory(
		    string path
        );
		
		/// <summary>
		/// Canvia el nom d'un directori
		/// </summary>
		/// <param name="spath">
		/// Ruta del directori original <see cref="System.String"/>
		/// </param>
		/// <param name="dpath">
		/// Ruta del directori després de canviar-li el nom <see cref="System.String"/>
		/// </param>
		void RenameDirectory(
		    string spath,string dpath
        );

		/// <summary>
		/// Comprovació de l'existencia d'un directori
		/// </summary>
		/// <param name="path">
		/// Ruta de la carpeta a comprovar <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// Existeix = True , No existeix = False <see cref="System.Boolean"/> 
		/// </returns>
		bool DirectoryExists(string path);

		/// <summary>
		/// Canvia el nom d'un fitxer
		/// </summary>
		/// <param name="sourceFileName">
		/// Ruta del fitxer original <see cref="System.String"/>
		/// </param>
		/// <param name="destFileName">
		/// Ruta del fitxer després de canviar-li el nom <see cref="System.String"/>
		/// </param>
		void Rename(
		string sourceFileName,
		string destFileName);
		
		
		/// <summary>
		/// Mou un fitxer
		/// </summary>
		/// <param name="sourceFileName">
		/// Ruta del fitxer original <see cref="System.String"/>
		/// </param>
		/// <param name="destFileName">
		/// Nova ruta del fitxer <see cref="System.String"/>
		/// </param>
		void Move(
		string sourceFileName,
		string destFileName);	
		
		
		/// <summary>
		/// Llista els recursos compartits d'un servidor CIFS
		/// </summary>
		/// <param name="UNCpath">
		/// Adreça del servidor CIFS (DNS o IP) <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// Col·lecció de recursos compartits <see cref="Shares"/>
		/// </returns>
		Shares ShareEnum(string UNCpath);
		
		
		/// <summary>
		/// Llegeix el contingut d'una carpeta
		/// </summary>
		/// <param name="UNCpath">
		/// Ruta de la carpeta que es vol llegir <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// Informació del directori<see cref="CIFSDirInfo"/>
		/// </returns>
		CIFSDirInfo ReadDir(string UNCpath);

			
	}
}
