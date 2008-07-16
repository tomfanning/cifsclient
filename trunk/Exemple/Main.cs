using System;
using CIFSClient; //utilitzem el namespace CIFSClient
using System.IO;

namespace Exemple
{
  class MainClass
  {
    public static void Main(string[] args)
    {
      //ruta remota
      string remot="//jmartin-desktop/fotos/";
      //ruta local
      string local="temp";
      //text de sortida
      string restext="";
      //text per guardar l'extensió dels fitxer
      string ext="";


      //si no existeix la carpeta local la crea
      if (!Directory.Exists(local)){
        Directory.CreateDirectory(local);
      }

      //instanciem un objecte de la biblioteca
      Cifs cifs = new Cifs();
      try{
        //llegim el contingut del directori remot
        CIFSDirInfo rdir = cifs.ReadDir(remot);
        //recorrem tots els fitxers
        foreach(String file in rdir.GetFiles() ){
          ext=file.Split('.')[1];
          //mirem si tenen l'extensió jpg
          if (ext=="jpg"){
            //copiem el fitxer remot a la carpeta local (permetem reescriure)
            cifs.Copy(remot+file,local+"/"+file,true);
            restext+="S'ha copiat el fitxer remot ("+remot+file+ ")";
            restext+="a ("+local+"/"+file+")";
            restext+=System.Environment.NewLine;
          }
        }
      }
      //controlem la excepció en cas que no tenir permisos per navegar la carpeta.
      catch (UnauthorizedAccessException e){
        restext+="Error: No hi ha permisos per realitzar aquesta comanda.";
        restext+=System.Environment.NewLine;
      }
      //controlem l'excepció en cas de no existir la carpeta remota o local
      catch (System.IO.DirectoryNotFoundException e){
        restext+="Error: No s'ha trobat el directori.";
        restext+=System.Environment.NewLine;
      }
      //controlem l'excepció en cas de no poder llegir ho  escriure els fitxers
      catch (System.IO.IOException e){
        restext+="Error: Al sistema de fitxers.";
        restext+=System.Environment.NewLine;
      }
      //controlem de forma genèrica qualsevol altra excepció
      catch (Exception e){
        restext+="Error: No definit.";
        restext+=System.Environment.NewLine;
      }
      System.Console.WriteLine(restext);

    }
  }
}