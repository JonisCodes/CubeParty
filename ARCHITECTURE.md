# CubeParty – Tower Defense Architecture

This document explains how the core systems fit together. Start here before reading any script.

---

## Table of Contents

1. [Ability System](#ability-system)
2. [Status Effect System](#status-effect-system)
3. [Stat Modifier System](#stat-modifier-system)
4. [Tower & Leveling](#tower--leveling)
5. [Upgrade System](#upgrade-system)
6. [How to Add a New Ability](#how-to-add-a-new-ability)
7. [How to Add a New Upgrade](#how-to-add-a-new-upgrade)

---

## Ability System

An ability is split into two layers: a **definition** and a **runtime instance**.

### Ability (ScriptableObject definition)

`Assets/Scripts/TowerDefense/Abilities/Ability.cs`

A ScriptableObject that lives in the project as an asset. It holds all the designer-facing data for an ability — it is never modified at runtime.

| Field | Purpose |
|-------|---------|
| `abilityName` / `description` / `icon` | Display info |
| `cooldown` | Seconds between fires |
| `damage` | Base damage per hit |
| `baseScalingPerLevel` | How much damage grows per ability level |
| `statModifiers` | Passive stat bonuses applied to the tower when this ability is equipped (see [Stat Modifier System](#stat-modifier-system)) |
| `onHitEffects` | Status effects automatically applied on every hit (see [Status Effect System](#status-effect-system)) |

Three concrete ability types exist, each created as a ScriptableObject asset via the `Abilities/` menu:

- **BasicRanged** — standard single-target attack, no extra fields
- **Sniper** — same shape as BasicRanged, distinguished by data values (high damage, long cooldown)
- **RingOfFire** — adds a `layerMask` for AoE physics overlap detection

### AbilityInstance (runtime wrapper)

`Assets/Scripts/TowerDefense/Abilities/AbilityInstance.cs`

Created per-tower at runtime; wraps an `Ability` definition. This is what actually fires and tracks state.

**Responsibilities:**
- Tracks cooldown (`CooldownRemaining`)
- Builds the `AbilityExecutionContext` for each shot
- Runs the execution modifier pipeline (see below)
- Maintains runtime-added `onHitEffects` and `executionModifiers`

**Execution flow (`TryExecute`):**

```
TryExecute()
  └─ cooldown check
  └─ Execute()
       └─ BuildContext()         — assembles damage, target, power multiplier
       └─ modifier pipeline      — each IAbilityExecutionModifier wraps the next
       └─ finalExecution()       — fires OnExecute event
            └─ Tower's BindAbility handler
                 ├─ target.TakeDamage(baseDamage * powerMultiplier, source)
                 └─ target.ApplyStatus(effect, source)  — for each onHitEffect
  └─ reset cooldown
```

### AbilityExecutionContext

A struct passed through the execution pipeline containing everything a modifier might need to read or forward: `Caster`, `Target`, `Origin`, `Source`, `BaseDamage`, `PowerMultiplier`, `OnHitEffects`.

### IAbilityExecutionModifier

`Assets/Scripts/TowerDefense/Interfaces/IAbilityExecutionModifier.cs`

```csharp
void ModifyExecution(AbilityInstance ability, AbilityExecutionContext context, Action execute);
```

Modifiers wrap the execution action using a chain-of-responsibility pattern. Each modifier receives the `execute` action and decides when (and how many times) to call it. This means modifiers compose cleanly without knowing about each other.

**Example — DoubleShotModifier:**

```csharp
public void ModifyExecution(AbilityInstance ability, AbilityExecutionContext context, Action execute)
{
    execute(); // first shot
    execute(); // second shot
}
```

Calling `execute()` twice fires the full downstream pipeline (damage + on-hit effects) twice.

---

## Status Effect System

### StatusEffectSO (definition)

`Assets/Scripts/TowerDefense/Abilities/StatusEffectSO.cs`

A ScriptableObject defining a debuff. Key fields:

| Field | Purpose |
|-------|---------|
| `effectTag` | Element type: Fire, Frost, Chemical, Physical, Soaked |
| `hasDuration` / `duration` | Whether the effect expires |
| `isStacking` / `maxStacks` / `decayStacks` | Stack behaviour |
| `tickRate` / `damagePerTick` | Periodic damage (multiplied by current stacks) |
| `icon` / `backgroundColor` / `uiPrefab` | UI display |

### StatusInstance (runtime)

`Assets/Scripts/TowerDefense/Abilities/StatusInstance.cs`

Created on an enemy when a status is applied. Tracks current stacks, duration, and tick timer. Implements `IDamageSource` so tick damage is correctly attributed to the original attacker.

`Tick(float dt, Enemy enemy)` is called each frame by the enemy and handles both periodic damage and duration countdown.

---

## Stat Modifier System

Passive stat changes — applied when an ability is equipped or an upgrade is installed.

### IStatModifier

```csharp
StatType Stat { get; }
float Apply(float baseValue);
```

### TowerStatModifier (ScriptableObject)

`Assets/Scripts/TowerDefense/Towers/TowerStatModifiers/TowerStatModifier.cs`

A ScriptableObject asset with a `StatType` (Damage, Range, AttackSpeed, Health), a `ModifierType` (Multiply or Add), and a `value`. Can be referenced directly from `Ability.statModifiers`.

### How Tower applies them

`Tower` holds a `List<IStatModifier>`. `RecalculateStats()` folds all modifiers for a given `StatType` over the base value using LINQ `Aggregate`. Modifiers are applied in insertion order — Multiply and Add modifiers are not sorted, so order matters if both types are present on the same stat.

When an ability is equipped via `AbilityInstance.Equip(tower)`, each of its `statModifiers` is registered with the tower. Modifiers can be removed via `Tower.RemoveStatModifier()`.

---

## Tower & Leveling

`Assets/Scripts/TowerDefense/Towers/Tower.cs`

The tower is a thin coordinator. It owns the ability list, stat modifier list, and XP/level state, but delegates all logic outward.

**Initialization order (`Awake`):**
1. `SetBaseStats()` — copies base values from `TowerData`
2. `InitializeAbilities()` — creates `AbilityInstance` per ability, binds the damage handler, calls `Equip()` (which registers stat modifiers). Wrapped in a modifier batch to defer `RecalculateStats()` until all abilities are registered.
3. `RecalculateStats()` — computes final Range and AttackSpeed

**Each frame (`Update`):**
Each ability is ticked (cooldown) and, if a target exists, `TryExecute` is called.

**XP & leveling:**
`AddXp(float amount)` is called externally (by enemies on death). When `CurrentXP >= xpToNextLevel`, `LevelUp()` fires:
- Increments `Level`, resets `CurrentXP`
- Calls `ApplyLevelStats()` (currently empty — the hook for per-level stat bumps)
- Fetches a random upgrade from `UpgradeDatabase` and calls `upgrade.Apply(this)`

> **Note:** `xpToNextLevel` is currently a fixed SerializeField. The planned improvement is to derive it from `TowerData.baseXp * Pow(Level, TowerData.growthExponent) + _totalUpgradePowerBias` so the curve steepens with level and installed upgrades.

---

## Upgrade System

### TowerUpgrade (base class)

`Assets/Scripts/TowerDefense/Upgrades/TowerUpgrade.cs`

Abstract ScriptableObject. Subclasses implement `Apply(Tower tower)` and get a reference to the full tower, so they can reach `tower.Abilities`, `tower.AddStatModifier()`, etc.

```csharp
public int tier; // intended weight for XP cost scaling — not yet wired
public abstract void Apply(Tower tower);
```

### Concrete upgrades

| Class | What it does |
|-------|-------------|
| `DoubleShotUpgrade` | Adds a `DoubleShotModifier` to every ability on the tower — each ability fires twice per execution |
| `AddBurnUpgrade` | Adds a `StatusEffectSO` (burn) to every ability's on-hit effects — all attacks apply burn |

Both iterate `tower.Abilities` and modify each instance directly. New upgrades follow the same pattern.

### UpgradeDatabase

`Assets/Scripts/TowerDefense/Managers/UpgradeDatabase.cs`

Singleton MonoBehaviour. Holds a designer-populated `List<TowerUpgrade>`. Currently `GetRandomUpgrade()` returns a uniformly random entry — no tier weighting yet.

---

## How to Add a New Ability

1. Create a new class inheriting `Ability` with `[CreateAssetMenu]`.
2. Add any extra fields the ability needs (e.g. `RingOfFire` adds `layerMask`).
3. Create a ScriptableObject asset via the menu and fill in the data fields.
4. Assign the asset to the `TowerData.abilities` list on the relevant tower.

The execution handler in `Tower.BindAbility()` is generic — it reads damage and on-hit effects from the context, so no code changes are needed in Tower for a new ability type unless it requires custom hit behaviour.

---

## How to Add a New Upgrade

1. Create a class inheriting `TowerUpgrade` with `[CreateAssetMenu]`.
2. Implement `Apply(Tower tower)`. Use `tower.Abilities` to modify ability instances, or `tower.AddStatModifier()` to modify stats.
3. Create a ScriptableObject asset and add it to the `UpgradeDatabase.allUpgrades` list in the scene.
