using System.IO;
using Microsoft.AspNet.Builder;
using Nancy.Owin;
using Nancy;
using Nancy.ViewEngines.Razor;
using System.Collections.Generic;

namespace InventoryList
{
  public class Startup
  {
    public void Configure(IApplicationBuilder app)
    {
      app.UseOwin(x => x.UseNancy());
    }
  }
  public class CustomRootPathProvider : IRootPathProvider
  {
    public string GetRootPath()
    {
      return Directory.GetCurrentDirectory();
    }
  }
  public class RazorConfig : IRazorConfiguration
  {
    public IEnumerable<string> GetAssemblyNames()
    {
      return null;
    }

    public IEnumerable<string> GetDefaultNamespaces()
    {
      return null;
    }

    public bool AutoIncludeModelNamespace
    {
      get { return false; }
    }
  }
  public static class DBConfiguration
  {
    //  DATA Source: identifies the server.
    //  INITIAL CATALOG: database name.
    //  INTEGRATED SECURITY: sets the security of the db access to the windows user that is currently logged in.
    public static string ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=inventory_database;Integrated Security=SSPI;";
  }
}
