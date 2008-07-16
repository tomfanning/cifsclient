// Test.cs
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
using System.IO;
using NUnit.Framework;

namespace CIFSClient
{
	
	/// <summary>
	/// Classe encarregada de realitzar els Test amb Nunit
	/// <remarks>En un futur s'hauria de separar del projecte</remarks>
	/// </summary>
	[TestFixture()]
	public class Tests
	{

		Cifs nc= new Cifs();
		string baseshare;
		string baselocal;
		string share1;
		string share2;
		string share3;
		string local1;
		
		
		[SetUp]
		/// <summary>
		/// Prepara les rutese del joc de proves per realitzar els test.
		/// </summary>
    	public void Init()
    	{
			int p = (int) Environment.OSVersion.Platform;
                if ((p == 4) || (p == 128)) {
					baselocal=@"/home/jmartin/jocproves/";
					baseshare=@"//jmartin-desktop/";
					share1=baseshare+@"share1/";
					share2=baseshare+@"share2/";
					share3=baseshare+@"share3/";
					local1=baselocal+@"local1/";	
                } else {
        			//incialitzem algunes rutes del joc de proves
					baselocal=@"C:\jocproves\";
					baseshare=@"\\10.0.2.2\";
					share1=baseshare+@"share1\";
					share2=baseshare+@"share2\";
					share3=baseshare+@"share3\";
					local1=baselocal+@"local1\";
                }

			    System.Console.WriteLine("Inici Test");

			
    	}
		
		
		
		[TearDown]
		/// <summary>
		/// Deixa el joc de proves igual que inicialment.
		/// </summary>
		public void Down()
    	{
			
			if (File.Exists(local1+@"newfile4.sql"))
			    File.Delete(local1+@"newfile4.sql");
	

			if (nc.DirectoryExists(share1+@"newDirectory"))
				nc.DeleteDirectory(share1+@"newDirectory");
			
			if (nc.DirectoryExists(share2+@"newDirectory"))
				nc.DeleteDirectory(share2+@"newDirectory");
			
			if (nc.DirectoryExists(share1+@"newDirectory2"))
				nc.DeleteDirectory(share1+@"newDirectory2");
			
			if (nc.DirectoryExists(share1+@"newDirectory5"))
				nc.DeleteDirectory(share1+@"newDirectory5");
			
			if (nc.DirectoryExists(share1+@"newDirectory6"))
				nc.DeleteDirectory(share1+@"newDirectory6");	

			if (File.Exists(local1+@"newfile3.pdf"))
			    File.Delete(local1+@"newfile3.pdf");
			
			System.Console.WriteLine("Final Test");
    	}
			
			
		[Test]
		
		public void TestCase()
		{
			

			
		}
		
		
#region testsExists
	    [Test]
		public void ExistsSambaCorrecte() {
			Assert.IsTrue(nc.Exists(share1+@"file2.txt"));
		}
		[Test] 
		public void ExistsSambaIncorrecte() {
			
			Assert.IsFalse(nc.Exists(share1+@"file2ss.jpg"));
		}
		[Test]
		public void ExistsLocalCorrecte() {
			
			Assert.IsTrue(nc.Exists(local1+@"file1.jpg"));
		}
		[Test]
		
		public void ExistsLocalIncorrecte() {
			
			Assert.IsFalse(nc.Exists(local1+@"file1ss.jpg"));
		}
		
