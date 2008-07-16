// Wrapper.cs
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
using System.Runtime.InteropServices;
using Mono.Unix.Native;

/// <summary>
/// Namespace que conté les classes del wrapper a Samba libsmbclient
/// 
/// 
/// </summary>
/// <remarks> Es pot trovar més informació libsmbclient aquí: http://www.koders.com/c/fid83446ABB1049F42825D490477BEB948EC1DB202C.aspx?s=md5</remarks>

namespace CIFSClient.Samba
{
	
	/// <summary>
	/// Classe que conté la firma de les funcions exportades de  
	/// </summary>
	public static class WrapperSambaClient
	{

        public static void SmbInit ( ){
			smbc_init(callBackAuth,0);
		}
		
		/// <summary>
		/// Crida callback per indicar credencials de connexió
		/// Per defecte s'utlitza l'usari guest 
		/// </summary>

        public static void callBackAuth([MarshalAs(UnmanagedType.LPStr)]String server,[MarshalAs(UnmanagedType.LPStr)]String share,
                                        [MarshalAs(UnmanagedType.LPStr)]String workgroup,int wgmaxlen,
                                        [MarshalAs(UnmanagedType.LPStr)]String username,int unmaxlen,
                                        [MarshalAs(UnmanagedType.LPStr)]String password,int pwmaxlen){
			server="";
			share="";
            username="guest";
			password="";
			workgroup="";
			unmaxlen=username.Length;
			pwmaxlen=password.Length;
			wgmaxlen=workgroup.Length;
        }
        
		/// <summary>
		/// Funció que delega el pas de credencials
		/// </summary>
        public delegate void smbCGetAuthDataFn([MarshalAs(UnmanagedType.LPStr)]String server,[MarshalAs(UnmanagedType.LPStr)]String share,
                                        [MarshalAs(UnmanagedType.LPStr)]String workgroup, int wgmaxlen,
                                        [MarshalAs(UnmanagedType.LPStr)]String username, int unmaxlen,
                                        [MarshalAs(UnmanagedType.LPStr)]String password, int pwmaxlen
                                        );
        

		[DllImport("libsmbclient.so",SetLastError=true)] extern internal static int smbc_init(smbCGetAuthDataFn callBackAuth, int debug);
		
		
		//intruccions de directori
		[DllImport("libsmbclient.so",SetLastError=true)] extern internal static int smbc_opendir([MarshalAs(UnmanagedType.LPStr)]String durl);
        [DllImport("libsmbclient.so",SetLastError=true)] extern internal static int smbc_closedir(int a);
        [DllImport("libsmbclient.so",SetLastError=true)] extern internal static IntPtr smbc_readdir(int dh);
        [DllImport("libsmbclient.so",SetLastError=true)] extern internal static int smbc_mkdir([MarshalAs(UnmanagedType.LPStr)]String durl,int mode);
		[DllImport("libsmbclient.so",SetLastError=true)] extern internal static int smbc_rename([MarshalAs(UnmanagedType.LPStr)]String ourl,[MarshalAs(UnmanagedType.LPStr)]String nurl);
		[DllImport("libsmbclient.so",SetLastError=true)] extern internal static int smbc_rmdir([MarshalAs(UnmanagedType.LPStr)]String durl);
		
		
		//instruccions de fitxer
		[DllImport("libsmbclient.so",SetLastError=true)] extern internal static int smbc_creat([MarshalAs(UnmanagedType.LPStr)]String furl, int mode);
		[DllImport("libsmbclient.so",SetLastError=true)] extern internal static int smbc_open([MarshalAs(UnmanagedType.LPStr)]String furl, int flags, int mode);
		[DllImport("libsmbclient.so",SetLastError=true)] extern internal static unsafe int smbc_write(int fd,void* buf, int bufsize);
		[DllImport("libsmbclient.so",SetLastError=true)] extern internal static unsafe int smbc_read(int fd,void*  buf, int bufsize);
		[DllImport("libsmbclient.so",SetLastError=true)] extern internal static int smbc_lseek(int fd, int offset, SeekFlags seek);
		[DllImport("libsmbclient.so",SetLastError=true)] extern internal static int smbc_close(int fd);
		[DllImport("libsmbclient.so",SetLastError=true)] extern internal static int smbc_unlink([MarshalAs(UnmanagedType.LPStr)]String furl);

		
	}
}
#endif
