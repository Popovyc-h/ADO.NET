using System.Data;
using System.Data.SqlClient;

namespace ConsoleApp1;

internal class Program
{
    public static void Level1_OfflineTable()
    {
        DataTable productsTable = new DataTable("Products");

        // ВАШ КОД ТУТ (Крок 1):
        // Додайте 3 стовпці: Id (тип int), Name (тип string), Price (тип decimal).
        // Зробіть стовпець Id первинним ключем (PrimaryKey) та автоінкрементним (AutoIncrement).

        DataColumn productsId = new DataColumn("Id", typeof(int));
        productsId.AutoIncrement = true;
        productsId.AutoIncrementSeed = 1;
        productsId.AutoIncrementStep = 1;

        productsTable.Columns.Add(productsId);
        productsTable.PrimaryKey = new DataColumn[]
        {
            productsTable.Columns["Id"]
        };

        DataColumn productsName = new DataColumn("Name", typeof(string));
        DataColumn productsPrice = new DataColumn("Price", typeof(decimal));

        productsTable.Columns.Add(productsName);
        productsTable.Columns.Add(productsPrice);

        // ВАШ КОД ТУТ (Крок 2):
        // Створіть новий рядок (NewRow), заповніть Name та Price, і додайте його в Rows.

        DataRow row = productsTable.NewRow();

        row["Name"] = "Alisa";
        row["Price"] = 1500;

        productsTable.Rows.Add(row);
        // ВАШ КОД ТУТ (Крок 3):
        // Виведіть у консоль RowState щойно доданого рядка (має вивести "Added").
        Console.WriteLine(row.RowState);

        // Викличте productsTable.AcceptChanges() і знову виведіть RowState (має стати "Unchanged").
        productsTable.AcceptChanges();
        Console.WriteLine(row.RowState);
    }

    public static void Level2_LoadAndFilter(string connectionString)
    {
        DataTable productsTable = new DataTable();
        using SqlConnection conn = new SqlConnection(connectionString);

        // SqlDataAdapter - наш кур'єр між БД та DataTable
        using SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Products", conn);

        // ВАШ КОД ТУТ (Крок 1):
        // Завантажте дані з БД у productsTable (метод Fill).
        // Зверніть увагу: conn.Open() писати не обов'язково, адаптер зробить це сам!
        adapter.Fill(productsTable);

        Console.WriteLine($"Завантажено рядків: {productsTable.Rows.Count}");

        // ВАШ КОД ТУТ (Крок 2):
        // Створіть DataView на основі productsTable.
        // Налаштуйте фільтр (RowFilter), щоб залишити тільки товари з Price > 1000.
        // Налаштуйте сортування (Sort), щоб вивести їх за спаданням ціни (Price DESC).

        DataView view = new DataView(productsTable);
        view.RowFilter = "Price > 1000";
        view.Sort = "Price DESC";

        // Крок 3: Виведіть відфільтровані дані в консоль (пройдіться циклом по вашому DataView).

        foreach (DataRowView v in view)
        {
            Console.WriteLine($"Id: {v["Id"]}\nName: {v["Name"]}\nPrice: {v["Price"]}");
        }
    }

    public static void Level3_SyncChanges(string connectionString)
    {
        DataTable productsTable = new DataTable();
        using SqlDataAdapter adapter = new SqlDataAdapter("SELECT Id, Name, Price FROM Products", connectionString);

        // ВАШ КОД ТУТ (Крок 1):
        // Створіть SqlCommandBuilder та передайте йому ваш adapter.
        // Це автоматично згенерує команди INSERT, UPDATE та DELETE для адаптера.

        SqlCommandBuilder builder = new SqlCommandBuilder(adapter);

        // Завантажуємо дані
        adapter.Fill(productsTable);

        // ІМІТАЦІЯ ОФЛАЙН-РОБОТИ КОРИСТУВАЧА:
        if (productsTable.Rows.Count > 0)
        {
            // 1. Змінюємо ціну першого товару (RowState стане Modified)
            productsTable.Rows[0]["Price"] = 9999.99m;

            // 2. Видаляємо другий товар (RowState стане Deleted)
            if (productsTable.Rows.Count > 1)
                productsTable.Rows[1].Delete();
        }

        // 3. Додаємо новий товар (RowState стане Added)
        DataRow newRow = productsTable.NewRow();
        newRow["Name"] = "Офлайн Товар";
        newRow["Price"] = 150.00m;
        productsTable.Rows.Add(newRow);

        // ВАШ КОД ТУТ (Крок 2):
        // Викличте метод адаптера для синхронізації всіх змін назад у БД.
        // Збережіть результат у змінну rowsAffected та виведіть у консоль.

        int rowsAffected = adapter.Update(productsTable);
        Console.WriteLine(rowsAffected);
    }

    static void Main(string[] args)
    {
        string connectionString =
           "Server=localhost,1433;Database=Hw3Db;User Id=sa;Password=YourStrongPass123!;TrustServerCertificate=True;";

        Level1_OfflineTable();
        Console.WriteLine();
        Level2_LoadAndFilter(connectionString);
        Console.WriteLine();
        Level3_SyncChanges(connectionString);
    }
}