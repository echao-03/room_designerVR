# room_designerVR
An in-progress project created using Unity and Meta XR SDK for real-time hand tracking. Design your dream room / living space all with your hands.

## Implemented foundation

The repository now contains a minimal Unity script foundation for a Meta Quest room designer flow:

- **Room setup and visualization**
  - `RoomDimensionsController`: applies preset or custom room dimensions by scaling a room root.
  - `ViewpointController`: switches the camera rig between predefined viewpoints.
- **Gesture-based interaction (Meta XR hand tracking)**
  - `GestureSelectable`: provides visual selection highlighting for furniture or walls.
  - `GestureManipulationController`: supports pinch selection, one-hand translation, and two-hand rotation + scaling.
- **Wall customization**
  - `WallColorController`: applies selected colors to wall renderers.
- **Menu navigation via hand ray + pinch**
  - `HandMenuRaycaster`: hand-pointer raycast and pinch click behavior.
  - `IPinchMenuItem` and `PinchMenuItemEvent`: menu action interface + UnityEvent bridge.

## Unity setup

1. Install Unity (LTS) and import the **Meta XR SDK** (including Interaction SDK).
2. Add an `OVRCameraRig` to your scene and enable hand tracking in project settings.
3. Add `OVRHandPrefab` (or equivalent hand visuals) and wire `OVRHand` references into:
   - `GestureManipulationController`
   - `HandMenuRaycaster`
4. Create a room GameObject hierarchy (walls, floor, ceiling), then add:
   - `RoomDimensionsController` on the room root
   - `WallColorController` on the wall parent
5. Add `GestureSelectable` + colliders to each furniture piece and wall section you want selectable.
6. Add `GestureManipulationController` to a manager object and set the selectable layer mask.
7. Add UI/menu colliders on a menu layer, then add `HandMenuRaycaster` + `LineRenderer`.
8. Add `PinchMenuItemEvent` to menu targets and bind UnityEvents to:
   - room preset handlers (`RoomDimensionsController.ApplyPreset`)
   - wall color handlers (`WallColorController.ApplyColorFromHex`)
   - viewpoint handlers (`ViewpointController.MoveToViewpoint`)
