using System;
using UnityEngine;

namespace Logic
{
    public class Board : MonoBehaviour
    {
        public GameObject Square;
        public GameObject SquareContainer;
        public GameObject BoardBackground;
        public Brick brick;
        public int Width = 10;
        public int Height = 10;
        public float CellSize = 1f;

        private int[,] table;


        private void Start()
        {
            InitTable();
            SpawnBrick();
            table[1, 1] = 1;
            UpdateCells();
        }

        public void InitTable()
        {
            BoardBackground.transform.localScale = new Vector3(Width, Height, 1);
            table = new int[Width, Height];
            for (int j = 0; j < Height; j++)
            {
                for (int i = 0; i < Width; i++)
                {
                    table[i, j] = 0;
                    GameObject obj = Instantiate(Square, GetPositionFromIndex(i,j), Quaternion.identity);
                    obj.SetActive(false);
                    obj.transform.SetParent(SquareContainer.transform);
                }
            }
        }
        public Vector2 RoundPosition(Vector3 position)
        {
            Vector2 newPosition = new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
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

        public void SpawnBrick()
        {
            Brick newBrick = Instantiate(brick, new Vector3(3.5f,2,-1), Quaternion.identity);
            newBrick.OnStopped += PlaceBrick;
            newBrick.transform.SetParent(transform);
        }

        public void PlaceBrick(Brick brick)
        {
            brick.gameObject.SetActive(false);
            Debug.Log(brick.transform.childCount);
            for (int i = 0; i < brick.transform.childCount; i++)
            {
                Transform square = brick.transform.GetChild(i);
                Vector2Int index = GetIndexFromPosition(square.position);
                Debug.Log(index);
                table[index.x, index.y] = 1;
            }
            Debug.Log("PlaceBrick");
            UpdateCells();
            Destroy(brick);
        }

        
    }
}
