using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace ShipsMultiplayerDemo
{
    public class LoginManager : MonoBehaviourPunCallbacks
    {
        #region Private Serializable Fields

        [Tooltip("Odalarda bulunabilecek maksimum kişi sayısı")]
        [SerializeField]
        private byte maxPlayersPerRoom = 2;

        [Tooltip("Bağlantı durum bilgisi")]
        [SerializeField]
        private Text connectionStatus;

        [Tooltip("Giriş Paneli")]
        [SerializeField]
        private GameObject loginPanel;

        [Tooltip("Oda Paneli")]
        [SerializeField]
        private GameObject roomPanel;

        #endregion

        #region Private Fields

        /// <summary>
        /// Oyunun versiyonu bilgisi tutulur. Farklı versiyondaki oyuncular birbiri ile etkileşime giremez.
        /// </summary>
        string gameVersion = "1";

        /// <summary>
        /// Master Server'a bağlı olup olmadığımızın bilgisi tutulur.
        /// </summary>
        bool isConnecting;

        #endregion

        #region MonoBehaviour CallBacks

        private void Awake()
        {
            //Bir odadaki oyuncuların Ana oyuncu ile aynı seviye/sahnede olmasını sağlar. 
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            loginPanel.SetActive(false);
            roomPanel.SetActive(false);
            ConnectToMaster();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Master Server'a bağlan.
        /// </summary>
        public void ConnectToMaster()
        {
            //İlk olarak herhangi bir bağlantının olup olmadığı kontrol edilir.
            if (PhotonNetwork.IsConnected)
            {
                connectionStatus.text = "Connected to MasterServer";
                PhotonNetwork.JoinRandomRoom();
                Debug.Log("Bağlantı Mevcut");
            }
            else//Herhangi bir server bağlantı yoksa
            {
                Debug.Log("Bağlantı Yok. Bağlantı kuruluyor..");
                connectionStatus.text = "No Connection. Connecting...";
                isConnecting = PhotonNetwork.ConnectUsingSettings();//Belirlenen bağlantı ayarları ile bağlan...
                PhotonNetwork.GameVersion = gameVersion;//Oyun versiyonu belirlenir.
            }
        }


        /// <summary>
        /// Login butonuna tıklanınca rastgele bir oda oluşturulup bağlanmaya çalışılır.
        /// </summary>
        public void OnLoginButtonClicked()
        {
            if (PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer.NickName != string.Empty)
            {
                PhotonNetwork.JoinRandomRoom();
                isConnecting = false;
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks


        //Oyuncu, Master Server'a başarılı bir şekilde bağlandığında çalışır. 
        public override void OnConnectedToMaster()
        {
            Debug.Log("Server'a bağlanıldı.");
            connectionStatus.text = "Connected to MasterServer. Waiting for Login.";
            loginPanel.SetActive(true);
        }

        //Oyuncu serverdan ayrıldığı zaman çalışır.
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"Oyuncu oyundan şu sebep ile ayrıldı : {cause} ");
        }

        //Herhangi bir odaya katılım sağlanamadığında çalışır.
        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Herhangi bir oda bulunamadı. Yeni bir oda kuruluyor...");
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom }); 
            
        }

        //Odaya başarılı bir şekilde bağlabıldığında çalışır.
        public override void OnJoinedRoom()
        {
            Debug.Log("Odaya bağlanıldı.");
            if (PhotonNetwork.IsMasterClient)
            {
                connectionStatus.text = "Connected a room. Waiting for Other Player";
            }
            else
            {
                connectionStatus.text = $"Connected {PhotonNetwork.MasterClient.NickName}'s room";
                if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
                {
                    connectionStatus.text = $"Connected {PhotonNetwork.MasterClient.NickName}'s room. Room is full.Game will start after 5 seconds.";


                }
            }
            
            loginPanel.SetActive(false);
            roomPanel.SetActive(true);
        }

       

        #endregion

    }
}
