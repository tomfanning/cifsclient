// SambaShareType.cs
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
	
namespace CIFSClient.Samba
{	
	/// <summary>
	/// Enumeració de tipus de recursos compartits de samba
	/// </summary>
	[Flags]
	public enum SambaShareType
	{
		SMBC_WORKGROUP=1,
        SMBC_SERVER=2,
        SMBC_FILE_SHARE=3,
        SMBC_PRINTER_SHARE=4,
        SMBC_COMMS_SHARE=5,
        SMBC_IPC_SHARE=6,
        SMBC_DIR=7,
        SMBC_FILE=8,
        SMBC_LINK=9,
		
	}
}
