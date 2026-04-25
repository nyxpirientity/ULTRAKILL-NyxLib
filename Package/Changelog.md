# USER FACING/FRONT-END
- Hugely improved (at least, in my testing) custom radiance composition. Should be more stable *and* preserve base-game radiance better, automatically converting it to the radiance compositor from this library (if it's needed, that is)
- EnemyRadiance now applies additive modifiers then multiplier modifiers rather than in just whatever order C# sets come in
- Added some more cheats check failsafes, simply meaning that more fail-safes were added to ensure the mod *shouldn't* affect non-cheats-enabled gameplay even if something goes wrong (still no guarantees!)
- Fixed a bug causing radient idols not to apply proper radiance to their blessed
- Added force clean music with battle music option to config file
- Added optional Cybergrind Starting Wave Override cheat
- Increased logging strictness to maybe possibly improve performance
- Added radiance debug option (not too useful for users, but it's technically accessible)

# BACK-END/INTERNAL/API
- Updated many events to use the 'newer' pre/post event pattern
- Started adding `AvoidHealthBasedSlowDown`flag for `EnemyComponents`
- Added some more events for grenades and cannonballs
- Added FieldAccess type
