using Model;

namespace ProductService;

public interface IProductService
{
    void Add(Product product);
    void Update(Guid productId, Product product);
    void Delete(Guid productId);
    IEnumerable<Product> Get();
}