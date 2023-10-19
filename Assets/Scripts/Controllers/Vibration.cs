using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;

public class Vibration : MonoBehaviour
{
    public GameObject tester;
    public Dropdown dropdown;
    public Text text;
    private HapticTypes hp = HapticTypes.None;
    private bool vibratefunc = false;
    public void VibrateFunction()
    {
        MMVibrationManager.Vibrate();
        text.text = "Vibrate Function";

    }
    public void Vibrate()
    {
            MMVibrationManager.Haptic(hp);
            text.text = hp.ToString();
    }
    public void SetVib() 
    {
        int val = dropdown.value;
        switch(dropdown.value)
        {
            case 0: hp = HapticTypes.None; break;
            case 1: hp = HapticTypes.MediumImpact; break;
            case 2: hp = HapticTypes.Success; break;
            case 3: hp = HapticTypes.SoftImpact; break;
            case 4: hp = HapticTypes.Failure; break;
            case 5: hp = HapticTypes.HeavyImpact; break;
            case 6: hp = HapticTypes.LightImpact; break;
            case 7: hp = HapticTypes.MediumImpact; break;
            case 8: hp = HapticTypes.RigidImpact; break;
            case 9: hp = HapticTypes.Selection; break;
        }
    }
    public void OpenClose()
    {
        tester.SetActive(!tester.activeSelf);
    }


}
