using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugCourtColor : MonoBehaviour
{
    public GameObject MainMenu;
    public List<Material> materials;

    private MeshRenderer mr;
    void Start()
    {
        mr = GameObject.Find("Backdrop").GetComponent<MeshRenderer>();
        //BallMaterial.SetTexture("_MainTex", BallFirstSprite.texture);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public void BackButton()
    {
        MainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void SelectColor(int i)
    {
        mr.sharedMaterial = materials[i];

        string buildIndex = SceneManager.GetActiveScene().buildIndex.ToString();
        PlayerPrefs.SetString("EditCourtMaterialName_" + buildIndex, materials[i].name);
    }

    public void RestoreDefaults()
    {
        string buildIndex = SceneManager.GetActiveScene().buildIndex.ToString();
        string text = PlayerPrefs.GetString("DefaultCourtMaterialName_" + buildIndex);
        Material m = materials.Find(x => x.name == text);
        mr.sharedMaterial = m;
        PlayerPrefs.DeleteKey("EditCourtMaterialName_" + buildIndex);
    }
}
