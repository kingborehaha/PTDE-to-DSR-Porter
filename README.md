# PTDE to DSR Porter
Tool for porting PTDE mods to DSR

Support the following file types:
* FFX
* ESD (talk/chr)
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

TPF conversion is not supported at the moment, DSR versions of files will be used when possible.

# Program Requirements
* .NET 6.0 Desktop Runtime [link]

# Game File Requirements
* Folder containing modified PTDE files
* Unpacked Vanilla PTDE installion
* DSR installation

# Misc Info
* Once the program is finished, there will be an output log that contains info on what the program did/didn't do, detailing things that must be manually ported
* Several resource .txt files are i ncluded in the program that let you define more detailed behavior during the porting procress. Including:
** Scaled objects to not be reverted to default scaling
** FFX to always/never port to DSR in spite of if they were modified

# Program Info
* Uses SoulsFormats [link]
* Uses SoulsFormatsExtensions [link]
* Contains paramdefs from DSMapStudio [link]
* Thanks to Dropoff for helping figure out FFX conversion.