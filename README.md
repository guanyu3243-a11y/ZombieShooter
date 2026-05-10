# Zombie Shooter

## Game Overview

Zombie Shooter is a 3D wave-based survival shooting game developed in Unity using C#.

The player must survive against continuously spawning zombies, defeat boss enemies, and upgrade skills through a roguelike progression system. The game has no fixed final level, so the main challenge is to survive as long as possible while choosing suitable skill combinations.

---

## Core Gameplay

The player controls a character from a top-down perspective.

The basic gameplay loop is:

1. Move around the map and avoid enemies.
2. Shoot zombies using the player's weapon.
3. Clear enemy waves.
4. Choose skill upgrades after certain waves.
5. Fight boss enemies every 10 waves.
6. Continue surviving as enemy difficulty increases.

Enemies become stronger as the wave number increases. Their health, damage, speed, and attack interval are scaled dynamically through the wave system.

---

## Features

- 3D player movement and shooting system
- Wave-based enemy spawning system
- Procedural enemy generation around the player
- Enemy difficulty scaling by wave number
- Normal zombie AI using NavMeshAgent
- Two different boss types
- Heavy Boss with dash attack and warning area
- Summoner Boss that summons enemies
- Boss health bar system
- Player health system
- Floating damage text
- Critical hit system
- Roguelike skill selection system
- Small skill upgrades every 2 waves
- Major skill selection before boss waves
- Upgradeable major skills
- Pause menu
- Main menu
- Settings menu
- Separate BGM and SFX volume control
- Player statistics panel using Tab key
- Character animation using Unity Animator Controller
- Sound effects and background music

---

## Controls

| Key | Action |
|---|---|
| W A S D | Move |
| J | Shoot |
| Left Shift | Dash |
| E | Enemy Slow |
| Tab | Show Player Stats Panel |
| Esc | Pause Menu |

---

## Game Mechanics

### Health System

The game uses a reusable `Health.cs` script for the player, normal enemies, and bosses.

This script controls:

- Maximum HP
- Current HP
- Taking damage
- Death logic
- Player Game Over
- Enemy death
- Boss death
- Hit flash feedback

When the player takes damage, the HP value is updated immediately on the UI.

---

### Shooting System

The player can shoot bullets using the `PlayerController.cs` script.

Bullets are controlled by `Bullet.cs`, which handles:

- Bullet lifetime
- Collision with enemies
- Damage calculation
- Critical hit calculation
- Floating damage text
- Bullet destruction after hitting enemies

The final bullet damage is calculated using the player's damage multiplier and critical hit chance from `PlayerStats.cs`.

---

### Wave System

The wave system is controlled by `WaveManager.cs`.

Each wave increases the number and strength of enemies. Normal enemies are spawned around the player using random positions within a minimum and maximum distance range.

Enemy values such as HP, damage, movement speed, and attack interval are scaled based on the current wave.

Boss waves appear every 10 waves.

---

### Skill System

The game includes a roguelike skill upgrade system.

Small skills are offered every 2 waves. These include:

- Damage Increase
- Fire Rate Increase
- Critical Hit Chance
- Heal 30 HP
- Max HP Increase
- Movement Speed Increase

Major skills are offered before boss waves. These include:

- Multi Shot
- Enemy Slow
- Dash

Major skills have levels and can be upgraded by choosing them repeatedly.

For example:

- Repeatedly choosing Dash increases dash distance.
- Repeatedly choosing Multi Shot increases bullet count and shooting range.
- Repeatedly choosing Enemy Slow reduces the slow skill cooldown.

This system encourages the player to think strategically about skill combinations. For example, if the player chooses Multi Shot, then selecting Damage Increase or Fire Rate Increase can make the build more powerful.

---

## Game AI

### Normal Enemy AI

Normal zombies use `EnemyAI.cs`.

They use Unity's `NavMeshAgent` to automatically follow the player. When they get close enough, they attack the player at fixed intervals.

### Heavy Boss AI

The Heavy Boss is controlled by `HeavyBossAI.cs`.

