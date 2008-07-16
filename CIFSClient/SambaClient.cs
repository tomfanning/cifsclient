// SambaClient.cs
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



#if(LINUX)

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Mono.Unix.Native;
using CIFSClient.Samba;
using System.Collections;


namespace CIFSClient
{
	
	/// <summary>
	/// Classe de gestió de les funcionalitats de samba
	/// </summary>
	public  class SambaClient : IClient
	{
		/// <summary>
		/// Constructor
		/// </summary>
		public SambaClient(){
			    //Inicialitza el Client samba. 
			    //Aquesta acció sempre es necessari avans d'utilitzar cap altra funció.
				WrapperSambaClient.SmbInit();
			
			
		}
		
		
		
		/// <summary>
		/// Valida un ruta i determina si es UNC
		/// </summary>
		private bool isUNCPath(String path)
		{
			path = Regex.Replace(path,@"\\","/");
			Regex regexUNC=new Regex(@"^(//[\w-]+/[^//:*?<>|""]+)((?:/[^//:*?<>|""]+)*/?)$");
			if (regexUNC.IsMatch(path)==true)
				return true;
			else
				return false;
		}
		
		
		/// <summary>
		/// Arregla errors a una ruta UNC
		/// </summary>
		private string parseUNCPath(String path)
		{
			
			path = Regex.Replace(path,@"\\","/");
			if (!path.StartsWith("//"))
			    path="//"+path;
			return path;
		

		}
		
		
		
