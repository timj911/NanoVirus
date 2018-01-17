
namespace NanoVirus
{
    public class Cell
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public CellType Type{ get; set; }

        public Cell(int X, int Y, int Z, CellType Type)
        {
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.Type = Type;
        }

        public void Move()
        {

        }
    }
}
