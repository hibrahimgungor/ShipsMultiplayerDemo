using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace ShipsMultiplayerDemoV2
{
    public class AiTargetController : MonoBehaviour
    {
        /// <summary>
        /// Hedef objelerin bilgisini tutar.
        /// </summary>
        public List<GameObject> targets;

        private Vector3 _target;

        /// <summary>
        /// Mevcut hedef konum bilgisini tutar.
        /// </summary>
        public Vector3 target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }

        PhotonView photonView;

        private void Awake()
        {
            photonView = GetComponentInParent<PhotonView>();
        }

        private void Update()
        {
            if (targets.Count ==0)
            {
                SetDefaultTargetTransform();
                return;
            }

            if (targets[0] == null)
            {
                if (targets.Count > 0)
                {
                    targets.RemoveAt(0);
                }   
            }
            else
            {
                target = targets[0].transform.position;
            }
        }


        private void OnTriggerEnter(Collider other)
        {
            IDamageable isDamageble = other.GetComponent<IDamageable>();
            if (isDamageble != null)//Çarpışılan nesne hasar alabilen bir obje ise
            {
                int isMineObject = other.GetComponent<PhotonView>().CreatorActorNr;//Diğer gemiyi kim üretti?

                if (isMineObject != photonView.CreatorActorNr)//Biz üretmediysek
                {
                    targets.Add(other.gameObject);//Hedefler listesine eklenir.
                }
            }


        }

        
        /// <summary>
        /// 
        /// </summary>
        private void SetDefaultTargetTransform()
        {
            if (photonView.CreatorActorNr == 1)
            {
                target = new Vector3(17, 0, 35);
            }
            else
            {
                
               target = new Vector3(-17, 0, -35);
            }
        }

    }
}
