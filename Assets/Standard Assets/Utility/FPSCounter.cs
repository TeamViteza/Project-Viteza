using System;
using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
    [RequireComponent(typeof (Text))]
    public class FPSCounter : MonoBehaviour
    {
        const float fpsMeasurePeriod = 0.5f;
        private int m_FpsAccumulator = 0;
        private float m_FpsNextPeriod = 0;
        private int m_CurrentFps;
        const string display = "{0} FPS";
        private Text m_Text;

        float t;

        // https://answers.unity.com/questions/228095/why-is-the-motion-jerky-in-a-simple-2d-game.html Troubleshooting using this thread.
        void Awake()
        {
            //QualitySettings.vSyncCount = 0;  // VSync must be disabled
            //Application.targetFrameRate = 60;

            Time.maximumDeltaTime = 0.03f; //<<just over your estimated average frame time.
                                           //or alternatively:
        }

        private void Start()
        {
            m_FpsNextPeriod = Time.realtimeSinceStartup + fpsMeasurePeriod;
            m_Text = GetComponent<Text>();
        }

        private void Update()
        {
            // measure average frames per second
            m_FpsAccumulator++;
            if (Time.realtimeSinceStartup > m_FpsNextPeriod)
            {
                m_CurrentFps = (int) (m_FpsAccumulator/fpsMeasurePeriod);
                m_FpsAccumulator = 0;
                m_FpsNextPeriod += fpsMeasurePeriod;
                m_Text.text = string.Format(display, m_CurrentFps);
            }
          
            t = Time.deltaTime;
            if (t > 0.03) { t = 0.03f; }//constrain it
                                       //Note that they also have Time.smoothDeltaTime, but it's not much help.
        }
    }
}