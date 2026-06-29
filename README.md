# Cute Aliens Card Game

A local two-player card drafting game built in Unity with C#.

## Features
- **Turn-based drafting** — Players take turns selecting cards from a shared hand
- **Hand passing** — Remaining cards pass to the opponent each round
- **Multiple rounds** — Score accumulates across rounds with a final result screen
- **Hidden hands** — Each player's selected cards are hidden from the opponent
- **Custom pixel art** — Original character and card artwork drawn in Aseprite

## Screenshots
<img width="320" height="180" alt="21" src="https://github.com/user-attachments/assets/73f7a98a-6478-4df9-889b-0b33446310bb" />
<img width="320" height="180" alt="22" src="https://github.com/user-attachments/assets/716cb038-4eed-40c8-8ef7-b1ff889e4f57" />
<img width="320" height="180" alt="23" src="https://github.com/user-attachments/assets/82d123e7-a894-41aa-b500-ba1a25593570" />
<img width="320" height="180" alt="24" src="https://github.com/user-attachments/assets/8c00113a-8d91-40d7-b890-c02ed3a01976" />
<img width="320" height="180" alt="25" src="https://github.com/user-attachments/assets/5577ecd1-11b5-49d3-8e25-1b90101afbc3" />
<img width="320" height="180" alt="26" src="https://github.com/user-attachments/assets/10b0bac5-4afc-45ef-8302-72f369726821" />


## How to Play
Open the project in Unity 2021+ and press Play. Two players share the same keyboard.

## Gameplay
Each round, players take turns picking cards from a shared pool. After all cards are drafted, hands are revealed and scores are calculated. The player with the highest score after all rounds wins.

## Tech Stack
- **Engine:** Unity
- **Language:** C#
- **Art:** Custom pixel art assets created in Aseprite
- **Data:** ScriptableObjects for card, deck, and game setup configuration
