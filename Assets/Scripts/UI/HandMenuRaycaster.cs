using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HandMenuRaycaster : MonoBehaviour
{
    [SerializeField] private OVRHand hand;
    [SerializeField] private LayerMask menuMask = ~0;
    [SerializeField] private float rayDistance = 8f;

    private LineRenderer _lineRenderer;
    private bool _pinchPrevious;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        var pose = GetHandPose();
        var origin = pose.position;
        var direction = pose.rotation * Vector3.forward;

        var hitFound = Physics.Raycast(origin, direction, out var hit, rayDistance, menuMask);
        var end = hitFound ? hit.point : origin + direction * rayDistance;
        DrawRay(origin, end);

        var isPinching = hand != null && hand.GetFingerIsPinching(OVRHand.HandFinger.Index);
        if (isPinching && !_pinchPrevious && hitFound)
        {
            var menuItem = hit.collider.GetComponentInParent<IPinchMenuItem>();
            menuItem?.OnPinchClick();
        }

        _pinchPrevious = isPinching;
    }

    private Pose GetHandPose()
    {
        if (hand != null && hand.PointerPose != null)
        {
            return new Pose(hand.PointerPose.position, hand.PointerPose.rotation);
        }

        return new Pose(transform.position, transform.rotation);
    }

    private void DrawRay(Vector3 start, Vector3 end)
    {
        _lineRenderer.positionCount = 2;
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }
}
