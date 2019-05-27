using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour {

    public float CurrentProg { get; set; }
    public float MaxProg { get; set; }

    public Slider powerMeter;

    // Use this for initialization
    void Start () {
        MaxProg = 20f;

        CurrentProg = MaxProg;

        powerMeter.value = CalulatePower();
	}

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.X))
        {
            ProgGet(0.8f);
        }
	}

    void ProgGet(float progValue)
    {
        CurrentProg += progValue;
        powerMeter.value = CalulatePower();
    }

    float CalulatePower()
    {
        return CurrentProg / MaxProg;
    }
}
