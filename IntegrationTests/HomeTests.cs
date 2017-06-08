using System.Net;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using System.Xml.Linq;
using AngleSharp.Parser.Html;

namespace IntegrationTests
{
  [TestClass]
  public class HomeTests
  {
    private IntegrationTestRouter _router = new IntegrationTestRouter();

    [TestInitialize]
    public void TestInitialize()
    {
    }

    [TestMethod]
    public async Task Home_Integral_GetIndexPage()
    {
      await _router.Login();
      var getResult = await _router.GetHtml("home/", HttpStatusCode.OK);

      var parser = new HtmlParser();
      var document = parser.Parse(getResult);
      Assert.IsNotNull(document);

      var title = document.QuerySelector("title").TextContent;
      Assert.AreEqual(title, "Home Page");

      var menuLinkHome = document.QuerySelector("ul.navbar-nav li:first-child a");
      Assert.IsNotNull(menuLinkHome);
      Assert.AreEqual(menuLinkHome.TextContent, "Home");

      var menuLinkApi = document.QuerySelector("ul.navbar-nav li:nth-child(2) a");
      Assert.IsNotNull(menuLinkApi);
      Assert.AreEqual(menuLinkApi.TextContent, "API");

      var menuLinkApp = document.QuerySelector("div.navbar-header a.navbar-brand");
      Assert.IsNotNull(menuLinkApp);
      Assert.AreEqual(menuLinkApp.TextContent, "Application name");
    }
  }
}
