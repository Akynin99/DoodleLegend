# 🏗️ Architecture Overview

## Key Components:
### Core Systems

GameManager: State machine for game flow

EventBus: Pub/sub communication backbone

GameProgress: Score/height tracking with save/load

### Player

PlayerController: Physics-based movement with collision handling

InputHandler: Abstracted input (Mobile/Keyboard implementations)

### World Generation

PlatformFactory: Pool-based platform spawning

LevelGenerator: Dynamic procedural generation

PlatformBase: Inheritable platform types (breakable/moving/etc)

### Power-Up System

IPowerUpStrategy: Interface for power-up behaviors

PowerUpSystem: Management and effect coordination

## DI & Optimization

Zenject dependency injection

Object pooling for WebGL performance

ScriptableObject-based configuration
