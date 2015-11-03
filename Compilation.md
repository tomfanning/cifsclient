# Compilation #


Compilation with Mono (Windows or Linux):
```

      gmcs -target:library -unsafe -define:LINUX -r:System.dll,Mono.Posix.dll
      ClientFactory.cs SambaClient.cs Shares.cs WrapperSambaClient.cs
      SambaDirInfo.cs WrapperWin32Api.cs Cifs.cs IClient.cs SambaShareType.cs
      Win32Client.cs CIFSDirInfo.cs Share.cs Win32ShareType.cs
      -out:CIFSClient.dll
```

Compilation with .Net:
```

      csc /target:library /unsafe /out:CIFSClient.dll /reference:System.dll
      ClientFactory.cs SambaClient.cs Shares.cs WrapperSambaClient.cs
      SambaDirInfo.cs WrapperWin32Api.cs Cifs.cs IClient.cs SambaShareType.cs
      Win32Client.cs CIFSDirInfo.cs Share.cs Win32ShareType.cs
```