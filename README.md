VSpniff - Visual Studio project not included files finder
=========================

VSpniff is a simple tool to lookup for some missing files references in the Visual Studio project xml file. 

Installation
-----------

You need have powershell installed.

1. Download [the latest version](https://github.com/downloads/tsubik/VSpniff/vspniff%201.0.ZIP) or whole repository and compile it by yourself.
2. Open Visual Studio command prompt 

<blockquote>
installutil -i VSpniff.Cmdlet.dll
</blockquote>

3. Add snapin to your powershell profile file.<br> 
In the package manager console.

<blockquote>
notepad $profile
</blockquote> 

In the file add

<blockquote>
Add-PSSnapIn VSpniff
</blockquote>

Save and close file. Restart shell or type

<blockquote>
. $profile
</blockquote>

Done.

Uninstallation
-------------

Close all Visual studios if you add cmdlet to the nuget profile, and all powershell's consoles if you add to powershell profile.<br>
Go to the directory where you have vspniff.cmdlet.dll
<blockquote>
installutil /u VSpniff.Cmdlet.dll
</blockquote>

Done.

Configuration
-------------

There is a option to specify which types of files will not be treated as a potentially missing and also in which directories searching will not be performed.

Only what do you have to do is add a simple file to your main solution directory or some subdirs (depends on your needs)

Sample config file (also this is a default hard-coded configuration so you do not have to add any file if you happy with below configuration)
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

Example
-------

Just type in the package manager console

<blockquote>
Find-MissingFiles
</blockquote>

And all potentially missing files will be listed.

You can specify directory where the module should start looking for project files

<blockquote>
Find-MissingFiles -d ./SomeRelativeDirectory
</blockquote>

<blockquote>
Find-MissingFiles -d d:\dev\myprojects\sample
</blockquote>

Copyright
--------
Copyright © 2012 Tomasz Subik. See [LICENSE](http://github.com/tsubik/) for details