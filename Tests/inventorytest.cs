using Xunit;
using System.Collections.Generic;
using System;
using System.Data;
using System.Data.SqlClient;

namespace InventoryList
{
  public class InventoryTest
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


  }
}
