using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace ShipsMultiplayerDemoV2
{
    public class InputManager : Singleton<InputManager>
    {
        /// <summary>
        /// Joystick'ten gelen yatay eksen girdisi.
        /// </summary>
        public float horizontal { get; private set; }

        /// <summary>
        /// Joystick'ten gelen dikey eksen girdisi.
        /// </summary>
        public float vertical { get; private set; }

        public Joystick joystick;

        public event Action OnFireButtonClickedEvents;
        public event Action OnBattleshipButtonClickedEvents;
        public event Action OnSubmarineButtonClickedEvents;
        

        void Update()
        {
            horizontal = joystick.Horizontal;
            vertical = joystick.Vertical;

        }


        public void OnFireButtonClicked()
        {
            OnFireButtonClickedEvents?.Invoke();
        }

        public void OnBattleshipButtonClicked()
        {
            OnBattleshipButtonClickedEvents?.Invoke();
        }

        public void OnSubmarineButtonClicked()
        {
            OnSubmarineButtonClickedEvents?.Invoke();
        }
    }
}
