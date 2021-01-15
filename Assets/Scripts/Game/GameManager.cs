using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace ShipsMultiplayerDemoV2
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("Oyuncunun Sahneye Eklenecek Prefab Dosyası")]
        public GameObject playerPrefab;

        [Tooltip("Sahneye eklenecek Liman Prefab")]
        public GameObject portPrefab;

        // Start is called before the first frame update
        void Awake()
        {
            StartGame();
        }

        private void StartGame()
        {
            Debug.Log("Oyuncu oluşturuluyor!");
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 0, -30), Quaternion.identity, 0);
                GameObject go = PhotonNetwork.Instantiate(this.portPrefab.name, new Vector3(-17, 5, -35), Quaternion.identity, 0);
               
            }
            else
            {
                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0, 0, 30), Quaternion.Euler(0, 180, 0), 0);
                GameObject go = PhotonNetwork.Instantiate(this.portPrefab.name, new Vector3(17, 5, 35), Quaternion.identity, 0);
                Camera.main.transform.rotation = Quaternion.Euler(90, 0, 180);
                
            }
        }

        //Diğer oyuncu odadan çıktığında çalışır.
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("Kazandınız");
        }



    }

}
