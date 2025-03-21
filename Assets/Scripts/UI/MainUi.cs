using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Logic;

namespace UI
{
    public class MainUi : MonoBehaviour
    {
        PlayersInput _playersInput;
        Main mainLogic;
        public GameObject gui;

        //public event Action<InputAction.CallbackContext> OnP1Move;
        //public event Action<InputAction.CallbackContext> OnP1Rotate;
        //public event Action<InputAction.CallbackContext> OnP2Move;
        //public event Action<InputAction.CallbackContext> OnP2Rotate;

        public void StartNewGame()
        {
            mainLogic = Main.Instance;
            
            if (mainLogic == null)
            {
                Debug.LogError("MainLogic is null");
                return;
            }

            gameObject.SetActive(false);
            gui.SetActive(true);
            mainLogic.StartGame();
        }

        private void Awake()
        {
            _playersInput = new PlayersInput();

        }

        private void OnEnable()
        {
            _playersInput.Enable();

            _playersInput.Player1.Move.performed += OnP1Move;
        }

        private void OnDisable()
        {
            _playersInput.Disable();
        }

        void OnP1Move(InputAction.CallbackContext context)
        {
           
        }

    }
}
