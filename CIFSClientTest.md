#Test aplication
# Details #

For test CIFSClient at real world has been created a aplication for test purpose.

This aplication simulate a console for working with shared files.

![http://cifsclient.googlecode.com/svn/trunk/test_win.png](http://cifsclient.googlecode.com/svn/trunk/test_win.png)

The avaible commands are:

  * shares
  * mkdir
  * ls
  * cd
  * abs
  * pwd
  * rm
  * rmdir
  * cp
  * mv
  * rename
  * renamedir
  * help
  * about


Use example:

```
      CIFS:>$ shares localhost
      print$        Share        Printer Drivers
      Dades               Share
      Pelis               Share
      Compartit           Share
      IPC$                IPC           IPC    Service (Samba, Ubuntu)
      PDF                 Printer       PDF
      Deskjet_D1300Printer       Deskjet_D1300
      mp3                 Share
      fotos               Share
      CIFS:>$ abs //localhost/mp3
      CIFS:>//localhost/mp3$ ls
      <DIR> Fischerspooner - Odyssey (2005)
      <DIR> Moby-Go-The_Very_Best_Of_Moby-2CD-2006-ESC
      <DIR> a rush of blood to the head - coldplay
      <DIR> coldplay - song for the one i love
      <DIR> Prodigy - the fat of the land (1997)
      <DIR> REM - Accelerate [2008]
      <DIR> The Chemical Brothers - Push The Button (2005)
      <DIR> Fatboy Slim - Palookaville (2005)
      <DIR> ROYKSOPP - The Understanding (2005)
      <DIR> coldplay x&y
      <FILE> Moby Y Amaral-Escapar.mp3
      <FILE> La Fuga - En vela.mp3
      <FILE> 3-10 Frozen Flame.mp3
      <FILE> 19-U2 - Window In The Skies # 18 Singles.mp3
      <FILE> test.mp3
      <FILE> Blood Quest - The Frozen Flame.mp3
      <FILE> Creep - Radio Head.MP3
      <FILE> Foo Fighters - DOA.mp3
      <FILE> 01moby - we are all made of stars.mp3
      <FILE> 04 - El 7 de Septiembre.mp3
      <FILE> 01-moby-we_are_all_made_of_stars-esc.mp3
      CIFS:>//localhost/mp3$ rm test.mp3
```