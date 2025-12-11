# Manual Environment Setup

If the automatic setup script cannot run or you're working on a system without PowerShell, follow the steps below to configure the development environment manually.

## 1. Create the required folders

Create the directory structure that the build system expects:

```
.ldenv/
.ldenv/lib/
.ldenv/share/
```

## 2. Create symbolic links

You must link your local game installation so the build process can reference its libraries.

- Create a symbolic link named ".gamedir" inside `.ldenv` that points to your Oxygen Not Included installation directory.
- Create a symbolic link named ".modsdir" inside `.ldenv` that points to your game's `mods` directory.

Example (paths will vary by system):

```
# Linux/macOS
ln -s "/path/to/Oxygen Not Included" ./.ldenv/.gamedir
ln -s "/path/to/Oxygen Not Included/mods" ./.ldenv/.modsdir

# Windows (PowerShell)
cmd /c mklink /D .ldenv\.gamedir "C:\Games\OxygenNotIncluded"
cmd /c mklink /D .ldenv\.modsdir "C:\Games\OxygenNotIncluded\mods"
```

## 3. Copy required game assemblies

From:
```
<gamedir>/OxygenNotIncluded_Data/Managed
```

Copy the following assemblies into `.ldenv/share`:

```
0Harmony.dll
Assembly-CSharp.dll
Assembly-CSharp-firstpass.dll
UnityEngine.dll
UnityEngine.CoreModule.dll
System.Runtime.CompilerServices.Unsafe.dll
```

Then copy these assemblies into `.ldenv/lib`:

```
Newtonsoft.Json.dll
Unity.TextMeshPro.dll
System.IO.Compression.dll
```

## 4. Install PLib

Download the **PLib4.19** version of DLL and place it into:

```
.ldenv/lib
```

