using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace InventoryList
{
  public class InventoryTest : IDisposable
  {
    public InventoryTest()
    {
      //  DATA Source: identifies the server.
      //  INITIAL CATALOG: database name.
      //  INTEGRATED SECURITY: sets the security of the db access to the windows user that is currently logged in.
      DBConfiguration.ConnectionString = "Data Source = (localdb)\\mssqllocaldb;Initial Catalog=inventory_database_test;Integrated Security=SSPI;";
    }

    [Fact]
    public void Test_DatabaseEmptyAtFirst()
    {
      int result = Inventory.GetAll().Count;
      Assert.Equal(0, result);
    }

    [Fact]
    public void Test_Equal_ReturnsTrueIfProductAreTheSame()
    {
      //Arrange, Act
      Inventory firstTask = new Inventory("Pants", 2);
      Inventory secondTask = new Inventory("Pants", 2);
      //Assert
      Assert.Equal(firstTask, secondTask);
    }

    [Fact]
    public void Test_Save_SavesToDatabase()
    {
      //Arrange
      Inventory testInventory = new Inventory("Shirts", 1);

      //Act
      testInventory.Save();
      List<Inventory> result = Inventory.GetAll();
      List<Inventory> testList = new List<Inventory>{testInventory};

      //Assert
      Assert.Equal(testList, result);
    }

    [Fact]
    public void Test_Save_AssignsIdToObject()
    {
      //Arrange
      Inventory testInventory = new Inventory("Shirts",1, 0);

      //Act
      testInventory.Save();
      Inventory savedInventory = Inventory.GetAll()[0];

      int result = savedInventory.GetId();
      int testId = testInventory.GetId();

      //Assert
      Assert.Equal(testId, result);
    }

    [Fact]
    public void Test_Find_FindsTaskInDatabase()
    {
      //Arrange
      Inventory testInventory = new Inventory("Shirts", 1, 0);
      testInventory.Save();

      //Act
      Inventory foundInventory = Inventory.Find(testInventory.GetId());

      //Assert
      Assert.Equal(testInventory, foundInventory);
    }

    public void Dispose()
    {
      Inventory.DeleteAll();
    }
  }
}
