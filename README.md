Dependencies
============

* C# (version?)
* Emgu CV >= 3.1.0

Installing Emgu Cv
==================


* Preferably download https://sourceforge.net/projects/emgucv/files/emgucv/3.1.0/
extract under ext_lib and rename the folder to "libemgucv" without the quotes.
This way you dont have edit any references.

* Alternative: edit the reference to Emgu.CV.World.dll to point to corresponding file under
where you have installed Emgu Cv

Building for Windows
====================

* If you have not installed Emgu Cv under ext_lib like descrived before
edit the projects post build events so that required emgu dll:s get copied to
the build directory (from where you have installed Emgu Cv).
* Alternatively you can copy the files manually after building the project.

Required files are:

* [emgu]\bin\x86\cvextern.dll
* [emgu]\bin\x86\msvcp120.dll
* [emgu]\bin\x86\msvcr120.dll

These files need to be copied to x86 directory under the root build directory.

IF YOU HAVE installed Emgu Cv under ext_lib like described before no changes should be neccessary.

Building 64-bit version has not been tried.