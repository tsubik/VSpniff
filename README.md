VSpniff - Visual Studio project not included files finder
=========================

VSpniff is a simple tool to lookup for some missing files references in the Visual Studio project xml file. 

Instalation
-----------

You need have powershell installed.

1. Download latest version or whole repository and compile it by yourself.
2. Open Visual Studio command prompt 

<blockquote>
installutil -i VSpniff.Cmdlet.dll
</blockquote>

3. Add snapin to your powershell profile file. In package manager console.

<blockquote>
notepad $profile
</blockquote> 

In file add

<blockquote>
Add-PSSnapIn VSpniff
</blockquote>

Save and close file. Restart shell or type

<blockquote>
. $profile
</blockquote>

Done

Example
-------

Let's assume our solution have some not included view:
~/Views/Account/Login.cshtml

Just type in package manager console

<blockquote>
Find-MissingFiles
</blockquote>

And all potencially missing files will be listened.

You can specify directory where module should start looking for project file

<blockquote>
Find-MissingFiles -d /SomeRelativeDirectory
</blockquote>

<blockquote>
Find-MissingFiles -d d:\dev\myprojects\sample
</blockquote>


Configuration
-------------

There is a option to specify which types of files will not be treated as potentially missing and also in which directories searching will not be performed.

Only what do you have to do is add a simple file to your main solution directory or some subdirs (depends on your needs)

Sample config file (also this is a default hard-coded configuration)
<blockquote>
#This is sample configuration file for this project<br>
#For more info go to http://github.com/tsubik/vspniff<br>
#Properties info<br>
#Mode - it is the way that the module will treat your options<br>
# append  - it will append your options to current context options<br>
# override  - in this and subdirs will only take this file options (unless in subdirs are also some config files)<br> 
#excludedExtensions - files with these extensions will not be listed as missing files<br>
#excludedDirs - program will not be looking in these locations for missing files<br>
<br>
mode: override<br>
excludedExtensions: user, csproj, aps, pch, vspscc, vssscc, ncb, suo, tlb, tlh, bak, log, lib<br>
excludedDirs: bin, obj<br>
</blockquote>



Copyright
--------
Copyright © 2012 Tomasz Subik. See [LICENSE](http://github.com/tsubik/) for details