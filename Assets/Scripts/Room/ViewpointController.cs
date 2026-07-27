using UnityEngine;

public class ViewpointController : MonoBehaviour
{
    [SerializeField] private Transform cameraRigRoot;
    [SerializeField] private Transform[] viewpoints;

    public void MoveToViewpoint(int index)
    {
        if (cameraRigRoot == null || viewpoints == null || index < 0 || index >= viewpoints.Length)
        {
            return;
        }

        var target = viewpoints[index];
        if (target == null)
        {
            return;
        }

        cameraRigRoot.position = target.position;
        cameraRigRoot.rotation = target.rotation;
    }
}
