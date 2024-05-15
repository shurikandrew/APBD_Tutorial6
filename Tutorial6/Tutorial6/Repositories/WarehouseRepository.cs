using System.Data;
using System.Data.SqlClient;
using Tutorial6.Models.DTOs;

namespace Tutorial6.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public int? AddProduct(OrderDTO order)
    {
        using SqlConnection connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using SqlCommand command = new SqlCommand();
        connection.Open();
        command.Connection = connection;
        
        if (order.Amount <= 0)
        {
            return null;
        }
        
        
        
        command.CommandText = "SELECT Price " +
                              "FROM Product " +
                              "WHERE IdProduct = @IdCheckProduct";
        command.Parameters.AddWithValue("@IdCheckProduct", order.IdProduct);
        var reader = command.ExecuteReader();
        
        if (!reader.HasRows) return null;
        reader.Read();
        var price = (decimal)reader["Price"]; 
        reader.Close();
        
        
        
        command.CommandText = "SELECT * " +
                              "FROM Warehouse " +
                              "WHERE IdWarehouse = @IdCheckWarehouse";
        command.Parameters.AddWithValue("@IdCheckWarehouse", order.IdWarehouse);
        reader = command.ExecuteReader();
        
        if (!reader.HasRows) return null;
        reader.Close();
        
        
        
        command.CommandText = "SELECT IdOrder " +
                              "FROM [Order] " +
                              "WHERE IdProduct = @IdCheckOrder AND Amount = @AmountCheckOrder AND CreatedAt < @CreatedAtCheckOrder";
        command.Parameters.AddWithValue("@IdCheckOrder", order.IdProduct);
        command.Parameters.AddWithValue("@AmountCheckOrder", order.Amount);
        command.Parameters.AddWithValue("@CreatedAtCheckOrder", order.CreatedAt);
        reader = command.ExecuteReader();

        if (!reader.HasRows) return null;
        reader.Read();
        var orderId = (int)reader["IdOrder"];
        reader.Close();
        
        
        
        command.CommandText = "SELECT * " +
                              "FROM Product_Warehouse " +
                              "WHERE IdOrder = @IdCheckProductWarehouse";
        command.Parameters.AddWithValue("@IdCheckProductWarehouse", orderId);
        reader = command.ExecuteReader();

        if (reader.HasRows) return null;
        reader.Close();
        
        
        
        command.CommandText = "UPDATE [Order] " +
                              "SET FulfilledAt = @Date " +
                              "WHERE IdOrder = @Id";
        command.Parameters.AddWithValue("@Date", DateTime.Now);
        command.Parameters.AddWithValue("@Id", orderId);
        var readerUpdate = command.ExecuteNonQuery();
        
        
        
        command.CommandText = "INSERT INTO Product_Warehouse " +
                              "VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt) " +
                              "SELECT SCOPE_IDENTITY() as lastReturn;";
        command.Parameters.AddWithValue("@IdWarehouse", order.IdWarehouse);
        command.Parameters.AddWithValue("@IdProduct", order.IdProduct);
        command.Parameters.AddWithValue("@IdOrder", orderId);
        command.Parameters.AddWithValue("@Amount", order.Amount);
        command.Parameters.AddWithValue("@Price", price*order.Amount);
        command.Parameters.AddWithValue("@CreatedAt", order.CreatedAt);
        reader = command.ExecuteReader();
        reader.Read();

        return (int)(decimal)reader["lastReturn"];
    }

    public int? AddProductWithProcedure(OrderDTO order)
    {
        using var connection = new SqlConnection(_configuration["ConnectionStrings:DefaultConnection"]);
        using var command = new SqlCommand("AddProductToWarehouse", connection)
        {
            CommandType = CommandType.StoredProcedure
        };
        command.Parameters.AddWithValue("@IdProduct", order.IdProduct);
        command.Parameters.AddWithValue("@IdWarehouse", order.IdWarehouse);
        command.Parameters.AddWithValue("@Amount", order.Amount);
        command.Parameters.AddWithValue("@CreatedAt", order.CreatedAt);
        
        connection.Open();
        var reader = command.ExecuteReader();
        
        if (!reader.HasRows) return null;
        reader.Read();

        return (int)(decimal)reader["NewId"];

    }
}