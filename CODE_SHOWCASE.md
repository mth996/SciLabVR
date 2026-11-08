# SciLab VR — Code Showcase

This guide highlights the most useful source files for reviewing the technical work in **SciLab VR**. The repository preserves several original Unity working-folder names to avoid breaking project references, so this page provides a cleaner navigation layer for portfolio review.

## Start Here

For a quick technical review, these systems provide the strongest overview of the project:

| Area | Representative files | What they demonstrate |
| --- | --- | --- |
| Experiment orchestration | `Scripts/Chapter1_Pulse/Chapter1Manager.cs`, `Scripts/Chapter3/Chapter3Manager.cs` | Experiment state, progression, activity coordination and chapter-level flow |
| Pulse simulation & VR feedback | `Scripts/Chapter1_Pulse/PulseSessionManager.cs`, `Scripts/Chapter1_Pulse/PulseHapticPlayer.cs`, `Scripts/Chapter1_Pulse/PulseVisualIndicator.cs`, `Scripts/Chapter1_Pulse/WristDetector.cs` | Timed experiment logic, haptic feedback, visual feedback and interaction detection |
| Experiment data recording | `Scripts/Chapter1_Pulse/NotebookRecorder.cs`, `Scripts/Core/NotebookSystem.cs`, `Scripts/Core/NotebookPageData.cs` | Runtime result capture, experiment records and reusable notebook UI |
| Chapter 2 science systems | `Scripts/Chapter2/Chapter2AExperiment.cs`, `Scripts/Chapter2/Chapter2BExperiment.cs`, `Scripts/Chapter2/SimpleLineGraph.cs`, `Scripts/Chapter2/VerticalRulerUI.cs` | Experiment-specific logic, measurement, graphing and data presentation |
| Chapter 3 experiments | `Scripts/Chapter3/Chapter3AExperiment.cs`, `Scripts/Chapter3/Chapter3CExperiment.cs`, `Scripts/Chapter3/Chapter3DExperiment.cs` | Multi-stage laboratory experiment workflows and validation |
| Measurement/UI generation | `Scripts/Chapter3/Chapter3CMeasurementBuilder.cs`, `Scripts/Chapter3/Chapter3CUIAmender.cs` | Runtime measurement interfaces and experiment-specific UI |
| VR interaction components | `Scripts/Core/PlacementSocket2A.cs`, `Scripts/Core/ConfirmationStation.cs`, `Scripts/Core/ChemicalDripper3D.cs`, `Scripts/Core/LatexBeaker3D.cs` | Object placement, interaction validation and reusable laboratory mechanics |
| User settings | `Scripts/Core/SettingsUI.cs` | Runtime settings and persistent preferences |
| Activity presentation | `Scripts/UI/ChapterOverlayUI.cs` | Chapter/activity information presented through world-space UI |

## System Relationships

```text
Chapter / Activity Manager
        |
        +--> Experiment Controller
        |       |
        |       +--> VR object interactions
        |       +--> measurement / validation
        |       +--> haptic + visual feedback
        |
        +--> Notebook / result recording
        |
        +--> UI / progression feedback
```

## Chapter 1 — Pulse & Health

The Chapter 1 code demonstrates a complete VR experiment loop rather than a single interaction script. The system coordinates experiment selection, pulse sessions, wrist interaction, feedback and result recording.

Recommended files:

- `Scripts/Chapter1_Pulse/Chapter1Manager.cs`
- `Scripts/Chapter1_Pulse/ActivitySelector.cs`
- `Scripts/Chapter1_Pulse/PulseSessionManager.cs`
- `Scripts/Chapter1_Pulse/PulseTarget.cs`
- `Scripts/Chapter1_Pulse/PulseHapticPlayer.cs`
- `Scripts/Chapter1_Pulse/PulseVisualIndicator.cs`
- `Scripts/Chapter1_Pulse/WristDetector.cs`
- `Scripts/Chapter1_Pulse/NotebookRecorder.cs`

## Chapter 2 — Growth, Measurement & Results

Chapter 2 adds experiment-specific measurement and visualization. It demonstrates how the broader experiment framework is adapted for a different scientific activity rather than duplicating one fixed interaction pattern.

Recommended files:

- `Scripts/Chapter2/Chapter2AExperiment.cs`
- `Scripts/Chapter2/Chapter2BExperiment.cs`
- `Scripts/Chapter2/SimpleLineGraph.cs`
- `Scripts/Chapter2/VerticalRulerUI.cs`

## Chapter 3 — Materials & Chemistry

Chapter 3 contains some of the larger experiment controllers in the repository. These scripts coordinate multi-step laboratory interactions, measurements, validation and experiment-specific interfaces.

Recommended files:

- `Scripts/Chapter3/Chapter3Manager.cs`
- `Scripts/Chapter3/Chapter3AExperiment.cs`
- `Scripts/Chapter3/Chapter3CExperiment.cs`
- `Scripts/Chapter3/Chapter3DExperiment.cs`
- `Scripts/Chapter3/Chapter3CMeasurementBuilder.cs`
- `Scripts/Chapter3/Chapter3CUIAmender.cs`
- `Scripts/Chapter3/BoilingTube3C.cs`

## Reusable Systems

Several scripts are intentionally smaller because they are reusable components supporting the larger experiment controllers. Examples include placement sockets, laboratory-object behaviours, audio/feedback components, UI utilities and activity/NPC behaviours.

These files should be read as supporting components rather than standalone portfolio projects.

## Folder Organisation

Scripts are grouped by chapter and responsibility under `Scripts/`. Each script was moved together with its Unity `.meta` file, so the GUIDs that scenes and prefabs rely on are preserved if the scripts are dropped back into a Unity project.

## Technical Themes

**Unity • C# • VR Interaction • Experiment State Management • Physics-Based Interaction • Haptic Feedback • Runtime Validation • Data Recording • Measurement Systems • World-Space UI • Educational XR**
