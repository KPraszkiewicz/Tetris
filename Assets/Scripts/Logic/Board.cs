using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Codice.Client.BaseCommands.BranchExplorer;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
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

            // pseudo unit tests

            table[0, 0] = 1;
            table[0, 1] = 1;
            table[0, 2] = 1;
            table[0, 3] = 1;
            table[0, 4] = 1;
            table[0, 5] = 1;
            table[0, 6] = 1;
            table[0, 7] = 1;
            table[0, 8] = 1;
            table[0, 9] = 1;

            table[1, 4] = 1;
            table[1, 5] = 1;
            table[1, 6] = 1;
            table[1, 7] = 1;

            for (int i = 0; i < 10; i++)
            {
                table[i, 0] = 1;
            }
            for (int i = 0; i < 10; i++)
            {
                table[i, 1] = 1;
            }
            CheckLines();

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
            SpawnPoint.transform.localPosition = new Vector3(Width / 2 + 0.5f, Height, -1);
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
            brick = newBrick;

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
                table[i, from] = 0;
            }
        }

        public bool LineIsEmpty(int index)
        {
            if (index >= Height)
            {
                return true;
            }
            for (int i = 0; i < Width; i++)
            {
                if (table[i, index] == 1)
                {
                    return false;
                }
            }
            return true;
        }

        public void CheckLines()
        {
            int deletedLines = 0;
            Queue<int> fullLines = new();

            for (int j = 0; j < Height; j++)
            {
                bool isFullLine = true;
                for (int i = 0; i < Width; i++)
                {
                    if (table[i, j] == 0)
                    {
                        isFullLine = false;
                        break;
                    }
                }
                if (isFullLine)
                {
                    fullLines.Enqueue(j);
                    DeleteLine(j);
                }
            }
            // TODOL wait 100 ms

            for (int j = 0; j < Height; j++)
            {
                int fromLine = deletedLines + j;

                while (fullLines.Count > 0 && fullLines.Peek() <= fromLine)
                {
                    deletedLines += 1;
                    fromLine += 1;
                    fullLines.Dequeue();
                }

                if (deletedLines > 0)
                {
                    if (fromLine >= Height)
                    {
                        DeleteLine(j);
                    }
                    else
                    {
                        ReplaceLine(deletedLines + j, j);
                    }
                }
            }


            if (deletedLines > 0)
            {
                UpdateCells();
                Score += deletedLines * 100;
            }
        }

        public void PlaceBrick()
        {
            int scoreBefore = Score;
            bool canBePlaced = false;

            Vector2Int[] indexes = brickToIndexes(brick);

            int minHeight = Height;
            int minX = Width;
            int maxX = -1;

            foreach (var index in indexes)
            { 
                if (minX > index.x) minX = index.x;
                if (maxX < index.x) maxX = index.x;
                if (minHeight > index.y) minHeight = index.y;

                if (index.y < Height && table[index.x, index.y] == 1)
                {
                    return;
                }
            }

            if (minHeight > 0)
            {
                for (int i = minX; i <= maxX; i++)
                {
                    if (table[i, minHeight - 1] == 1)
                    {
                        canBePlaced = true;
                        break;
                    }
                }

                if (!canBePlaced)
                {
                    return;
                }
            }

            foreach (var index  in indexes)
            {
                if (index.y >= Height)
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

            CheckLines();
            UpdateCells();
            brick.gameObject.SetActive(false);
            brick.OnStopped -= PlaceBrick;
            Destroy(brick.gameObject);

            EventArgsBrickPlaced args = new()
            {
                Scores = Score - scoreBefore,
                NextBrick = SpawnBrick()
            };

            OnBrickPlaced?.Invoke(this, args);

        }

        Vector2Int[] brickToIndexes(Brick brick)
        {
            Vector2Int[] indexes = new Vector2Int[brick.transform.childCount];

            for (int i = 0; i < indexes.Length; i++)
            {
                Transform square = brick.transform.GetChild(i);
                indexes[i] = GetIndexFromPosition(brick.transform.localPosition + brick.transform.rotation * square.localPosition);
            }

            return indexes;
        }
         
        // controls
        public bool MoveBrick(int direction)
        {

            if(brick)
            {
                Vector2Int[] indexes = brickToIndexes(brick);

                foreach (var index in indexes)
                {
                    int newX = index.x + direction;

                    if (newX < 0 || newX >= Width)
                    {
                        return false;
                    }

                    if (index.y < Height && table[newX, index.y] == 1)
                    {
                        return false;
                    }
                }

                brick.GetComponent<Rigidbody2D>().position += new Vector2(direction * CellSize, 0);
                return true;
            }
            return false;
        }

        public bool RotateBrick(int direction)
        {
            if(direction == 1) { }
            // TODO: ADD ROTATION: https://tetris.wiki/Super_Rotation_System
            //if (brick)
            //{
            //    var previousRotation = brick.transform.rotation;
            //    brick.transform.rotation = brick.transform.rotation * Quaternion.Euler(Vector3.forward * 90 * direction);

            //    Vector2Int[] indexes = brickToIndexes(brick);

            //    foreach (var index in indexes)
            //    {
            //        int newX = index.x + direction;

            //        if (newX < 0 || newX >= Width)
            //        {
            //            brick.transform.rotation = previousRotation;
            //            return false;
            //        }

            //        if (index.y < Height && table[newX, index.y] == 1)
            //        {
            //            brick.transform.rotation = previousRotation;
            //            return false;
            //        }
            //    }


            //    brick.GetComponent<Rigidbody2D>().rotation += 90*direction;
            //    return true;
            //}
            return false;
        }

    }
}
