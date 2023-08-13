using System.Text.Json;
using System.IO;
using Newtonsoft.Json;


namespace OwnaAssessment
{
    internal class Program
    {

        static List<Author> authors = new List<Author>();
        static List<Book> books = new List<Book>();

        static int authorCounter = 0;
        static int bookCounter = 0;
        static void Main(string[] args)
        {
            SeedData();

            while (true)
            {
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Add new author");
                Console.WriteLine("2. Add new book");
                Console.WriteLine("3. Search for a book");
                Console.WriteLine("4. Exit");

                

                if(int.TryParse(Console.ReadLine(), out int choice))
                {
                    switch (choice)
                    {
                        case 1:
                            AddNewAuthor();
                            break;

                        case 2:
                             AddNewBook();
                            break;

                        case 3:
                            SearchBooks();
                            break;

                        case 4:
                            System.Environment.Exit(0);
                            break;

                         default:
                            Console.WriteLine("Invalid choice:");
                            break;

                    }
                }
                else 
                { 
                    Console.WriteLine("Please enter a valid number.");
                }
               
            }

        }

        private static void SeedData()
        {
            try
            {
                string jsonText = File.ReadAllText("SeedData.json");
                var jsonData = JsonConvert.DeserializeObject<JsonData>(jsonText);

                authors = jsonData.Authors.Select(a => new Author
                {
                    Id = a.Id,
                    Name = a.Name
                }).ToList();

                books = jsonData.Books.Select(b => new Book
                {
                    Id = b.Id,
                    Title = b.Title,
                    Description = b.Description,
                    CoverImageUrl = b.CoverImageUrl,
                    Authors = b.Authors.Select(a => authors.First(auth => auth.Id == a.Id)).ToList()
                }).ToList();

                authorCounter += authors.Count();
                bookCounter += books.Count();
            }

            catch (Exception ex) 
            {
                Console.WriteLine($"Error occurred while seeding data:{ex.Message}");
            }
        }

        private static void SearchBooks()
        {
            Console.WriteLine("Search by (1) Title or (2) Author:");

            if (int.TryParse(Console.ReadLine(),out int choice))
            {
                List<Book> results= new List<Book>();

                if (choice == 1)
                {
                    Console.WriteLine("Enter book title:");
                    var title = Console.ReadLine();
                    results = books.Where
                        (b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
                }
                else if (choice == 2)
                {

                    Console.WriteLine("Enter author name:");
                    var name= Console.ReadLine();   
                    results= books.Where(b=>b.Authors.Any(a=> a.Name.Equals(name, StringComparison.OrdinalIgnoreCase))).ToList();

                }
                else
                {
                    Console.WriteLine("Invalid choice. Try again.");
                    return;
                }

                if(results.Any())
                {
                    Console.WriteLine("Search results:");

                  for(int i=0; i< results.Count; i++)
                    {
                        Console.WriteLine($"{i+1}. {results[i].Title}");
                    }
                  
                    Console.WriteLine("Select a book numbers for details(comma seperated) or press Enter to go back to the main menu:");

                    var bookChoices= Console.ReadLine().Split(',').Select(a=>a.Trim()).ToList();

                    foreach( var bookChoice in bookChoices)
                    {
                        if(int.TryParse(bookChoice, out int selectedIndex) && selectedIndex >0 && selectedIndex <=results.Count)
                        {
                            var selectedBook = results[selectedIndex - 1];
                            DisplayBookDetails(selectedBook);
                            Console.WriteLine("-------------------");
                        }

                        else
                        {
                            Console.WriteLine($"Invalid book number: {bookChoice}.Skipping");
                        }
                    }
                    
                }
                else
                {
                    Console.WriteLine("No books found");
                }
            }
            else
            {
                Console.WriteLine("Please enter a valid number");
            }
        }

        private static void DisplayBookDetails(Book book)
        {
            if(book == null)
            {
                Console.WriteLine("No book details to display.");
                return;
            }

            Console.WriteLine($"Title : {book.Title}");
            Console.WriteLine("Author:");
            foreach (var author in book.Authors)
            {
                Console.WriteLine($"- {author.Name}");
            }

            Console.WriteLine($" Description: {book.Description}");
            Console.WriteLine($" CoverImageUrl: {book.CoverImageUrl}");
        }

        private static void AddNewBook()
        {
            Console.WriteLine(" Enter the book's title");
            var title= Console.ReadLine();

            Console.WriteLine(" Enter the book's description title");
            var description = Console.ReadLine();

            Console.WriteLine(" Enter the book's cover image Url");
            var coverImageUrl = Console.ReadLine();

            Console.WriteLine("Select the authors by their Name (comma seperated for multiple authors;)");

            var authorIdsInput = Console.ReadLine().Split(',');

            List<int> authorIds = new List<int>();
            foreach (var author in authorIdsInput)
            {
                if(int.TryParse(author.Trim(), out int id))
                {
                    authorIds.Add(id);
                }
                else { Console.WriteLine($"Invalid author ID: {author}.Skipping. "); }
            }

            var newBook = new Book { Id= ++bookCounter, Title=title, Description= description, CoverImageUrl= coverImageUrl };

            foreach(var id in authorIds)
            {
                var author= authors.FirstOrDefault(x => x.Id == id);
                if(author != null)
                {
                   newBook.Authors.Add(author);

                }
                else
                {
                    Console.WriteLine($"Author with Id{id} not found. Skipping.");

                }
            }

            books.Add(newBook);
            Console.WriteLine($"Book {title} added successfully. \n");
        }

        private static void AddNewAuthor()
        {
            Console.WriteLine("Enter the author's name.");

            var name= Console.ReadLine();

            if(name != null && name.Length >0)
            {
                var newAuthor = new Author
                {
                    Id = ++authorCounter,
                    Name = name
                };

                authors.Add(newAuthor);
                Console.WriteLine($"Author {name} added successfully \n");
            }
            else
            {
                Console.WriteLine("Please enter valid name.");
            }

           
        }
    }
}