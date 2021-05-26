using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NDRCellularAutomata
{
    public class Game : MonoBehaviour
    {
        public static Game instance;
        private void Awake()
        {
            instance = this;
        }

        private static int SCREEN_WIDTH = 64; // 1024
        private static int SCREEN_HEIGHT = 48; // 728

        [SerializeField] float speed = 0.1f;

        Cell[,] grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];

        private float timer = 0;

        private void Start()
        {
            PlaceCells(1);
        }

        private void Update()
        {
            if (timer >= speed)
            {
                CountNeighbors();
                PopulationControl();
                timer = 0;
            }
            else
                timer += Time.deltaTime;
        }


        void PlaceCells(int type)
        {
            if (type == 1)
            {
                for (int y = 0; y < SCREEN_HEIGHT; y++)
                {
                    for (int x = 0; x < SCREEN_WIDTH; x++)
                    {
                        Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector2(x, y), Quaternion.identity) as Cell;
                        grid[x, y] = cell;
                        grid[x, y].SetAlive(false);
                    }
                }

                for (int y = 21; y < 24; y++)
                {
                    for (int x = 31; x < 38; x++)
                    {
                        if (x != 34)
                        {
                            if (y == 21 || y == 23)
                            {
                                grid[x, y].SetAlive(true);
                            }
                            else if (y == 22 && ((x != 32) && x != 36))
                            {
                                grid[x, y].SetAlive(true);
                            }
                        }
                    }
                }
            }
            else if (type == 2)
            {
                for (int y = 0; y < SCREEN_HEIGHT; y++)
                {
                    for (int x = 0; x < SCREEN_WIDTH; x++)
                    {
                        Cell cell = Instantiate(Resources.Load("Prefabs/Cell", typeof(Cell)), new Vector2(x, y), Quaternion.identity) as Cell;
                        grid[x, y] = cell;
                        grid[x, y].SetAlive(GetRandomAliveCell());
                    }
                }
            }
        }

        bool GetRandomAliveCell()
        {
            int rand = UnityEngine.Random.Range(0, 100);
            if (rand > 75)
                return true;

            return false;
        }

        void CountNeighbors()
        {
            for (int y = 0; y < SCREEN_HEIGHT; y++)
            {
                for (int x = 0; x < SCREEN_WIDTH; x++)
                {
                    int numNeighbors = 0;

                    //- North
                    if(y + 1 < SCREEN_HEIGHT)
                        if (grid[x, y + 1].isAlive)
                            numNeighbors++;

                    //- East
                    if(x+1 < SCREEN_WIDTH)
                        if (grid[x + 1, y].isAlive)
                            numNeighbors++;

                    //- South
                    if (y - 1 >= 0)
                        if (grid[x, y - 1].isAlive)
                            numNeighbors++;

                    //- West
                    if (x - 1 >= 0)
                        if (grid[x - 1, y].isAlive)
                            numNeighbors++;

                    //- North East
                    if (x + 1 < SCREEN_WIDTH && y + 1 < SCREEN_HEIGHT)
                        if (grid[x + 1, y + 1].isAlive)
                            numNeighbors++;

                    //- North West
                    if (x - 1 >= 0 && y + 1 < SCREEN_HEIGHT)
                        if (grid[x - 1, y + 1].isAlive)
                            numNeighbors++;

                    //- South East
                    if (x + 1 < SCREEN_WIDTH && y - 1 >= 0)
                        if (grid[x + 1, y - 1].isAlive)
                            numNeighbors++;

                    //- South West
                    if (x - 1 >= 0 && y - 1 >= 0)
                        if (grid[x - 1, y - 1].isAlive)
                            numNeighbors++;


                    grid[x, y].numNeighbors = numNeighbors;
                }
            }
        }
        void PopulationControl()
        {
            for (int y = 0; y < SCREEN_HEIGHT; y++)
            {
                for (int x = 0; x < SCREEN_WIDTH; x++)
                {
                    //- Rules.
                    // Any Live Cell with two or three live neighbors, survives.
                    // Any dead cell with 3 live neighbors, becomes alive.
                    // All other live cells die in the next Generation and all dead cells stay dead.

                    if(grid[x, y].isAlive)
                    {
                        // Cell is Alive.
                        if(grid[x, y].numNeighbors != 2 && grid[x, y].numNeighbors != 3)
                        {
                            grid[x, y].SetAlive(false);
                        }
                    }
                    else
                    {
                        // Cell is dead.
                        if(grid[x, y].numNeighbors == 3)
                        {
                            grid[x, y].SetAlive(true);
                        }
                    }

                }
            }
        }
    }

   
}