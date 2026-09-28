# SciLab VR — Code Showcase

This guide highlights the most useful source files for reviewing the technical work in **SciLab VR**. The repository preserves several original Unity working-folder names to avoid breaking project references, so this page provides a cleaner navigation layer for portfolio review.

## Start Here

For a quick technical review, these systems provide the strongest overview of the project:

| Area | Representative files | What they demonstrate |
| --- | --- | --- |
| Experiment orchestration | `3a codes/Chapter1Manager.cs`, `9b/Chapter3Manager.cs` | Experiment state, progression, activity coordination and chapter-level flow |
| Pulse simulation & VR feedback | `3a codes/PulseSessionManager.cs`, `3a codes/PulseHapticPlayer.cs`, `3a codes/PulseVisualIndicator.cs`, `3a codes/WristDetector.cs` | Timed experiment logic, haptic feedback, visual feedback and interaction detection |
| Experiment data recording | `3a codes/NotebookRecorder.cs`, `NotebookSystem.cs`, `NotebookPageData.cs` | Runtime result capture, experiment records and reusable notebook UI |
| Chapter 2 science systems | `Chapter 6/Chapter2AExperiment.cs`, `Chapter 6/Chapter2BExperiment.cs`, `Chapter 6/SimpleLineGraph.cs`, `Chapter 6/VerticalRulerUI.cs` | Experiment-specific logic, measurement, graphing and data presentation |
| Chapter 3 experiments | `9b/Chapter3AExperiment.cs`, `9b/Chapter3CExperiment.cs`, `9b/Chapter3DExperiment.cs` | Multi-stage laboratory experiment workflows and validation |
| Measurement/UI generation | `9b/Chapter3CMeasurementBuilder.cs`, `9b/Chapter3CUIAmender.cs` | Runtime measurement interfaces and experiment-specific UI |
| VR interaction components | `PlacementSocket2A.cs`, `ConfirmationStation.cs`, `ChemicalDripper3D.cs`, `LatexBeaker3D.cs` | Object placement, interaction validation and reusable laboratory mechanics |
| User settings | `SettingsUI.cs` | Runtime settings and persistent preferences |
| Activity presentation | `10a/ChapterOverlayUI.cs` | Chapter/activity information presented through world-space UI |

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

- `3a codes/Chapter1Manager.cs`
- `3a codes/ActivitySelector.cs`
- `3a codes/PulseSessionManager.cs`
- `3a codes/PulseTarget.cs`
- `3a codes/PulseHapticPlayer.cs`
- `3a codes/PulseVisualIndicator.cs`
- `3a codes/WristDetector.cs`
- `3a codes/NotebookRecorder.cs`

## Chapter 2 — Growth, Measurement & Results

Chapter 2 adds experiment-specific measurement and visualization. It demonstrates how the broader experiment framework is adapted for a different scientific activity rather than duplicating one fixed interaction pattern.

Recommended files:

- `Chapter 6/Chapter2AExperiment.cs`
- `Chapter 6/Chapter2BExperiment.cs`
- `Chapter 6/SimpleLineGraph.cs`
- `Chapter 6/VerticalRulerUI.cs`

## Chapter 3 — Materials & Chemistry

Chapter 3 contains some of the larger experiment controllers in the repository. These scripts coordinate multi-step laboratory interactions, measurements, validation and experiment-specific interfaces.

Recommended files:

- `9b/Chapter3Manager.cs`
- `9b/Chapter3AExperiment.cs`
- `9b/Chapter3CExperiment.cs`
- `9b/Chapter3DExperiment.cs`
- `9b/Chapter3CMeasurementBuilder.cs`
- `9b/Chapter3CUIAmender.cs`
- `9b/BoilingTube3C.cs`

## Reusable Systems

Several scripts are intentionally smaller because they are reusable components supporting the larger experiment controllers. Examples include placement sockets, laboratory-object behaviours, audio/feedback components, UI utilities and activity/NPC behaviours.

These files should be read as supporting components rather than standalone portfolio projects.

## Why the Original Folder Names Remain

Some source folders retain development names such as `3a codes`, `9b`, `10a`, and `Chapter 6`. Renaming or moving Unity scripts without the complete project context can disrupt `.meta` GUID-based references in scenes and prefabs.

For that reason, this portfolio currently prioritizes **reference safety and code traceability** over cosmetic folder renaming. This showcase provides the cleaner navigation layer while the original source paths remain intact.

## Technical Themes

**Unity • C# • VR Interaction • Experiment State Management • Physics-Based Interaction • Haptic Feedback • Runtime Validation • Data Recording • Measurement Systems • World-Space UI • Educational XR**
