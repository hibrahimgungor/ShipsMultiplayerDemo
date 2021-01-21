using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;


namespace ShipsMultiplayerDemoV2
{
    public class PlayerHealthController : HealthController
    {
        [Tooltip("Helth Bar Slider")]
        [SerializeField]
        private TMP_Text healthUI;


        private void Update()
        {
            healthUI.text = health.ToString();
        }

        protected override void Die()
        {
            Debug.Log("Kaybettim");
            PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
            PhotonNetwork.Disconnect();
            
        }
    }
}
