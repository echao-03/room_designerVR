using UnityEngine;

public class RoomDimensionsController : MonoBehaviour
{
    [System.Serializable]
    public struct RoomPreset
    {
        public string label;
        public Vector3 dimensions;
    }

    [SerializeField] private Transform roomRoot;
    [SerializeField] private Vector3 baseDimensions = new Vector3(4f, 3f, 4f);
    [SerializeField] private RoomPreset[] presets;

    private Vector3 _currentDimensions;

    public Vector3 CurrentDimensions => _currentDimensions;

    private void Awake()
    {
        if (roomRoot == null)
        {
            roomRoot = transform;
        }

        SetDimensions(baseDimensions);
    }

    public void SetDimensions(Vector3 dimensions)
    {
        var safeDimensions = new Vector3(
            Mathf.Max(dimensions.x, 1f),
            Mathf.Max(dimensions.y, 2f),
            Mathf.Max(dimensions.z, 1f));

        _currentDimensions = safeDimensions;
        roomRoot.localScale = new Vector3(
            safeDimensions.x / Mathf.Max(baseDimensions.x, 0.01f),
            safeDimensions.y / Mathf.Max(baseDimensions.y, 0.01f),
            safeDimensions.z / Mathf.Max(baseDimensions.z, 0.01f));
    }

    public void ApplyPreset(int presetIndex)
    {
        if (presetIndex < 0 || presetIndex >= presets.Length)
        {
            return;
        }

        SetDimensions(presets[presetIndex].dimensions);
    }
}
