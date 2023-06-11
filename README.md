# PTDE to DSR Porter
Tool for porting PTDE mods to DSR

### Supports the following file types:
* FFX
* ESD (talkesd/chresd)
* EMEVD
* GameParam
* DrawParam (Values are offset relatively. Manual tweaks [preferably through code] is encouraged)
* ANIBND (Uses DSR HKX)
* CHRBND (Uses DSR HKX)
* OBJBND (Uses DSR HKX) (Object textures will become self-contained when an object is added to a new msb)
* LUABND (Requires decompiled lua. Lua can optionally be compiled during porting to reduce lua memory footprint)
* PARTSBND
* MSB
* Sound (natively compatible)

TPF conversion is not supported at the moment, DSR TPFs will be used (when possible).

### Contains optional settings to:
* Apply DSR dispgroup/drawgroup improvements
* Compile lua to reduce lua memory footprint
* Other misc improvements and fixes

## Program Requirements
* .NET 6.0 Desktop Runtime: https://dotnet.microsoft.com/en-us/download/dotnet/6.0

## How to use
* Start the program and provide the locations of:
* * Folder containing PTDE mod files
* * Folder containing unpacked vanilla PTDE files
* * Folder containing DSR files
* Click the port button
* The program runs luac50.exe for lua compilation. If it repeatedly prompts you to run, you may need to adjust security permissions (exe is located in "Resources\lua").

## Misc Info
* Once the program is finished, there will be an output log that contains info on what the program did/didn't do, detailing things that must be manually ported
* Resource txt files are included which lets you modify porting behavior for certain systems, including:
* * Which scaled MSB objects to not revert to default scaling
* * Which FFX to always/never port to DSR, in spite of if they were modified (bonfire FFX included by default)

# Credits
* SoulsFormats: https://github.com/JKAnderson/SoulsFormats
* SoulsFormatsExtensions: https://github.com/Meowmaritus/SoulsFormatsExtensions
* Contains paramdefs from DSMapStudio: https://github.com/soulsmods/DSMapStudio
* Contains lua 5.0.3 executables for x64 lua compilation: https://sourceforge.net/projects/luabinaries/
* Thanks to Dropoff for helping figure out FFX conversion.
