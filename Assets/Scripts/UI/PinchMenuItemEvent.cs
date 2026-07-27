using UnityEngine;
using UnityEngine.Events;

public class PinchMenuItemEvent : MonoBehaviour, IPinchMenuItem
{
    [SerializeField] private UnityEvent onPinchClick;

    public void OnPinchClick()
    {
        onPinchClick?.Invoke();
    }
}
