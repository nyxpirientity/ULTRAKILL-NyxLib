# USER FACING/FRONT-END
- remove a left-over log patch that got rid of a info log from the vanilla game (the TryIgniteAt log, not really helpful still but I didn't mean to leave it in this mod)
- added a warning for when a certain bug in the vanilla game occurrs (because it confused me a couple of times into think it was something new and caused by me!)
- added LevelQuickLoaderType option that users aren't really supposed to touch but they technically can, but like, leave it on default, Additive doesn't really work and even when it does it isn't much improved currently.

# BACK-END/INTERNAL/API
- added AEnemyType to represent types in a way that can be dynamically added to rather than the base ULTRAKILL global::EnemyType enum
- added EnemyPrefabDatabase which automatically fills with enemy prefabs from the sandbox spawn menu, for easier access to enemy game objects and spawning
- added `NailTouchEnemy` events
- asset pickers now iterate through every object of a certain type for each level load until they return true (then they of course should never iterate again)
- Added `OverrideFullName` to `EnemyComponents`
- cleaned up some stuff