using UnityEngine;

[RequireComponent(typeof(Collider))]
public class GestureSelectable : MonoBehaviour
{
    [SerializeField] private Renderer[] highlightRenderers;
    [SerializeField] private Color highlightColor = new Color(0.25f, 0.75f, 1f, 1f);
    [SerializeField] private string emissionProperty = "_EmissionColor";

    private readonly MaterialPropertyBlock _block = new MaterialPropertyBlock();
    private bool _selected;

    public bool IsSelected => _selected;

    private void Awake()
    {
        if (highlightRenderers == null || highlightRenderers.Length == 0)
        {
            highlightRenderers = GetComponentsInChildren<Renderer>();
        }

        SetSelected(false);
    }

    public void SetSelected(bool selected)
    {
        _selected = selected;

        foreach (var rendererRef in highlightRenderers)
        {
            if (rendererRef == null)
            {
                continue;
            }

            rendererRef.GetPropertyBlock(_block);
            _block.SetColor(emissionProperty, selected ? highlightColor : Color.black);
            rendererRef.SetPropertyBlock(_block);
        }
    }
}
