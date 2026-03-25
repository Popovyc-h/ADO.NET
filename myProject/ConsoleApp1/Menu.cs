using ConsoleApp1.Database;
using ConsoleApp1.Models;
using ConsoleApp1.Repositories;

namespace ConsoleApp1;

public class Menu
{
    public void Run()
    {
        DatabaseHelper helper = new DatabaseHelper();

        AuthorRepository authorRepo = new AuthorRepository(helper);
        BookRepository bookRepo = new BookRepository(helper);
        do
        {
            Console.WriteLine("1. Show all authors");
            Console.WriteLine("2. Show all books");
            Console.WriteLine("3. Add author");
            Console.WriteLine("4. Add book");
            Console.WriteLine("5. Delete author");
            Console.WriteLine("6. Delete book");
            Console.WriteLine("0. Exit");
            Console.Write("Your choice: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    authorRepo.GetAllAuthors();
                    break;
                case "2":
                    bookRepo.GetAllBooks();
                    break;
                case "3":
                    Console.Write("\nEnter author name: ");
                    string name = Console.ReadLine();

                    Console.Write("Enter author surname: ");
                    string surname = Console.ReadLine();

                    var author = new Author
                    {
                        Name = name,
                        Surname = surname
                    };

                    authorRepo.AddAuthor(author);
                    break;
                case "4":
                    Console.Write("\nEnter title: ");
                    string title = Console.ReadLine();

                    Console.Write("Enter published year: ");
                    int publishedYear = int.Parse(Console.ReadLine());

                    Console.Write("Enter author id: ");
                    int AuthorId = int.Parse(Console.ReadLine());

                    var book = new Book
                    {
                        Title = title,
                        PublishedYear = publishedYear,
                        AuthorId = AuthorId
                    };

                    bookRepo.AddBook(book);
                    break;
                case "5":
                    Console.Write("Enter id: ");
                    int authorId = Convert.ToInt32(Console.ReadLine());
                    authorRepo.DeleteAuthor(authorId);
                    break;
                case "6":
                    Console.Write("Enter id: ");
                    int bookId = Convert.ToInt32(Console.ReadLine());
                    bookRepo.DeleteBook(bookId);
                    break;
                case "0":
                    Console.WriteLine("Goodbye!");
                    return;
            }
        } while (true);
    }
}