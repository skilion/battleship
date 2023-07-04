namespace Battleship;

public class GameManager
{
    public Grid grid { get; private set; } = new Grid();
    
    private record Ship(int count, int length);

    private readonly Ship[] ships =
    {
        new(1, 5),
        new(2, 4)
    };

    private readonly Random random = new();

    public void NewGame()
    {
        grid.Reset();
        foreach (var ship in ships)
        {
            for (int i = 0; i < ship.count; i++)
            {
                bool placed = false;
                while (!placed)
                {
                    int row = random.Next(0, Grid.rows);
                    int column = random.Next(0, Grid.columns);
                    var direction = (Grid.Direction)random.Next(Enum.GetValues(typeof(Grid.Direction)).Length);
                    placed = grid.PlaceShip(new Position(row, column), ship.length, direction);
                }
            }
        }
    }

    public bool IsGameOver()
    {
        return !grid.IsAnyShipAlive();
    }

    public Grid.ShootResult Shoot(Position position)
    {
        return grid.Shoot(position);
    }
}