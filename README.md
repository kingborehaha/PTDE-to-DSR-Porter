# PTDE to DSR Porter
Tool for porting PTDE mods to DSR

Built to support the following files:
* FFX
* ESD (talk/chr)
* EMEVD
* GameParam
* DrawParam (Values will be offset)
* ANIBND (Uses DSR HKX)
* OBJBND (Uses DSR HKX) (Object textures will become self-contained when an object is added to a new msb)
* LUABND (Decompiled lua only. Compiled lua will be replaceed with DSR version)
* Parts
* MSB
* Misc (breakobj, sound)

# Requirements for use
* Folder containing modified PTDE files
* Unpacked Vanilla PTDE installion
* DSR installation


# Misc
* Scaled objects not included in exception .txt file will be reverted to default scaling

# Info
* Uses SoulsFormats [link]
* Uses SoulsFormatsExtensions [link]
* Thanks to Dropoff for helping figure out FFX conversion.