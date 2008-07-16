// SambaDirInfo.cs
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
using System.Runtime.InteropServices;

namespace CIFSClient.Samba
{
	
	/// <summary>
	/// Estructura d'informació de directoris utilitzada per samba
	/// </summary>
	[StructLayout(LayoutKind.Sequential)]
	   
        public struct SambaDirInfo
        {

    
		public uint smbc_type;
			/** Length of this smbc_dirent in bytes
			 */
		public uint dirlen;
			/** The length of the comment string in bytes (does not include
			 *  null terminator)
			 */
		public uint commentlen;
			/** Points to the null terminated comment string
			 */
		[MarshalAs(UnmanagedType.LPStr)] public String comment;
			/** The length of the name string in bytes (does not include
			 *  null terminator)
			 */
		public uint namelen;
			/** Points to the null terminated name string
			 */
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst=1)] public String name;
		};
        
 

}
