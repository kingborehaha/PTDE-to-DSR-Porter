# PTDE to DSR Porter
Tool for porting PTDE mods to DSR

Supports the following file types:
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
* breakobj, sound (natively compatible)

TPF conversion is not supported at the moment, DSR TPFs will be used (when possible).

# Program Requirements
* .NET 6.0 Desktop Runtime: https://dotnet.microsoft.com/en-us/download/dotnet/6.0

# Game File Requirements
* Folder containing PTDE mod files
* Folder containing unpacked vanilla PTDE files
* Folder containing DSR files

# Misc Info
* Once the program is finished, there will be an output log that contains info on what the program did/didn't do, detailing things that must be manually ported
* Editable Resource txt files are included which let's you modify porting behavior for certain systems, including:
** Scaled MSB objects to not revert to default scaling
** FFX to always/never port to DSR in spite of if they were modified (bonfire FFX included by default)

# Credits
* SoulsFormats: https://github.com/JKAnderson/SoulsFormats
* SoulsFormatsExtensions: https://github.com/Meowmaritus/SoulsFormatsExtensions
* Contains paramdefs from DSMapStudio: https://github.com/soulsmods/DSMapStudio
* Thanks to Dropoff for helping figure out FFX conversion.