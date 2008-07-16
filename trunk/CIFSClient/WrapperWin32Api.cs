// WrapperWin32Api.cs
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
using System.Runtime.InteropServices;

namespace CIFSClient.Win32
{
	
	
	public static class WrapperWin32Api
	{
		

		
		
		//constants
		
		//Maximum path length
		public const int MAX_PATH = 260;
		//No error
		public const int NO_ERROR = 0;
		//Access denied
		public const int ERROR_ACCESS_DENIED = 5;
		//Access denied
		public const int ERROR_WRONG_LEVEL = 124;
		//More data available
		public const int ERROR_MORE_DATA = 234;
		//Not connected
		public const int ERROR_NOT_CONNECTED = 2250;
		//Level 1
		public const int UNIVERSAL_NAME_INFO_LEVEL = 1;
		//Max extries (9x)
		public const int MAX_SI50_ENTRIES = 20;
		

		
		///<summary>Estructura per guardar una ruta Unc</summary>
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
		public struct UNIVERSAL_NAME_INFO 
		{
			[MarshalAs(UnmanagedType.LPTStr)]
			public string lpUniversalName;
		}

		/// <summary>Estructura de dades de recursos compartits de nivell 2, amb NT</summary>

		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
		public struct SHARE_INFO_2 
		{
			[MarshalAs(UnmanagedType.LPWStr)]
			public string NetName;
			public Win32ShareType ShareType;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Remark;
			public int Permissions;
			public int MaxUsers;
			public int CurrentUsers;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Path;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Password;
		}
		
		/// <summary>Estructura de dades de recursos compartits de nivell 1, amb NT</summary>
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Unicode)]
		public struct SHARE_INFO_1 
		{
			[MarshalAs(UnmanagedType.LPWStr)]
			public string NetName;
			public Win32ShareType ShareType;
			[MarshalAs(UnmanagedType.LPWStr)]
			public string Remark;
		}
		
		/// <summary>Estructura de dades de recursos compartits de nivell 2, amb Win9x</summary>
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
		public struct SHARE_INFO_2_9x 
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=13)]
			public string NetName;

			public byte bShareType;
			public ushort Flags;
			
			[MarshalAs(UnmanagedType.LPTStr)]
			public string Remark;
			[MarshalAs(UnmanagedType.LPTStr)]
			public string Path;

			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=9)]
			public string PasswordRW;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=9)]
			public string PasswordRO;
			
			public Win32ShareType ShareType
			{
				get { return (Win32ShareType)((int)bShareType & 0x7F); }
			}
		}
		
		/// <summary>Estructura de dades de recursos compartits de nivell 1, amb Win9x</summary>
		[StructLayout(LayoutKind.Sequential, CharSet=CharSet.Ansi, Pack=1)]
		public struct SHARE_INFO_1_9x 
		{
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=13)]
			public string NetName;
			public byte Padding;

			public ushort bShareType;
			
			[MarshalAs(UnmanagedType.LPTStr)]
			public string Remark;
			
			public Win32ShareType ShareType
			{
				get { return (Win32ShareType)((int)bShareType & 0x7FFF); }
			}
		}


		
		// Funcions per obtindre una ruta UNC
		[DllImport("mpr", CharSet=CharSet.Auto)]
		public static extern int WNetGetUniversalName (string lpLocalPath,
			int dwInfoLevel, ref UNIVERSAL_NAME_INFO lpBuffer, ref int lpBufferSize);


		[DllImport("mpr", CharSet=CharSet.Auto)]
		public static extern int WNetGetUniversalName (string lpLocalPath,
			int dwInfoLevel, IntPtr lpBuffer, ref int lpBufferSize);

		//funcions per obtindre els recursos compartits de un servidor CIFS
		[DllImport("netapi32", CharSet=CharSet.Unicode)]
		public static extern int NetShareEnum (string lpServerName, int dwLevel,
			out IntPtr lpBuffer, int dwPrefMaxLen, out int entriesRead,
			out int totalEntries, ref int hResume);

		//per a win9x
		[DllImport("svrapi", CharSet=CharSet.Ansi)]
		public static extern int NetShareEnum(
			[MarshalAs(UnmanagedType.LPTStr)] string lpServerName, int dwLevel,
			IntPtr lpBuffer, ushort cbBuffer, out ushort entriesRead,
			out ushort totalEntries);

		//neteja del buffer
		[DllImport("netapi32")]
		public static extern int NetApiBufferFree(IntPtr lpBuffer);
		

		
	}
	    
	}
