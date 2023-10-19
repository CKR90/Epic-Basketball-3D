using UnityEngine;
using System.Linq;

public class HoopAutomation : MonoBehaviour
{
    [HideInInspector]public bool Start_Automation = false;

    private bool setRigid = true;
    void Update()
    {
        if (Start_Automation)
        {
            SetRigidbody();
            AddForce();

            if (transform.position.y > 100f) Destroy(gameObject);
        }
    }
    private void SetRigidbody()
    {
        if (setRigid)
        {
            Transform t = transform.Find("HoopPivot");
            if (t != null) t = t.Find("Model");
            if(t != null)
            {
                Destroy(t.gameObject.GetComponent<Rigidbody>());
                Destroy(t.gameObject.GetComponent<MeshCollider>());
            }

            setRigid = false;
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = false;
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
            Transform cylinders = gameObject.transform.Find("Cylinders");
            Transform skateT = gameObject.transform.Find("skateboard");
            Animator anim = GetComponent<Animator>();

            if(anim != null) Destroy(anim);

            if(skateT != null)
            {
                skateT.SetParent(null);
            }
            if (cylinders != null)
            {
                cylinders.SetParent(null);
            }
        }
    }
    private void AddForce()
    {
        GetComponent<Rigidbody>().AddForce(Vector3.up * 5f);
    }
}
