// Share.cs
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

namespace CIFSClient
{
	
	/// <summary>
	/// Estructura en infomació minima d'un recurs compartit
	/// Aquesta estructura es utilitzada per CIFSClient per homogeneïtzar les dades d'un recurs compartit
	/// </summary>
	public struct Share
	{
		public string type;
		public string name;
		public string comment;
	}
	
	
}
