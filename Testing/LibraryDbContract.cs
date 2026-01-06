using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassProject
{
    public interface ILibraryDbContract
    {
        public Book GetBookFromISBN(string isbn);
        public Book GetBookFromName(string Name);
        public void AddBook(Book book);
        IEnumerable<BookUser> GetUsersWithBook(Book book);
        void RemoveBook(Book book);
        BookUser GetBookUserFromName(string name);
    }
}
