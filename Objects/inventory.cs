using System.Collections.Generic;
using System.Data.SqlClient;
using System;

namespace InventoryList
{
  public class Inventory
  {
    private int _id;
    private string _product;
    private int _amount;

    public Inventory(string Product, int Amount, int Id = 0)
    {
      _id = Id;
      _product = Product;
      _amount = Amount;
    }

    //  Alters how the 'Assert.Equal' method works in our InventoryTest file.
    //  argument must match method signature
    public override bool Equals(System.Object otherInventory)
    {
      //  filters only inventory type objects
      if(!(otherInventory is Inventory))
      {
        return false;
      }
      //  once both objects are determined to be of type inventory, compare their 'product' property and return TRUE or FALSE
      else
      {
        Inventory newInventory = (Inventory) otherInventory; // "Gethashcode" exception is thrown upon compiling if this line is absent.
        bool idEquality = (this.GetId() == newInventory.GetId());
        bool sameProduct = (this.GetProduct() == newInventory.GetProduct());
        bool sameAmount = (this.GetAmount() == newInventory.GetAmount());
        return (idEquality && sameProduct && sameAmount);
      }
    }
    public int GetId()
    {
      return _id;
    }
    public int GetAmount()
    {
      return _amount;
    }
    public void SetAmount(int newAmount)
    {
      _amount = newAmount;
    }
    public string GetProduct()
    {
      return _product;
    }
    public void SetProduct(string newProduct)
    {
      _product = newProduct;
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();
      SqlCommand cmd = new SqlCommand("DELETE FROM inventory;", conn);
      cmd.ExecuteNonQuery();
      conn.Close();
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO inventory (product, amount) OUTPUT INSERTED.id VALUES (@Product, @Amount);", conn);

      SqlParameter productParameter = new SqlParameter();
      productParameter.ParameterName = "@Product";
      productParameter.Value = this.GetProduct();
      cmd.Parameters.Add(productParameter);

      SqlParameter amountParameter = new SqlParameter();
      amountParameter.ParameterName = "@Amount";
      amountParameter.Value = this.GetAmount();
      cmd.Parameters.Add(amountParameter);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        this._id = rdr.GetInt32(0);
      }
      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }
    }
    // public void Save()
    // {
    //   SqlConnection conn = DB.Connection();
    //   conn.Open();
    //
    //   SqlCommand cmd = new SqlCommand("INSERT INTO inventory (product, amount) OUTPUT INSERTED.id VALUES (@ProductName, @Amount);", conn);
    //
    //   SqlParameter productParameter = new SqlParameter();
    //   SqlParameter amountParameter = new SqlParameter();
    //   productParameter.ParameterName = "@ProductName";
    //   amountParameter.ParameterName = "@Amount";
    //   productParameter.Value = this.GetProduct();
    //   amountParameter.Value = this.GetAmount();
    //   cmd.Parameters.Add(amountParameter);
    //   cmd.Parameters.Add(productParameter);
    //   SqlDataReader rdr = cmd.ExecuteReader();
    //   while(rdr.Read())
    //   {
    //     this._id = rdr.GetInt32(0);
    //   }
    //   if (rdr != null)
    //   {
    //     rdr.Close();
    //   }
    //   if (conn != null)
    //   {
    //     conn.Close();
    //   }
    // }

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
        int productId = rdr.GetInt32(2);
        string productName = rdr.GetString(0);
        int productAmount = rdr.GetInt32(1);
        //  use above variables to instansiate a new Inventory object.
        Inventory newInventory = new Inventory(productName, productId, productAmount);
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

    public static Inventory Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM inventory WHERE id = @InventoryId;", conn);
      SqlParameter inventoryIdParameter = new SqlParameter();
      inventoryIdParameter.ParameterName = "@InventoryId";
      inventoryIdParameter.Value = id.ToString();
      cmd.Parameters.Add(inventoryIdParameter);
      SqlDataReader rdr = cmd.ExecuteReader();

      int foundInventoryId = 0;
      string foundInventoryProduct = null;
      while(rdr.Read())
      {
        foundInventoryId = rdr.GetInt32(0);
        foundInventoryProduct = rdr.GetString(1);
      }
      Inventory foundInventory = new Inventory(foundInventoryProduct, foundInventoryId);

      if (rdr != null)
      {
        rdr.Close();
      }
      if (conn != null)
      {
        conn.Close();
      }

      return foundInventory;
    }
  }
}
