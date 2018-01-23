using System;
using System.IO;
using System.Collections.Generic;

namespace NanoVirus
{
    class Program
    {
        static void Main(string[] args)
        {

            //Init variables
            List<Cell> HumanCells = new List<Cell>();
            Cell theVirus = null;

            int theVirusIndex = 0;
            int CycleCount = 0;

            Random r = new Random();

            bool isActive = true;
            bool notRed = true;

            string date = DateTime.Now.ToString();

            date = date.Replace(":","-");

            if (!Directory.Exists("C:\\NanoVirusLog"))
            {
                Directory.CreateDirectory("C:\\NanoVirusLog");
            }
          
            string fileName = "C:\\NanoVirusLog\\" + date + ".txt";

            //Generate list of cells
            for (int index = 0; index < Constants.NumberOfCells; index++)
            {
                //Get random co-ordiante values
                int x = r.Next(Constants.MinCellValue, Constants.MaxCellValue);
                int y = r.Next(Constants.MinCellValue, Constants.MaxCellValue);
                int z = r.Next(Constants.MinCellValue, Constants.MaxCellValue);

                //Get random cell type
                CellType cellType = GetCellType();
                HumanCells.Add(new Cell(x, y, z, cellType));

            }

            //Get random first red cell
            do
            {
                int first = r.Next(Constants.MinCellValue, Constants.NumberOfCells);

                if (HumanCells.ToArray()[first].Type == CellType.RedBloodCell)
                {
                    theVirus = HumanCells.ToArray()[first];
                    theVirusIndex = first;
                    string data = "First random red index is: " + theVirusIndex;
                    PrintOutToFile(fileName,data);
                    notRed = false;
                }

            } while (notRed);

            //Cycle though
            do
            {

                if (++CycleCount % Constants.InfectCycleNumber == Constants.Zero)
                {

                    List<int> cancer = new List<int>();
                    List<int> angryCells = new List<int>();

                    for (int i = 0; i < HumanCells.Count; i++)
                    {
                        if (HumanCells.ToArray()[i].Type == CellType.Tumorous)
                            cancer.Add(i);

                        if (HumanCells.ToArray()[i].Type == CellType.RedBloodCell)
                            angryCells.Add(i);
                    }

                    if (cancer.Count == Constants.Zero)
                    {
                        string endGame = "End of game, no more tumorous cells";
                        PrintOutToFile(fileName, endGame);
                        break;
                    }

                    if (angryCells.Count == 100)
                    {
                        string endGame = "End of game, red blood cell count = 100";
                        PrintOutToFile(fileName, endGame);
                        break;
                    }

                }

                string cycledata =  "Cycle number: " + CycleCount;
                PrintOutToFile(fileName, cycledata);

                if (theVirus.Type == CellType.Tumorous)
                {
                    string removingInfo = "Removing tumorous at: " + theVirusIndex;
                    PrintOutToFile(fileName, removingInfo);
                    HumanCells.RemoveAt(theVirusIndex);
                    theVirus.Type = CellType.RedBloodCell;
                }
                else
                {
                    int nextCell = r.Next(Constants.MinCellValue, HumanCells.Count);
                    Cell theNextCell = HumanCells.ToArray()[nextCell];
                    double distance = CalculateDistance(theVirus, theNextCell);

                    if (distance < Constants.MaxCellValue)
                    {
                        theVirus = theNextCell;
                        theVirusIndex = nextCell;
                        string moveInfo = "Virus index is: " + theVirusIndex + " - " + theVirus.Type.ToString();
                        PrintOutToFile(fileName, moveInfo);
                    }
                    else
                    {
                        string noMoveData = "No move, distance too far";
                        PrintOutToFile(fileName, noMoveData);
                    }
                }

            } while (isActive);

            Console.WriteLine("Output file in directory C:\\NanoVirusLog, press enter to close the program");
            Console.Read();
        }
        //End Program

        public static double CalculateDistance(Cell cell1, Cell cell2)
        {
            return Math.Sqrt(Math.Pow(cell1.X - cell2.X, Constants.Squred) +
                             Math.Pow(cell1.Y - cell2.Y, Constants.Squred) +
                             Math.Pow(cell1.Z - cell2.Z, Constants.Squred));
        }

        public static void InfectCell(Cell cell)
        {
            cell.Type = CellType.Tumorous;
            Console.WriteLine("infected");
        }

        public static CellType GetCellType()
        {
            Random r = new Random();
            double diceRoll = r.NextDouble();
            double cumulative = 0.0;

            //Random cell type list
            List<KeyValuePair<string, double>> elements = new List<KeyValuePair<string, double>>
            {
                new KeyValuePair<string, double>("T", Constants.TumorousCellProbability),
                new KeyValuePair<string, double>("W", Constants.WhiteBloodCellProbability),
                new KeyValuePair<string, double>("R", Constants.RedBloodCellProbability)
            };

            for (int j = 0; j < elements.Count; j++)
            {
                cumulative += elements[j].Value;
                if (diceRoll < cumulative)
                {
                    string selectedElement = elements[j].Key;

                    switch (selectedElement)
                    {
                        case "T":
                            return CellType.Tumorous;
                        case "W":
                            return CellType.WhiteBloodCell;
                        case "R":
                            return CellType.RedBloodCell;
                    }
                }
            }
            return CellType.Tumorous;
        }

        public static void PrintOutToFile(string fileName, string data)
        {
            using (StreamWriter writer =  new StreamWriter(fileName, true))
            {
                writer.WriteLine(data);
            }
        }

    }
}
