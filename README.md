# Survive: Total Lockdown — NUS Orbital 2026

**Team Name:** Hello World
**Members:** Srinivasan Natarajan · Ronith Kartikeyan
**Proposed Level of Achievement:** Apollo 11
**Tech Stack:** Unity (2D LTS) · C# · Aseprite/Piskel · GitHub

---

## About the Game

Survive: Total Lockdown is a 2D side-view survival action game where players must escape a high-tech facility under an AI-controlled lockdown. Inspired by the mechanical depth of 3D survival games, the game is designed to be accessible on low-spec hardware while delivering the tension, resource scarcity, and complex systems of the genre.

Players must scavenge materials, craft equipment, solve environmental puzzles, and fight or evade a variety of infected enemies — all under a persistent 30-minute countdown before the facility self-destructs.

---

## Motivation

As long-time fans of 3D survival games, we often found our own hardware couldn't run popular titles smoothly. This sparked the idea of translating the core fundamentals of high-stakes 3D survival — tension, resource scarcity, and complex systems — into an accessible 2D side-view format, without sacrificing mechanical depth.

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
- Player movement controls including walking, running and jumping
- Basic background pixel art for the Second floor admin office area
- Grid-based inventory UI with item pickup and drop functionality
- Functional security gate with a basic passcode interaction
- Basic infected enemy with patrol movement and a detection radius that damages the player on contact
- Basic combat with both pistol and knife implemented; player can switch between them
- Medkit item that allows the player to heal
- Health bar display on the HUD showing current player health

### Demo Video
[INSERT VIDEO LINK HERE]

---

## Milestone 2 — Prototype (Core Features)

### Goals
- **Inventory Grid:** Full implementation of item dragging and management
- **Survival Loop:** Ticking clock active; crafting of basic items functional (Molotov cocktails, Acid Flasks, medkits)
- **Basic Combat:** Player can aim and shoot the pistol at simple AI enemies with ammo scarcity enforced
- **Goal:** A playable loop of scavenge, craft, and fight

---

## Software Engineering Practices

### 1. Git Issue Tracking & Branch Strategy
We created a GitHub Issue for every feature or bug before writing any code. Each issue was given a descriptive label (enhancement or bug) and assigned to Milestone 1. A dedicated branch was then created for each issue, with branches and pull requests named following a consistent convention for easy tracking.

![Issues](./docs/issues.png)
![Pull Requests](./docs/prs.png)

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
We used inheritance to avoid code duplication across game entities. A shared `Entity` base class captures common properties between the player and enemies (e.g. health, damage handling), with `Player` and `Enemy` extending it with their own specific behaviours.

Unity Prefabs were also used for reusable game objects such as the keypad, pickup gun, pickup knife, and pickup medkit — allowing us to update shared behaviour from a single master prefab.

---

## Challenges Faced

### Merge Conflicts with Unity Scene Files
The biggest challenge was collaborating via GitHub. Unity scene files are YAML, and unlike C# scripts, they cannot easily be resolved manually when conflicts occur. We spent significant time resolving merge conflicts, especially when both of us modified the same GameObject.

To address this, we set up the **UnityYAMLMerge (Smart Merge)** tool in our terminal, which handles Unity-specific YAML conflicts automatically. For cases where the same GameObject was modified by both members, we had to manually take one version and re-apply the other's changes directly in the Unity Editor.

### Obscure Unity Inspector Bugs
Debugging in Unity is harder than in pure code projects because bugs can originate from Inspector settings rather than logic errors. One example: the player's health slider was drifting left and right during movement. After spending significant time searching through the movement and UI code, we discovered the cause was entirely in the Inspector — Unity's A and D keys can inadvertently interact with UI Sliders. The fix was simply to disable the **Interactable** option on the health slider GameObject, something that would never have been caught by reading the code alone.

---

## Timeline

| Milestone | Key Deliverables | Date |
|---|---|---|
| Milestone 1 | Basic movement, combat, inventory, enemy patrol, security gate | 2 June 2026 |
| Milestone 2 | Full inventory, ticking clock, crafting loop, improved combat | 28 June 2026 |
| Milestone 3 | All core features, enemy mutations, full vertical slice | 27 July 2026 |
| Splashdown | Add-ons, refined AI, playtesting, polished build | 26 August 2026 |

---

## Tech Stack

| Tool | Purpose |
|---|---|
| Unity (2D LTS) | Game engine |
| C# with Scriptable Objects | Scripting and data management |
| Aseprite / Piskel | Pixel art and animation |
| GitHub | Version control and collaboration |
