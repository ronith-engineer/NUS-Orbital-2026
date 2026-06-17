using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class TrajectoryLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    
    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void RenderLine(Vector3 startPoint, Vector3 endPoint)
    {
        lineRenderer.positionCount = 2;
        Vector3[] points = new Vector3[] { startPoint, endPoint };
        lineRenderer.SetPositions(points);
    }

    public void Endline()
    {
        lineRenderer.positionCount = 0;
    }
}
