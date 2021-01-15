using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;


namespace ShipsMultiplayerDemo
{
    [RequireComponent(typeof(InputField))]
    public class PlayerNameInputField : MonoBehaviour
    {

        private void Start()
        {
            InputField inputField = GetComponent<InputField>();
            if (inputField.text=="")
            {
                inputField.text = CreateRandomName();
                PhotonNetwork.LocalPlayer.NickName = inputField.text;
            }
            
        }

        /// <summary>
        /// Oyuncunun adını değiştirir. InputField OnValuChanged eventinde çalıştırılır.
        /// </summary>
        /// <param name="value">Oyuncunun Adı</param>
        public void SetPlayerName(string value)
        {
            // #Important
            if (string.IsNullOrEmpty(value))
            {
                Debug.LogError("Oyuncu Adı Boş Bırakılamaz");
                PhotonNetwork.NickName = string.Empty;
                return;
            }
            PhotonNetwork.NickName = value;
        }


        /// <summary>
        /// Player için rastgele bir isim oluşturur.
        /// </summary>
        /// <returns></returns>
        private string CreateRandomName()
        {
            return "Player" + Random.Range(0, 999).ToString();
        }





    }
}
