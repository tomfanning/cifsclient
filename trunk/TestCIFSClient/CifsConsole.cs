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
using CIFSClient;

namespace TestCIFSClient
{
	/// <summary>
	/// Description of Console.
	/// </summary>
	public class CifsConsole
	{
		Cifs cifs= new Cifs();
		string path="";
		public CifsConsole()
		{
		}
		
		public string addComand(string c){
			string[] tmpodre;
			string[] tmpvars;
			string ordre="";
			string vars="";
			string var1="";
			string var2="";
			
			if (c.IndexOf('"')>0){
				tmpodre=c.Split(' ');
				tmpvars=c.Split('"');
				ordre = tmpodre[0].ToLower();
				vars=tmpvars[1];
				var1=tmpvars[1];
				if (tmpvars.Length>3)
					var2 =tmpvars[3];
			}
			else{
				tmpodre=c.Split(' ');
				ordre = tmpodre[0].ToLower();
				if (tmpodre.Length>1){
					vars=tmpodre[1];
					var1 = tmpodre[1];
					
					if (tmpodre.Length>2)
						var2 =tmpodre[2];
				}
			}
			
			int p = (int) Environment.OSVersion.Platform;
			if ((p == 4) || (p == 128)){
				var1=this.path+"/"+var1;
				var2=this.path+"/"+var2;
			}
			else{
				var1=this.path+"\\"+var1;
				var2=this.path+"\\"+var2;
			}
			
			string restext="CIFS:>"+this.path+"$ "+c+System.Environment.NewLine;
			
			try{
				switch( ordre )
					
				{
						
					case "shares":
						restext+=fshares(vars);
						break;
						
					case "mkdir":
						restext+=fmkdir(var1);
						break;
					case "ls":
						restext+=fls(this.path);
						break;
					case "cd":
						restext+=fcd(vars);
						break;
					case "abs":
						restext+=fabs(vars);
						break;
					case "pwd":
						restext+=this.path;
						break;
					case "rm":
						restext+=frm(var1);
						break;
						
					case "rmdir":
						restext+=frmdir(var1);
						break;
						
					case "cp":
						restext+=fcp(var1,var2);
						break;
					case "mv":
						restext+=fmv(var1,var2);
						break;
					case "rename":
						restext+=frename(var1,var2);
						break;
					case "renamedir":
						restext+=frenamedir(var1,var2);
						break;
					
				    case "about":
						restext+="CIFSClient Test"+System.Environment.NewLine;
						restext+="TFC – UOC (2008)"+System.Environment.NewLine;
						restext+="Jordi Martín"+System.Environment.NewLine;
					break;
					
					case "help":
						restext+="Comandes disponibles:"+System.Environment.NewLine;
						restext+="shares     Mostra els recursos compartits d'una servidor CIFS"+System.Environment.NewLine;
						restext+="mkdir      Crea una carpeta"+System.Environment.NewLine;
						restext+="ls         Mostra el contingut de la carpeta actual"+System.Environment.NewLine;
						restext+="cd         Fixa una nova carpeta indicant una ruta relativa"+System.Environment.NewLine;
						restext+="abs        Fixa una nova carpeta indicant una ruta absoluta"+System.Environment.NewLine;
						restext+="pwd        Mostra la carpeta actual"+System.Environment.NewLine;
						restext+="rm         Borra un fitxer"+System.Environment.NewLine;
						restext+="rmdir      Borra una carpeta"+System.Environment.NewLine;
						restext+="cp         Copia un fitxer"+System.Environment.NewLine;
						restext+="mv         Mou un fitxer"+System.Environment.NewLine;
						restext+="rename     Canvia el nom a un fitxer"+System.Environment.NewLine;
						restext+="renamedir  Canvia el nom a una carpeta"+System.Environment.NewLine;
						restext+="help       Mostra aquesta ajuda"+System.Environment.NewLine;
						restext+="about      Mostra informació sobre l'autor i el projecte."+System.Environment.NewLine;
						
						break;
					default:
						restext+="Comanda no vàlida. Utilitza help per consultar les comandes vàlides."+System.Environment.NewLine;
						break;
				}
			}
			catch (UnauthorizedAccessException e){
				restext+="Error: No hi ha permisos per realitzar aquesta comanda."+System.Environment.NewLine;
				restext+="Debug:"+System.Environment.NewLine+e.StackTrace+System.Environment.NewLine;
			}
			
			catch (System.IO.FileNotFoundException e){
				restext+="Error: No s'ha trovat el fitxer"+System.Environment.NewLine;
				restext+="Debug:"+System.Environment.NewLine+e.StackTrace+System.Environment.NewLine;
			}
			

						catch (System.NotSupportedException e){
				restext+="Error: Acció no suportada"+System.Environment.NewLine;
				restext+="Debug:"+System.Environment.NewLine+e.StackTrace+System.Environment.NewLine;
			}
			
						catch (System.IO.DirectoryNotFoundException e){
				restext+="Error: No s'ha trobat el directori"+System.Environment.NewLine;
				restext+="Debug:"+System.Environment.NewLine+e.StackTrace+System.Environment.NewLine;
			}
						catch (System.IO.IOException e){
				restext+="Error: Al sistema de fitxers"+System.Environment.NewLine;
				restext+="Debug:"+System.Environment.NewLine+e.StackTrace+System.Environment.NewLine;
			}
						catch (Exception e){
				restext+="Error: No definit"+System.Environment.NewLine;
				restext+="Debug:"+System.Environment.NewLine+e.StackTrace+System.Environment.NewLine;
			}
			
			

			
			
			return restext;
			
		}
		
