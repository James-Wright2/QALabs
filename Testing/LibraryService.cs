using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassProject
{
    public class LibraryService
    {
        private ILibraryDbContract _libraryDb;

        public LibraryService(ILibraryDbContract libraryDb)
        {
            _libraryDb = libraryDb;
        }

        public Book GetBookByISBN(string isbn)
        {
            return _libraryDb.GetBookFromISBN(isbn);
        }

        public Book GetBookByName(string name)
        {
            return _libraryDb.GetBookFromName(name);
        }

        public void AddBook(Book book)
        {
            _libraryDb.AddBook(book);
        }

        public void RemoveBook(string isbn)
        {
            Book book = _libraryDb.GetBookFromISBN(isbn);
            if(book != null)
            {
                IEnumerable<BookUser> usersWithBook = _libraryDb.GetUsersWithBook(book);    
                if(usersWithBook.Any())
                {
                    foreach(var user in usersWithBook)
                    {
                        user.setAlertUser(true);
                    }
                    throw new Exception("This book is on loan so can't be removed");
                }
                _libraryDb.RemoveBook(book);
            }
            else
            {
                throw new ArgumentException("Book with given ISBN does not exist");
            }
        }

        public void BorrowBook(string isbn, BookUser user)
        {
            Book book = _libraryDb.GetBookFromISBN(isbn);
            if(book.IsAvailable())              
            {
                book.borrowBook();
                user.addBookToCollection(book);

            }
            else
            {
                throw new Exception("Book is not available");
            }
        }

        public void ReturnBook(string isbn, BookUser user)
        {
            Book book = _libraryDb.GetBookFromISBN(isbn);
            book.returnBook();
            user.removeBookFromCollection(book);
        }
    }
}
