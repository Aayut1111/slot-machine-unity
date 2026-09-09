# Slot Machine — Aayut Rohela

## Play it
Live build: https://aayut1111.github.io/slot-machine-unity/
(Local fallback: open the game from a local server, e.g. run `python -m http.server` inside the `docs/` folder and visit `http://localhost:8000` — opening `index.html` directly via `file://` will not work due to browser CORS restrictions.)

## Overview
A 2D slot machine built in Unity as a take-home assignment. Three reels spin independently with weighted random outcomes, and a win is triggered when all three reels land on the same symbol, paying out a multiple of the bet based on that symbol's value.

## Bonus features
[fill this in — see note below]

## Approach
Symbol data lives in `SymbolData` ScriptableObjects (icon, weight, payout multiplier), so designers can add or rebalance symbols without touching code. `RngService` handles weighted symbol selection using `System.Random`, kept deliberately separate from `UnityEngine.Random` (which is used only for the purely visual "blur" flicker during a spin, where true randomness quality doesn't matter). Win detection (`WinChecker`) and payout calculation (`PayoutManager`) are plain C# classes with no Unity dependencies, so the game's rules can be unit tested independently of the engine. `Reel` owns its own spin animation via a coroutine that eases into the stop using an `AnimationCurve`, and reels are staggered with an increasing start delay so they land one after another rather than all at once. `SlotMachine` orchestrates a spin and raises events (`OnSpinStarted`, `OnBalanceChanged`, `OnSpinResolved`) rather than reaching into the UI directly; `UIManager` listens to those events and updates the balance display, win banner, and Spin button state, keeping game logic and UI cleanly one-directional.

## Bonus features
- Adjustable bet amount via UI buttons
- Dedicated jackpot symbol with a 20x payout multiplier
- Win sound effect and scale-punch animation on winning reels

## Bonus features
None implemented — focused on a solid, well-structured core loop within the time available.

