using Model;

namespace ProductService;

public sealed class ProductInMemoryService : IProductService
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

    public void Update(Guid productId, Product product)
    {
        var target = _products.FirstOrDefault(x => x.Id == productId);
        if (target is null)
        {
            throw new Exception($"Product with ID={product.Id} not found!");
        }

        _products.Remove(target);
        _products.Add(product);
    }

    public void Delete(Guid productId)
    {
        var target = _products.FirstOrDefault(x => x.Id == productId);
        if (target is null)
        {
            throw new Exception($"Product with ID={productId} not found!");
        }

        _products.Remove(target);
    }

    public IEnumerable<Product> Get() => _products;
}