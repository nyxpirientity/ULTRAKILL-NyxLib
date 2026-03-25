# About
NyxLib is *mostly* a library I created for my own personal ULTRAKILL mods. It can technically be used by anyone, for any mods, but I, ESPECIALLY now, advise against it because I could change the API at anytime, as it's unfinished and also very probably not good. Fundamentally, this doesn't matter much for people to read up on, it's just a framework for me to make mods for ULTRAKILL.
It features things for convenience and organization for my mods, such as a small system to organize the composition of radiance, events for various gameplay events (Player events, enemy events, projectile events) and ways for those events to be cancelled whilst other mods using the library can tell if and why those events were cancelled, and even a system for dropping messages to the player. In general, it's just convenience related things for my mods.
There is no documentation at the moment.

# Cheats
It features a few cheats, most exotic cheats are part of my *other* (many not released on Thunderstore as I write this) ULTRAKILL mods, but I'll list what there is in this one here alongside some explanation/description. Cheats labeled optional can be enabled/disabled in the configuration file.

## UTILITY:
- Radiant All Enemies - Does what it says- similar to forcerad, but it uses it's own configuration settings, configured in the config file. Why? Well, simply because the base ULTRAKILL radiance system was a bit too simplistic for my mods to combine from what I could tell, so I had to make a layer on top of it which simply takes in multiple *radiance modifiers* and compounds them together as either additives or multipliers. This *will* interfere with base game radiance if anything is actually using it, but if cheats are off, then as long as all mods are respecting my general policy of leaving all gameplay altering things behind cheats, it'll make no difference (in theory, but like, software has bugs and all of my mods add to be a big solo-unpaid-personal-project)
- Sand All Enemies - Just a convenience cheat for the sand all enemies command in the game (optional)
- Force Next Wave - Tries to force the next cybergrind wave to occur. (optional)
- Hide Cheats Enabled Status - Hides the cheats enabled status UI in the top left (optional)

# Note
It also features a feature named "Level Quick Loading" which loads levels at startup to allow mods to pick assets from those levels they may need. This causes startup to look a bit different, and can make it take longer. Frankly, I just don't know of a different way to do what I wanted lol