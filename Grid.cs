namespace Battleship;

public class Grid
{
    public enum Direction
    {
        Vertical,
        Horizontal
    }

    public enum ShootResult
    {
        Miss,
        Hit,
        Sink
    }

    public const int rows = 10;
    public const int columns = 10;
    
    public const int cellShipHit = -2;
    public const int cellShot = -1;
    public const int cellEmpty = 0;
    
    private int[,] grid = new int[rows, columns];
    private int shipCount;

    public int this[Position position]
    {
        get => grid[position.row, position.column];
        private set => grid[position.row, position.column] = value;
    }

    public void Reset()
    {
        Array.Clear(grid);
    }

    public bool PlaceShip(Position position, int length, Direction direction)
    {
        var gridBackup = grid.Clone() as int[,];
        int shipId = shipCount + 1;

        int placed = 0;
        while (placed < length)
        {
            if (!IsPositionValid(position) || (this[position] != cellEmpty))
            {
                break;
            }

            this[position] = shipId;

            switch (direction)
            {
                default:
                case Direction.Vertical:
                    position.row++;
                    break;
                case Direction.Horizontal:
                    position.column++;
                    break;
            }

            placed++;
        }

        if (placed != length)
        {
            grid = gridBackup;
            return false;
        }

        shipCount++;
        return true;
    }

    public static bool IsPositionValid(Position position) => position.row is >= 0 and < rows && position.column is >= 0 and < columns;

    public ShootResult Shoot(Position position)
    {
        int cell = this[position];
        if (cell > 0)
        {
            this[position] = cellShipHit;
            if (IsShipSinked(cell))
            {
                return ShootResult.Sink;
            }

            return ShootResult.Hit;
        }
        
        
        this[position] = cellShot;
        return ShootResult.Miss;
    }
    
    public bool IsAnyShipAlive()
    {
        return IsAny(cell => cell > cellEmpty);
    }

    private bool IsShipSinked(int shipId)
    {
        return !IsAny(cell => cell == shipId);
    }

    private bool IsAny(Predicate<int> predicate)
    {
        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                if (predicate(grid[row, column]))
                {
                    return true;
                }
            }
        }

        return false;
    }
}