# Planetary-Diversity

Creates some diversity on KSP Planets by changing the seeds of PQSMods and other things.


## In a Hurry

* [Releases](./Archive)
	* [Latest Release](https://github.com/net-lisias-kspu/Planetary-Diversity/releases)
* [Source](https://github.com/net-lisias-kspu/Planetary-Diversity)


## Description

### How does it work?
When a savegame is loaded, this mod loads and changes some parameters of the planets, for example the noise functions that are used to generate the terrain.
The new parameters are calculated from a static seed that is assigned to your savegame upon creation.

When you load a savegame the first time with Planetary Diversity installed, it will lock down for some time. This happens because in KSP, every planet needs a low-res representation for higher distances. 
Building this low-res version is quite expensive, thats why it is done once and then gets cached inside of your savegame folder. When finished, your game should unlock and you should be able to explore a slightly
different solar system.

### What does it change exactly?
* Terrain noise seeds
* Planet names (thanks Sigma!)
* Gasplanet colors
* It toggles atmospheres (adds atmospheres to bodies without one or removes an existing atmosphere)
* Atmospheric pressure and temperature
* Orbital parameters

Due to the modularity of Planetary Diversity, other people can write custom tweaks for it and ship them independently.

### I dont want to change {insert Feature here}!
Don't worry, all tweaks can be disabled seperately. Just open GameData/PlanetaryDiversity/Config/{config name}.cfg (for custom tweaks please ask the respective modder) and set everything you don't want to `false`


## License
Planetary Diversity is licensed under the Terms of the MIT License

Copyright (c) Dorian Stoll (StollD / Thomas P.) 2017


## UPSTREAM
* [Thomas P.](https://forum.kerbalspaceprogram.com/index.php?/profile/113164-thomas-p/)
	* [Forum](https://forum.kerbalspaceprogram.com/index.php?/topic/164870-130131-free-for-adoption-planetary-diversity-release-4-oct-29/)
	* [GitHub](https://github.com/net-lisias-ksp/Planetary-Diversity)
