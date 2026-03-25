using System.Data.SqlClient;

namespace level3;

internal class Program
{
    public void PlaceOrder(string connectionString, int productId, int quantity, string customerName)
    {
        using SqlConnection conn = new SqlConnection(connectionString);
        conn.Open();

        using SqlTransaction transaction = conn.BeginTransaction();

        // ВАШ КОД ТУТ (Транзакція -> Перевірка -> Insert -> Update -> Commit/Rollback)

        using SqlCommand cmd = new SqlCommand("SELECT Id, Name, Price, Description, Stock FROM Products WHERE @productId = Id AND Stock >= @quantity", conn, transaction);

        SqlParameter productIdParam = new SqlParameter
        {
            ParameterName = "@productId",
            SqlDbType = System.Data.SqlDbType.Int,
            Value = productId
        };

        SqlParameter quantityParam = new SqlParameter
        {
            ParameterName = "@quantity",
            SqlDbType = System.Data.SqlDbType.Int,
            Value = quantity
        };

        cmd.Parameters.Add(productIdParam);
        cmd.Parameters.Add(quantityParam);

        using SqlDataReader reader = cmd.ExecuteReader();

        if (reader.Read())
        {
            reader.Close();

            using SqlCommand insertCmd = new SqlCommand("INSERT INTO Orders (ProductId, Quantity, CustomerName) VALUES (@productId, @quantity, @customerName)", conn, transaction);

            SqlParameter productIdParameter = new SqlParameter
            {
                ParameterName = "@productId",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = productId
            };

            SqlParameter quantityParameter = new SqlParameter
            {
                ParameterName = "@quantity",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = quantity
            };

            SqlParameter customerNameParameter = new SqlParameter
            {
                ParameterName = "@customerName",
                SqlDbType = System.Data.SqlDbType.NVarChar,
                Size = 100,
                Value = customerName
            };

            insertCmd.Parameters.Add(productIdParameter);
            insertCmd.Parameters.Add(quantityParameter);
            insertCmd.Parameters.Add(customerNameParameter);

            insertCmd.ExecuteNonQuery();

            using SqlCommand updateCmd = new SqlCommand("UPDATE Products SET Stock = Stock - @quantity WHERE Id = @productId", conn, transaction);

            SqlParameter updateProductIdParam = new SqlParameter
            {
                ParameterName = "@productId",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = productId
            };

            SqlParameter updateQuantityParam = new SqlParameter
            {
                ParameterName = "@quantity",
                SqlDbType = System.Data.SqlDbType.Int,
                Value = quantity
            };

            updateCmd.Parameters.Add(updateProductIdParam);
            updateCmd.Parameters.Add(updateQuantityParam);

            updateCmd.ExecuteNonQuery();
            transaction.Commit();
            Console.WriteLine("Замовлення успішне!");
        }
        else
        {
            reader.Close();
            transaction.Rollback();
            Console.WriteLine("Товару недостатньо!");
        }
    }

    static void Main(string[] args)
    {
        string connectionString =
            "Server=localhost,1433;Database=AdoNetAdvancedDB;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True;";

        Program p = new Program();
        p.PlaceOrder(connectionString, 3, 2, "Oleg");
        p.PlaceOrder(connectionString, 2, 2, "Eva");
    }
}