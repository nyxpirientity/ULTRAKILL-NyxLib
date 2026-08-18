# USER FACING/FRONT-END
- make radiant all enemies use enemy's base radiance as a... well, base (and many additional configuration options!)

# BACK-END/INTERNAL/API
- improve timing of cheat registration events
- Simplify `EnemyRadiance.Modifier` with a `CompositionType` enum instead of multiple bools
- Add projectile asset prefabs
- Overhauls `AEnemyType` (now `EnemyTypeData`) by making it a single class
- Overhauls asset handling greatly, sorting things into their own singletons and files, and assigning tasks to specific classes rather than a single global Assets class
- rename `ScenesEvents` to `SceneEvents` and make some event names more (I think) reasonable
- rename `AEnemyType.ReadableName` to `EnemyTypeData.DisplayName`
- rename `EnemyPrefabStore` to `EnemyCloning`, `EnemyPrefabStore.InstanceStore` to `EnemyCloneStore`, and `EnemyPrefabManager` to `EnemyCloneManager`
- add `Gear` (player weapon prefabs) to `Assets`
- add `ExplosionRoot` monobehaviour
- add `ExplosionStartModifier`
- add `Shaker3D`
- add `Shaker1D`
- add image and obj (mesh format) loader for external asset loading
- add `DebugPrintMaterialProperties` method in new `AssetInspecting` class
- add `includeFileInfo` parameter for `StackDebug`
- add `Coincident`, `CoincidentProject`, and `Snapped` methods to `NyxMath`
- add `VelocityTracker`
- add `CollisionCenter` property to `EnemyComponents`
- remove `ExplosionAdditions`
- many stability adjustments to `EnemyPrefabStore`/`EnemyCloneStore` and friends

