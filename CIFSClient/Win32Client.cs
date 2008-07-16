// Win32Client.cs
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
using CIFSClient.Win32;
using System.Runtime.InteropServices;
using System.Collections;
using System.IO;

namespace CIFSClient
{
	
	/// <summary>
	/// Classe de gestió de les funcionalitats de win32
	/// </summary>
	public class Win32Client : IClient
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public Win32Client()
		{
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
		public void Copy(
		string sourceFileName,
        string destFileName,
        bool overwrite
        ){
		
		File.Copy(sourceFileName,
        destFileName,
        overwrite);
		
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
		File.Move(sourceFileName,destFileName);
		
		}

		/// <summary>
		/// Elimina un fitxer
		/// </summary>
		/// <param name="path">
		/// Ruta del fitxer que es vol eliminar <see cref="System.String"/>
		/// </param>
		
		public void Delete(
		string file
        ){

			if (!File.Exists(file))
				throw new IOException();
			File.Delete(file);
			
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
			if (!Directory.Exists(path))
				Directory.CreateDirectory(path);
			else
				throw new IOException();
			
		
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
			Directory.Delete(path);
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
		
			File.Move(sourceFileName,destFileName);
		}
		
		/// <summary>
		/// Comprovació de l'existencia d'un fitxer
		/// </summary>
		/// <param name="path">
		/// Ruta del fitxer a comprovar <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// Existeix = True , No existeix = False <see cref="System.Boolean"/>
		/// </returns>
		public bool Exists(
		    string path
        ){return File.Exists(path);}
		
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
			Directory.Move(spath,dpath);
		}

