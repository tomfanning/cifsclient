// NetClient.cs
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


/// <summary>
/// El namespace CIFSClient conté les classes de treball de la biblioteca
/// </summary>
namespace CIFSClient
{
	
	/// <summary>
	/// La classe CIFS conte totes les funcions publiques que posa a 
	/// disposició del programador la la biblioteca de classes CIFSClient. 
	/// </summary>

	public class Cifs : IClient
	{
		/// <summary>
		/// Verifica i repara les rutes tenint amb compte el sistema opreatiu en que es treballa.
		/// </summary>
		/// <param name="path">
		/// Ruta d'entrada <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// La ruta reparada <see cref="System.String"/>
		/// </returns>
		private string parsePath(string path){
		int p = (int) Environment.OSVersion.Platform;
                if ((p == 4) || (p == 128)) {
				  
				  path= path.Replace(@"\",@"/");  ;//Linux
				  
				      
                } else {
				  path= path.Replace(@"/",@"\");  ;//windows
                }
			return path;
		}
					
		IClient client;
		
		/// <summary>
		/// Constructor
		/// </summary>
		public Cifs()
		{
			client= ClientFactory.GetClient();
		}
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
		public void Copy(string sourceFileName,string destFileName,bool overwrite)
		{
			sourceFileName =parsePath(sourceFileName);
			destFileName=parsePath(destFileName);
			client.Copy(sourceFileName,destFileName,overwrite);
		}
		
		/// <summary>
		/// Comprovació de l'existencia d'un fitxer
		/// </summary>
		/// <param name="path">
		/// Ruta del fitxer a comprovar <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public bool Exists(string path)
		{
			path =parsePath(path);
			return client.Exists(path);
		}
		
		/// <summary>
		/// Comprovació de l'existencia d'un directori
		/// </summary>
		/// <param name="path">
		/// Ruta de la carpeta a comprovar <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// <see cref="System.Boolean"/>
		/// </returns>
		public bool DirectoryExists(string path)
		{
			path=parsePath(path);
			return client.DirectoryExists(path);
		}
		
		/// <summary>
		/// Llista els recursos compartits d'un servidor CIFS
		/// </summary>
		/// <param name="path">
		/// Adreça del servidor CIFS (DNS o IP) <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// Col·lecció de recursos compartits <see cref="Shares"/>
		/// </returns>
		public Shares ShareEnum(string path){ 
			path=parsePath(path);
			return client.ShareEnum(path); 
		}
		
		/// <summary>
		/// Mou un fitxer
		/// </summary>
		/// <param name="sourceFileName">
		/// Ruta del fitxer original <see cref="System.String"/>
		/// </param>
		/// <param name="destFileName">
		/// Nova ruta del fitxer <see cref="System.String"/>
		/// </param>
		public void Move(
		string sourceFileName,
		string destFileName){
			sourceFileName =parsePath(sourceFileName);
			destFileName=parsePath(destFileName);
			client.Move(sourceFileName,destFileName);
		}
		
		
		/// <summary>
		/// Canvia el nom d'un fitxer
		/// </summary>
		/// <param name="sourceFileName">
		/// Ruta del fitxer original <see cref="System.String"/>
		/// </param>
		/// <param name="destFileName">
		/// Ruta del fitxer després de canviar-li el nom <see cref="System.String"/>
		/// </param>
		public void Rename(
		string sourceFileName,
		string destFileName){
			sourceFileName =parsePath(sourceFileName);
			destFileName=parsePath(destFileName);
			client.Rename(sourceFileName,destFileName);
		}
		
		/// <summary>
		/// Elimina un fitxer
		/// </summary>
		/// <param name="path">
		/// Ruta del fitxer que es vol eliminar <see cref="System.String"/>
		/// </param>			
		public void Delete(
		string path
        ){
			path=parsePath(path);
			client.Delete(path);
		
		}
		
		/// <summary>
		/// Creació d'un directori
		/// </summary>
		/// <param name="path">
		/// Ruta del directori que es vol crear <see cref="System.String"/>
		/// </param>
		public void CreateDirectory(
		    string path
        ){
			path=parsePath(path);
			client.CreateDirectory(path);
		}
		
		/// <summary>
		/// Elimina un directori
		/// </summary>
		/// <param name="path">
		/// Ruta del directri que es vol eliminar <see cref="System.String"/>
		/// </param>
		public void DeleteDirectory(
		    string path
        ){
			path=parsePath(path);
			client.DeleteDirectory(path);
		}
		
		/// <summary>
		/// Canvia el nom d'un directori
		/// </summary>
		/// <param name="spath">
		/// Ruta del directori original <see cref="System.String"/>
		/// </param>
		/// <param name="dpath">
		/// Ruta del directori després de canviar-li el nom <see cref="System.String"/>
		/// </param>
		public void RenameDirectory(string spath,string dpath) {
			spath =parsePath(spath);
			dpath=parsePath(dpath);
		    client.RenameDirectory(spath,dpath);
		}
		
		/// <summary>
		/// Llegeix el contingut d'una carpeta
		/// </summary>
		/// <param name="path">
		/// Ruta de la carpeta que es vol llegir <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// Informació del directori<see cref="CIFSDirInfo"/>
		/// </returns>
		public CIFSDirInfo ReadDir(string path){ 
			path=parsePath(path);
			return client.ReadDir(path);
		}

	}
}
