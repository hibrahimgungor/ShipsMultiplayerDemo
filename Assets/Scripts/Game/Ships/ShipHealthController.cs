using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ShipsMultiplayerDemoV2
{
    public class ShipHealthController : HealthController
    {
        [SerializeField]
        private Slider healthSlider;

        private void Start()
        {
            healthSlider.maxValue = maxHealth;
        }


        private void Update()
        {
            healthSlider.value = health;
        }
    }
}

