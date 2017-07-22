# Creating Your Own Package
A package is a collection of chalk extensions, widgets, plug-Ins, themes, and any other files in Graffiti CMS grouped together as a single unit. For example, you may have created a theme, a plug-in, and a chalk extension for a photography Website. Instead of installing all of these items separately, you can simply offer them as a photography package.

_**Procedure**_

1.  Set up a directory on your computer that matches the folder structure of Graffiti. You should include copy your Graffiti files over to the matching folders in the new directory.
2. Create additional folders into which you can install the files you have in your package. For example, assuming your photography package includes chalk extension, widgets, plug-ins, and themes, your directory may include the following folders:
* C:\Photography\
* C:\Photography\bin – Contains the two .dlls for your plug-in and chalk extension.
* C:\Photography\files\themes\phototheme\ – Contains the files for your theme.
3. To package the files, run Package.exe from the /Tools/ folder. For help on this utility you can run Package /? at the command line. The package will be created in the directory in which package.exe is located.