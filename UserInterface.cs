namespace Battleship;

public class UserInterface
{
    private const string insertCoordinates = "Insert the coordinates for the shot: ";
    private const string invalidCoordinates = "Invalid coordinates";
    private const string miss = "Miss";
    private const string hit = "Hit";
    private const string sink = "Ship sinked";
    private const string gameOver = "Game over, press any key to exit";
    
    private GameManager gameManager = new GameManager();

    public void Run()
    {
        gameManager.NewGame();
        
        while (!gameManager.IsGameOver())
        {
            PrintGrid(gameManager.grid);
            ReadCoordinatesAndShoot();
        }
        
        Console.WriteLine(gameOver);
        Console.ReadKey();
    }

    private void ReadCoordinatesAndShoot()
    {
        Console.Write(insertCoordinates);
        var coordinates = Console.ReadLine();
        
        Position position;
        if (!DecodeCoordinates(coordinates, out position))
        {
            Console.WriteLine(invalidCoordinates);
            return;
        }

        var result = gameManager.Shoot(position);
        switch (result)
        {
            default:
            case Grid.ShootResult.Miss:
                Console.WriteLine(miss);
                break;
            case Grid.ShootResult.Hit:
                Console.WriteLine(hit);
                break;
            case Grid.ShootResult.Sink:
                Console.WriteLine(sink);
                break;
        }
    }

    private bool DecodeCoordinates(string? coordinates, out Position position)
    {
        position = new Position();
        if (string.IsNullOrEmpty(coordinates)) return false;
        if (coordinates.Length < 2) return false;

        position.column = char.ToLower(coordinates[0]) - 'a';
        if (position.column is < 0 or >= Grid.columns) return false;
        
        if (!int.TryParse(coordinates[1..], out int row)) return false;
        position.row = row - 1;
        if (position.row is < 0 or >= Grid.rows) return false;

        return true;
    }

    private void PrintGrid(Grid grid)
    {
        Console.Write("   ");
        for (int column = 0; column < Grid.columns; column++)
        {
            Console.Write($"{(char)('A' + column)} ");
        }
        Console.WriteLine(string.Empty);

        Position position = new();
        for (position.row = 0; position.row < Grid.rows; position.row++)
        {
            Console.Write($"{position.row + 1,2} ");
            for (position.column = 0; position.column < Grid.columns; position.column++)
            {
                int cell = grid[position];
                switch (cell)
                {
                    case Grid.cellShipHit:
                        Console.Write('O');
                        break;
                    case Grid.cellShot:
                        Console.Write('X');
                        break;
                    default:
                        Console.Write('.');
                        break;
                }
                Console.Write(' ');
            }
            Console.WriteLine(string.Empty);
        }
    }
}