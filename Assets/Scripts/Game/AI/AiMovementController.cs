using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace ShipsMultiplayerDemoV2
{
    public class AiMovementController : MonoBehaviour
    {
        [Tooltip("Ne kadar hızlı hareket edecek")]
        [SerializeField]
        private float movementSpeed = 5f;

        //Hareket yönü
        private Vector3 movementDirection;

        //Hareket edeceği hedef konumu
        [SerializeField]
        Vector3 target;

        //Hareket ettiği konum ile arasındaki mesafe
        private float targetDistance;

        [Tooltip("Ne kadar uzaktan ateş edecek")]
        [SerializeField]
        private float shootDistance;


        Rigidbody rigidBody;
        ProjectileLauncher projectileLauncher;
        PhotonView photonView;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
            rigidBody = GetComponent<Rigidbody>();
            projectileLauncher = GetComponent<ProjectileLauncher>();

            target = GetComponentInChildren<AiTargetController>().target;
            CalculateDistance(target);
        }

        void Update()
        {
            if (target == null) return;
            CalcuteDirection(target);
            CalculateDistance(target);
            target = GetComponentInChildren<AiTargetController>().target;
        }

        void FixedUpdate()
        {
            if (target == null) return;

            if (targetDistance >= shootDistance)
            {
                MoveToTarget();
            }
            else
            {
                StopMovement();
                if (photonView.IsMine)
                {
                    projectileLauncher.Fire(target);
                }

            }

        }

        /// <summary>
        /// Objenin hareket etmesini durdurur.
        /// </summary>
        private void StopMovement()
        {
            rigidBody.velocity = Vector3.zero;
        }

        /// <summary>
        /// Objeyi hedefe doğru hareket ettirir.
        /// </summary>
        private void MoveToTarget()
        {
            Vector3 velocity = movementDirection * movementSpeed;
            rigidBody.velocity = velocity;
        }


        /// <summary>
        /// objenin hareket edeceği yönü hesaplar.
        /// </summary>
        /// <param name="pos1">Birinci pozisyon</param>
        /// <param name="pos2">İkinci pozisyon</param>
        /// <returns></returns>
        public void CalcuteDirection(Vector3 target)
        {
            Vector3 tempDirection = Vector3.Normalize(target - this.gameObject.transform.position);

            movementDirection = new Vector3(tempDirection.x, tempDirection.y * 0, tempDirection.z);

        }

        /// <summary>
        /// Objenin hedef konumuna olan uzaklığını hesaplar.
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        private void CalculateDistance(Vector3 target)
        {
            targetDistance = Vector3.Distance(this.gameObject.transform.position, target);

        }

    }
}