It includes:

- Player tracking
- Normal attack
- Dash attack
- Red warning area before dash
- Dash damage detection
- Cooldown-based attack logic
- Animation and sound effects

### Summoner Boss AI

The Summoner Boss is controlled by `SummonerBossAI.cs`.

It includes:

- Periodic enemy summoning
- Faster summoning at lower health
- Normal attack
- Summon animation
- Summon sound effect
- Clearing summoned enemies when the boss dies

---

## User Interface

The game UI is implemented using Unity Canvas and TextMeshPro.

The UI includes:

- Player HP
- Wave number
- Skill cooldown display
- Boss health bar
- Floating damage text
- Game Over text
- Pause menu
- Settings menu
- Player statistics panel

The player statistics panel can be opened by holding the Tab key. It shows current player upgrades such as damage multiplier, fire rate multiplier, movement speed multiplier, max HP, and acquired skills.

The UI elements use anchors so they can remain in suitable screen positions when the resolution changes.

---

## Animation

The game uses Unity Animator Controller to control character animations.

Implemented animation states include:

- Idle
- Run
- Attack
- Death
- Summon
- Dash / Boss attack animation

Animation transitions are controlled using Animator parameters such as triggers and speed values.

Some animations were obtained from external sources such as Mixamo and then integrated into Unity.

---

## Audio

The game includes background music and sound effects.

The settings menu allows the player to control:

- BGM volume
- SFX volume

BGM is used in the main menu, while SFX is used for shooting, boss attacks, dash, summoning, slow skill, and other gameplay effects.

The settings are saved using Unity `PlayerPrefs`.

---

## Player Progression

The player becomes stronger by selecting skills during the game.

Possible upgrades include:

- Higher bullet damage
- Faster shooting speed
- Higher movement speed
- Increased max HP
- Healing
- Critical hit chance
- Dash ability
- Multi-shot ability
- Enemy slow ability

Because there is no fixed final level, the goal is to survive as many waves as possible.

---

## Built With

- Unity 2022.3
- C#
- TextMeshPro
- Unity Animator Controller
- Unity NavMeshAgent
- Unity UI System

---

## Main Scripts

| Script | Purpose |
|---|---|
| `Health.cs` | Controls HP, damage, death, and game over logic |
| `PlayerController.cs` | Controls player movement, shooting, dash, and input |
| `Bullet.cs` | Controls bullet damage, collision, and critical hit logic |
| `EnemyAI.cs` | Controls normal zombie movement and attack AI |
| `WaveManager.cs` | Controls waves, enemy spawning, boss spawning, and difficulty scaling |
| `PlayerStats.cs` | Controls player upgrades, skill levels, and slow ability |
| `SkillSelectionManager.cs` | Controls random skill selection and skill application |
| `SkillCooldownUI.cs` | Displays dash and slow cooldown status |
| `HeavyBossAI.cs` | Controls Heavy Boss attack and dash behavior |
| `SummonerBossAI.cs` | Controls Summoner Boss summoning and attack behavior |
| `UIManager.cs` | Controls HP UI, Game Over UI, floating messages, and damage text |
| `BossHealthBarUI.cs` | Controls boss health bar display and updating |
| `PauseManager.cs` | Controls pause menu, resume, settings, and return to main menu |
| `MenuManager.cs` | Controls main menu, settings menu, BGM and SFX volume |
| `PlayerStatsPanelUI.cs` | Displays player statistics when holding Tab |
| `CameraFollow.cs` | Makes the camera follow the player smoothly |

---

## How to Run

1. Open the project using Unity Hub.
2. Open the `MainMenu` scene.
3. Press Play in the Unity Editor.
4. Click Start to enter the game.

---

## Project Status

The game is fully playable and includes core gameplay, enemy AI, boss AI, wave progression, skill upgrades, UI systems, animation, audio, and settings.

Future improvements could include:

- More enemy types
- More boss attack patterns
- More maps
- More visual effects
- Final score and leaderboard system
