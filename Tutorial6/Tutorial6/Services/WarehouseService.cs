using Tutorial6.Models.DTOs;
using Tutorial6.Repositories;

namespace Tutorial6.Services;

public class WarehouseService : IWarehouseService
{
    private readonly IWarehouseRepository _warehouseRepository;

    public WarehouseService(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    public int? AddProduct(OrderDTO order)
    {
        return _warehouseRepository.AddProduct(order);
    }

    public int? AddProductWithProcedure(OrderDTO order)
    {
        return _warehouseRepository.AddProductWithProcedure(order);
    }
}