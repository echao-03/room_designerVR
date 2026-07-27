using UnityEngine;

public class WallColorController : MonoBehaviour
{
    [SerializeField] private Renderer[] wallRenderers;
    [SerializeField] private string colorProperty = "_BaseColor";

    private readonly MaterialPropertyBlock _block = new MaterialPropertyBlock();

    private void Awake()
    {
        if (wallRenderers == null || wallRenderers.Length == 0)
        {
            wallRenderers = GetComponentsInChildren<Renderer>();
        }
    }

    public void ApplyColor(Color color)
    {
        foreach (var wall in wallRenderers)
        {
            if (wall == null)
            {
                continue;
            }

            wall.GetPropertyBlock(_block);
            _block.SetColor(colorProperty, color);
            wall.SetPropertyBlock(_block);
        }
    }

    public void ApplyColorFromHex(string hex)
    {
        if (ColorUtility.TryParseHtmlString(hex, out var color))
        {
            ApplyColor(color);
        }
    }
}
