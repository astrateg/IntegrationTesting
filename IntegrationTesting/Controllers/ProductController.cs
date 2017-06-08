using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using IntegrationTesting.Models;

namespace IntegrationTesting.Controllers
{
  [Authorize]
  public class ProductController : ApiController
  {
    // 79791EB2-4C25-469E-B203-572A20A7D81A
    private static List<Product> _products = new List<Product>();

    //public static Guid InitialProductGuid {
    //  get {
    //    return new Guid("79791EB2-4C25-469E-B203-572A20A7D81A");
    //  }
    //}

    // GET api/product
    [Route("api/product/")]
    [HttpGet]
    public IEnumerable<Product> Get()
    {
      return _products;
    }

    // GET api/product/79791EB2-4C25-469E-B203-572A20A7D81A
    [Route("api/product/{id}")]
    [HttpGet]
    public Product Get(Guid id)
    {
      if (id == Guid.Empty)
      {
        throw new HttpResponseException(HttpStatusCode.BadRequest);
      }

      var product = _products.FirstOrDefault(p => p.Id == id);
      if (product == null)
      {
        throw new HttpResponseException(HttpStatusCode.NotFound);
      }

      return product;
    }

    // POST api/product
    [Route("api/product/")]
    [HttpPost]
    public Guid Post([FromBody]Product product)
    {
      if (product == null)
      {
        throw new HttpResponseException(HttpStatusCode.BadRequest);
      }

      product.Id = Guid.NewGuid();
      _products.Add(product);

      return product.Id;
    }

    // PUT api/product/
    [Route("api/product/")]
    [HttpPut]
    public void Put([FromBody]Product product)
    {
      if (product == null)
      {
        throw new HttpResponseException(HttpStatusCode.BadRequest);
      }

      var productToUpdate = _products.FirstOrDefault(p => p.Id == product.Id);
      if (product == null)
      {
        throw new HttpResponseException(HttpStatusCode.NotFound);
      }

      productToUpdate.Name = product.Name;
    }

    // DELETE api/product/79791EB2-4C25-469E-B203-572A20A7D81A
    [Route("api/product/{id}")]
    [HttpDelete]
    public void Delete(Guid id)
    {
      if (id == Guid.Empty)
      {
        throw new HttpResponseException(HttpStatusCode.BadRequest);
      }

      var product = _products.FirstOrDefault(p => p.Id == id);
      if (product == null)
      {
        throw new HttpResponseException(HttpStatusCode.NotFound);
      }

      _products.Remove(product);
    }
  }
}
