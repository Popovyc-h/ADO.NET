<!-- ### Рівень 1: Базовий (Синтаксис та Виправлення)

_Фокус: Захист від SQL Injection та безпечне читання NULL-значень._

1. **"Зупини хакера (SQL Injection)"**:
   Ось заготовка методу авторизації, яка вразлива до SQL ін'єкцій. Перепишіть її, використовуючи `SqlParameter` (бажано через колекцію `Parameters.Add()` з вказанням типу `SqlDbType`).

```csharp
public bool Login(SqlConnection conn, string username, string password)
{
    // ВРАЗЛИВИЙ КОД! Виправте його:
    string sql = $"SELECT COUNT(*) FROM Users WHERE Username = '{username}' AND Password = '{password}'";
    using SqlCommand cmd = new SqlCommand(sql, conn);

    int count = (int)cmd.ExecuteScalar();
    return count > 0;
}

```

2. **"Безпечний DataReader"**:
   Цей код "впаде" з помилкою, якщо в базі у стовпці `Description` буде значення `NULL`. Додайте перевірку через `reader.IsDBNull()`.

```csharp
public void PrintProducts(SqlDataReader reader)
{
    int descOrdinal = reader.GetOrdinal("Description");
    while (reader.Read())
    {
        // ДОДАЙТЕ ПЕРЕВІРКУ ТУТ:
        string description = reader.GetString(descOrdinal);
        Console.WriteLine($"Опис: {description}");
    }
}

``` -->

<!-- ### Рівень 2: Логіка та Архітектура (Транзакції та Фабрики)

_Фокус: ACID-транзакції та провайдер-незалежний код._

1. **"Усе або нічого (Транзакція)"**:
   Нижче наведено заготовку банківського переказу. Додайте створення `SqlTransaction`, прив'яжіть її до команд, зробіть `Commit()` у разі успіху та `Rollback()` у блоці `catch`.

```csharp
public void TransferMoney(SqlConnection conn, int fromAccount, int toAccount, decimal amount)
{
    // 1. СТВОРІТЬ ТРАНЗАКЦІЮ ТУТ

    try
    {
        using SqlCommand cmdWithdraw = new SqlCommand("UPDATE Accounts SET Balance = Balance - @amount WHERE Id = @from", conn);
        // 2. ПРИВ'ЯЖІТЬ ТРАНЗАКЦІЮ ДО КОМАНДИ
        cmdWithdraw.Parameters.AddWithValue("@amount", amount);
        cmdWithdraw.Parameters.AddWithValue("@from", fromAccount);
        cmdWithdraw.ExecuteNonQuery();

        // Імітація збою мережі: throw new Exception("Мережа впала!");

        using SqlCommand cmdDeposit = new SqlCommand("UPDATE Accounts SET Balance = Balance + @amount WHERE Id = @to", conn);
        // 2. ПРИВ'ЯЖІТЬ ТРАНЗАКЦІЮ ДО КОМАНДИ
        cmdDeposit.Parameters.AddWithValue("@amount", amount);
        cmdDeposit.Parameters.AddWithValue("@to", toAccount);
        cmdDeposit.ExecuteNonQuery();

        // 3. ЗРОБІТЬ COMMIT ТУТ
        Console.WriteLine("Переказ успішний!");
    }
    catch (Exception ex)
    {
        // 4. ЗРОБІТЬ ROLLBACK ТУТ
        Console.WriteLine($"Помилка: {ex.Message}. Гроші повернуто.");
    }
}

```

2. **"Універсальний солдат (DbProviderFactory)"**:
   Перепишіть цей код так, щоб він використовував абстракції (`DbConnection`, `DbCommand`) та фабрику `SqlClientFactory.Instance`.

```csharp
public void ConnectToDatabase(string connectionString)
{
    // ЗАМІНІТЬ ЦІ РЯДКИ НА ВИКОРИСТАННЯ ФАБРИКИ (DbProviderFactory):
    using SqlConnection conn = new SqlConnection(connectionString);
    conn.Open();

    using SqlCommand cmd = new SqlCommand("SELECT GETDATE()", conn);
    var result = cmd.ExecuteScalar();
    Console.WriteLine($"Час на сервері: {result}");
}

``` -->

### Рівень 3: Архітектура та Створення (Міні-проєкт)

_Фокус: Поєднання DataReader, Parameters та Transactions._

1. **Міні-проєкт "Оформлення замовлення"**:
   Реалізуйте метод `PlaceOrder`. Логіка:

