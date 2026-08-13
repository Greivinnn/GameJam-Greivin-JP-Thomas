# 🐧 Penguin Slide

A tile-based puzzle game where you slide across ice, push rocks to block enemy sightlines, and sink obstacles into water to cross safely — built in **1 month** by a team of 3 for a class project.

## 🎮 Overview

Penguin Slide combines grid-based sokoban puzzle mechanics with a stealth twist: enemies use raycast vision along four cardinal directions, and getting caught in their line of sight ends your run. Every move is undoable, letting players experiment freely with puzzle solutions.

## 🛠️ Systems I Built

- **Grid Movement & Raycasting** — Custom tile-snapping movement system using `Physics2D.Raycast` to detect walls, floors, water, and checkpoints, replacing standard physics-based movement for precise puzzle-grid control
- **Ice Sliding Mechanic** — State-driven momentum system that lets the player continue sliding across ice tiles instead of stopping at each grid cell, adding a layer of momentum-based puzzle-solving
- **Sokoban Push System** — Pushable objects propagate movement recursively (pushing a rock into another rock), sink permanently when pushed into water to form crossable bridges, and can be used to eliminate enemies on contact
- **Enemy Vision & Attack AI** — Enemies raycast along 4 directions every frame, visualize sightlines via `LineRenderer`, and trigger a lightning-strike attack with dynamically calculated position, rotation, and scale between enemy and player
- **Undo / State Rollback System** — A full game-state snapshot stack (player position, enemy status, pushable positions, key/door state) supporting up to 100 steps of undo, built entirely with custom serializable structs
- **Key/Door & Checkpoint System** — ID-matched key-door unlocking with checkpoint-based respawn and multi-scene level progression

## 🧰 Tech Stack

`Unity` · `C#` · `Unity Input System` · `2D Physics (Raycasting)` · `Animator/Blend Trees`

## 👥 Team

Built in 1 month by a team of 3 as a class project.

## ▶️ How to Play

- **Arrow Keys / WASD** — Move (grid-based, one tile per press)
- **Undo Key** — Rewind to the previous state
- Push rocks to block enemy sightlines or sink them in water to cross
- Avoid enemy vision lines — getting spotted ends your run
