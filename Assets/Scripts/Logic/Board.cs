using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

namespace Logic
{
    public class Board : MonoBehaviour
    {
        public GameObject Square;
        public GameObject SquareContainer;
        public GameObject BoardBackground;
        public GameObject SpawnPoint;

        public Brick[] BrickVariats;

        public Brick brick;
        public Brick NextBrick;
        public int Score = 0;

        public int Width = 10;
        public int Height = 10;
        public float CellSize = 1f;
        public float BrickSpeed = 5f;

        private int[,] table;

        bool gameRunning = true;

        public struct EventArgsBrickPlaced
        {
            public int Scores;
            public int NextBrick;
        }

        public event EventHandler OnPlayerLose;
        public event EventHandler<EventArgsBrickPlaced> OnBrickPlaced;

        private void Start()
        {
            InitTable();
            UpdateCells();
        }

        public void InitTable()
        {
            BoardBackground.transform.localScale = new Vector3(Width, Height + 2*CellSize, 1);
            table = new int[Width, Height];
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    table[i, j] = 0;
                    GameObject obj = Instantiate(Square);  
                    obj.SetActive(false);
                    obj.transform.SetParent(SquareContainer.transform);
                    obj.transform.localPosition = GetPositionFromIndex(i, j);
                }
            }
            SpawnPoint.transform.localPosition = new Vector3(Width / 2, Height, -1);
        }
        public static Vector3 RoundPosition(Vector3 position)
        {
            Vector3 newPosition = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), position.z);
            return newPosition;
        }

        public Vector2Int GetIndexFromPosition(Vector3 position)
        {
            int indexX = Mathf.RoundToInt((position.x - 0.5f) / CellSize);
            int indexY = Mathf.RoundToInt((position.y - 0.5f) / CellSize);
            return new Vector2Int(indexX, indexY);
        }

        public Vector3 GetPositionFromIndex(int x, int y)
        {
            float pX = x * CellSize + 0.5f;
            float pY = y * CellSize + 0.5f;

            return new Vector3(pX, pY, 0);
        }

        public void UpdateCells()
        {
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    SquareContainer.transform.GetChild(j * Width + i).gameObject.SetActive(table[i, j] != 0);
                }
            }
        }

        public void AddSquare(Vector2 position)
        {
            Vector2Int index = GetIndexFromPosition(position);
            table[index.x, index.y] = 1;
        }

        public int SpawnBrick()
        {
            if (!gameRunning)
                return 0;

            Debug.Log("SPAWN");
            int brickVariant = UnityEngine.Random.Range(0, BrickVariats.Length);

            Brick newBrick = Instantiate(BrickVariats[brickVariant], SpawnPoint.transform.position, Quaternion.identity);
            newBrick.OnStopped += PlaceBrick;
            newBrick.transform.SetParent(transform);
            newBrick.StartMoving(BrickSpeed);

            return brickVariant;
        }

        public void DeleteLine(int index)
        {
            for (int i = 0; i < Width; i++)
            {
                table[i, index] = 0;
            }
        }

        public void ReplaceLine(int from, int to)
        {
            for (int i = 0; i < Width; i++)
            {
                table[i, to] = table[i, from];
            }
        }

        public void CheckLines()
        {
            int deletedLines = 0;
            for (int j = 0; j < Height; j++)
            {
                int product = 1;
                for (int i = 0; i < Width; i++)
                {
                    if (table[i, j] == 0)
                    {
                        product = 0;
                        break;
                    }
                }

                if (product == 1)
                {
                    deletedLines += 1;
                }
                if (deletedLines > 0)
                {
                    if (j + deletedLines < Height)
                    {
                        ReplaceLine(j, j + deletedLines);
                    }
                    else
                    {
                        DeleteLine(j);
                    }
                }
            }
            if (deletedLines > 0)
            {
                UpdateCells();
                Score += deletedLines * 100;
            }
        }

        public void PlaceBrick(Brick brick)
        {
            int scoreBefore = Score;

            brick.gameObject.SetActive(false);
            Debug.Log(brick.transform.childCount);
            for (int i = 0; i < brick.transform.childCount; i++)
            {
                Transform square = brick.transform.GetChild(i);
                Vector2Int index = GetIndexFromPosition(brick.transform.localPosition + square.localPosition); // <- do zmiany, gloabal position jest nieodpowiednie TODO
                Debug.Log(index);
                if(index.y >= Height)
                {
                    OnPlayerLose?.Invoke(this, EventArgs.Empty);
                    gameRunning = false;
                    Debug.Log("Gracz przegra³");
                    return;
                }
                else
                {
                    table[index.x, index.y] = 1;
                }
            }

            UpdateCells();
            Destroy(brick);

            EventArgsBrickPlaced args =  new EventArgsBrickPlaced();
            args.Scores = Score - scoreBefore;
            args.NextBrick = SpawnBrick();  
            OnBrickPlaced?.Invoke(this, args);

        }

        
    }
}
