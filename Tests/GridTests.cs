using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Battleship.Tests;

[TestClass]
public class GridTests
{
    private Grid grid = new();

    [TestMethod]
    public void PlaceShip_ValidPosition_ReturnsTrue()
    {
        bool result = grid.PlaceShip(new Position(0, 0), 5, Grid.Direction.Horizontal);
        Assert.IsTrue(result);
    }
    
    [TestMethod]
    public void PlaceShip_ValidPosition_ActuallyPlacesTheShip()
    {
        grid.PlaceShip(new Position(0, 0), 3, Grid.Direction.Horizontal);
        Assert.IsTrue(grid[new Position(0, 0)] == 1);
        Assert.IsTrue(grid[new Position(0, 1)] == 1);
        Assert.IsTrue(grid[new Position(0, 2)] == 1);
    }
    
    [TestMethod]
    public void PlaceShip_InvalidPosition_ReturnsFalse()
    {
        bool result = grid.PlaceShip(new Position(7, 0), 5, Grid.Direction.Vertical);
        Assert.IsFalse(result);
    }
    
    [TestMethod]
    public void PlaceShip_OverlapExistingShip_ReturnsFalse()
    {
        grid.PlaceShip(new Position(3, 0), 5, Grid.Direction.Horizontal);
        bool result = grid.PlaceShip(new Position(0, 3), 5, Grid.Direction.Vertical);
        Assert.IsFalse(result);
    }
    
    [TestMethod]
    public void IsAnyShipAlive_WithNoShips_ReturnsFalse()
    {
        bool result = grid.IsAnyShipAlive();
        Assert.IsFalse(result);
    }
    
    [TestMethod]
    public void IsAnyShipAlive_WithOneShip_ReturnsTrue()
    {
        grid.PlaceShip(new Position(0, 0), 5, Grid.Direction.Horizontal);
        bool result = grid.IsAnyShipAlive();
        Assert.IsTrue(result);
    }
    
    [TestMethod]
    public void Shoot_ShipMiss_ReturnsMiss()
    {
        var position = new Position(0, 0);
        var result = grid.Shoot(position);
        Assert.IsTrue(result == Grid.ShootResult.Miss);
        Assert.IsTrue(grid[position] == Grid.cellShot);
    }
    
    [TestMethod]
    public void Shoot_ShipHit_ReturnsHit()
    {
        var position = new Position(0, 0);
        grid.PlaceShip(position, 5, Grid.Direction.Horizontal);
        var result = grid.Shoot(position);
        Assert.IsTrue(result == Grid.ShootResult.Hit);
        Assert.IsTrue(grid[position] == Grid.cellShipHit);
    }
    
    [TestMethod]
    public void Shoot_ShipSinked_ReturnsSink()
    {
        var position = new Position(0, 0);
        grid.PlaceShip(position, 3, Grid.Direction.Horizontal);
        
        grid.Shoot(position);
        position.column++;
        grid.Shoot(position);
        position.column++;
        var result = grid.Shoot(position);
        
        Assert.IsTrue(result == Grid.ShootResult.Sink);
    }
}