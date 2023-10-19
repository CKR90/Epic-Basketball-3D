using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public LineRenderer lineRenderer;

    [Range(5, 50)]
    public int lineSegments = 20;

    private List<Vector3> linePoints = new List<Vector3>();

    public static Trajectory Instance;

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTrajectory(Vector3 forceVector, Rigidbody rigidbody, Vector3 startingPoint)
    {
        Vector3 velocity = (forceVector / rigidbody.mass) * Time.fixedDeltaTime;
        float FlightDuration = (2 * velocity.y) / Physics.gravity.y;
        float stepTime = FlightDuration / lineSegments;

        linePoints.Clear();

        for(int i = 0; i < lineSegments; i++)
        {
            float stepTimePassed = stepTime * i;

            Vector3 movementVector = new Vector3(
                -velocity.x * stepTimePassed,
                -(velocity.y * stepTimePassed - (0.5f * Physics.gravity.y * stepTimePassed * stepTimePassed)),
                -velocity.z * stepTimePassed
                );
            linePoints.Add(movementVector + startingPoint);
        }

        lineRenderer.positionCount = linePoints.Count;
        lineRenderer.SetPositions(linePoints.ToArray());
    }

    public void HideLine()
    {
        lineRenderer.positionCount = 0;
    }
}
