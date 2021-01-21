using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ShipsMultiplayerDemoV2
{
    public class ProjectileLauncher : MonoBehaviour
    {

        [Tooltip("Yapay zeka tarafından kontrol edilecekse işaretlenmelidir.")]
        [SerializeField]
        private bool isAI;

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
            if (photonView.IsMine && !isAI)
            {
                inputManager.OnFireButtonClickedEvents += Fire;
            }

        }



        /// <summary>
        /// FireRPC fonksiyonunu çalıştırır. Tüm serverdaki oyunculara ateş edildiği bilgisini yollar.
        /// </summary>
        public void Fire()
        {
            // Debug.Log("Fire");
            photonView.RPC("FireRPC", RpcTarget.All);
        }

        public void Fire(Vector3 target)
        {
            
            if (Time.time - firedTime > fireRate)
            {
                firedTime = Time.time;
                photonView.RPC("FireTargetRPC", RpcTarget.All, target);
            }
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
                    GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, this.transform.position/*shootPoint.position*/, Quaternion.identity);
                    projectile.GetComponent<Projectile>().CalcuteDirection(this.transform.position, shootPoint.position);
                }
            }


        }

        /// <summary>
        /// Oyuncu ateş etmek istiyor. Bu istek kendisinden geldiyse ve belirlenen atış sıklığı süresi aşıldıysa mermi üretilir.
        /// </summary>
        [PunRPC]
        private void FireTargetRPC(Vector3 target)
        {
            if (photonView.IsMine)
            {
                Debug.Log("FireTargetRPC");
               
                    
                    GameObject projectile = PhotonNetwork.Instantiate(projectilePrefab.name, this.transform.position, Quaternion.identity);
                    projectile.GetComponent<Projectile>().CalcuteDirection(this.transform.position, target);
                
            }


        }


        //Ana gemi yok olduğunda ateş butonu event içerisinden çıkarılır.
        private void OnDestroy()
        {
            if (isAI)
            {
                inputManager.OnFireButtonClickedEvents -= Fire;
            }
        }

    }
}
