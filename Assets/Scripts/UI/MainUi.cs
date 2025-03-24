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
        [SerializeField] GameObject gui;
        [SerializeField] GameObject MainMenu;

        //public event Action<InputAction.CallbackContext> OnP1Move;
        //public event Action<InputAction.CallbackContext> OnP1Rotate;
        //public event Action<InputAction.CallbackContext> OnP2Move;
        //public event Action<InputAction.CallbackContext> OnP2Rotate;

        public void StartNewGame()
        {
            



            MainMenu.SetActive(false);
            gui.SetActive(true);
            mainLogic.StartGame();


        }

        public void resetGame()
        {
            //gui.reset();
            gui.SetActive(false);
            MainMenu.SetActive(true);
        }    

        private void Awake()
        {
            _playersInput = new PlayersInput();
            mainLogic = Main.Instance;

            if (mainLogic == null)
            {
                Debug.LogError("MainLogic is null");
                return;
            }

            mainLogic.OnGameEnded += resetGame;
        }



        private void OnEnable()
        {
            _playersInput.Enable();

            _playersInput.Player1.Move.performed += OnP1Move;
            _playersInput.Player1.Rotate.performed += OnP1Rotate;

            _playersInput.Player2.Move.performed += OnP2Move;
            _playersInput.Player2.Rotate.performed += OnP2Rotate;
        }

        private void OnDisable()
        {
            _playersInput.Disable();

            _playersInput.Player1.Move.performed -= OnP1Move;
            _playersInput.Player1.Rotate.performed -= OnP1Rotate;

            _playersInput.Player2.Move.performed -= OnP2Move;
            _playersInput.Player2.Rotate.performed -= OnP2Rotate;
        }

        void OnP1Move(InputAction.CallbackContext context)
        {
            int direction = (int)context.ReadValue<float>();
            mainLogic.OnP1Move(direction);
        }
        void OnP1Rotate(InputAction.CallbackContext context)
        {
            int direction = (int)context.ReadValue<float>();
            mainLogic.OnP1Rotate(direction);
        }
        void OnP2Move(InputAction.CallbackContext context)
        {
            int direction = (int)context.ReadValue<float>();
            mainLogic.OnP2Move(direction);
        }

        void OnP2Rotate(InputAction.CallbackContext context)
        {
            int direction = (int)context.ReadValue<float>();
            mainLogic.OnP2Rotate(direction);
        }
    }
}
