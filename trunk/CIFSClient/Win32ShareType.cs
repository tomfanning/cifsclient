/*
 * Creado por SharpDevelop.
 * Usuario: Administrador
 * Fecha: 13/03/2008
 * Hora: 8:54
 * 
 * Para cambiar esta plantilla use Herramientas | Opciones | Codificación | Editar Encabezados Estándar
 */

	/// <summary>
	/// Type of share
	/// </summary>
	
	
using System;
	
namespace CIFSClient.Win32
{	
	[Flags]
	public enum Win32ShareType
	{
		/// <summary>Disk share</summary>
		Disk		= 0,
		/// <summary>Printer share</summary>
		Printer		= 1,
		/// <summary>Device share</summary>
		Device		= 2,
		/// <summary>IPC share</summary>
		IPC			= 3,
		/// <summary>Special share</summary>
		Special		= -2147483648, // 0x80000000,
		
		
		
	}
}
