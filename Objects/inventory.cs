using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace InventoryList
{
  public class Inventory
  {
    private int _id;
    private string _product;
    private int _count;

    public Inventory(string Product, int Count, int Id = 0)
    {
      _id = Id;
      _product = Product;
      _count = Count;
    }
    public int GetId()
    {
      return _id;
    }
    public int GetCount()
    {
      return _count;
    }
    public void SetCount(int newCount)
    {
      _count = newCount;
    }
    public string GetProduct()
    {
      return _product;
    }
    public void SetProduct(string newProduct)
    {
      _product = newProduct;
    }

    public static List<Inventory> GetAll()
    {
      List<Inventory> allInventory = new List<Inventory>{};
      // Represents the db using the connection info that it was set to.
      SqlConnection conn = DB.Connection();
      //  Opens a connection to db.
      conn.Open();

      //  used to send sql queries to db.
      SqlCommand cmd = new SqlCommand("SELECT * FROM inventory;", conn);  //  this is a sql query.
      //  the return of above sql query is held in rdr. (this is case a table is returned).
      SqlDataReader rdr = cmd.ExecuteReader();

      //
      while(rdr.Read()) //  TRUE if there are more rows; otherwise FALSE.
      {
        //  reads columns 1,2,3 retrieves info then stores it in variables.
        int productId = rdr.GetInt32(0);
        string productName = rdr.GetString(1);
        int productCount = rdr.GetInt32(2);
        //  use above variables to instansiate a new Inventory object.
        Inventory newInventory = new Inventory(productName, productId, productCount);
        //  adds new Inventory object to allInventory list.
        allInventory.Add(newInventory);
      }
      if (rdr != null) 
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return allInventory;
    }
  }
}
