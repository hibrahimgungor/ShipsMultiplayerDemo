using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

namespace ShipsMultiplayerDemo
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {

        [Tooltip("Bağlantı durum bilgisi")]
        public Text connectionStatus;

        [Tooltip("Oda bilgisini gösteren UI objesi")]
        public GameObject roomPanel;

       
        //Diğer Oyuncu odaya bağlandığında tetiklenir
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            connectionStatus.text = $"{newPlayer.NickName} connected room.";
            //Debug.Log($"{newPlayer.NickName} odaya bağlandı");

            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                connectionStatus.text = $"{newPlayer.NickName} connected room. Room is full.Game will start after 5 seconds.";
                PhotonNetwork.CurrentRoom.IsVisible = false;
                Invoke("LoadGameScene", 5f);
            }
        }

        //Diğer Oyuncu odadan ayrıldığında tetiklenir.
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            connectionStatus.text = $"{otherPlayer.NickName} left room.";
            //Debug.Log($"{otherPlayer.NickName} odadan ayrıldı.");

        }


        //Leave Room butonuna tıklandığında olacaklar
        public void OnLeaveRoomButtonClicked()
        {
            PhotonNetwork.LeaveRoom();
        }

        //Yerel oyuncu odadan ayrıldığında tetiklenir
        public override void OnLeftRoom()
        {
            roomPanel.SetActive(false);
        }

        //Oyun sahnesini başlatır.
        private void LoadGameScene()
        {
            PhotonNetwork.LoadLevel("GameScene");
        }


    }
}
