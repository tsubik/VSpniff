VSpniff - Visual Studio project not included files finder
=========================

VSpniff is a simple tool to lookup for some missing files references in the Visual Studio project xml file. 

Installation as a Powershell cmdlet
-----------

You need have powershell installed.

Download [the latest version](https://github.com/downloads/tsubik/VSpniff/vspniff_1.0.zip) or whole repository and compile it by yourself.

Open Visual Studio command prompt 

<blockquote>
installutil -i VSpniff.Cmdlet.dll
</blockquote>

Check if your nuget profile file exists, if not create empty file. To obtain current path to nuget profile file in the PM console type:

<blockquote>
$profile
</blockquote> 

Add snapin to your nuget powershell profile file.<br> 
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

Troubleshooting
--------------

There could be some troubles with tool installation due some security policy and fact that it was written in .NET 4.0

**Problem:** Installutil error "Could not load file or assembly .... or one of it's dependencies. Operation not supported. (Exception from HRESULT: 0x80131515)."

**Solution:** vspniff.cmdlet.dll could be blocked, because it comes from another computer. So go to properties of this dll and simply unblock it.

If you have an another. Please write down in comments.

-----






Uninstallation
-------------

Close all Visual studios if you add cmdlet to the nuget profile, and all powershell's consoles if you add to powershell profile.<br>
Go to the directory where you have vspniff.cmdlet.dll
<blockquote>
installutil /u VSpniff.Cmdlet.dll
</blockquote>

Done.

Visual Studio Extension
------------

Just open extension manager and search for Vspniff. Here you go direct [http://visualstudiogallery.msdn.microsoft.com/99f0e072-0e93-4dcc-84d1-a8089a56a8d5](http://visualstudiogallery.msdn.microsoft.com/99f0e072-0e93-4dcc-84d1-a8089a56a8d5)


Configuration
-------------

There is a option to specify which types of files will not be treated as a potentially missing and also in which directories searching will not be performed.

Only what do you have to do is add a simple file to your main solution directory or some subdirs (depends on your needs)


Sample config file (also this is a default hard-coded configuration so you do not have to add any file if you happy with below configuration)<br>
All config file must have .vspniff extensions and be in like a JSON configuration file format

config.vspniff
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
{
  "mode": "override",
	"excludedextensions": "user, csproj, aps, pch, vspscc, vssscc, ncb, suo, tlb, tlh, bak, log, lib, scc",
	"excludeddirs": "bin, obj"
}
</blockquote>

Using when you install as a powershell cmdlet
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

Using when you install as an extension
-----

You will find "Find missing files" command in Tool menu, that command looks for missing files for every project in the solution. You also have "Find missing files" command in context menu for solution and project nodes in solution explorer, if you choose project it will be looking for missing files only for selected project. Missing files will be listed in the Vspniff output pane in the output window it should be opened automatically. 



------

Copyright
--------
Copyright © 2012 Tomasz Subik. See [LICENSE](http://github.com/tsubik/) for details