- **Крок 1**: Через `ExecuteScalar` (або `DataReader`) перевірити, чи є товар в наявності (таблиця `Products`, поле `Stock`).
- **Крок 2**: Якщо товару достатньо — створити запис у таблиці `Orders` (INSERT).
- **Крок 3**: Зменшити `Stock` у таблиці `Products` (UPDATE).
- **Вимоги**: Усе має працювати в рамках однієї **транзакції**. Всі вхідні дані мають передаватися через **параметри**.

_Заготовка методу:_ 

```csharp
public void PlaceOrder(string connectionString, int productId, int quantity, string customerName)
{
    using SqlConnection conn = new SqlConnection(connectionString);
    conn.Open();

    // ВАШ КОД ТУТ (Транзакція -> Перевірка -> Insert -> Update -> Commit/Rollback)
}

```

---

### Summary (Підсумок дня)

- **DataReader (Потокове читання)**: Це найшвидший спосіб прочитати мільйон рядків, бо він не вантажить їх усі в пам'ять одразу (forward-only, read-only). Головне — перевіряти `IsDBNull()` перед читанням пустих полів.
- **SQL Injection**: Найнебезпечніша вразливість. Ніколи не "клеїмо" рядки через `+` у SQL-запитах. `SqlParameter` — наш єдиний щит, який чітко розділяє "команду" від "даних користувача".
- **Транзакції (ACID)**: Рятують від частково виконаних операцій (гроші зняли, але не нарахували). Завдяки парадигмі `Commit()` або `Rollback()` у блоці `catch`, база завжди залишається у валідному стані.
- **DbProviderFactory (Абстрактна фабрика)**: Дозволяє писати код, який працює і з SQL Server, і з PostgreSQL, спираючись на базові класи (`DbConnection`, `DbCommand`), а не на конкретні реалізації.

```sql
-- 1. Створення бази даних
CREATE DATABASE AdoNetAdvancedDB;
GO

USE AdoNetAdvancedDB;
GO

-- ==========================================
-- ТАБЛИЦІ ДЛЯ РІВНЯ 1 (SQL Injection та DataReader)
-- ==========================================

-- Таблиця для перевірки авторизації (Завдання 1.1)
CREATE TABLE Users (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL -- У реальності тут має бути Hash, але для завдання залишаємо так
);

-- Таблиця товарів для DataReader та Міні-проєкту (Завдання 1.2 та 3.1)
CREATE TABLE Products (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    Price DECIMAL(18, 2) NOT NULL,
    Description NVARCHAR(MAX) NULL, -- Може бути NULL для перевірки IsDBNull()
    Stock INT NOT NULL DEFAULT 0    -- Кількість на складі для транзакцій
);

-- ==========================================
-- ТАБЛИЦІ ДЛЯ РІВНЯ 2 (Транзакції)
-- ==========================================

-- Таблиця банківських рахунків (Завдання 2.1)
CREATE TABLE Accounts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    OwnerName NVARCHAR(100) NOT NULL,
    Balance DECIMAL(18, 2) NOT NULL CHECK (Balance >= 0) -- Баланс не може бути від'ємним
);

-- ==========================================
-- ТАБЛИЦІ ДЛЯ РІВНЯ 3 (Міні-проєкт)
-- ==========================================

-- Таблиця замовлень (Завдання 3.1)
CREATE TABLE Orders (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ProductId INT NOT NULL,
    Quantity INT NOT NULL,
    CustomerName NVARCHAR(100) NOT NULL,
    OrderDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Orders_Products FOREIGN KEY (ProductId) REFERENCES Products(Id)
);
GO

-- ==========================================
-- ЗАПОВНЕННЯ ТЕСТОВИМИ ДАНИМИ
-- ==========================================

-- Додаємо користувачів
INSERT INTO Users (Username, Password)
VALUES
('admin', 'supersecret'),
('student', '123456');

-- Додаємо товари (зверніть увагу на NULL в Description)
INSERT INTO Products (Name, Price, Description, Stock)
VALUES
(N'Ігровий Ноутбук', 45000.00, N'Потужний ноутбук для ігор', 10),
(N'Офісна Мишка', 350.00, NULL, 50),
(N'Механічна Клавіатура', 2500.00, N'Свічі Cherry MX Red', 5),
(N'Монітор 4K', 12000.00, NULL, 0); -- Немає в наявності (Stock = 0)

-- Додаємо банківські рахунки
INSERT INTO Accounts (OwnerName, Balance)
VALUES
(N'Рахунок А (Відправник)', 5000.00),
(N'Рахунок Б (Отримувач)', 1500.00);
GO

-- Перевірка створених даних
SELECT * FROM Users;
SELECT * FROM Products;
SELECT * FROM Accounts;
```
