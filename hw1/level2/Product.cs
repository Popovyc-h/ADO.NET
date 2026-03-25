using Microsoft.Data.SqlClient;

namespace level2;

public class Product
{
    private int id;
    private string name;
    private decimal price;
    private bool isAvailable;
    private string? description;

    public int Id
    {
        get => id;

        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            id = value;
        }
    }

    public string Name
    {
        get => name;

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentOutOfRangeException(nameof(value));
            name = value;
        }
    }

    public decimal Price
    {
        get => price;

        set
        {
            if (value <= 0)
                throw new ArgumentOutOfRangeException(nameof(value));
            price = value;
        }
    }

    public string Description
    {
        get => description;
        set => description = value;
    }

    public bool IsAvailable
    {
        get => isAvailable;
        set => isAvailable = value;
    }

    public List<Product> GetProducts()
    {
        string connectionString =
            "Server=localhost,1433;Database=AcademyDB;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True;";

        List<Product> products = new List<Product>();

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand cmd = new SqlCommand("SELECT Id, Name, Price, IsAvailable, Description FROM Products", connection);

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int Id = reader.GetInt32(0);
                string Name = reader.GetString(1);
                decimal Price = reader.GetDecimal(2);
                bool IsAvailable = reader.GetBoolean(3);
                string? Description = reader.IsDBNull(4) ? null : reader.GetString(4);

                Product product = new Product();

                product.Id = Id;
                product.Name = Name;
                product.Price = Price;
                product.IsAvailable = IsAvailable;
                product.Description = Description;

                products.Add(product);
            }
            return products;
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }
        return products;
    }

    public List<Product> GetProductsByPrice(decimal maxPrice)
    {
        string connectionString =
            "Server=localhost,1433;Database=AcademyDB;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True;";

        List<Product> products = new List<Product>();

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand cmd = new SqlCommand("SELECT Id, Name, Price, IsAvailable, Description FROM Products WHERE Price < @maxPrice", connection);

            SqlParameter param = new SqlParameter
            {
                ParameterName = "@maxPrice",
                SqlDbType = System.Data.SqlDbType.Decimal,
                Value = maxPrice
            };

            cmd.Parameters.Add(param);

            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int Id = reader.GetInt32(0);
                string Name = reader.GetString(1);
                decimal Price = reader.GetDecimal(2);
                bool IsAvailable = reader.GetBoolean(3);
                string? Description = reader.IsDBNull(4) ? null : reader.GetString(4);

                Product product = new Product();

                product.Id = Id;
                product.Name = Name;
                product.Price = Price;
                product.IsAvailable = IsAvailable;
                product.Description = Description;

                products.Add(product);
            }
            return products;
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }
        return products;
    }

    public void Print()
    {
        Console.WriteLine($"Id: {Id}" +
            $"\nName: {Name}" +
            $"\nPrice: {Price}" +
            $"\nIsAvailable: {IsAvailable}" +
            $"\nDescription: {Description}\n");
    }
}