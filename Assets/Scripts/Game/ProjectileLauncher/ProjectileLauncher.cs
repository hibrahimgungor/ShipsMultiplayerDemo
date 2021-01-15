using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ShipsMultiplayerDemoV2
{
    public class ProjectileLauncher : MonoBehaviour
    {
        [Tooltip("Atış için üretilecek mermi Prefabi")]
        [SerializeField]
        private GameObject projectilePrefab;

        [Tooltip("Merminin üretileceği konum")]
        [SerializeField]
        private Transform shootPoint;

        [Tooltip("Atış sıklığı")]
        [SerializeField]
        private float fireRate = 2.5f;

        //Ateş edil
        private float firedTime;

     
        private InputManager inputManager;
        private PhotonView photonView;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
            inputManager = InputManager.Instance;
        }

        void Start()
        {
            if (photonView.IsMine)
            {
                inputManager.OnFireButtonClickedEvents += Fire;
            }
            
        }



        /// <summary>
        /// FireRPC fonksiyonunu çalıştırır. Tüm serverdaki oyunculara ateş edildiği bilgisini yollar.
        /// </summary>
        private void Fire()
        {
           // Debug.Log("Fire");
            photonView.RPC("FireRPC", RpcTarget.All);
        }

        /// <summary>
        /// Oyuncu ateş etmek istiyor. Bu istek kendisinden geldiyse ve belirlenen atış sıklığı süresi aşıldıysa mermi üretilir.
        /// </summary>
        [PunRPC]
        private void FireRPC()
        {
            if (photonView.IsMine)
            {
               // Debug.Log("FireRPC");
                if (Time.time - firedTime > fireRate)
                {
                    firedTime = Time.time;
                    GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, shootPoint.position, Quaternion.identity);
                    projectile.GetComponent<Projectile>().CalcuteDirection(this.transform, shootPoint);
                }
            }
            

        }
    }
}
