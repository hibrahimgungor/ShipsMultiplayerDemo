using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ShipsMultiplayerDemoV2
{
    public abstract class HealthController : MonoBehaviour, IDamageable, IPunObservable
    {
        [Tooltip("Oyuncunun sahip olabileceği maksimum can puanı")]
        [SerializeField]
        private int maxHealth = 100;

        private int _health;
        private bool isDead;

        public int health
        {
            get
            {
                return _health;
            }
            private set
            {
                _health = value;
            }
        }

        private PhotonView photonView;


        void Awake()
        {
            photonView = GetComponent<PhotonView>();
            health = maxHealth;
            isDead = false;
        }

        /// <summary>
        /// Hasar alma RPC fonksiyonunu tetikler. Serverdaki tüm oyunculara hasar alınması gerektiğini gönderir.
        /// </summary>
        /// <param name="amount"></param>
        public void TakeDamage(int amount)
        {

            photonView.RPC("TakeDamageRPC", RpcTarget.All, amount);

        }

        /// <summary>
        /// Hasar alma işlemlerini yerine getirir. 
        /// </summary>
        /// <param name="amount">Alınacak hasar miktarı</param>
        [PunRPC]
        protected virtual void TakeDamageRPC(int amount)
        {
            if (!photonView.IsMine)
            {
                return;
            }
            if (!isDead)
            {
                health -= amount;

                CheckIsDead();
            }
        }

        /// <summary>
        /// Yaşam puanının 0'ın altına düşüp düşmediğini kontrol eder.0'ın altına düştüğünde ölüm işlemlemlerini başlatır.
        /// </summary>
        private void CheckIsDead()
        {
            if (health <= 0)
            {
                isDead = true;
                Die();
            }
            else
            {
                isDead = false;
            }
        }

        /// <summary>
        /// Yaşam puanı sıfırlanınca gerçekleşecekler
        /// </summary>
        protected virtual void Die()
        {
            PhotonNetwork.Destroy(this.gameObject);
        }

       

        //Objelerin can bilgisi server tarafında senkronize edilir.
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(health);
            }
            else
            {
                this.health = (int)stream.ReceiveNext();
            }
        }

    }


}
