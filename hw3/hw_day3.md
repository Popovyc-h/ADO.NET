### Рівень 1: Базовий (Схема в пам'яті)

_Фокус: Створення `DataTable`, `DataColumn` та розуміння `RowState` без підключення до бази._

**«Фундамент у пам'яті»**:
На цьому етапі ми створюємо локальну таблицю "Products" вручну. Студенту потрібно визначити схему таблиці та додати один рядок, звернувши увагу на те, як змінюється його стан (`RowState`).

_Заготовка коду:_

```csharp
public void Level1_OfflineTable()
{
    DataTable productsTable = new DataTable("Products");

    // ВАШ КОД ТУТ (Крок 1):
    // Додайте 3 стовпці: Id (тип int), Name (тип string), Price (тип decimal).
    // Зробіть стовпець Id первинним ключем (PrimaryKey) та автоінкрементним (AutoIncrement).

    // ВАШ КОД ТУТ (Крок 2):
    // Створіть новий рядок (NewRow), заповніть Name та Price, і додайте його в Rows.

    // ВАШ КОД ТУТ (Крок 3):
    // Виведіть у консоль RowState щойно доданого рядка (має вивести "Added").

    // Викличте productsTable.AcceptChanges() і знову виведіть RowState (має стати "Unchanged").
}

```

### Рівень 2: Логіка та Фільтрація (DataAdapter та DataView)

_Фокус: Завантаження даних через `Fill` та локальна робота з `DataView`._

**«Завантаження і пошук офлайн»**:
Тепер ми підключаємо базу даних. Студенту потрібно завантажити реальні дані в таблицю за допомогою `SqlDataAdapter`, а потім створити `DataView` для фільтрації результатів _без_ повторного звернення до SQL Server.

_Заготовка коду:_

```csharp
public void Level2_LoadAndFilter(string connectionString)
{
    DataTable productsTable = new DataTable();
    using SqlConnection conn = new SqlConnection(connectionString);

    // SqlDataAdapter - наш кур'єр між БД та DataTable
    using SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Products", conn);

    // ВАШ КОД ТУТ (Крок 1):
    // Завантажте дані з БД у productsTable (метод Fill).
    // Зверніть увагу: conn.Open() писати не обов'язково, адаптер зробить це сам!

    Console.WriteLine($"Завантажено рядків: {productsTable.Rows.Count}");

    // ВАШ КОД ТУТ (Крок 2):
    // Створіть DataView на основі productsTable.
    // Налаштуйте фільтр (RowFilter), щоб залишити тільки товари з Price > 1000.
    // Налаштуйте сортування (Sort), щоб вивести їх за спаданням ціни (Price DESC).

    // Крок 3: Виведіть відфільтровані дані в консоль (пройдіться циклом по вашому DataView).
}

```

### Рівень 3: Архітектура (Повна Синхронізація)

_Фокус: Автогенерація команд та `DataAdapter.Update()`._

**«Глобальна синхронізація»**:
Фінальний етап. Ми завантажуємо дані, імітуємо роботу користувача офлайн (він додає, змінює та видаляє рядки), а потім зберігаємо всі ці зміни назад у базу даних **одним викликом**.

_Заготовка коду:_

```csharp
public void Level3_SyncChanges(string connectionString)
{
    DataTable productsTable = new DataTable();
    using SqlDataAdapter adapter = new SqlDataAdapter("SELECT Id, Name, Price FROM Products", connectionString);

    // ВАШ КОД ТУТ (Крок 1):
    // Створіть SqlCommandBuilder та передайте йому ваш adapter.
    // Це автоматично згенерує команди INSERT, UPDATE та DELETE для адаптера.

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
}

```

---

### Summary

- **Від'єднаний режим (Disconnected Mode)**: Дозволяє завантажити шматок бази даних у пам'ять, закрити мережеве з'єднання, працювати з даними годинами, а потім синхронізувати їх назад.
- **DataTable / DataSet**: Це справжня міні-база даних у вашій оперативній пам'яті. Вона має свою схему (стовпці, первинні ключі) та підтримує обмеження.
- **RowState — це ключ до синхронізації**: Коли ви змінюєте дані в `DataTable`, вона нічого не пише в базу одразу. Вона просто маркує рядок: `Added`, `Modified` або `Deleted`.
- **DataView**: Ваш найкращий інструмент для пошуку та сортування завантажених даних без написання нових SQL-запитів до сервера.
- **DataAdapter + SqlCommandBuilder**: `DataAdapter` — це "кур'єр". `Fill()` привозить дані, `Update()` відвозить зміни. А `SqlCommandBuilder` — це "магія", яка пише нудні запити `INSERT/UPDATE/DELETE` за вас на основі вашого базового `SELECT`.
