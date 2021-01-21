using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;



namespace ShipsMultiplayerDemoV2
{
    [RequireComponent(typeof(Rigidbody))]
    public class MainShipMovement : MonoBehaviour
    {
        /// <summary>
        /// Objenin hareket hızı
        /// </summary>
        [SerializeField]
        private float speed = 5f;

        private Rigidbody rigidBody;
        private InputManager inputManager;
        private PhotonView photonvView;

        void Start()
        {
            inputManager = InputManager.Instance;
            rigidBody = GetComponent<Rigidbody>();

            photonvView = GetComponent<PhotonView>();
            if (!PhotonNetwork.IsMasterClient)
            {
                speed *= -1;//Odayı kuran kişi değilsek hareketlerimiz ters yönde olmalıdır.
            }


        }

        void FixedUpdate()
        {
            VerticalMovement(inputManager.vertical);
            HorizontalMovement(inputManager.horizontal);
        }

        /// <summary>
        /// Objenin Z eksenindeki hareketlerini kontrol eder.
        /// </summary>
        /// <param name="verticalInput">Input managerdan gelecek dikey eksen girdisi</param>
        private void VerticalMovement(float verticalInput)
        {
            if (photonvView.IsMine)
            {
                Vector3 verticalVelocity = new Vector3(rigidBody.velocity.x, 0, verticalInput * speed);
               rigidBody.velocity = verticalVelocity;
                //rigidBody.MovePosition(transform.position + verticalVelocity * Time.fixedDeltaTime);
            }
        }

        /// <summary>
        /// Objenin X eksenindeki hareketlerini kontrol eder.
        /// </summary>
        /// <param name="horizontalInput">Input managerdan gelecek yatay eksen girdisi</param>
        private void HorizontalMovement(float horizontalInput)
        {
            if (photonvView.IsMine)
            {
                Vector3 horizontalVelocity = new Vector3(horizontalInput * speed, 0, rigidBody.velocity.z);
                rigidBody.velocity = horizontalVelocity;
            }

        }
    }
}
