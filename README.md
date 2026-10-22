# NEON DRIFT

A neon-styled mobile 3D tilt game built with **Unity 6 (URP)** for iOS.
Guide a glowing ball through a floating maze **by tilting your phone**, collect every energy orb and reach the exit portal as fast as possible.

## Features
- **Tilt-to-play**: accelerometer-driven gravity with low-pass filtering, calibration, dead zone and clamping
- **Physics gameplay**: Rigidbody ball, bouncy neon walls (Physics Materials), trigger-based collectibles and exit portal
- **Unified input** (Unity Input System): keyboard, gamepad, on-screen joystick and button, swipe-to-dash, pinch / scroll zoom
- **Responsive mobile UI**: safe area support, HUD (orbs, timer), start / pause / win menus, event-driven UIManager
- **Haptic feedback** on orb pickup, wall impacts and victory
- **Neon look**: emissive URP materials, Bloom, tonemapping and vignette

## Tech
Unity 6.6 · C# · URP · Input System · TextMeshPro · iOS (Xcode) · Git LFS on GitLab

## Project structure

Assets/_Project/
├── Scenes/ MainScene
├── Scripts/ Gameplay, UI, sensors (+ Editor/ build post-processing)
├── Prefabs/ Orb
└── Materials/ Neon materials, Physics Materials


## Run it
1. Open the project with Unity 6.6 (iOS Build Support module installed)
2. Open `Assets/_Project/Scenes/MainScene` and press Play (keyboard / joystick in the editor)
3. iOS: File → Build Profiles → iOS → Build, then run the Xcode project on a device

## Controls
| Platform | Move | Jump | Dash | Zoom |
|---|---|---|---|---|
| iPhone | Tilt the phone | Jump button | Swipe | Pinch |
| Mac / PC | WASD / arrows | Space | — | Scroll |
| Gamepad | Left stick | South button | — | — |