		[Test]
		public void ExistsSambaDirCorrecte() {
			
			Assert.IsTrue(nc.DirectoryExists(share1));
		}
		[Test]
		public void ExistsSambaDirIncorrecte() {
			
			
			Assert.IsFalse(nc.DirectoryExists(share1+@"newDirectory\"));
		}
	
		[Test]
		public void ExistsLocalDirCorrecte() {
	
			Assert.IsTrue(nc.DirectoryExists(local1));
		}
		[Test]
		public void ExistsLocalDirIncorrecte() {
			
			Assert.IsFalse(nc.DirectoryExists(baselocal+@"/local333"));
		}

#endregion	
	
		
		
#region testsCopyLocalToSamba
  	    [Test]
		public void CopyLocalToSamba() {
			//copiem un fitxer nou del local a samba
			nc.Copy(local1+@"file1.jpg",share1+@"newfile1.jpg",false );
			Assert.IsTrue(nc.Exists(share1+@"newfile1.jpg"));
		}
		
		[Test]
		[ExpectedException(typeof(IOException))]
		public void CopyLocalToSamba1() {
			//copiem un fitxer nou del local a samba (pero ja existeix). Ha de saltar una excepció
			nc.Copy(local1+@"file4.jpg",share1+@"newfile1.jpg",false );
	
		}
		
		
		[Test]
		public void CopyLocalToSamba2() {
			//copiem un fixer del local a samba (ja exiteix). S'ha de sobreescriure
			nc.Copy(local1+@"file4.jpg",share1+@"newfile1.jpg",true);
			
		}
		
		
		[Test]
		[ExpectedException(typeof(UnauthorizedAccessException))]
		public void CopyLocalToSamba3() {
			//intentem copiar el fitxer a un lloc sense permis
			nc.Copy(local1+@"file1.jpg",share3+@"newfile1.jpg",true);
			
		}
		
		//intentem copiar un fitxer que no existeix a samba
		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void CopyLocalToSamba4() {
			nc.Copy(local1+@"file1sss.jpg",share1+@"newfile1.jpg",true);
			
		}
#endregion
		
#region testsCopySambaToLocal
  	    [Test]
		public void CopySambaToLocal() {
			//copiem un fitxer nou del local a samba
			nc.Copy(share2+@"file3.pdf",local1+@"newfile3.pdf",false );
			Assert.IsTrue(nc.Exists(local1+@"newfile3.pdf"));
		}
		
		[Test]
		[ExpectedException(typeof(IOException))]
		public void CopySambaToLocal1() {
			
			nc.Copy(share2+@"file3.pdf",local1+@"newfile3.pdf",false );
			
			if ( !nc.Exists(local1+@"newfile3.pdf"))
				Assert.Fail();
			
			//copiem un fitxer de samba  a local(pero ja existeix). Ha de saltar una excepció
			nc.Copy(share2+@"file3.pdf",local1+@"newfile3.pdf",false );
	
		}
		
		
		[Test]
		public void CopySambaToLocal2() {
			//copiem un fixer del local a samba (ja exiteix). S'ha de sobreescriure
			nc.Copy(share2+@"file3.pdf",local1+@"newfile3.pdf",true );
			
		}
		
		//intentem copiar el fitxer a un lloc sense permis
		[Test]
		public void CopySambaToLocal3() {
			
			nc.Copy(share3+@"file4.sql",local1+@"newfile4.sql",true);
			
		}
		
		//intentem copiar un fitxer que no existeix a samba
		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void CopySambaToLocal4() {
			
			nc.Copy(share2+@"file33.sql",local1+@"newfile3.sql",true);
			
		}
#endregion
		
#region testsCopySambaToSamba
  	    [Test]
		public void CopySambaToSamba() {
			//copiem un fitxer nou del local a samba
			nc.Copy(share1+@"file2.txt",share1+@"newfile2.txt",false );
			Assert.IsTrue(nc.Exists(share1+@"newfile2.txt"));
		}
		
		[Test]
		[ExpectedException(typeof(IOException))]
		public void CopySambaToSamba1() {
			//copiem un fitxer nou del local a samba (pero ja existeix). Ha de saltar una excepció
			nc.Copy(share1+@"file2.txt",share1+@"newfile2.txt",false );
	
		}
		
		
		[Test]
		public void CopySambaToSamba2() {
			//copiem un fixer del local a samba (ja exiteix). S'ha de sobreescriure
			nc.Copy(share1+@"file2.txt",share1+@"newfile2.txt",true);
			
		}
		
		
		[Test]
		[ExpectedException(typeof(UnauthorizedAccessException))]
		public void CopySambaToSamba3() {
			//intentem copiar el fitxer a un lloc sense permis
			nc.Copy(share1+@"file2.txt",share3+@"newfile2.txt",true);
			
		}
		
		//intentem copiar un fitxer que no existeix a samba
		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void CopySambaToSamba4() {
			//copiem un fixer del local a samba (ja exiteix). S'ha de sobreescriure
			nc.Copy(share1+@"file2222.txt",share1+@"newfile2.txt",true);
			
		}

	#endregion	


	#region testsDelete
		[Test]
		public void Delete1() {
		
			nc.Delete(share1+@"newfile1.jpg");
			nc.Delete(share1+@"newfile2.txt");
		}
		
		[Test]
		[ExpectedException(typeof(UnauthorizedAccessException))]
		public void Delete2() {
			//no hi ha permisos per borrar
			nc.Delete(share3+@"file4.sql");
			
		}
		
		[Test]
		[ExpectedException(typeof(IOException))]
		public void Delete3() {
		    //no existeix
			nc.Delete(share1+@"newfile111.txt");
			
		}
		
	
	#endregion
		
		

		
	#region Move
		
		[Test]
		public void Move1() {
		
			nc.Move(share1+@"file5.jpg",share2+@"rfile5.jpg");

			if ( nc.Exists(share1+@"file5.jpg") && !nc.Exists(share2+@"rfile5.jpg") )
				Assert.Fail();
			
			nc.Move(share2+@"rfile5.jpg",share1+@"file5.jpg");
			
			if ( nc.Exists(share2+@"file5.jpg") && !nc.Exists(share1+@"rfile5.jpg") )
				Assert.Fail();

		
		}
		
		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void Move2() {
			//el fitxer origen no existeix
			nc.Move(share1+@"file5ddd.jpg",share2+@"rfile5.jpg");
			
		}
		
		[Test]
		[ExpectedException(typeof(UnauthorizedAccessException))]
		
		public void Move3() {
		    //no te permis
			nc.Move(share1+@"file5.jpg",share3+@"rfile5.jpg");
			
		}
		
	#endregion
		

    #region Rename
		
		[Test]
		public void Rename1() {
		
			
			nc.Rename(share1+@"file5.jpg",share1+@"rfile5.jpg");

			if ( nc.Exists(share1+@"file5.jpg") && !nc.Exists(share1+@"rfile5.jpg") )
				Assert.Fail();
			
			nc.Rename(share1+@"rfile5.jpg",share1+@"file5.jpg");
			
			if ( !nc.Exists(share1+@"file5.jpg") && nc.Exists(share1+@"rfile5.jpg") )
				Assert.Fail();

		}
		
		[Test]
		[ExpectedException(typeof(FileNotFoundException))]
		public void Rename2() {
			//no existeix el fitxer origen
			nc.Rename(share1+@"file5sss.jpg",share1+@"rfile5.jpg");

			
		}
		
		[Test]
		[ExpectedException(typeof(UnauthorizedAccessException))]
		public void Rename3() {
		    //no hi ha permisos
			nc.Rename(share3+@"file4.sql",share3+@"rfile4.sql");
			
		}
		
	#endregion
		
	
	#region testsmkdir
		
		[Test]
		public void CreateDirectory1() {
			nc= new Cifs();
			nc.CreateDirectory(share1+@"newDirectory");
			if ( !nc.DirectoryExists(share1+@"newDirectory"))
				Assert.Fail();
			nc.DeleteDirectory(share1+@"newDirectory");
			if (nc.DirectoryExists(share1+@"newDirectory"))
				Assert.Fail();
		}

		
		[Test]
		[ExpectedException(typeof(UnauthorizedAccessException))]
		public void CreateDirectory2() {
			//no hi ha  permisos per fer això
			nc.CreateDirectory(share3+@"newDirectory2");

		}
		

		[Test]
		[ExpectedException(typeof(IOException))]
		public void CreateDirectory3() {
			//prova de reescritura
			nc.CreateDirectory(share1+@"newDirectory2");
			nc.CreateDirectory(share1+@"newDirectory2");
			
		}
		
		
		
		[Test]
		[ExpectedException(typeof(DirectoryNotFoundException))]
		public void CreateDirectory4() {
			//ruta no vàlida
			nc.CreateDirectory(@"\\aasd\\newDirectory");
		}
		
	#endregion
		
	#region testsrmdir
		
		[Test]
		public void DeleteDirectory1() {
			//borra una carpeta
			
			nc.CreateDirectory(share1+@"newDirectory");
			if ( !nc.DirectoryExists(share1+@"newDirectory"))
				Assert.Fail();
			nc.DeleteDirectory(share1+@"newDirectory");
			if (nc.DirectoryExists(share1+@"newDirectory"))
				Assert.Fail();

		}
		
		[Test]
		[ExpectedException(typeof(IOException))]
		public void DeleteDirectory2() {
			//borrem un directori sense permis
			nc.DeleteDirectory(share3);
			
		}
		
		[Test]
		[ExpectedException(typeof(DirectoryNotFoundException))]
		public void DeleteDirectory3() {
		    //Borrem un directori que no existeix o no es vàlid
			nc.DeleteDirectory(share1+@"newDirectory3");
			
		}
		
	#endregion	
		
		
	#region RenameDirectory
		
		[Test]
		
		public void RenameDirectory1() {
		
			nc.CreateDirectory(share1+@"newDirectory5");
			if ( !nc.DirectoryExists(share1+@"newDirectory5"))
				Assert.Fail();
			
			nc.RenameDirectory(share1+@"newDirectory5",share1+@"newDirectory6");
			if ( !nc.DirectoryExists(share1+@"newDirectory6"))
				Assert.Fail();
			
			nc.DeleteDirectory(share1+@"newDirectory6");
			if (nc.DirectoryExists(share1+@"newDirectory6"))
				Assert.Fail();

		}
		
		[Test]
		[ExpectedException(typeof(DirectoryNotFoundException))]
		
		public void RenameDirectory2() {
			//no existeix el fitxer origen
			nc.RenameDirectory(share1+@"newDirectory5",share1+@"newDirectory6");

			
		}
		
		[Test]
		[ExpectedException(typeof(IOException))]
		public void RenameDirectory3() {
		    //no existeix el direcory
			nc.RenameDirectory(share3,baseshare+"\newDirectory6");
			
		}
		
	#endregion
		
		
	#region ShareEnum
		[Test]
		public void ShareEnum1() {
			bool s1=false;
			bool s2=false;
			bool s3=false;
			Shares shares=(Shares) nc.ShareEnum(baseshare);
			
			foreach (Share s in shares){
				if (s.name=="share1")
					s1=true;
				if (s.name=="share2")
					s2=true;
				if (s.name=="share3")
					s3=true;
			}   
			Assert.IsTrue(s1&&s2&&s3);
		}
		
		
		[Test]
		[ExpectedException(typeof(IOException))]
		public void ShareEnum2() {
			//el recurs no exiteix
			Shares shares=(Shares) nc.ShareEnum(baseshare+@"newDirectory/");
			
			
		}
	#endregion
		
		
	#region ReadDir
		[Test]
		public void ReadDirFiles() {
			CIFSDirInfo dirinfo=nc.ReadDir(share1);
			string[] pro = new string[] {"file5.jpg", "file2.txt"};
			string[] res=dirinfo.GetFiles();
			
						
			if (pro.Length <= res.Length)
			{
				for (int x = 0; x < pro.Length; x++)
				{
					if (pro[x] != res[x])  Assert.IsTrue(false);
				}
			}
			else
				Assert.IsTrue(false);

			Assert.IsTrue(true);
		}
		
		[Test]
		public void ReadDirDir() {
			
			CIFSDirInfo dirinfo=nc.ReadDir(share1);
			string[] pro = new string[] { "subcarpeta"};
			string[] res=dirinfo.GetDirectories();
			
						
			if (pro.Length <= res.Length)
			{
				for (int x = 0; x < pro.Length; x++)
				{
					if (pro[x] != res[x]) Assert.IsTrue(false);
				}
			}
			else
				Assert.IsTrue(false);

			Assert.IsTrue(true);

		}
		
		
				
		[Test]
		[ExpectedException(typeof(DirectoryNotFoundException))]
		public void ReadDirError() {
			//No existeix el directory
			CIFSDirInfo dirinfo=nc.ReadDir(share1+@"newDirectory3");
		}
	#endregion
		
	}
}
