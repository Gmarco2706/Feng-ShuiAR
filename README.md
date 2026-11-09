# Feng-ShuiAR

Feng-ShuiAR is a mobile augmented reality application for experimenting with object placement according to the Feng Shui Bagua map. The project detects real-world surfaces, places the Bagua map on a suitable horizontal plane, lets users add and manipulate objects, and evaluates whether each object is positioned in its assigned area.

## Features

- Horizontal plane detection using Unity AR Foundation.
- Automatic placement and anchoring of the Bagua map.
- Automatic scaling of the map to fit the detected surface.
- Nine Bagua areas:
  - Southeast: wealth
  - South: fame
  - Southwest: relationships
  - East: family
  - Center: health
  - West: creativity
  - Northeast: knowledge
  - North: career
  - Northwest: helpful people
- Touch-based object placement, movement, and rotation.
- Automatic detection of the Bagua area occupied by each object.
- Equilibrium score based on correct and incorrect object placements.
- Onboarding tutorial for scanning surfaces and learning the interaction gestures.

## Requirements

- Unity **6000.2.12f1**.
- An Android or iOS device compatible with ARCore or ARKit.
- Camera and motion sensors available on the target device.
- For editor testing, a compatible XR device or simulator is recommended.

## Main dependencies

The project uses the following Unity packages:

- AR Foundation **6.3.1**
- ARCore **6.3.1**
- ARKit **6.3.1**
- XR Interaction Toolkit **3.3.1**
- Universal Render Pipeline **17.2.0**
- Input System **1.15.0**

The complete dependency list is available in `Packages/manifest.json`.

## Installation and execution

1. Clone the repository:

   ```bash
   git clone https://github.com/Gmarco2706/Feng-ShuiAR.git
   cd Feng-ShuiAR
   ```

2. Open the project with Unity Hub using Unity `6000.2.12f1`.
3. Wait for Unity to import the packages and project assets.
4. Open the main scene:

   `Assets/Scenes/SampleScene.unity`

5. Configure the target device in **File > Build Profiles**.
6. Enable ARCore for Android or ARKit for iOS, depending on the target platform.
7. Connect a compatible device and run the application.

## How to use

1. Move the device until the application detects a horizontal surface.
2. Complete the initial onboarding tutorial.
3. Select the option to create the Bagua map.
4. The map will be anchored automatically to the largest valid horizontal plane in front of the camera.
5. Add objects to the scene and place them in the appropriate Bagua areas.
6. Drag an object with one finger to move it and use two fingers to rotate it.
7. Check the equilibrium result to see the percentage of correct placements.

An object placement is considered correct when its assigned `BaguaZone` matches the area in which it is placed. The equilibrium score uses the following formula:

```text
equilibrium = 100 × M / (M + K × MM)
```

Where `M` is the number of matches, `MM` is the number of mismatches, and `K` is the mismatch penalty.

## Project structure

```text
Assets/
├── Scenes/
│   └── SampleScene.unity             # Main scene
├── MobileARTemplateAssets/
│   ├── Scripts/                      # AR, Bagua, UI, and interaction logic
│   ├── Shaders/                      # Shaders and Shader Graph assets
│   └── UI/                           # User interface and tutorial assets
├── Samples/XR Interaction Toolkit/   # XR Interaction Toolkit sample assets
└── ...                               # Models, materials, and textures
Packages/
└── manifest.json                    # Project dependencies
ProjectSettings/
└── ProjectVersion.txt               # Unity version
```

The main Bagua-related scripts are located in `Assets/MobileARTemplateAssets/Scripts/`:

- `BaguaPlacer`: detects a suitable plane, creates the anchor, and places the map.
- `BaguaGrid`: generates the nine interactive cells.
- `BaguaClassComponent`: defines the correct area for each object.
- `BaguaLabelDetector`: detects when an object enters or leaves an area.
- `BaguaEquilibriumCalculator`: calculates and displays the equilibrium score.
- `DraggableRotatable`: handles touch movement and rotation.
- `GoalManager`: controls the onboarding tutorial.

## Development

Code changes should be made inside `Assets/`. After modifying scenes, prefabs, or scripts, open the project in Unity and test `SampleScene` on a real AR device. Before creating a distributable build, verify camera permissions, the target platform, and the scene configuration in the build profile.

## Project status

This repository contains the Unity project and its development assets. The current configuration is intended for a mobile AR experience and may require platform-specific adjustments before publishing to Google Play or the App Store.
