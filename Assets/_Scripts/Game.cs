using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Serialization;

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

        [SerializeField] bool simulationEnabled = false;

        Cell[,] grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];

        private float timer = 0;

        private void Start()
        {
            EventManager.StartListining("SavePattern", SavePattern);
            EventManager.StartListining("LoadPattern", LoadPattern);

            PlaceCells(1);
        }

        private void Update()
        {
            if (simulationEnabled)
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

            UserInput();
        }

        private void LoadPattern()
        {
            string path = "pattern";

            if(!Directory.Exists(path))
            {
                return;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Pattern));

            string patternName = UIManager.instance.LoadDialog.PatternName.options[UIManager.instance.LoadDialog.PatternName.value].text;

            path = path + "/" + patternName + ".xml";

            StreamReader reader = new StreamReader(path);

            Pattern pattern = (Pattern)serializer.Deserialize(reader.BaseStream);
            
            reader.Close();

            bool isAlive;

            int x = 0, y = 0;

            foreach (char c in pattern.patternString)
            {
                if (c.ToString() == "1")
                {
                    isAlive = true;
                }
                else
                {
                    isAlive = false;
                }

                grid[x, y].SetAlive(isAlive);

                x++;

                if(x == SCREEN_WIDTH)
                {
                    x = 0;
                    y++;
                }
            }
        }

        private void SavePattern()
        {
            string path = "pattern";

            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            Pattern pattern = new Pattern();

            string patternString = null;

            for (int y = 0; y < SCREEN_HEIGHT; y++)
            {
                for (int x = 0; x < SCREEN_WIDTH; x++)
                {
                    if (!grid[x, y].isAlive)
                    {
                        patternString += "0";
                    }
                    else
                    {
                        patternString += "1";
                    }
                }
            }

            pattern.patternString = patternString;

            XmlSerializer serializer = new XmlSerializer(typeof(Pattern));

            StreamWriter writer = new StreamWriter(path + "/" + UIManager.instance.SaveDialog.PatternInout.text + ".xml");
            serializer.Serialize(writer.BaseStream, pattern);

            writer.Close();

            Debug.Log(pattern.patternString);
        }

        void UserInput()
        {
            if (!UIManager.instance.IsActive)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                    int x = Mathf.RoundToInt(mousePoint.x);
                    int y = Mathf.RoundToInt(mousePoint.y);

                    if (x >= 0 && y >= 0 && x < SCREEN_WIDTH && y < SCREEN_HEIGHT)
                    {
                        grid[x, y].SetAlive(!grid[x, y].isAlive);
                    }
                }
            }

            if(Input.GetKeyUp(KeyCode.P))
            {
                // Pause Simulation.
                simulationEnabled = false;
            }

            if(Input.GetKeyUp(KeyCode.B))
            {
                // Build Simulation.
                simulationEnabled = true;
            }

            if(Input.GetKeyUp(KeyCode.S))
            {
                // Save Pattern.
                UIManager.instance.ShowSaveDialog();
            }

            if(Input.GetKeyUp(KeyCode.L))
            {
                // Load Pattern.
                UIManager.instance.ShowLoadDialog();
            }
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