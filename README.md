# Survive: Total Lockdown — NUS Orbital 2026

**Team Name:** Hello World

**Members:** Srinivasan Natarajan · Ronith Kartikeyan

**Proposed Level of Achievement:** Apollo 11

**Tech Stack:** Unity (2D LTS) · C# · Piskel · GitHub

---

## About the Game

Survive: Total Lockdown is a 2D side view survival action game where players must escape a high tech facility under a controlled lockdown. Inspired by the depth and tension in 3D survival games, the game is designed to be accessible on low spec hardware while delivering the tension, resource scarcity, and complex systems of the genre.

Players must scavenge materials, craft equipment, solve puzzles, and fight or evade a variety of infected enemies, all under a persistent 30 minute countdown before the facility self destructs.

---

## Motivation

As long time fans of 3D survival games, we often found that our own hardware was never powerful enough to run the most popular titles smoothly. This frustration sparked the idea for our project. We want to take the core fundamentals of high stakes 3D survival tension, resource scarcity, and complex systems and translate them into a 2D side view format. By doing so, we are building a game that is accessible to everyone, regardless of their laptop’s processing power, while still offering the mechanical depth usually reserved for heavy 3D titles.

---

## User Stories

1. As a player, I want to play a game with a tense and atmospheric setting so that I feel genuinely immersed in the high-stakes facility escape.
2. As a player, I want a game with diverse enemy types that each require distinct counter-strategies so that combat remains engaging and rewards preparation.
3. As a player, I want a grid-based inventory system that forces meaningful trade-offs between carrying crafting materials and upgrade components so that every scavenging decision feels impactful.
4. As a player, I want a ticking countdown with escalating consequences at key milestones so that I must constantly adapt my strategy rather than settle into a safe routine.
5. As a player, I want a stealth system based on shadows and noise so that I can choose between aggressive and evasive approaches depending on my situation.
6. As a player, I want to permanently upgrade my weapons at limited workstations so that I can tailor my playstyle while being mindful of finite upgrade opportunities.
7. As a player, I want environmental hazards like electrified floors and explosive barrels to be usable tactically so that the environment feels like a tool rather than just an obstacle.
8. As a player, I want security gates that require exploration and puzzle-solving to unlock so that progression feels earned through wit as well as combat skill.
9. As a player, I want a multi-floor facility where each floor has a unique hazard type and enemy composition so that I must plan my route and loadout carefully throughout the run.
10. As a player, I want a clear HUD displaying my health, ammo, and countdown timer so that I can make informed decisions without interrupting the flow of gameplay.

---

## Milestone 1 — Technical Proof of Concept

### Goals
- Basic character controller with walking, running, jumping and crouching
- Basic background art for the Second floor admin office area
- Simple grid-based inventory UI where items can be picked up and dropped
- A functional security gate that opens with a basic passcode interaction
- Basic infected enemy with patrol movement and detection radius
- Basic combat with pistol and knife implemented

**Overall Goal:** A minimal playable system where the player can move, pick up items, shoot enemies and interact with a door.

### What We Achieved
- Player movement controls including walking and jumping
- Basic background pixel art for the Second floor admin office area
- Grid based inventory UI with item pickup and drop functionality
- Functional security gate with a basic passcode interaction
- Basic infected enemy with patrol movement and a detection radius that damages the player on contact
- Basic combat with both pistol and knife implemented; player can switch between them
- Medkit item that allows the player to heal
- Health bar display on the Heads Up Display showing current player health

### Demo Video

