# Planetary-Diversity
This is a modification for Kerbal Space Program, that creates some diversity on the otherwise static planets.

### How does it work?
When a savegame is loaded, this mod loads and changes some parameters of the PQSMods (i.e. the components that are used to compose a planets terrain in KSP), for example the seeds of noise functions.
The new parameters are calculated from a static seed that is assigned to your savegame upon creation.

When you load a savegame the first time with Planetary Diversity installed, it will lock down for some time. This happens because in KSP, every planet needs a low-res representation for higher distances. 
Building this low-res version is quite expensive, thats why it is done once and then gets cached inside of your savegame folder. When finished, your game should unlock and you should be able to explore a slightly
different solar system.

### What does it change exactly?
At the moment it only changes seeds. But I plan to implement some more tweaks, like changing the deformity, or manipulating orbits. Due to the modularity of Planetary Diversity, other people can write custom tweaks for it
and ship them independently.

### I dont want to change {insert PQSMod here}!
Don't worry, all tweaks can be disabled seperately. Just open GameData/PlanetaryDiversity/Config/{config name}.cfg (for custom tweaks please ask the respective modder) and set everything you don't want to `false`

### License
Planetary Diversity is licensed under the Terms of the MIT License

Copyright (c) Dorian Stoll (StollD / Thomas P.) 2017
