using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

        [Tooltip("Askeri Bot Prefab")]
        public GameObject aiShipPrefab;
        
        [Tooltip("Denizaltı Prefab")]
        public GameObject aiSubmarinePrefab;

        public GameObject wonText;
        public GameObject loseText;




        //Manuel olarak girililecek master client obje spawn noktaları
        [SerializeField]
        private Vector3 masterPlayerPortLocation;
        [SerializeField]
        private Vector3 masterPlayerMainShipLocation;
        [SerializeField]
        private Vector3 masterPlayerAISpawnLocation;

        //Master client verilerine göre otomatik hesaplanacak 2. oyuncu spawn noktaları
        private Vector3 otherPlayerPortLocation;
        private Vector3 otherPlayerMainShipLocation;
        private Vector3 otherPlayerAISpawnLocation;



        // Start is called before the first frame update
        void Awake()
        {
            SetOtherPlayerSpawnLocations();
            StartGame();
        }

        private void Start()
        {
            InputManager.Instance.OnBattleshipButtonClickedEvents += CreateWarShip;
            InputManager.Instance.OnSubmarineButtonClickedEvents += CreateSubMarine;
        }

        private void StartGame()
        {
            Debug.Log("Oyuncu oluşturuluyor!");
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(this.playerPrefab.name, masterPlayerMainShipLocation, Quaternion.identity, 0);
                PhotonNetwork.Instantiate(this.portPrefab.name, masterPlayerPortLocation, Quaternion.identity, 0);

            }
            else
            {
                PhotonNetwork.Instantiate(this.playerPrefab.name, otherPlayerMainShipLocation, Quaternion.Euler(0, 180, 0), 0);
                PhotonNetwork.Instantiate(this.portPrefab.name, otherPlayerPortLocation, Quaternion.identity, 0);
                Camera.main.transform.rotation = Quaternion.Euler(90, 0, 180);

            }
        }

        //Diğer oyuncu odadan çıktığında çalışır.
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.Log("Kazandınız");
            wonText.SetActive(true);
            Time.timeScale = 0f;
            
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            loseText.SetActive(true);
        }

        /// <summary>
        /// Savaş gemisi üret.
        /// </summary>
        public void CreateWarShip()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(aiShipPrefab.name, masterPlayerAISpawnLocation, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate(aiShipPrefab.name, otherPlayerAISpawnLocation, Quaternion.identity);
            }

        }

        /// <summary>
        /// Denizaltı üret.
        /// </summary>
        public void CreateSubMarine()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Instantiate(aiSubmarinePrefab.name, masterPlayerAISpawnLocation, Quaternion.identity);
            }
            else
            {
                PhotonNetwork.Instantiate(aiSubmarinePrefab.name, otherPlayerAISpawnLocation, Quaternion.identity);
            }
        }


        /// <summary>
        /// Master oyuncunun konum bilgilerinin simetriğini alıp 2. oyuncunun spawn noktalarını belirler.
        /// </summary>
        private void SetOtherPlayerSpawnLocations()
        {
            otherPlayerPortLocation = new Vector3(masterPlayerPortLocation.x * -1, masterPlayerPortLocation.y, masterPlayerPortLocation.z * -1);
            otherPlayerMainShipLocation = new Vector3(masterPlayerMainShipLocation.x * -1, masterPlayerMainShipLocation.y, masterPlayerMainShipLocation.z * -1);
            otherPlayerAISpawnLocation = new Vector3(masterPlayerAISpawnLocation.x * -1, masterPlayerAISpawnLocation.y, masterPlayerAISpawnLocation.z * -1);
        }

    }

}