[![Milestone 1 Demo](https://img.youtube.com/vi/vHirqD36FPY/0.jpg)](https://youtu.be/vHirqD36FPY)

---

## Software Engineering Practices

### 1. Git Issue Tracking & Branch Strategy
We created a GitHub Issue for every feature or bug before writing any code. Each issue was given a descriptive label (enhancement or bug) and assigned to Milestone 1. A dedicated branch was then created for each issue, with branches and pull requests named following a consistent convention for easy tracking.

#### Git Issues
![Issues](./readme-assets/Git%20issues%20image.png) 

#### Github PRs
![Pull Requests](./readme-assets/Github%20pull%20requests%20page.png)

### 2. Safe Rebasing Workflow

To avoid destructive merge conflicts, we adopted the following branching flow:

```
1. Complete work on local feature branch
2. Push local feature branch to remote (backup)
3. Rebase local feature branch onto origin/main
4. If rebase succeeds, push to remote feature branch
5. Open Pull Request: remote feature branch → main
```

This ensured that even if a rebase caused issues locally, a clean backup of the feature branch was always available remotely. It also kept the main branch history linear and readable.

### 3. Object-Oriented Programming (OOP)

We used inheritance to model shared behaviour between game entities cleanly. A base Entity class defines properties and logic common to both the player and enemies like health, knockback, damage flashing, movement gating and sprite flipping. Core per frame methods like HandleMovement(), HandleAnimations() and HandleFlip() are declared as virtual in Entity, allowing Player and Enemy to override only the parts specific to them while inheriting everything else.

The best example of this is HandleMovement(). The base Entity class declares it as a virtual method and calls it every frame inside Update(), but only when the entity is not in a knockback state. This means the knockback gating logic is written once and applies automatically to both the player and the enemy. Player then overrides HandleMovement() to read keyboard input and handle crouching, while Enemy overrides it with an entirely different set of behaviours like patrol logic, detection radius checking, chasing and a waiting coroutine. Two completely different movement systems, cleanly separated in their own subclasses, both automatically inheriting the shared knockback protection from Entity. This pattern will scale naturally in Milestone 2 when new enemy variants like Sprinters and Blind infected are added, as each will simply override HandleMovement() with their own behaviour without touching any shared logic.

### 4. Prefabs

Reusable GameObjects such as GunPickup, KnifePickup and MedkitPickup are implemented as Unity Prefabs. These are basically pickable items spread across the environment in the game. Each unique pickup's behaviour, collider, sprite and inventory logic is defined once in a master prefab. Placing multiple instances across the game world or tweaking pickup behaviour only requires editing the prefab once, with all instances updating automatically. As we scale to a 6-floor facility in later milestones, this will be essential for managing the growing number of interactable objects consistently.

---

## Challenges Faced

### Merge Conflicts with Unity Scene Files
The biggest challenge was collaborating via GitHub. Unity scene files are YAML, and unlike C# scripts, they cannot easily be resolved manually when conflicts occur. We spent significant time resolving merge conflicts, especially when both of us modified the same GameObject.
To address this, we set up the **UnityYAMLMerge (Smart Merge)** tool in our terminal, which handles Unity-specific YAML conflicts automatically. For cases where the same GameObject was modified by both members, we had to manually take one version and re-apply the other's changes directly in the Unity Editor.

### Obscure Unity Inspector Bugs
Debugging in Unity is harder than in pure code projects because bugs can originate from Inspector settings rather than logic errors. One example: the player's health slider was drifting left and right during movement. After spending significant time searching through the movement and UI code, we discovered the cause was entirely in the Inspector — Unity's A and D keys can inadvertently interact with UI Sliders. The fix was simply to disable the **Interactable** option on the health slider GameObject, something that would never have been caught by reading the code alone.

---

## Next up : Milestone 2! 

### Goals
1) Ammo scarcity with scavengeable ammo packs for pistol and shotgun
2) Shotgun as a second ranged weapon
3) Grenades and Molotov cocktails as throwable weapons
4) Noise alert radius that pulses outward visibly when the player runs or fires, alerting nearby infected
5) Crouch mechanic for reducing noise
6) Shadow-based stealth for sighted enemies, with Blind infected relying purely on sound and ignoring shadows entirely
7) Basic crafting system allowing players to craft Molotov cocktails and medkits from raw materials
8) Keycard requirement alongside passcodes for multi-layer security gates
9) Weapon upgrade workstations where players can permanently modify their weapons using rare scavenged materials
10) 30 minute countdown on HUD with self-destruct sequence triggering on expiry
11) Fist escalation milestone at 25 minutes where all unlocked doors lock down
12) Staircase traversal across 2 playable floors, each with a distinct layout
13) Improved pixel art for player character and background animations
14) System testing across all implemented features with known bugs documented
15) Update README and documentation to reflect Milestone 2 progress with an updated demo video
    
---

| Milestone | Key Deliverables | Date |
|---|---|---|
| Milestone 1 | Basic movement, combat, inventory, enemy patrol, security gate | 1 June 2026 |
| Milestone 2 | Ammo scarcity, shotgun, throwables, stealth, noise alert, crouch, sprinter & blind infected, crafting, keycard gates, weapon upgrades, 3 floors, improved art | 29 June 2026 |
| Milestone 3 | Decryption grid minigame, armoured & spitter infected, full crafting, all escalation milestones, remaining 3 floors, environmental hazards, user testing | 27 July 2026 |
| Splashdown | Bug fixes, UI polish, balancing, final playtesting, complete documentation | 26 August 2026 |

---

## Tech Stack

| Tool | Purpose |
|---|---|
| Unity (2D LTS) | Game engine |
| C# with Scriptable Objects | Scripting and data management |
| Piskel | Pixel art and animation |
| GitHub | Version control and collaboration |
