using System.Data.Common;
using System.Data.SqlClient;

namespace level2;

internal class Program
{
    public static void TransferMoney(SqlConnection conn, int fromAccount, int toAccount, decimal amount)
    {
        // 1. СТВОРІТЬ ТРАНЗАКЦІЮ ТУТ
        using SqlTransaction transaction = conn.BeginTransaction();

        try
        {
            using SqlCommand cmdWithdraw = new SqlCommand("UPDATE Accounts SET Balance = Balance - @amount WHERE Id = @from", conn, transaction);
            // 2. ПРИВ'ЯЖІТЬ ТРАНЗАКЦІЮ ДО КОМАНДИ
            cmdWithdraw.Parameters.AddWithValue("@amount", amount);
            cmdWithdraw.Parameters.AddWithValue("@from", fromAccount);
            cmdWithdraw.ExecuteNonQuery();

            // Імітація збою мережі: throw new Exception("Мережа впала!");

            using SqlCommand cmdDeposit = new SqlCommand("UPDATE Accounts SET Balance = Balance + @amount WHERE Id = @to", conn, transaction);
            // 2. ПРИВ'ЯЖІТЬ ТРАНЗАКЦІЮ ДО КОМАНДИ
            cmdDeposit.Parameters.AddWithValue("@amount", amount);
            cmdDeposit.Parameters.AddWithValue("@to", toAccount);
            cmdDeposit.ExecuteNonQuery();

            // 3. ЗРОБІТЬ COMMIT ТУТ
            transaction.Commit();
            Console.WriteLine("Переказ успішний!");
        }
        catch (Exception ex)
        {
            // 4. ЗРОБІТЬ ROLLBACK ТУТ
            transaction.Rollback();
            Console.WriteLine($"Помилка: {ex.Message}. Гроші повернуто.");
        }
    }

    public void ConnectToDatabase(string connectionString)
    {
        // ЗАМІНІТЬ ЦІ РЯДКИ НА ВИКОРИСТАННЯ ФАБРИКИ (DbProviderFactory):
        DbProviderFactory factory = SqlClientFactory.Instance;

        using DbConnection connection = factory.CreateConnection();
        connection.ConnectionString = connectionString;
        connection.Open();

        using DbCommand cmd = connection.CreateCommand();
        cmd.CommandText = "SELECT GETDATE()";

        var result = cmd.ExecuteScalar();
        Console.WriteLine($"Час на сервері: {result}");
    }

    static void Main(string[] args)
    {
        string connectionString =
            "Server=localhost,1433;Database=AdoNetAdvancedDB;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True;";

        try
        {
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            using SqlCommand cmd = new SqlCommand("SELECT Id, OwnerName, Balance FROM Accounts", connection);

            using SqlDataReader reader = cmd.ExecuteReader();

            int IdOrd = reader.GetOrdinal("Id");
            int NameOrd = reader.GetOrdinal("OwnerName");
            int BalanceOrd = reader.GetOrdinal("Balance");

            while (reader.Read())
            {
                int Id = reader.GetInt32(IdOrd);
                string Name = reader.GetString(NameOrd);
                decimal Balance = reader.GetDecimal(BalanceOrd);

                Console.WriteLine($"Id: {Id}\nOwnerName: {Name}\nBalance: {Balance}");
            }
            reader.Close();

            Console.WriteLine();
            TransferMoney(connection, 1, 2, 100);
            Console.WriteLine();

            using SqlDataReader read = cmd.ExecuteReader();

            while (read.Read())
            {
                int Id = read.GetInt32(IdOrd);
                string Name = read.GetString(NameOrd);
                decimal Balance = read.GetDecimal(BalanceOrd);

                Console.WriteLine($"Id: {Id}\nOwnerName: {Name}\nBalance: {Balance}");
            }
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.Message);
        }

        Program p = new Program();
        p.ConnectToDatabase(connectionString);
    }
}