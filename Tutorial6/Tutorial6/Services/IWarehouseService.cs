using Tutorial6.Models.DTOs;

namespace Tutorial6.Services;

public interface IWarehouseService
{
    public int? AddProduct(OrderDTO order);
    public int? AddProductWithProcedure(OrderDTO order);
}