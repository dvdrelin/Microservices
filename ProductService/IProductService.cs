using Model;

namespace ProductService;

public interface IProductService
{
    void Add(Product product);
    void Update(int id, Product product);
    void Delete(int id);
    IEnumerable<Product> Get();
}