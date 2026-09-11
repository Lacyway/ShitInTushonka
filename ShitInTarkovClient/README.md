# 💩 Shit In Tarkov (S.I.T.)

> *"Norvinsk region was already a pile of shit. Now it's official."*

A lightweight, fully immersive ambient mod for Single Player Tushonka. **Shit In Tarkov** introduces real-time surface impacts, custom positional audio, and persistent visual decals to add an extra layer of raw, tactical realism to your raids.

---

## ⚡ Key Features

* **Dynamic Surface Decals:** High-resolution impact quads that align dynamically with collision surface normals. Feature-packed with random rotation and size variation to prevent visual repetition.
* **Spatialized Sound Effects:** Pre-processed directional audio clips mapped directly to actions and surface hits.
* **Zero-Lag Asset Pipeline:** Operates on an optimized native `AssetBundle` loading system—no disk-reading lag spikes (`UnityWebRequest` completely eliminated), zero memory leaks during scene transitions.
* **Material Property Optimization:** Built using GPU-friendly `MaterialPropertyBlock` instances to ensure zero FPS impact, even during chaotic firefights.

---

## 📸 Media / Preview

> **[INTERCEPTED MILITARY BAND // FREQUENCY 441.20 MHz]**  
> `50 52 56 52 52 20 4F 56 42 59 42 54 56 50 4E 59 20 52 56 52 41 47`  
> **[T-0010M]**  
> `AB PYRNAVAZ PRRJ PAN PRAGQVN GUVF`  
> `God save us all`

---

## 🛠️ Installation

1. Ensure you have **BepInEx** installed for your SPT setup.
2. Download the latest release `.zip`.
3. Extract the contents directly into your main SPT directory:
```text
SPT_Folder/
├── BepInEx/
│   └── plugins/
│       └── ShitInTarkovClient/
│           ├── ShitInTarkovClient.dll
│           ├── LICENSE.md
│           └── Assets/
│               └── sit.bundle
└── SPT_Runtime/
    └── user/
        └── mods/
            └── ShitInTarkovServer/
                ├── ShitInTarkovServer.dll
                ├── bundles.json
                ├── db/
                │   ├── CustomItems/
                │   │   └── shit.json
                │   └── CustomLocales/
                │       └── en.json
                └── bundles/
                    └── shit/
                        ├── caca_a.bundle
                        ├── caca_d.bundle
                        └── caca_e.bundle
```
4. Launch the game and enjoy the fresh air.

## ⚙️ Requirements & Compatibility

- SPT Version: 4.1.3+
- Dependencies: None (Standalone BepInEx Client Plugin)
- Incompatibilities: None reported.

## 🐛 Bug Reports & Source

Found an issue or want to check out the code?

- Drop a line in the comments tab or submit an issue on the project repository.