		/// <summary>
		/// Copia un fitxer d'una ubicació origen a una destí
		/// 
		/// Controla automaticament si es copia d'un fitxer local a un remot, d'un remot a un altre remot o de un remot a un local. 
		/// </summary>
		public void Copy(string sourceFileName,string destFileName, bool overwrite ){
			bool remoteSource=isUNCPath(sourceFileName);
			bool remotedest=isUNCPath(destFileName);
			
			//Comprovem quin tipus de copy s'utlitzarà.
			if(remoteSource && !remotedest){
				CopySambaToLocal(sourceFileName,destFileName,overwrite); //copiem el fitxer remot a un lloc local
				
			}
			else if(!remoteSource && remotedest){
				CopyLocalToSamba(sourceFileName,destFileName,overwrite); //copiem el fitxer local a un lloc remot
				
			}
			else if(remoteSource && remotedest){
				CopySambaToSamba(sourceFileName,destFileName,overwrite); //copiem el fitxer remot a un lloc remot
				
			}
	
		
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
		public bool Exists(string path){
			//comprovació del fitxer remot
			if( isUNCPath(path)){
				int fd;
				string fullpath=@"smb:"+parseUNCPath(path);
				//per comprovar si existeix en samba miro si es pot obrir
				fd=WrapperSambaClient.smbc_open(fullpath,(int)OpenFlags.O_RDONLY,(int)FileMode.Open );
				if (fd<0){
					if (Stdlib.GetLastError().ToString()=="ENOENT")
						return false;
					if (Stdlib.GetLastError().ToString()=="EINVAL")
						return false;
					else
						throw new Exception( Stdlib.GetLastError().ToString() );
				}
				else 
				{
					//si es pot obrir el tanco.
					WrapperSambaClient.smbc_close(fd);
					return true;
				}
			}
			else{ //comprovació del fitxer local
				return File.Exists(path);
			}
				
			
			
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
		
			//comrovació d'una ruta samba
			if( isUNCPath(path)){
				int fd;
				string fullpath=@"smb:"+parseUNCPath(path);
				//per comprovar si existeix amb samba miro si es pot obir el directori
				fd=WrapperSambaClient.smbc_opendir(fullpath);
				if (fd<0){
					if (Stdlib.GetLastError().ToString()=="ENOENT")
						return false;
					if (Stdlib.GetLastError().ToString()=="EINVAL")
						return false;
					else
						throw new Exception( Stdlib.GetLastError().ToString() );
				}
				else 
				{
					WrapperSambaClient.smbc_close(fd);
					return true;
				}
			}
			else{
				return Directory.Exists(path); //comprovació d'una ruta local; realment no es necessari
			}
	
		}
				
		
		/// <summary>
		/// Copia un fitxer remot a un ruta local
		/// 
		/// Aquesta funció esta marcada com a unsafe, ja que s'ha de utilitzar un punter directe a memoria
		/// </summary>
		/// <param name="sourceFileName">
		/// Fitxer origen <see cref="System.String"/>
		/// </param>
		/// <param name="destFileName">
		/// Fitxer destinació <see cref="System.String"/>
		/// </param>
		/// <param name="overwrite">
		///  Permet la reescritura d'un fitxer ja exitent. <see cref="System.Boolean"/>
		/// </param>
		unsafe private void CopySambaToLocal(string sourceFileName,
        string destFileName,
        bool overwrite){
				
			    int bytesRead=1024;
   		        byte[] buffer = new byte[bytesRead];

			
			    fixed (void* b  = buffer){ //La instrucció  fixed evita que el recolector de elements no utilitzats ubique de nou la varaible. 
				int fs;
				int fd;
				int res;
 			    
				
				//comprovem que exiteix el fitxer origen
				if(!Exists(sourceFileName))
					throw new FileNotFoundException("Fitxer Origen no existeix");
				//comprovem si existeix el fitxer destinació, sinò el creem
				if(!Exists(destFileName)){
					fd=Syscall.creat(destFileName,FilePermissions.S_IRWXU);
					if (fd<0){
						System.Console.WriteLine(Stdlib.GetLastError().ToString());
						if (Stdlib.GetLastError().ToString()=="EACCES" || Stdlib.GetLastError().ToString()=="EROFS" || Stdlib.GetLastError().ToString()=="EBADF")
							throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
						if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT" )
							throw new FileNotFoundException(Stdlib.GetLastError().ToString());
					}
					Syscall.close(fd);	
				}
				else if (!overwrite){ //si el fitxer ja existeix i no es vol sobreescriure salta una exepcio
					throw new IOException("El fitxer destí ja exiteix");
				}
				
				
				
				
				//obrim el fitxer remot
				fs=WrapperSambaClient.smbc_open(@"smb:"+parseUNCPath(sourceFileName),(int)OpenFlags.O_RDONLY,(int)FileMode.Open );
				if (fs<0){
					System.Console.WriteLine(Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EACCES" || Stdlib.GetLastError().ToString()=="EROFS" || Stdlib.GetLastError().ToString()=="EBADF")
						throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT" )
						throw new FileNotFoundException(Stdlib.GetLastError().ToString());
				}
				
				
				//obrim el fitxer local
				fd=Syscall.open(destFileName,OpenFlags.O_WRONLY);
				
				if (fd<0){
					System.Console.WriteLine(Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EACCES" || Stdlib.GetLastError().ToString()=="EROFS" || Stdlib.GetLastError().ToString()=="EBADF")
						throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT" )
						throw new FileNotFoundException(Stdlib.GetLastError().ToString());
				}
                
                //copiem el contigut d'un punter a l'altre
				while ((res=WrapperSambaClient.smbc_read(fs,b, bytesRead)) > 0){
					Syscall.write(fd,b,(ulong)res);

				}
			    
				Syscall.close(fd);
				WrapperSambaClient.smbc_close(fs);
			
			}
			
			
		}	
		
		/// <summary>
		/// Copia un local remot a un ruta remota
		/// 
		/// Aquesta funció esta marcada com a unsafe, ja que s'ha de utilitzar un punter directe a memoria
		/// </summary>
		/// <param name="sourceFileName">
		/// Fitxer origen <see cref="System.String"/>
		/// </param>
		/// <param name="destFileName">
		/// Fitxer destinació <see cref="System.String"/>
		/// </param>
		/// <param name="overwrite">
		///  Permet la reescritura d'un fitxer ja exitent. <see cref="System.Boolean"/>
		/// </param>		
		unsafe private void CopyLocalToSamba(string sourceFileName,
        string destFileName,
        bool overwrite){
				
			    ulong bytesRead=1024;
   		        byte[] buffer = new byte[bytesRead];

			
			    fixed (void* b  = buffer){ //La instrucció  fixed evita que el recolector de elements no utilitzats ubique de nou la varaible.
				int fs;
				int fd;
				long res;
 			    
				
				//comprovem que exiteix el fitxer origen
				if(!Exists(sourceFileName))
					throw new FileNotFoundException("Fitxer Origen no existeix");
				//comprvem si existeix el fitxer destinació, sino el creem
				if(!Exists(destFileName)){
					fd=WrapperSambaClient.smbc_creat(@"smb:"+parseUNCPath(destFileName),(int)FileMode.CreateNew);
					System.Console.WriteLine(Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EACCES" || Stdlib.GetLastError().ToString()=="EROFS" || Stdlib.GetLastError().ToString()=="EBADF")
						throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT" )
						throw new FileNotFoundException(Stdlib.GetLastError().ToString());
					WrapperSambaClient.smbc_close(fd);
					System.Console.WriteLine("Fitxer destinació no existeix");
					
				}
				else if (!overwrite){ //si el fitxer ja existeix i no es vol sobreescriure salta una exepció
					throw new IOException("El fitxer destí ja exiteix");
				}

				//obrim el fitxer local
				fs=Syscall.open(sourceFileName,OpenFlags.O_RDONLY);
				if (fs<0){
					System.Console.WriteLine(Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EACCES" || Stdlib.GetLastError().ToString()=="EROFS" || Stdlib.GetLastError().ToString()=="EBADF")
						throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT" )
						throw new FileNotFoundException(Stdlib.GetLastError().ToString());
				}
				
				
				//obrim el fitxer remot
				fd=WrapperSambaClient.smbc_open(@"smb:"+parseUNCPath(destFileName),(int)OpenFlags.O_WRONLY ,(int) FileMode.CreateNew);
				if (fd<0){
					System.Console.WriteLine(Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EACCES" || Stdlib.GetLastError().ToString()=="EROFS" || Stdlib.GetLastError().ToString()=="EBADF")
						throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT" )
						throw new FileNotFoundException(Stdlib.GetLastError().ToString());
				}

                
                //copiem el contigut
				while ((res=Syscall.read(fs,b,bytesRead)) > 0){
					WrapperSambaClient.smbc_write(fd,b,(int)res);
				}
				Syscall.close(fs);
				WrapperSambaClient.smbc_close(fd);
			
			}
			
			
		}
		

		
		/// <summary>
		/// Copia un fitxer remot a un ruta remota
		/// 
		/// Aquesta funció esta marcada com a unsafe, ja que s'ha de utilitzar un punter directe a memoria
		/// </summary>
		/// <param name="sourceFileName">
		/// Fitxer origen <see cref="System.String"/>
		/// </param>
		/// <param name="destFileName">
		/// Fitxer destinació <see cref="System.String"/>
		/// </param>
		/// <param name="overwrite">
		///  Permet la reescritura d'un fitxer ja exitent. <see cref="System.Boolean"/>
		/// </param>		
		unsafe private void CopySambaToSamba(string sourceFileName,
        string destFileName,
        bool overwrite){
				
						
			    int bytesRead=1024;
   		        byte[] buffer = new byte[bytesRead];

			
			    fixed (void* b  = buffer){ //La instrucció  fixed evita que el recolector de elements no utilitzats ubique de nou la varaible.
				int fs;
				int fd;
				int res;
 			    
				
				//comprovem que exiteix el fitxer origen
				if(!Exists(sourceFileName))
					throw new FileNotFoundException("Fitxer Origen no existeix");
				//comprvem si existeix el fitxer destinació, sino el creem
				if(!Exists(destFileName)){
					fd=WrapperSambaClient.smbc_creat(@"smb:"+parseUNCPath(destFileName), (int)FileMode.CreateNew);
					if (fd<0){
					System.Console.WriteLine(Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EACCES" || Stdlib.GetLastError().ToString()=="EROFS" || Stdlib.GetLastError().ToString()=="EBADF")
						throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT" )
						throw new FileNotFoundException(Stdlib.GetLastError().ToString());
					}
					WrapperSambaClient.smbc_close(fd);
					
				}
				else if (!overwrite){ //si el fitxer ja existeix i no es vol sobreescriure salta una exepcio
					throw new IOException("El fitxer destí ja exiteix");
				}
				
				
				
				
				//obrim els dos fitxers
				fs=WrapperSambaClient.smbc_open(@"smb:"+parseUNCPath(sourceFileName),(int)OpenFlags.O_RDONLY,(int)FileMode.Open);
				if (fs<0){
					System.Console.WriteLine(Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EACCES" || Stdlib.GetLastError().ToString()=="EROFS" || Stdlib.GetLastError().ToString()=="EBADF")
						throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT" )
						throw new FileNotFoundException(Stdlib.GetLastError().ToString());
				}
				fd=WrapperSambaClient.smbc_open(@"smb:"+parseUNCPath(destFileName),(int)OpenFlags.O_WRONLY,(int)FileMode.CreateNew);
               	
				if (fd<0){
					System.Console.WriteLine(Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EACCES" || Stdlib.GetLastError().ToString()=="EROFS" || Stdlib.GetLastError().ToString()=="EBADF")
						throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
					if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT" )
						throw new FileNotFoundException(Stdlib.GetLastError().ToString());
				}
				
                //copiem el contigut
				
				while ((res=WrapperSambaClient.smbc_read(fs,b, bytesRead)) > 0){
			    
					WrapperSambaClient.smbc_write(fd,b,res);
					
				}
			    
				
				WrapperSambaClient.smbc_close(fd);
				WrapperSambaClient.smbc_close(fs);
			
			}
			
			
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
			
			Copy(sourceFileName,destFileName,false);
			Delete(sourceFileName);
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
			int res=WrapperSambaClient.smbc_unlink(@"smb:"+parseUNCPath(path));
			if (res<0){
				System.Console.WriteLine(Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EACCES" || Stdlib.GetLastError().ToString()=="EROFS")
					throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT" || Stdlib.GetLastError().ToString()=="ETIMEDOUT" )
					throw new DirectoryNotFoundException(Stdlib.GetLastError().ToString());
				throw new IOException(Stdlib.GetLastError().ToString());
			}
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
		
			//no cal comprovar la exitencia, ja tractem l'error
			int res=WrapperSambaClient.smbc_mkdir(@"smb:"+parseUNCPath(path),(int)FileMode.CreateNew);
			if (res<0){
				System.Console.WriteLine(Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EEXIST")
					throw new IOException ("El directori ja exiteix");
				if (Stdlib.GetLastError().ToString()=="EACCES" || Stdlib.GetLastError().ToString()=="EROFS")
					throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT" || Stdlib.GetLastError().ToString()=="ETIMEDOUT" )
					throw new DirectoryNotFoundException(Stdlib.GetLastError().ToString());
			}
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
		
			//no cal comprovar la exitencia, ja tractem l'error
			int res=WrapperSambaClient.smbc_rmdir(@"smb:"+parseUNCPath(path));
			
			if (res<0){
				System.Console.WriteLine(Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EEXIST" ||  Stdlib.GetLastError().ToString()=="EROFS")
					throw new IOException ("El directori ja exiteix");
				if (Stdlib.GetLastError().ToString()=="EACCES" )
					throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT" )
					throw new DirectoryNotFoundException(Stdlib.GetLastError().ToString());
			}
		}

		
		/// <summary>
		/// Canvia el nom d'un directori
		/// </summary>
		/// <param name="sourceFileName">
		/// Ruta del directori original <see cref="System.String"/>
		/// </param>
		/// <param name="destFileName">
		/// Ruta del directori després de canviar-li el nom <see cref="System.String"/>
		/// </param>
		
		public void Rename(
		string sourceFileName,
		string destFileName){
			int res=WrapperSambaClient.smbc_rename(@"smb:"+parseUNCPath(sourceFileName),@"smb:"+parseUNCPath(destFileName));
			if (res<0){
				System.Console.WriteLine(Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EEXIST")
					throw new IOException (Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EACCES" || Stdlib.GetLastError().ToString()=="EROFS")
					throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT")
					throw new FileNotFoundException(Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EXDEV")
					throw new NotSupportedException(Stdlib.GetLastError().ToString());
					
				
			}
				

		}
				
				
		/// <summary>
		/// Llista els recursos compartits d'un servidor CIFS
		/// </summary>
		/// <param name="UNCpath">
		/// Adreça del servidor CIFS (DNS o IP) <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// Col·lecció de recursos compartits <see cref="Shares"/>
		/// </returns>
		
		public  Shares ShareEnum(string UNCpath){ 
			/* Realment aquesta mateixa ordre en el cas de samba podria 
			 * navegar per una carpeta, però s'ha diferenciat de ReadDir
			 * per compatibilitat en Win32
			 */
			 
			int dh=0;
			Shares shares = new Shares();
			//Comprovació de l'existencia del recurs o carpeta
			dh=WrapperSambaClient.smbc_opendir(@"smb:"+parseUNCPath(UNCpath));
			if (dh<0)
				throw new IOException(Stdlib.GetLastError().ToString());
			else{
			
				IntPtr dirbuf = new IntPtr(); //creem un punter gestionat
		    
			dirbuf= WrapperSambaClient.smbc_readdir(dh);
			Share s;
			while(dirbuf!= IntPtr.Zero){
				 SambaDirInfo dir = (SambaDirInfo)Marshal.PtrToStructure(dirbuf, typeof(SambaDirInfo)); //passem el punter a l'estructura
				 dirbuf= WrapperSambaClient.smbc_readdir(dh);
				 s= new Share();
				 s.comment=dir.comment;
				 s.name=dir.name;
				 s.type=CommonShareTypes((int)dir.smbc_type);
				 shares.addShare(s);
			}
	
			WrapperSambaClient.smbc_closedir(dh);
			}

			return shares;
		}
		
		/// <summary>
		/// Llegeix el contingut d'una carpeta
		/// </summary>
		/// <param name="UNCpath">
		/// Ruta de la carpeta que es vol llegir <see cref="System.String"/>
		/// </param>
		/// <returns>
		/// Informació del directori<see cref="CIFSDirInfo"/>
		/// </returns>		
		public CIFSDirInfo ReadDir(string UNCpath){
			
			CIFSDirInfo dirinfo = new CIFSDirInfo(); 
			
			int dh=0;
			dh=WrapperSambaClient.smbc_opendir(@"smb:"+parseUNCPath(UNCpath));
			if (dh<0){
				System.Console.WriteLine(Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EACCES" || Stdlib.GetLastError().ToString()=="EROFS")
					throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT")
					throw new DirectoryNotFoundException(Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EXDEV")
					throw new ArgumentException(Stdlib.GetLastError().ToString());
			}
			else{
			
				IntPtr dirbuf = new IntPtr();
		    
			dirbuf= WrapperSambaClient.smbc_readdir(dh);
			while(dirbuf!= IntPtr.Zero){ //recorrem tot el directori
				 SambaDirInfo dir = (SambaDirInfo)Marshal.PtrToStructure(dirbuf, typeof(SambaDirInfo)); //passem el punter a l'estructura
				 dirbuf= WrapperSambaClient.smbc_readdir(dh);
				 
				 //s'ha de controlar quin tipus de recurs s'està llegint
				
				 System.Console.WriteLine();	
				 if (dir.smbc_type ==7 || dir.smbc_type==8){
					if (dir.name!=".." && dir.name!="."){
							if (dir.smbc_type ==7)
								dirinfo.addDir(dir.name); //afegim el nom del directori a la matriu de directoris del objecte CIFSDirInfo
							if (dir.smbc_type ==8)
								dirinfo.addFile(dir.name); //afegim el nom del fitxer a la matriu de fitxers del objecte CIFSDirInfo
						}
				}
						
			}
	
			WrapperSambaClient.smbc_closedir(dh);
			}


			return dirinfo;
			
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
			
			
			int res=WrapperSambaClient.smbc_rename(@"smb:"+parseUNCPath(spath),@"smb:"+parseUNCPath(dpath));
			if (res<0){
				System.Console.WriteLine(Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EEXIST")
					throw new IOException (Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EACCES" || Stdlib.GetLastError().ToString()=="EROFS")
					throw new UnauthorizedAccessException (Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EINVAL" || Stdlib.GetLastError().ToString()=="ENOENT")
					throw new DirectoryNotFoundException(Stdlib.GetLastError().ToString());
				if (Stdlib.GetLastError().ToString()=="EXDEV")
					throw new ArgumentException(Stdlib.GetLastError().ToString());
			}
		}
		


		/// <summary>
		/// Funció de traducció entre els tipus de recurs compartit samba i els tipus de CIFSClient
		/// </summary>
		/// <param name="share">
		/// Tipus de recurs samba<see cref="System.Int32"/>
		/// </param>
		/// <returns>
		/// Tipus de recurs CIFSClient <see cref="System.String"/>
		/// </returns>
		private string CommonShareTypes(int share){

		if(share==4)
			return "Printer";		
		if(share==3)
			return "Share";
		if(share==6)
			return "IPC";
		
		return "Other";
	
	}
		
	}
	

	
	
}

#endif
