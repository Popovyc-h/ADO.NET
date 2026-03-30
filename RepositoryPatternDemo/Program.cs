using Microsoft.Data.Sqlite;
using RepositoryPatternDemo.Persistence.Entities;
using RepositoryPatternDemo.Persistence.Entities.Comparators;
using RepositoryPatternDemo.Persistence.Exceptions;
using RepositoryPatternDemo.Persistence.Repositories;
using RepositoryPatternDemo.Persistence.Repositories.AdoImpl;
using RepositoryPatternDemo.Persistence.Repositories.Impl;
using System.Configuration;

namespace RepositoryPatternDemo;

internal class Program
{
    static void Main(string[] args)
    {
        var connectionStringSettings = ConfigurationManager.ConnectionStrings["BlogConnectionString"];
        string connectionString = connectionStringSettings.ConnectionString;
        string providerName = connectionStringSettings.ProviderName;
        var connectionManager = new ConnectionManager(connectionString, providerName, SqliteFactory.Instance);

        var userRepository = new UserAdoRepository(connectionManager);

        //InsertTest(userRepository);

        /*List<User> users = (List<User>)userRepository.GetAll();
        users.Sort(new UserCompererByCreatedAt());
        foreach (var user in users)
        {
            Console.WriteLine(user);
        }*/

        //userRepository.Remove(Guid.Parse("26200D4B-8631-4CE6-8AED-2FF0D203B53D"));
        //Console.WriteLine(user);

        //userRepository.Add(new User(Guid.Parse("0EC31953-BF79-4C3A-9B4F-7D6141E43D5F"), "Bob2", "bob2@example.com", "password2", null, DateTime.Now, DateTime.Now));

    }

    private static void InsertTest(UserAdoRepository userRepository)
    {
        var random = new Random();
        DateTime GetRandomDate()
        {
            // Визначаємо період в 5 років
            var start = DateTime.Now.AddYears(-5);
            int range = (DateTime.Now - start).Days;
            return start.AddDays(random.Next(range));
        }

        DateTime createdAt1 = GetRandomDate();
        DateTime createdAt2 = GetRandomDate();
        DateTime createdAt3 = GetRandomDate();
        DateTime createdAt4 = GetRandomDate();
        DateTime createdAt5 = GetRandomDate();
        DateTime createdAt6 = GetRandomDate();
        DateTime createdAt7 = GetRandomDate();
        DateTime createdAt8 = GetRandomDate();
        DateTime createdAt9 = GetRandomDate();
        DateTime createdAt10 = GetRandomDate();
        var users = new List<User>
            {
                new User("Alice", "alice@example.com", "password1", null, createdAt1, createdAt1),
                new User("Bob", "bob@example.com", "password2", null, createdAt2, createdAt2),
                new User("Charlie", "charlie@example.com", "password3", null, createdAt3, createdAt3),
                new User("David", "david@example.com", "password4", null, createdAt4, createdAt4),
                new User("Eva", "eva@example.com", "password5", null, createdAt5, createdAt5),
                new User("Frank", "frank@example.com", "password6", null, createdAt6, createdAt6),
                new User("Grace", "grace@example.com", "password7", null, createdAt7, createdAt7),
                new User("Henry", "henry@example.com", "password8", null, createdAt8, createdAt8),
                new User("Ivy", "ivy@example.com", "password9", null, createdAt9, createdAt9),
                new User("Jack", "jack@example.com", "password10", null, createdAt10, createdAt10)
            };

        // Додаємо користувачів до репозиторію
        foreach (var user in users)
        {
            userRepository.Add(user);
        }
    }

    private static void FileRepositoryTests()
    {
        // Ініціалізуємо репозиторій користувачів
        var userRepository = new UserFileRepository();
        //TestGetAllUsers(userRepository);
        //TestFind(userRepository);
        //GetUserById(userRepository);
        //CreateAndSerizalizeTest(userRepository);
    }

    private static void TestGetAllUsers(UserFileRepository userRepository)
    {
        List<User> users = (List<User>)userRepository.GetAll();
        users.Sort(new UserCompererByCreatedAt());
        foreach (var user in users)
        {
            Console.WriteLine(user);
        }
    }

    private static void TestFind(UserFileRepository userRepository)
    {
        User? user = userRepository.Find(u => u.Password == "password3");
        Console.WriteLine(user);
    }

    private static void GetUserById(UserFileRepository userRepository)
    {
        User? user = userRepository.Get(Guid.Parse("6580deab-4a2f-468f-a4a8-051e2c7d4aa6"));
        Console.WriteLine(user);
    }

    private static void CreateAndSerizalizeTest(UserFileRepository userRepository)
    {
        var random = new Random();
        DateTime GetRandomDate()
        {
            // Визначаємо період в 5 років
            var start = DateTime.Now.AddYears(-5);
            int range = (DateTime.Now - start).Days;
            return start.AddDays(random.Next(range));
        }


        try
        {
            DateTime createdAt1 = GetRandomDate();
            DateTime createdAt2 = GetRandomDate();
            DateTime createdAt3 = GetRandomDate();
            DateTime createdAt4 = GetRandomDate();
            DateTime createdAt5 = GetRandomDate();
            DateTime createdAt6 = GetRandomDate();
            DateTime createdAt7 = GetRandomDate();
            DateTime createdAt8 = GetRandomDate();
            DateTime createdAt9 = GetRandomDate();
            DateTime createdAt10 = GetRandomDate();
            var users = new List<User>
            {
                new User(Guid.NewGuid(), "Alice", "alice@example.com", "password1", null, createdAt1, createdAt1),
                new User(Guid.NewGuid(), "Bob", "bob@example.com", "password2", null, createdAt2, createdAt2),
                new User(Guid.NewGuid(), "Charlie", "charlie@example.com", "password3", null, createdAt3, createdAt3),
                new User(Guid.NewGuid(), "David", "david@example.com", "password4", null, createdAt4, createdAt4),
                new User(Guid.NewGuid(), "Eva", "eva@example.com", "password5", null, createdAt5, createdAt5),
                new User(Guid.NewGuid(), "Frank", "frank@example.com", "password6", null, createdAt6, createdAt6),
                new User(Guid.NewGuid(), "Grace", "grace@example.com", "password7", null, createdAt7, createdAt7),
                new User(Guid.NewGuid(), "Henry", "henry@example.com", "password8", null, createdAt8, createdAt8),
                new User(Guid.NewGuid(), "Ivy", "ivy@example.com", "password9", null, createdAt9, createdAt9),
                new User(Guid.NewGuid(), "Jack", "jack@example.com", "password10", null, createdAt10, createdAt10)
            };

            // Додаємо користувачів до репозиторію
            foreach (var user in users)
            {
                userRepository.Add(user);
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        catch (EntityValidationException ex)
        {
            foreach (var error in ex.errors)
            {
                Console.WriteLine($"Помилка валідації, поле {error.Key}: ");
                foreach (var message in error.Value)
                {
                    Console.WriteLine(message);
                }
            }

        }
        finally
        {
            // Зберігаємо всіх користувачів у файл
            // AppDomain.CurrentDomain.ProcessExit += (sender, eventArgs) =>
            //{
            userRepository.SerializeAll();
            Console.WriteLine("Users have been saved to file.");
            //};
        }
    }
}
