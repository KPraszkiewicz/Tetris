using UnityEngine;
using UnityEngine.InputSystem;
using Logic;
using UnityEngine.SceneManagement;
using System;
using TMPro;

namespace UI
{
    public class MainUi : MonoBehaviour
    {
        PlayersInput _playersInput;
        Main mainLogic;
        [SerializeField] GUI gui;
        [SerializeField] GameObject MainMenu;
        [SerializeField] GameObject GameResultLabel;

        public void StartNewGame()
        {
            if (!mainLogic)
            {
                mainLogic = Main.Instance;

                if (mainLogic == null)
                {
                    Debug.LogError("MainLogic is null");
                    return;
                }

                mainLogic.OnGameEnded += MainLogic_OnGameEnded;
                mainLogic.OnScoreUpdated += MainLogic_OnScoreUpdated;
                mainLogic.OnNewNextBrick += MainLogic_OnNewNextBrick;
            }

            MainMenu.SetActive(false);
            gui.gameObject.SetActive(true);
            gui.ResetGui();
            mainLogic.StartGame();
        }

        private void MainLogic_OnGameEnded(int playerId)
        {
            GameResultLabel.GetComponent<TextMeshProUGUI>().text = String.Format("Wygra³ gracz {0}", playerId);
            ResetGame();
        }

        public void ResetGame()
        {
            MainMenu.SetActive(true);
        }    

        private void Awake()
        {
            if(SceneManager.sceneCount < 2)
            {
                SceneManager.LoadScene("TetrisBoard", LoadSceneMode.Additive);
            }
            
            _playersInput = new PlayersInput();
            GameResultLabel.GetComponent<TextMeshProUGUI>().text = String.Format("");
        }

        private void MainLogic_OnNewNextBrick(object sender, Main.EventArgsPlayerData e)
        {
            gui.SetNextBrick(e.PlayerId, e.Value);
        }

        private void MainLogic_OnScoreUpdated(object sender, Main.EventArgsPlayerData e)
        {
            gui.UpdateScore(e.PlayerId, e.Value);
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
