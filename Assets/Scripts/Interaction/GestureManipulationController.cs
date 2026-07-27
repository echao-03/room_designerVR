using UnityEngine;

public class GestureManipulationController : MonoBehaviour
{
    [SerializeField] private OVRHand leftHand;
    [SerializeField] private OVRHand rightHand;
    [SerializeField] private LayerMask selectableMask = ~0;
    [SerializeField] private float maxSelectionDistance = 6f;
    [SerializeField] private Vector2 scaleLimits = new Vector2(0.2f, 5f);

    private GestureSelectable _selection;
    private bool _leftPinchPrevious;
    private bool _rightPinchPrevious;
    private bool _dragActive;
    private OVRHand _dragHand;
    private Vector3 _dragOffset;

    private bool _twoHandActive;
    private Vector3 _startHandVector;
    private Vector3 _startScale;
    private Quaternion _startRotation;

    private void Update()
    {
        var leftPinch = IsPinching(leftHand);
        var rightPinch = IsPinching(rightHand);

        var leftPinchDown = leftPinch && !_leftPinchPrevious;
        var rightPinchDown = rightPinch && !_rightPinchPrevious;

        if (leftPinchDown)
        {
            TrySelect(leftHand);
        }
        else if (rightPinchDown)
        {
            TrySelect(rightHand);
        }

        if (_selection != null)
        {
            HandleManipulation(leftPinch, rightPinch);
        }

        _leftPinchPrevious = leftPinch;
        _rightPinchPrevious = rightPinch;
    }

    private void HandleManipulation(bool leftPinch, bool rightPinch)
    {
        if (leftPinch && rightPinch)
        {
            _dragActive = false;
            HandleTwoHandManipulation();
            return;
        }

        _twoHandActive = false;

        var hand = leftPinch ? leftHand : rightPinch ? rightHand : null;
        if (hand == null)
        {
            _dragActive = false;
            _dragHand = null;
            return;
        }

        if (!_dragActive || _dragHand != hand)
        {
            _dragHand = hand;
            _dragActive = true;
            _dragOffset = _selection.transform.position - GetHandPose(hand).position;
        }

        _selection.transform.position = GetHandPose(hand).position + _dragOffset;
    }

    private void HandleTwoHandManipulation()
    {
        if (_selection == null)
        {
            return;
        }

        var leftPose = GetHandPose(leftHand);
        var rightPose = GetHandPose(rightHand);
        var currentVector = rightPose.position - leftPose.position;

        if (!_twoHandActive)
        {
            _twoHandActive = true;
            _startHandVector = currentVector;
            _startScale = _selection.transform.localScale;
            _startRotation = _selection.transform.rotation;
            return;
        }

        var startDistance = Mathf.Max(_startHandVector.magnitude, 0.001f);
        var currentDistance = Mathf.Max(currentVector.magnitude, 0.001f);
        var scaleFactor = Mathf.Clamp(currentDistance / startDistance, scaleLimits.x, scaleLimits.y);
        _selection.transform.localScale = _startScale * scaleFactor;

        var startFlat = Vector3.ProjectOnPlane(_startHandVector, Vector3.up);
        var currentFlat = Vector3.ProjectOnPlane(currentVector, Vector3.up);
        if (startFlat.sqrMagnitude > 0.001f && currentFlat.sqrMagnitude > 0.001f)
        {
            var angleDelta = Vector3.SignedAngle(startFlat, currentFlat, Vector3.up);
            _selection.transform.rotation = Quaternion.AngleAxis(angleDelta, Vector3.up) * _startRotation;
        }
    }

    private void TrySelect(OVRHand sourceHand)
    {
        if (sourceHand == null)
        {
            return;
        }

        var pose = GetHandPose(sourceHand);
        if (!Physics.Raycast(pose.position, pose.rotation * Vector3.forward, out var hit, maxSelectionDistance, selectableMask))
        {
            return;
        }

        var target = hit.collider.GetComponentInParent<GestureSelectable>();
        if (target == null)
        {
            return;
        }

        if (_selection != null && _selection != target)
        {
            _selection.SetSelected(false);
        }

        _selection = target;
        _selection.SetSelected(true);
        _dragHand = sourceHand;
        _dragActive = false;
        _twoHandActive = false;
    }

    private static bool IsPinching(OVRHand hand)
    {
        return hand != null && hand.GetFingerIsPinching(OVRHand.HandFinger.Index);
    }

    private static Pose GetHandPose(OVRHand hand)
    {
        if (hand != null && hand.PointerPose != null)
        {
            return new Pose(hand.PointerPose.position, hand.PointerPose.rotation);
        }

        return hand == null
            ? new Pose(Vector3.zero, Quaternion.identity)
            : new Pose(hand.transform.position, hand.transform.rotation);
    }
}
