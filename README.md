# Planetary Diversity /L

Creates some diversity on KSP Planets by changing the seeds of PQSMods and other things. Fork by Lisias.


## In a Hurry

* [Latest Release](https://github.com/net-lisias-kspu/Planetary-Diversity/releases)
	+ [Binaries](https://github.com/net-lisias-kspu/Planetary-Diversity/tree/Archive)
* [Source](https://github.com/net-lisias-kspu/Planetary-Diversity)
* [Issue Tracker](https://github.com/net-lisias-ksp/KSPAPIPlanetary-Diversity/issues)
* Documentation	
	+ [Homepage](http://ksp.lisias.net/add-ons/Planetary-Diversity) on L Aerospace KSP Division
	+ [Project's README](https://github.com/net-lisias-ksp/Planetary-Diversity/blob/master/README.md)
	+ [Change Log](./CHANGE_LOG.md)
	+ [TODO](./TODO.md) list
* Official Distribution Sites:
	+ [Homepage](http://ksp.lisias.net/add-ons/Planetary-Diversity) on L Aerospace
	+ [Source and Binaries](https://github.com/net-lisias-ksp/Planetary-Diversity) on GitHub.


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
Planetary Diversity is licensed under the Terms of the MIT License.

Copyright for portions of project `Planetary Diversity` are held by Dorian Stoll (StollD / Thomas P.) 2017 as part of project `Planetary Diversity`. All other copyright for project `Planetary Diversity /L` are held by Lisias Toledo 2018.


## Parent
* [Thomas P.](https://forum.kerbalspaceprogram.com/index.php?/profile/113164-thomas-p/)
	* [Forum](https://forum.kerbalspaceprogram.com/index.php?/topic/164870-130131-free-for-adoption-planetary-diversity-release-4-oct-29/)
	* [GitHub](https://github.com/net-lisias-ksp/Planetary-Diversity)
