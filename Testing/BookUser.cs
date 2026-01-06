using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassProject
{
    public class BookUser
    {
        private string name;
        private int userId;
        private List<Book> borrowedBooks;
        private bool AlertUser = false;

        public BookUser(string name, int userId)
        {
            this.name = name;
            this.userId = userId;
            this.borrowedBooks = new List<Book>();
        }

        public string getName() { return name; }
        public int getUserId() { return userId; }

        public List<Book> getBorrowedBooks() { return borrowedBooks; }

        public void addBookToCollection(Book book)
        {
            borrowedBooks.Add(book);
        }

        public void removeBookFromCollection(Book book)
        {
            borrowedBooks.Remove(book);
        }

        public void setAlertUser(bool alert)
        {
            AlertUser = alert;
        }

        public bool getAlertUser()
        {
            return AlertUser;
        }
    }

}