		private string fshares(string s){
			string res="";
			
			foreach (Share sh in  this.cifs.ShareEnum(s))
				res+=sh.name+"\t"+sh.type+"\t"+sh.comment+System.Environment.NewLine;
			
			
			
			return res;
		}
		
		private string fls(string s){
			string res="";
			
			CIFSDirInfo dirinfo =this.cifs.ReadDir(s);
			
			foreach (string dir in  dirinfo.GetDirectories())
				res+= "<DIR> \t"+dir+System.Environment.NewLine;
			foreach (string file in  dirinfo.GetFiles())
				res+= "<FILE>\t"+file+System.Environment.NewLine;
			
			
			return res;
		}
		
		private string fmkdir(string s){
			string res="";
			this.cifs.CreateDirectory(s);
			return res;
		}
		
		
		private string frmdir(string s){
			string res="";
			this.cifs.DeleteDirectory(s);
			return res;
		}
		
		private string fcp(string s,string d){
			string res="";
			this.cifs.Copy(s,d,true);
			return res;
		}
		
		
		private string fmv(string s,string d){
			string res="";
			this.cifs.Move(s,d);
			return res;
		}
		
		private string frename(string s,string d){
			string res="";
			this.cifs.Rename(s,d);
			return res;
		}
		
		private string frenamedir(string s,string d){
			string res="";
			this.cifs.RenameDirectory(s,d);
			return res;
		}
		
		
		private string frm(string s){
			string res="";
			this.cifs.Delete(s);
			return res;
		}
		
		private string fabs(string s){
			
			string tmp=s;
			int p = (int) Environment.OSVersion.Platform;
			
			
			
			
			if ((p == 4) || (p == 128)) {
				
				tmp = tmp.Replace(@"\",@"/"); //Linux
				
				
			} else {
				tmp = tmp.Replace(@"/",@"\");  //windows
			}
			
			if (this.cifs.DirectoryExists(tmp)){
				this.path=tmp;
				System.Console.WriteLine(this.path);
				return "";
			}
			else{
				System.Console.WriteLine(this.path);
				return "Ruta no valida"+System.Environment.NewLine;
			}
			
		}
		
		private string fcd(string s){
			
			string tmp="";
			int p = (int) Environment.OSVersion.Platform;
			
			//es vol baixar una carpeta
			if (s=="..")
				if ((p == 4) || (p == 128))
				tmp=this.path.Substring(0,this.path.LastIndexOf(@"/"));
			else
				tmp=this.path.Substring(0,this.path.LastIndexOf(@"\"));
			
			else{
				if (s.LastIndexOf('/') == s.Length-1)
					if (this.path!="")
					tmp = this.path+'/' + s.Remove(s.LastIndexOf('/'),1);
				else
					tmp = this.path + s.Remove(s.LastIndexOf('/'),1);
				else
					if (this.path!="")
					tmp = this.path+'/' + s;
				else
					tmp = this.path+ s;
				
				
				
				if ((p == 4) || (p == 128)) {
					
					tmp = tmp .Replace(@"\",@"/"); //Linux
					
					
				} else {
					tmp = tmp .Replace(@"/",@"\");  //windows
				}
			}
			
			if (this.cifs.DirectoryExists(tmp)){
				this.path=tmp;
				System.Console.WriteLine(this.path);
				return "";
			}
			else{
				System.Console.WriteLine(this.path);
				return "Ruta no valida"+System.Environment.NewLine;
			}
			
		}
		
	}
}
