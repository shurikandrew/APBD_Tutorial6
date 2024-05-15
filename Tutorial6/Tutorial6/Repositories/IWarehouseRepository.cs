using Tutorial6.Models.DTOs;

namespace Tutorial6.Repositories;

public interface IWarehouseRepository
{
    public int? AddProduct(OrderDTO order);
    public int? AddProductWithProcedure(OrderDTO order);
}