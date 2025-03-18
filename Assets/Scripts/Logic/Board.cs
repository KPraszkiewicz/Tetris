using UnityEngine;

namespace Logic
{
    public class Board : MonoBehaviour
    {
        public int Width = 10;
        public int Height = 10;

        private int[,] table;


        private void Start()
        {
            table = new int[Width, Height];
        }

        public void InitTable()
        {
            table = new int[Width, Height];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    table[i, j] = 0;
                }
            }
        }

        public void PlaceBrick(Brick brick)
        {
            brick.name = "XDD";
        }

        
    }
}
