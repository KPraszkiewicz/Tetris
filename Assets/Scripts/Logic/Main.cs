using System;
using UnityEngine;

namespace Logic
{
    public class Main : MonoBehaviour
    {
        public Board Player1Board;
        public Board Player2Board;

        public struct EventArgsPlayerData
        {
            public int PlayerId;
            public int Value;
        }

        public event EventHandler OnGameEnded;
        public event EventHandler<EventArgsPlayerData> OnScoreUpdated;
        public event EventHandler<EventArgsPlayerData> OnNewNextBrick;

        public void StartGame()
        {
            Player1Board.SpawnBrick();
        }


        ///////////////////////////////////////////////
        private static Main _instance = null;
        public static Main Instance
        {
            get { return _instance; }
        }
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                _instance = this;
            }

            DontDestroyOnLoad(this.gameObject);
        }
    }
}
