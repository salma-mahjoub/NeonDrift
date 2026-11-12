<h1 align="center">NEON DRIFT</h1>

<p align="center">
  <img alt="Unity" src="https://img.shields.io/badge/Unity-6.6-black?logo=unity">
  <img alt="Platform" src="https://img.shields.io/badge/platform-iOS-lightgrey?logo=apple">
  <img alt="Render pipeline" src="https://img.shields.io/badge/render-URP-00e5ff">
  <img alt="Language" src="https://img.shields.io/badge/C%23-239120?logo=csharp&logoColor=white">
  <img alt="License" src="https://img.shields.io/badge/license-MIT-green">
</p>

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
Unity 6.6 · C# · URP · Input System · TextMeshPro · iOS (Xcode) · Git LFS

## Project structure
```
Assets/_Project/
├── Scenes/      MainScene
├── Scripts/     Gameplay, UI, sensors
│   └── Editor/  iOS build post-processing
├── Prefabs/     Orb
└── Materials/   Neon materials, Physics Materials
```

## Run it
1. Open the project with **Unity 6.6** (iOS Build Support module installed)
2. Open `Assets/_Project/Scenes/MainScene` and press **Play** (keyboard / on-screen joystick in the editor)
3. **iOS**: File → Build Profiles → iOS → Build, then run the generated Xcode project on a device

## Controls
| Platform | Move | Jump | Dash | Zoom |
|---|---|---|---|---|
| iPhone | Tilt the phone | Jump button | Swipe | Pinch |
| Mac / PC | WASD / arrows | Space | — | Scroll |
| Gamepad | Left stick | South button | — | — |

## What I learned
Unity scene and component architecture, Transform and MonoBehaviour lifecycle, PhysX physics (forces, colliders, triggers),
the Input System (actions, events, multi-device), responsive mobile UI, mobile sensors and haptics, and shipping an iOS build.

## Roadmap
- [ ] Multiple levels and level selection
- [ ] Persistent best times (local leaderboard)
- [ ] Sound design and music
- [ ] Android build

## Author
**Salma Mahjoub** — [GitHub](https://github.com/salma-mahjoub)

## License
Released under the [MIT License](LICENSE).
