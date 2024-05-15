using Microsoft.AspNetCore.Mvc;
using Tutorial6.Models.DTOs;
using Tutorial6.Services;

namespace Tutorial6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehouseController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public IActionResult Add(OrderDTO order)
    {
        var id = _warehouseService.AddProduct(order);
        if (id == null)
        {
            return BadRequest();
        }

        return Ok(id);
    }
    
    [HttpPost("procedure")]
    public IActionResult AddWithProcedure(OrderDTO order)
    {
        var id = _warehouseService.AddProductWithProcedure(order);
        if (id == null)
        {
            return BadRequest();
        }

        return Ok(id);
    }
    
}