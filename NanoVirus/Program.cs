using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace NanoVirus
{
    class Program
    {

        static void Main(string[] args)
        {
            List<Cell> HumanCells;
            HumanCells = GenerateCells();

            SimulateVirus(HumanCells, GenerateOutputFile());

            Console.WriteLine("Output file in directory C:\\NanoVirusLog, press any key to close the program");
            Console.ReadKey();
        }

        static Random randomGenerator = new Random();
        static Cell theVirus;
        static int theVirusIndex;
        static int cycleNumber = 0;

        public static void InfectCell(List<Cell> cellList)
        {
            List<int> indexs = new List<int>();
            int count = 0;

            foreach (var item in cellList)
            {
                count++;
                if (item.Type == CellType.Tumorous)
                {
                    indexs.Add(count);
                }
            }

            foreach (var item in indexs)
            {
                int right = item;

                if (++right < cellList.Count)
                {
                    cellList.ToArray()[right].Type = CellType.Tumorous;
                }
                else
                {
                    cellList.ToArray()[item - 1].Type = CellType.Tumorous;
                }
            }
        }

        public static int GetFirstRedCell(List<Cell> theCells)
        {
            return theCells.FindIndex(item => item.Type == CellType.RedBloodCell);
        }

        public static double CalculateDistance(Cell cell1, Cell cell2)
        {
            return Math.Sqrt(Math.Pow(cell1.X - cell2.X, Constants.Squared) +
                             Math.Pow(cell1.Y - cell2.Y, Constants.Squared) +
                             Math.Pow(cell1.Z - cell2.Z, Constants.Squared));
        }

        public static void PrintOutToFile(string fileName, string data)
        {
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine(data);
            }
        }

        public static string GenerateOutputFile()
        {
            string date = DateTime.Now.ToString();

            date = date.Replace(":", "-");

            if (!Directory.Exists("C:\\NanoVirusLog"))
            {
                Directory.CreateDirectory("C:\\NanoVirusLog");
            }

            return string.Format("C:\\NanoVirusLog\\{0}.txt", date);
        }

        public static List<Cell> GenerateCells()
        {
            List<Cell> newCells = new List<Cell>();

            for (int index = 0; index < Constants.NumberOfCells; index++)
            {
                int x = randomGenerator.Next(Constants.MinCellValue, Constants.MaxCellValue);
                int y = randomGenerator.Next(Constants.MinCellValue, Constants.MaxCellValue);
                int z = randomGenerator.Next(Constants.MinCellValue, Constants.MaxCellValue);

                CellType cellType = GetCellType();
                newCells.Add(new Cell(x, y, z, cellType));
            }

            return newCells;
        }

        public static CellType GetCellType()
        {
            double cellLottery = randomGenerator.NextDouble();

            if (cellLottery < Constants.TumorousCellProbability)
            {
                return CellType.Tumorous;
            }
            else if (cellLottery > Constants.TumorousCellProbability && cellLottery < Constants.WhiteBloodCellProbability)
            {
                return CellType.WhiteBloodCell;
            }
            else
            {
                return CellType.RedBloodCell;
            }
        }

        public static void SimulateVirus(List<Cell> theCells, string outputFile)
        {
            theVirusIndex = GetFirstRedCell(theCells);
            theVirus = theCells.ToArray()[theVirusIndex];
            List<Cell> bloodstream = theCells;

            PrintOutToFile(outputFile, string.Format("First red index: {0}", theVirusIndex));

            do
            {

                PrintOutToFile(outputFile, string.Format("Cycle number: {0}", ++cycleNumber));

                int tumorCount = bloodstream.Where(x => x.Type <= CellType.Tumorous).Count();

                if (tumorCount == 0)
                {
                    PrintOutToFile(outputFile, string.Format("End of simulation no more tumorous cells"));
                    break;
                }
                else if (tumorCount == bloodstream.Count)
                {
                    PrintOutToFile(outputFile, string.Format("End of simulation tumorous cells have taken over"));
                    break;
                }

                if (bloodstream.Count <= 2)
                {
                    PrintOutToFile(outputFile, string.Format("End of simulation, all cells dead"));
                    break;
                }

                if (cycleNumber % Constants.InfectCycleNumber == 0)
                {
                    InfectCell(bloodstream);
                    PrintOutToFile(outputFile, "Infecting red blood cells");
                }



                if (theVirus.Type == CellType.Tumorous)
                {
                    PrintOutToFile(outputFile, string.Format("Removing tumorous cell at index: {0}", theVirusIndex));
                    theCells.RemoveAt(theVirusIndex);
                    theVirus.Type = CellType.None;
                }
                else
                {
                    int nextCell = randomGenerator.Next(Constants.MinCellValue, bloodstream.Count);
                    Cell theNextCell = bloodstream.ToArray()[nextCell];
                    double distance = CalculateDistance(theVirus, theNextCell);

                    if (distance < Constants.MaxCellValue)
                    {
                        theVirus = theNextCell;
                        theVirusIndex = nextCell;
                        PrintOutToFile(outputFile, string.Format("Virus is now at index: {0} - {1}", theVirusIndex, theVirus.Type.ToString()));
                    }
                    else
                    {
                        PrintOutToFile(outputFile, string.Format("No move, distance too far"));
                    }
                }

            } while (true);

        }

    }
}
