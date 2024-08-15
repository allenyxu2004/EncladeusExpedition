using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    Slider slider;
    void Start()
    {
        slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat("Sensitivity");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerPrefs.SetFloat("Sensitivity", slider.value);
        Debug.Log(PlayerPrefs.GetFloat("Sensitivity"));
    }
}
