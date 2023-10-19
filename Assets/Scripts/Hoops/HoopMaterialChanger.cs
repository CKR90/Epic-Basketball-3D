using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoopMaterialChanger : MonoBehaviour
{
    public Throw throwScript;
    public ThrowBot throwBotSctipt;
    public GameObject QuestionMark;
    public ParticleSystem particle;
    public List<Material> ColorMaterials = new List<Material>();
    public Material NetTransparent;
    public Material NetOpaque;

    private MeshRenderer m;
    private MeshRenderer mHoop;
    private string hoopName = "Net_WithAnim(Clone)";
    private GameObject net;

    private bool check = true;
    bool catchNet = false;
    void Start()
    {
        m = GetComponent<MeshRenderer>();
        mHoop = transform.parent.Find("HoopPivot").Find("Model").gameObject.GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!check) return;

        Transform n = transform.parent.Find("HoopPivot").Find(hoopName);
        if(n != null && !catchNet)
        {
            catchNet = true;
            net = n.Find("Cylinder").gameObject;
            net.GetComponent<SkinnedMeshRenderer>().sharedMaterial = NetTransparent;
        }

        if(throwScript.score >= throwScript.HoopTransforms.Count || throwBotSctipt.score >= throwBotSctipt.HoopTransforms.Count)
        {
            m.sharedMaterials = ColorMaterials.ToArray();
            mHoop.sharedMaterials = ColorMaterials.ToArray();
            net.GetComponent<SkinnedMeshRenderer>().sharedMaterial = NetOpaque;
            Destroy(QuestionMark);
            particle.Play();
            check = false;
        }

    }
}
