using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace ShipsMultiplayerDemoV2
{
    public class Projectile : MonoBehaviour
    {
        [Tooltip("Merminin hareket hızı")]
        [SerializeField]
        private float movementSpeed = 5f;

        [Tooltip("Merminin vereceği hasar miktarı")]
        [SerializeField]
        private int damageAmount = 5;

        /// <summary>
        /// Merminin hareket edeceği hedef yönü
        /// </summary>
        private Vector3 movementDirection;

        private Rigidbody rigidBody;
        private PhotonView photonView;

        void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            photonView = GetComponent<PhotonView>();
        }

        void FixedUpdate()
        {
            Move();
        }



        private void OnTriggerEnter(Collider other)
        {

            IDamageable isDamageble = other.GetComponent<IDamageable>();
            Debug.Log("isdamagable" + isDamageble);
            if (isDamageble != null)//Çarpışılan nesne hasar alabilen bir obje ise
            {
                int isMineObject = other.GetComponent<PhotonView>().CreatorActorNr;//Mermi kime ait
                if (isMineObject != photonView.CreatorActorNr )//Mermi benim değilse
                {
                    Debug.Log("Take damage öncesi");
                    isDamageble.TakeDamage(damageAmount);
                    Debug.Log("Hasar Verildi");
                    PhotonNetwork.Destroy(this.gameObject);
                }
            }
            else
            {
                if (other.tag =="StaticObject")
                {
                    PhotonNetwork.Destroy(this.gameObject);
                }
                

            }

        }



        /// <summary>
        /// Mermiyi hareket ettirir.
        /// </summary>
        private void Move()
        {
            rigidBody.velocity = movementDirection * movementSpeed;
        }


        /// <summary>
        /// Merminin hareket edeceği yönü hesaplar.
        /// </summary>
        /// <param name="pos1">Birinci pozisyon</param>
        /// <param name="pos2">İkinci pozisyon</param>
        /// <returns></returns>
        public void CalcuteDirection(Vector3 pos1, Vector3 pos2)
        {
            movementDirection = Vector3.Normalize(pos2 - pos1);
        }


    }
}
