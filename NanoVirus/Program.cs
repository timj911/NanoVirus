using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace NanoVirus
{
    class Program
    {
        static Random randomGenerator = new Random();

        static void Main(string[] args)
        {
            List<Cell> HumanCells;

            //Generate list of cells
            HumanCells = GenerateCells();


            foreach (var item in HumanCells)
            {
                Console.WriteLine("X-{0} Y-{1} Z-{2} C-{3}", item.X, item.Y, item.Z, item.Type);
            }

            //Get random first red cell
            GetFirstRedCell(HumanCells);

            //Cycle though

            Console.WriteLine("Output file in directory C:\\NanoVirusLog, press any key to close the program");
            Console.ReadKey();
        }
        //End Program

        public static List<Cell> GenerateCells()
        {

            List<Cell> Cells = new List<Cell>();

            for (int index = 0; index < Constants.NumberOfCells; index++)
            {
                //Get random co-ordiante values
                int x = randomGenerator.Next(Constants.MinCellValue, Constants.MaxCellValue);
                int y = randomGenerator.Next(Constants.MinCellValue, Constants.MaxCellValue);
                int z = randomGenerator.Next(Constants.MinCellValue, Constants.MaxCellValue);

                //Get random cell type
                CellType cellType = GetCellType();
                Cells.Add(new Cell(x, y, z, cellType));
            }

            return Cells;
        }

        public static void GetFirstRedCell(List<Cell> cells)
        {

            Console.WriteLine("--------------");

            var numQuery = from num in cells
                           where (num.Type == CellType.RedBloodCell)
                           select num;

            foreach (var item in numQuery)
            {
                Console.WriteLine("X-{0} Y-{1} Z-{2} C-{3}", item.X, item.Y, item.Z, item.Type);
            }

            Console.WriteLine("--------------");

            int first = randomGenerator.Next(Constants.MinCellValue, Constants.NumberOfCells);

        }

        public static double CalculateDistance(Cell cell1, Cell cell2)
        {
            return Math.Sqrt(Math.Pow(cell1.X - cell2.X, Constants.Squared) +
                             Math.Pow(cell1.Y - cell2.Y, Constants.Squared) +
                             Math.Pow(cell1.Z - cell2.Z, Constants.Squared));
        }

        public static void InfectCell(Cell cell)
        {
            cell.Type = CellType.Tumorous;
        }

        public static CellType GetCellType()
        {

            double diceRoll = randomGenerator.NextDouble();
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
            using (StreamWriter writer = new StreamWriter(fileName, true))
            {
                writer.WriteLine(data);
            }
        }

    }
}
