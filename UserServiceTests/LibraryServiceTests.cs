using ClassProject;
using Testing;

namespace TestProject;

[TestClass]
public class LibraryServiceTests
{
    private LibraryService _libraryService;
    private LibraryDbStub _libraryDb;

    public LibraryServiceTests()
    {
        _libraryDb = new LibraryDbStub();
        _libraryService = new LibraryService(_libraryDb);
    }


    [TestMethod]
    public void Getting_Non_Existent_Book_By_ISBN_Throws_Error()
    {
        Assert.Throws<ArgumentException>(() => _libraryService.GetBookByISBN("Fake book ISBN"));
    }

    [TestMethod]
    public void Getting_Non_Existent_Book_By_Name_Throws_Error()
    {
        Assert.Throws<ArgumentException>(() => _libraryService.GetBookByName("Fake book Name"));
    }

    [TestMethod]
    public void ReAdding_Book_With_Existing_ISBN_Throws_Error()
    {
        Book existingBook = new Book("New Author", "New Book", "1234567890", 3);
        Assert.Throws<ArgumentException>(() => _libraryService.AddBook(existingBook));
    }

    [TestMethod]
    public void Can_Borrow_And_Return_Book_Successfully()
    {
        BookUser user = _libraryDb.GetBookUserFromName("Charlie");
        string isbn = "0987654321";
        _libraryService.BorrowBook(isbn, user);
        Book borrowedBook = _libraryService.GetBookByISBN(isbn);
        Assert.IsFalse(borrowedBook.IsAvailable());
        _libraryService.ReturnBook(isbn, user);
        Book returnedBook = _libraryService.GetBookByISBN(isbn);
        Assert.IsTrue(returnedBook.IsAvailable());
    }

    [TestMethod]
    public void Unavailable_Book_Cannot_Be_Borrowed()
    {
        BookUser user1 = _libraryDb.GetBookUserFromName("Alice");
        BookUser user2 = _libraryDb.GetBookUserFromName("Bob");
        string isbn = "0987654321";
        _libraryService.BorrowBook(isbn, user1);
        Assert.Throws<Exception>(() => _libraryService.BorrowBook(isbn, user2));
    }

    [TestMethod]
    public void Borrowing_And_Returning_Book_Updates_User_Borrowed_Books()
    {
        BookUser user = _libraryDb.GetBookUserFromName("Bob");
        string isbn = "0987654321";
        _libraryService.BorrowBook(isbn, user);
        Assert.Contains(_libraryService.GetBookByISBN(isbn), user.getBorrowedBooks());
        _libraryService.ReturnBook(isbn, user);
        Assert.DoesNotContain(_libraryService.GetBookByISBN(isbn), user.getBorrowedBooks());
    }

    [TestMethod]
    public void Can_Add_New_Book_Successfully()
    {
        Book newBook = new Book("New Author", "New Book", "1122334455", 5);
        _libraryService.AddBook(newBook);
        Book retrievedBook = _libraryService.GetBookByISBN("1122334455");
        Assert.IsNotNull(retrievedBook);
        Assert.AreEqual("New Book", retrievedBook.getTitle());
    }

    [TestMethod]
    public void Removing_Book_On_Loan_Throws_Error_And_Alerts_Users()
    {
        BookUser user = _libraryDb.GetBookUserFromName("Alice");
        string isbn = "0987654321";
        _libraryService.BorrowBook(isbn, user);
        Exception ex = Assert.Throws<Exception>(() => _libraryService.RemoveBook(isbn));
        Assert.IsTrue(user.getAlertUser());
    }
}