		/// <summary>
		/// Comprovació de l'existencia d'un directori
		/// </summary>
		/// <param name="path">
		/// Ruta de la carpeta a comprovar <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// Existeix = True , No existeix = False <see cref="System.Boolean"/> 
		/// </returns>		
		public bool DirectoryExists(string path){
			return Directory.Exists(path);
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
		public Shares ShareEnum(string UNCpath){ 
			
			Shares s= EnumerateShares(UNCpath);
			if (s.Count()<1)
				throw new IOException();
			return s;
		
		
		}
		

		
		/// <summary>
		/// Comrova si la plataforma es NY
		/// </summary>
		protected static bool IsNT
		{
			get { return (PlatformID.Win32NT == Environment.OSVersion.Platform); }
		}

		/// <summary>
		/// Retrona true si plataforma es Windows 2000 o versions posteriors
		/// </summary>
		protected static bool IsW2KUp
		{
			get 
			{
				OperatingSystem os = Environment.OSVersion;
				if (PlatformID.Win32NT == os.Platform && os.Version.Major >= 5) 
					return true;
				else 
					return false;
			}
		}
		

		
		
		
				
		/// <summary>
		/// Obte els recursos compartits d'un servidor CIFS NT
		/// </summary>
		/// <param name="server">Nom del servidor</param>
		/// <param name="shares">Colecció de recursos</param>
		protected static Shares EnumerateSharesNT(string server)
		{
			Shares shares= new Shares();
			Share s;
			
			int level = 2;
			int entriesRead, totalEntries, nRet, hResume = 0;
			IntPtr pBuffer = IntPtr.Zero;

			try 
			{
				nRet = WrapperWin32Api.NetShareEnum(server, level, out pBuffer, -1, 
					out entriesRead, out totalEntries, ref hResume);
				

				if (WrapperWin32Api.ERROR_ACCESS_DENIED == nRet) 
				{
					//depenent del tipus de nivell d'acces que tenim la consulta obtindrà un tipus de dades
					// o un altres, això afectarà a l'estructura on es carrega el resultat
					level = 1;
					nRet = WrapperWin32Api.NetShareEnum(server, level, out pBuffer, -1, 
						out entriesRead, out totalEntries, ref hResume);
				}

				if (WrapperWin32Api.NO_ERROR == nRet && entriesRead > 0) 
				{
					Type t = (2 == level) ? typeof(WrapperWin32Api.SHARE_INFO_2) : typeof(WrapperWin32Api.SHARE_INFO_1);
					int offset = Marshal.SizeOf(t);

					for (int i=0, lpItem=pBuffer.ToInt32(); i<entriesRead; i++, lpItem+=offset) 
					{
						IntPtr pItem = new IntPtr(lpItem);
						if (1 == level) //estructura basica
						{
							WrapperWin32Api.SHARE_INFO_1 si = (WrapperWin32Api.SHARE_INFO_1)Marshal.PtrToStructure(pItem, t);
							
							s= new Share();
							s.comment=si.Remark;
							s.name=si.NetName;
							s.type=CommonShareTypes(si.ShareType);
							shares.addShare(s);
						}
						else //estructura amb més informació, però seguirem agafant la mateixa
						{
							WrapperWin32Api.SHARE_INFO_2 si = (WrapperWin32Api.SHARE_INFO_2)Marshal.PtrToStructure(pItem, t);
							s= new Share();
							s.comment=si.Remark;
							s.name=si.NetName;
							s.type=CommonShareTypes(si.ShareType);
							shares.addShare(s);
							
						}
					}
				}
				return shares;
			}
			finally 
			{
				// Netejem el buffer
				if (IntPtr.Zero != pBuffer) 
					WrapperWin32Api.NetApiBufferFree(pBuffer);
			}
		}
		
		/// <summary>
		/// Obte els recursos compartits d'un servidor CIFS NTWindows 9x
		/// </summary>
		/// <param name="server">Nom del servidor</param>
		/// <param name="shares">Colecció de recursos</param>
		protected static Shares EnumerateShares9x(string server)
		{
			Shares shares= new Shares();
			Share s;
			int level = 50;
			int nRet = 0;
			ushort entriesRead, totalEntries;
			
			Type t = typeof(WrapperWin32Api.SHARE_INFO_2_9x );
			int size = Marshal.SizeOf(t);
			ushort cbBuffer = (ushort)(WrapperWin32Api.MAX_SI50_ENTRIES * size);
			//A Win9x, s'ha de reserva la memòria del buffer abans de cridar l'API
			IntPtr pBuffer = Marshal.AllocHGlobal(cbBuffer);

			try 
			{
				nRet = WrapperWin32Api.NetShareEnum(server, level, pBuffer, cbBuffer, 
					out entriesRead, out totalEntries);

				
				if (WrapperWin32Api.ERROR_WRONG_LEVEL == nRet)
				{
					level = 1;
					//el matix problema del permisos i les estructures de retorn de dades
					t = typeof(WrapperWin32Api.SHARE_INFO_1_9x);
					size = Marshal.SizeOf(t);
					
					nRet = WrapperWin32Api.NetShareEnum(server, level, pBuffer, cbBuffer, 
						out entriesRead, out totalEntries);
				}

				if (WrapperWin32Api.NO_ERROR == nRet || WrapperWin32Api.ERROR_MORE_DATA == nRet) 
				{
					for (int i=0, lpItem=pBuffer.ToInt32(); i<entriesRead; i++, lpItem+=size) 
					{
						IntPtr pItem = new IntPtr(lpItem);
						
						if (1 == level)
						{
							WrapperWin32Api.SHARE_INFO_1_9x si = (WrapperWin32Api.SHARE_INFO_1_9x)Marshal.PtrToStructure(pItem, t);
							s= new Share();
							s.comment=si.Remark;
							s.name=si.NetName;
							s.type=CommonShareTypes(si.ShareType);
							shares.addShare(s);
						}
						else
						{
							WrapperWin32Api.SHARE_INFO_2_9x si = (WrapperWin32Api.SHARE_INFO_2_9x)Marshal.PtrToStructure(pItem, t);
							System.Console.WriteLine(si.NetName);
							s= new Share();
							s.comment=si.Remark;
							s.name=si.NetName;
							s.type=CommonShareTypes(si.ShareType);
							shares.addShare(s);
						}
					}
				}
				else
					Console.WriteLine(nRet);
				return shares;
			}
			finally 
			{
				//Netejem el buffe
				Marshal.FreeHGlobal(pBuffer);
			}
		}
		
		/// <summary>
		/// Obte la colecció de recursos comparits  d'un servidor CIFS
		/// </summary>
		/// <param name="server">Nom del servidor</param>
		/// <param name="shares">Colecció de recursos</param>
		protected Shares EnumerateShares(string server)
		{
			Shares shares= new Shares();
			if (null != server && 0 != server.Length && !IsW2KUp)
			{
				server = server.ToUpper();
				
				// Comprovem que la ruta sigui la correcta i comence per "\\"
				if (!('\\' == server[0] && '\\' == server[1])) 
					server = @"\\" + server;
			}
			
			//Seleccionem de quin tipus de servidor hem de obtenir les dades.
			if (IsNT)
				shares=EnumerateSharesNT(server);
			else
				shares=EnumerateShares9x(server);
			return shares;
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
		public CIFSDirInfo ReadDir(string UNCpath){
			CIFSDirInfo dirinfo = new CIFSDirInfo(); 
			
			
			DirectoryInfo dir = new DirectoryInfo(UNCpath);
			DirectoryInfo[] dirs = dir.GetDirectories();
			FileInfo[] fi = dir.GetFiles();
				
			if(fi!=null)
			{
				foreach(FileInfo f in fi)
				{
					dirinfo.addFile(f.Name);
				}
			}
			if(dirs!=null)
			{
				foreach(DirectoryInfo d in dirs)
				{
					dirinfo.addDir(d.Name);
				}
			}
			return dirinfo;
		}
		
		
		/// <summary>
		/// Funció de traducció entre els tipus de recurs compartit win32 i els tipus de CIFSClient
		/// </summary>
		/// <param name="share">
		/// Tipus de recurs win32<see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// Tipus de recurs CIFSClient <see cref="Win32ShareType"/>
		/// </returns>		
	private static string CommonShareTypes(Win32ShareType st){
		int share=(int)st;
		if(share==0)
			return "Share";
		if(share==1)
			return "Printer";		

		if(share==3)
			return "IPC";
		
		return "Other";
	
	}
		
	}
}
