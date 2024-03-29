# PTDE to DSR Porter
Tool for porting PTDE mods to DSR

### Supports the following file types:
* FFX
* ESD (talkesd/chresd)
* EMEVD
* GameParam
* DrawParam (Values are offset relatively. Manual tweaks [preferably through code] are encouraged)
* ANIBND (Uses DSR HKX)
* CHRBND (Uses DSR HKX)
* OBJBND (Uses DSR HKX) (Object textures will become self-contained when an object is added to a new msb)
* LUABND (Ports decompiled lua)
* PARTSBND
* MSB
* Sound (natively compatible)

HKX and TPF conversion are not supported at the moment, DSR files will be used (when possible).

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
* * Note: The program runs luac50.exe for lua compilation. If it repeatedly prompts you to run, you may need to adjust security permissions (exe is located in "Resources\lua")
* Once the porting process is finished, ported files can be found in the "Output" folder, which also contains "Output Log.txt"
* Review "Output Log.txt" for issues during the porting process and to see if any files must be ported manually

## Misc Info
* Once the program is finished, there will be an output log that contains info on what the program did/didn't do, detailing things that must be manually ported
* Resource txt files are included which lets you modify porting behavior for certain systems, including:
* * Which scaled MSB objects to not revert to default scaling
* * Which FFX to always/never port to DSR, in spite of if they were modified (bonfire FFX included by default)

# Changelog
### v1.0.1
* Fixed blacklisted FFX not being ported to ffxbnds that did not contain the FFX originally
* Fixed DSR folder browser saying it wanted PTDE files

# Credits
* SoulsFormats: https://github.com/JKAnderson/SoulsFormats
* SoulsFormatsExtensions: https://github.com/Meowmaritus/SoulsFormatsExtensions
* Contains paramdefs from DSMapStudio: https://github.com/soulsmods/DSMapStudio
* Contains lua 5.0.3 executables for x64 lua compilation: https://sourceforge.net/projects/luabinaries/
* Thanks to Dropoff for helping figure out FFX conversion.
