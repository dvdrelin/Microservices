using Model;

namespace ProductService;

public sealed class ProductService : IProductService
{
    private readonly IList<Product> _products = new List<Product>();
    public void Add(Product product)
    {
        var target = _products.FirstOrDefault(x => x.Id == product.Id);
        if (target is not null)
        {
            throw new Exception($"Product with ID={product.Id} already exists!");
        }
        _products.Add(product);
    }

    public void Update(int id, Product product)
    {
        var target = _products.FirstOrDefault(x => x.Id == id);
        if (target is null)
        {
            throw new Exception($"Product with ID={product.Id} not found!");
        }

        _products.Remove(target);
        _products.Add(product);
    }

    public void Delete(int id)
    {
        var target = _products.FirstOrDefault(x => x.Id == id);
        if (target is null)
        {
            throw new Exception($"Product with ID={id} not found!");
        }

        _products.Remove(target);
    }

    public IEnumerable<Product> Get() => _products;
}