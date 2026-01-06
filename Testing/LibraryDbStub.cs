namespace ClassProject
{
    public class LibraryDbStub : ILibraryDbContract
    {
        List<Book> books = new List<Book>
        {
            new Book("F. Scott Fitzgerald", "The Great Gatsby", "0987654321", 1 ),
            new Book("Harper Lee",  "To Kill a Mockingbird", "1234567890", 2),
            new Book("George Orwell", "1984","45495405", 0 ),
            new Book("J.R.R Tolkien", "The Hobbit", "9876543210", 2),
            new Book("J.R.R Tolkien", "The Lord Of The Rings", "9876543211", 1),
        };

        List<BookUser> users = new List<BookUser>
        {
            new BookUser("Alice", 1),
            new BookUser("Bob", 2),
            new BookUser("Charlie", 3)
        };

        public Book GetBookFromISBN(string isbn)
        {
            Book book = books.FirstOrDefault(b => b.getIsbn() == isbn);

            if (book == null)
            {
                throw new ArgumentException("Book with given ISBN not found");
            }

            return book;

        }

        public Book GetBookFromName(string name)
        {
            Book book = books.FirstOrDefault(b => b.getTitle() == name);

            if (book == null)
            {
                throw new ArgumentException("Book with given name not found");
            }

            return book;
        }

        public void AddBook(Book book)
        {
            if (books.Any(b => b.getIsbn() == book.getIsbn()))
            {
                throw new ArgumentException("Book with given ISBN already exists");
            }

            books.Add(book);   
        }

        public void RemoveBook(Book book)
        {
            books.Remove(book);
        }   

        public void AddBookUser(BookUser user)
        {
            users.Add(user);
        }

        public IEnumerable<BookUser> GetUsersWithBook(Book book)
        {
            return users.Where(x => x.getBorrowedBooks().Contains(book)).ToList();
        }

        public BookUser GetBookUserFromName(string name)
        {
            BookUser user = users.FirstOrDefault(u => u.getName() == name);
            if (user == null)
            {
                throw new ArgumentException("User with given name not found");
            }
            return user;
        }
    }
}
