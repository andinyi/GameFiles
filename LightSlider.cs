using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightSlider : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Light light; 
    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
        if(light == null) {
            light = GameObject.Find("Directional Light").GetComponent<Light>();
    }
    }
    void Start()
    {
        slider.value = 0.5f;
    }
    void Update() {
        light.intensity = slider.value;
    }
}
