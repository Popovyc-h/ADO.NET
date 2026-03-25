namespace level2;
using Microsoft.Data.SqlClient;

internal class level2
{
    static void Main(string[] args)
    {
        string connectionString =
            "Server=localhost,1433;Database=AcademyDB;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True;";

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand cmd = new SqlCommand("SELECT * FROM Products", connection);

            using SqlDataReader reader = cmd.ExecuteReader();

            int IdOrd = reader.GetOrdinal("Id");
            int NameOrd = reader.GetOrdinal("Name");
            int PriceOrd = reader.GetOrdinal("Price");
            int IsAvailableOrd = reader.GetOrdinal("IsAvailable");
            int DescriptionOrd = reader.GetOrdinal("Description");

            while (reader.Read())
            {
                int Id = reader.GetInt32(IdOrd);
                string Name = reader.GetString(NameOrd);
                decimal Price = reader.GetDecimal(PriceOrd);
                bool IsAvailable = reader.GetBoolean(IsAvailableOrd);
                string? Description = reader.IsDBNull(DescriptionOrd) ? null : reader.GetString(DescriptionOrd);

                Console.WriteLine($"{Id}, {Name}, {Price}, {IsAvailable}, {Description}");
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }

        Product product = new Product();

        List<Product> products = product.GetProducts();

        Console.WriteLine();

        foreach (var p in products)
            p.Print();

        List<Product> productPrice = product.GetProductsByPrice(1000);
       
        foreach (var p in productPrice)
            p.Print();
    }
}