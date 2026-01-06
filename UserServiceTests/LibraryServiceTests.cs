using ClassProject;
using Moq;
using Testing;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;

namespace TestProject;

[TestClass]
public class LibraryServiceTests
{
    private LibraryService _libraryService;
    private Mock<ILibraryDbContract> _libraryMockdb;

    public LibraryServiceTests()
    {
        _libraryMockdb = new Mock<ILibraryDbContract>();
        _libraryService = new LibraryService(_libraryMockdb.Object);
    }


    [TestMethod]
    public void Getting_Non_Existent_Book_By_ISBN_Throws_Error()
    {
        _libraryMockdb.Setup(db => db.GetBookFromISBN("Fake book ISBN"))
                  .Throws<ArgumentException>();

        Assert.Throws<ArgumentException>(() => _libraryService.GetBookByISBN("Fake book ISBN"));
    }

    [TestMethod]
    public void Getting_Non_Existent_Book_By_Name_Throws_Error()
    {
        _libraryMockdb.Setup(db => db.GetBookFromName("Fake book Name"))
                  .Throws<ArgumentException>();

        Assert.Throws<ArgumentException>(() => _libraryService.GetBookByName("Fake book Name"));
    }

    [TestMethod]
    public void ReAdding_Book_With_Existing_ISBN_Throws_Error()
    {
        Book existingBook = new Book("New Author", "New Book", "1234567890", 3);
        
        _libraryMockdb.Setup(db => db.AddBook(It.Is<Book>(b => b.getIsbn() == "1234567890")))
                  .Throws<ArgumentException>();

        Assert.Throws<ArgumentException>(() => _libraryService.AddBook(existingBook));
    }

    [TestMethod]
    public void Can_Borrow_And_Return_Book_Successfully()
    {
        BookUser user = new BookUser("Charlie", 3);
        Book newBook = new Book("New Author", "New Book", "1122334455", 1);
        string isbn = "1122334455";


        _libraryMockdb.Setup(db => db.GetBookUserFromName("Charlie"))
                      .Returns(user);

        _libraryMockdb.Setup(db => db.GetBookFromISBN(isbn))
                      .Returns(newBook);

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
        BookUser user1 = new BookUser("Alice", 1);
        BookUser user2 = new BookUser("Bob", 2);
        Book newBook = new Book("New Author", "New Book", "1122334455", 1);
        string isbn = "1122334455";

        _libraryMockdb.Setup(db => db.GetBookUserFromName("Charlie"))
              .Returns(user1);

        _libraryMockdb.Setup(db => db.GetBookFromISBN(isbn))
                      .Returns(newBook);

        _libraryService.BorrowBook(isbn, user1);


        Assert.Throws<Exception>(() => _libraryService.BorrowBook(isbn, user2));
    }

    [TestMethod]
    public void Borrowing_And_Returning_Book_Updates_User_Borrowed_Books()
    {
        BookUser user = new BookUser("Charlie", 3);
        Book newBook = new Book("New Author", "New Book", "1122334455", 5);
        string isbn = "1122334455";


        _libraryMockdb.Setup(db => db.GetBookUserFromName("Charlie"))
                      .Returns(user);

        _libraryMockdb.Setup(db => db.GetBookFromISBN(isbn))
                      .Returns(newBook);

        _libraryService.BorrowBook(isbn, user);

        Assert.Contains(_libraryService.GetBookByISBN(isbn), user.getBorrowedBooks());
        _libraryService.ReturnBook(isbn, user);
        Assert.DoesNotContain(_libraryService.GetBookByISBN(isbn), user.getBorrowedBooks());
    }

    [TestMethod]
    public void Can_Add_New_Book_Successfully()
    {
        Book newBook = new Book("New Author", "New Book", "1122334455", 5);
        
        _libraryMockdb.Setup(x => x.AddBook(It.IsAny<Book>()));
        _libraryMockdb.Setup(x => x.GetBookFromISBN("1122334455")).Returns(newBook);

        Book retrievedBook = _libraryService.GetBookByISBN("1122334455");
        Assert.IsNotNull(retrievedBook);
        Assert.AreEqual("New Book", retrievedBook.getTitle());
    }

    [TestMethod]
    public void Removing_Book_On_Loan_Throws_Error_And_Alerts_Users()
    {
        BookUser user = new BookUser("Charlie", 3);
        Book newBook = new Book("New Author", "New Book", "1122334455", 5);
        string isbn = "1122334455";

        _libraryMockdb.Setup(db => db.GetBookUserFromName("Charlie"))
                      .Returns(user);

        _libraryMockdb.Setup(db => db.GetBookFromISBN(isbn))
                      .Returns(newBook);

        _libraryService.BorrowBook(isbn, user);

        _libraryMockdb.Setup(db => db.GetUsersWithBook(newBook)).Returns(new List<BookUser> { user });

        Exception ex = Assert.Throws<Exception>(() => _libraryService.RemoveBook(isbn));
        Assert.IsTrue(user.getAlertUser());
    }
}