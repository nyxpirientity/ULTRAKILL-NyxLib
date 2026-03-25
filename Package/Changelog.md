# USER FACING/FRONT-END
## OPTIONS
- Made cheat 'HideCheatsStatus' optional
- Made cheat 'ForceNextWave' optional
- Made cheat 'SandAllEnemies' optional

## ADJUSTMENTS
- Made RegisterCheats (hopefully always!) get called *after* the game registers its cheats, so they appear on the bottom rather than the top of the cheats list

## PACKAGE PAGE
- Updated readme c:
- Added changelog (clearly)

# BACK-END/INTERNAL/API
- Removed EnemyRevolverBullet and EnemyRevolverAltBullet from Assets, for dependant mods to instead pick those assets
- Updated enemy hurt events to use newer event structure