using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndShoot : MonoBehaviour
{
    public float forceMultiplier = 2f;
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;

    private Rigidbody rb;

    private bool isShoot;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        isShoot = false;
        mousePressDownPos = Input.mousePosition;
    }

    private void OnMouseDrag()
    {
        Vector3 forceInit = (Input.mousePosition - mousePressDownPos);
        Vector3 forceV = (new Vector3(forceInit.x, forceInit.y, forceInit.y)) * forceMultiplier;
    
        if(!isShoot)
        {
            Trajectory.Instance.UpdateTrajectory(forceV, rb, transform.position);
        }

    }

    private void OnMouseUp()
    {
        Trajectory.Instance.HideLine();
        mouseReleasePos = Input.mousePosition;
        Shoot(mouseReleasePos - mousePressDownPos);
    }

    private void Shoot(Vector3 force)
    {
        if (isShoot) return;

        rb.isKinematic = false;
        rb.AddForce(new Vector3(force.x, force.y, force.y) * forceMultiplier);
        isShoot = true;
    }
}
