using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using IntegrationTesting.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace IntegrationTests
{
  public class IntegrationTestRouter
  {
    private TokenResponse _token;
    HttpClient _client = new HttpClient(new LoggingHandler(new HttpClientHandler()));
    Uri _baseUri = new Uri("http://localhost:64046/");

    public async Task Register(string url = "/api/Account/Register", HttpStatusCode statusCode = HttpStatusCode.OK)
    {
      var data = new
      {
        Email = "admin@gmail.com",
        Password = "Password1!",
        ConfirmPassword = "Password1!"
      };

      var uri = new Uri(_baseUri, url);
      var json = JsonConvert.SerializeObject(data);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var response = await _client.PostAsync(uri, content);
      Assert.AreEqual(statusCode, response.StatusCode);
    }

    public async Task Login(string url = "/Token", HttpStatusCode statusCode = HttpStatusCode.OK)
    {
      var data = new Dictionary<string, string>
      {
        { "grant_type", "password" },
        { "username", "admin@gmail.com" },
        { "password", "Password1!" }
      };

      var uri = new Uri(_baseUri, url);
      var content = new FormUrlEncodedContent(data);

      var response = await _client.PostAsync(uri, content);
      if (response.StatusCode == HttpStatusCode.BadRequest)
      {
        await Register();
        await _client.PostAsync(uri, content);
      }

      _token = await response.Content.ReadAsAsync<TokenResponse>();
      _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token.AccessToken);
    }

    public async Task<string> GetHtml(string url, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
      var uri = new Uri(_baseUri, url);

      var response = await _client.GetAsync(uri);
      Assert.AreEqual(statusCode, response.StatusCode);

      var stringResult = response.Content.ReadAsStringAsync().Result;

      return stringResult;
    }

    public async Task<IEnumerable<T>> Get<T>(string url, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
      var uri = new Uri(_baseUri, url);

      var response = await _client.GetAsync(uri);
      Assert.AreEqual(statusCode, response.StatusCode);

      var stringResult = response.Content.ReadAsStringAsync().Result;
      IEnumerable<T> result = JsonConvert.DeserializeObject<IEnumerable<T>>(stringResult);

      return result;
    }

    public async Task<T> Get<T>(string url, string id, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
      var uri = new Uri(_baseUri, url);
      var uriWithId = new Uri(uri, id);

      //// Adding query parameter "id" into Uri - could be used in ASP.NET MVC
      //var builder = new UriBuilder(uri);
      //var query = HttpUtility.ParseQueryString(builder.Query);
      //query["id"] = id;
      //builder.Query = query.ToString();

      var response = await _client.GetAsync(uriWithId);
      Assert.AreEqual(statusCode, response.StatusCode);

      var stringResult = response.Content.ReadAsStringAsync().Result;
      T result = JsonConvert.DeserializeObject<T>(stringResult);

      return result;
    }

    public async Task<string> Post<T>(string url, T model, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
      var uri = new Uri(_baseUri, url);
      var json = JsonConvert.SerializeObject(model);
      var content = new StringContent(json, Encoding.UTF8, "application/json");

      var response = await _client.PostAsync(uri, content);
      Assert.AreEqual(statusCode, response.StatusCode);

      var result = response.Content.ReadAsStringAsync().Result;
      return result;
    }

    public async Task<string> Delete<T>(string url, string id, HttpStatusCode statusCode = HttpStatusCode.NoContent)
    {
      var uri = new Uri(_baseUri, url);
      var uriWithId = new Uri(uri, id);

      var response = await _client.DeleteAsync(uriWithId);
      Assert.AreEqual(statusCode, response.StatusCode);

      var result = response.Content.ReadAsStringAsync().Result;
      return result;
    }
  }
}
