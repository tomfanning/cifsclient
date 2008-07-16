// Shares.cs
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
	/// Classe enumerable que contè una col·lecció de recursos compartits
	/// </summary>
	public class Shares : System.Collections.IEnumerable
	{
		/// <summary>
		/// Array de recursos compartits
		/// </summary>
		private ArrayList shares;
		
		/// <summary>
		/// Constructor
		/// </summary>		
		public Shares() 
		{
			shares = new ArrayList();
		}
		/// <summary>
		/// Afegiex un recurs compartit a la collecció
		/// </summary>
		/// <param name="share">
		/// Recurs Compartit <see cref="Share"/>
		/// </param>
		public void addShare(Share share){
			shares.Add(share);
		}
		
		/// <summary>
		/// Quantitat de recursos compartits
		/// </summary>
		public int Count(){
			return shares.Count;
		}
		
		/// <summary>
		/// Obte un enumerador de recursos
		/// </summary>
		
    public IEnumerator GetEnumerator()
    {
        
        return shares.GetEnumerator() ;
        
    }
		
	}
}
