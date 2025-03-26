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

        public event Action OnGameEnded;
        public event EventHandler<EventArgsPlayerData> OnScoreUpdated;
        public event EventHandler<EventArgsPlayerData> OnNewNextBrick;

        public void Start()
        {
            Player1Board.OnPlayerLose += PlayerLose;
            Player1Board.OnBrickPlaced += Player1Board_OnBrickPlaced;
            Player2Board.OnBrickPlaced += Player2Board_OnBrickPlaced;
        }

        private void Player1Board_OnBrickPlaced(object sender, Board.EventArgsBrickPlaced e)
        {
            var scoreArgs = new EventArgsPlayerData
            {
                PlayerId = 1,
                Value = e.Scores
            };
            OnScoreUpdated?.Invoke(this, scoreArgs);

            var nextBrickArgs = new EventArgsPlayerData
            {
                PlayerId = 1,
                Value = e.NextBrick
            };
            OnNewNextBrick?.Invoke(this, nextBrickArgs);
        }

        private void Player2Board_OnBrickPlaced(object sender, Board.EventArgsBrickPlaced e)
        {
            var scoreArgs = new EventArgsPlayerData
            {
                PlayerId = 2,
                Value = e.Scores
            };
            OnScoreUpdated?.Invoke(this, scoreArgs);

            var nextBrickArgs = new EventArgsPlayerData
            {
                PlayerId = 2,
                Value = e.NextBrick
            };
            OnNewNextBrick?.Invoke(this, nextBrickArgs);
        }

        

        public void StartGame()
        {

            Player1Board.SpawnBrick();
        }

        public void OnP1Move(int direction)
        {
            Player1Board.MoveBrick(direction);
        }

        public void OnP1Rotate(int direction)
        {
            Player1Board.RotateBrick(direction);
        }

        public void OnP2Move(int direction)
        {
            Player2Board.MoveBrick(direction);
        }

        public void OnP2Rotate(int direction)
        {
            Player2Board.RotateBrick(direction);
        }

        void PlayerLose(object sender, EventArgs _args)
        {
            OnGameEnded?.Invoke();
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
