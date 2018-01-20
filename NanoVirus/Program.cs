using System;
using System.Collections.Generic;

namespace NanoVirus
{
    class Program
    {
        static void Main(string[] args)
        {

            //Init variables
            List<Cell> HumanCells = new List<Cell>();
            CellType cellType = CellType.RedBloodCell;
            Cell theVirus = null;

            int theVirusIndex = 0;
            int CycleCount = 0;

            Random r = new Random();

            bool isActive = true;
            bool notRed = true;

            double diceRoll = r.NextDouble();
            double cumulative = 0.0;

            //Random cell type list
            List<KeyValuePair<string, double>> elements = new List<KeyValuePair<string, double>>
            {
                new KeyValuePair<string, double>("T", Constants.TumorousCellProbability),
                new KeyValuePair<string, double>("W", Constants.WhiteBloodCellProbability),
                new KeyValuePair<string, double>("R", Constants.RedBloodCellProbability)
            };

            //Generate list of cells
            for (int index = 0; index < Constants.NumberOfCells; index++)
            {

                //Get random co-ordiante values
                int x = r.Next(Constants.MinCellValue, Constants.MaxCellValue);
                int y = r.Next(Constants.MinCellValue, Constants.MaxCellValue);
                int z = r.Next(Constants.MinCellValue, Constants.MaxCellValue);

                //Get random cell type
                for (int j = 0; j < elements.Count; j++)
                {
                    cumulative += elements[j].Value;
                    if (diceRoll < cumulative)
                    {
                        string selectedElement = elements[j].Key;

                        switch (selectedElement)
                        {
                            case "T":
                                cellType = CellType.Tumorous;
                                break;
                            case "W":
                                cellType = CellType.WhiteBloodCell;
                                break;
                            case "R":
                                cellType = CellType.RedBloodCell;
                                break;
                        }
                    }
                }

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
                    Console.WriteLine("Red index is:" + theVirusIndex);
                    notRed = false;
                }

            } while (notRed);

            //Cycle though
            do
            {
                if (++CycleCount % Constants.InfectCycleNumber == Constants.Zero)
                {
                    Console.WriteLine("Kill: " + CycleCount);

                    if (CycleCount == 60)
                    {
                        break;
                    }

                }

                if (theVirus.Type == CellType.Tumorous)
                {
                    Console.WriteLine("Remove: " + theVirusIndex);
                    HumanCells.RemoveAt(theVirusIndex);

                    int nextCell = r.Next(Constants.MinCellValue, HumanCells.Count);
                    Cell theNextCell = HumanCells.ToArray()[nextCell];
                    double distance = CalculateDistance(theVirus, theNextCell);

                    if (distance < Constants.MaxCellValue)
                    {
                        theVirus = theNextCell;
                        theVirusIndex = nextCell;
                        Console.WriteLine("Virus index is:" + theVirusIndex);
                    }
                    else
                    {
                        Console.WriteLine("No move: " + distance);
                    }

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
                        Console.WriteLine("Virus index is:" + theVirusIndex);
                    }
                    else
                    {
                        Console.WriteLine("No move: " + distance);
                    }
                }

            } while (isActive);

            Console.ReadLine();
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
        }

    }
}
