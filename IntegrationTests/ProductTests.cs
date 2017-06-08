using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using IntegrationTesting.Controllers;
using IntegrationTesting.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace IntegrationTests
{
  [TestClass]
  public class ProductTests
  {
    private const string ApiProductRoot = "api/product/";
    private IntegrationTestRouter _router = new IntegrationTestRouter();

    [TestInitialize]
    public void TestInitialize()
    {
      _router.Login().Wait();
    }

    [TestMethod]
    public async Task Product_Integral_AddNewProduct_DeleteProduct()
    {
      var product = new Product
      {
        Name = "Second Product"
      };

      var postResult = await _router.Post<Product>(ApiProductRoot, product);
      var id = JsonConvert.DeserializeObject<Guid>(postResult);
      Assert.AreNotEqual<Guid>(id, product.Id);
      product.Id = id;

      var getResult = await _router.Get<Product>(ApiProductRoot, id.ToString());
      Assert.IsNotNull(getResult);
      Assert.AreEqual(product, getResult);

      var deleteResult = await _router.Delete<Product>(ApiProductRoot, id.ToString());
      Assert.AreEqual(string.Empty, deleteResult);
    }

    [TestMethod]
    public async Task Product_Integral_AddNullProduct_BadRequest()
    {
      var postResult = await _router.Post<Product>(ApiProductRoot, null, HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task Product_Integral_GetProductByIncorrectId_BadRequest()
    {
      var getResult = await _router.Get<Product>(ApiProductRoot, "InvalidGuid", HttpStatusCode.BadRequest);
    }

    [TestMethod]
    public async Task Product_Integral_GetProductByNonExistId_NotFound()
    {
      var getResult = await _router.Get<Product>(ApiProductRoot, Guid.NewGuid().ToString(), HttpStatusCode.NotFound);
    }
  }
}
