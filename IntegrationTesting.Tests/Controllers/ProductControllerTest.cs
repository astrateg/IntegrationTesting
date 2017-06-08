using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IntegrationTesting;
using IntegrationTesting.Controllers;
using IntegrationTesting.Models;
using System.Net;

namespace IntegrationTesting.Tests.Controllers
{
  [TestClass]
  public class ProductControllerTest
  {
    [TestMethod]
    public void Product_AddNewProduct_Verify()
    {
      // Arrange
      ProductController controller = new ProductController();
      var productPost = new Product { Name = "First Product" };

      // Act
      var id = controller.Post(productPost);
      var productGet = controller.Get(id);

      // Assert
      Assert.AreEqual(productGet.Name, productPost.Name);
    }

    [TestMethod]
    public void Product_RenameExistingProduct_Verify()
    {
      // Arrange
      ProductController controller = new ProductController();
      var productPost = new Product { Name = "Second Product" };
      var id = controller.Post(productPost);
      var productGet = controller.Get(id);
      productGet.Name = "Renamed Second Product";

      // Act
      controller.Put(productGet);
      var productUpdated = controller.Get(id);

      // Assert
      Assert.AreEqual(productUpdated.Name, productGet.Name);
    }

    [TestMethod]
    public void Product_DeleteExistingProduct_Verify()
    {
      // Arrange
      ProductController controller = new ProductController();

      // Act
      var productPost = new Product { Name = "Third Product" };
      var id = controller.Post(productPost);

      controller.Delete(id);
      try
      {
        var productGet = controller.Get(id);
      }
        catch (HttpResponseException ex)
      {
        Assert.AreEqual(HttpStatusCode.NotFound, ex.Response.StatusCode);
      }
    }
  }
}
