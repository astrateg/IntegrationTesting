using System;

namespace IntegrationTesting.Models
{
  public class Product
  {
    public Guid Id { get; set; }

    public string Name { get; set; }

    public override bool Equals(object obj)
    {
      Product other = obj as Product;
      if (other == null)
      {
        return false;
      }

      return (this.Name == other.Name) && (this.Id == other.Id);
    }

    public override int GetHashCode()
    {
      unchecked // Overflow is fine, just wrap 
      {
        int hash = 17;
        hash = hash * 23 + Id.GetHashCode();
        hash = hash * 23 + Name.GetHashCode();
        return hash;
      }
    }
  }
}