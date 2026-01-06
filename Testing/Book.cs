using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClassProject
{
    public class Book
    {
        private string title;
        private string isbn;
        private string author;
        private int copiesInStock;

        public Book(string author, string title, string isbn, int copiesInStock)
        {
            this.title = title;
            this.isbn = isbn;
            this.author = author;
            this.copiesInStock = copiesInStock;
        }

        

        public string getTitle() { return title; }
        public string getIsbn() { return isbn; }
        public string getAuthor() { return author; }
        public bool IsAvailable() { return this.copiesInStock > 0 ? true : false; }
        public void borrowBook() 
        {
            if (this.copiesInStock <= 0)
            {
                throw new InvalidOperationException("No copies available to borrow");
            }
            this.copiesInStock--;
        }
        public void returnBook() { this.copiesInStock++; }
    }

}
