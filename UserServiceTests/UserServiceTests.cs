using System.Runtime.CompilerServices;
using Testing;

namespace UserServiceTests
{

    [TestClass]
    public sealed class RegisterUserTests
    {
        private UserService _userService;

        public RegisterUserTests()
        {
            _userService = new UserService();
        }


        [TestMethod]
        public void Does_Username_Have_Value()
        {
            Assert.Throws<ArgumentException>(() => _userService.RegisterUser("", "Password1!", "admin"));
        }

        [TestMethod]
        public void Is_Username_Unique()
        {
            _userService.RegisterUser("testuser", "Password1!", "admin");
            Assert.Throws<ArgumentException>(() => _userService.RegisterUser("testuser", "Password1!", "admin"));
        }

        [TestMethod]
        public void Is_Password_Length_Valid()
        {
            string shortPassword = new string('a', _userService.PasswordMinLength - 1);

            Assert.Throws<ArgumentException>(() => _userService.RegisterUser("testuser", shortPassword, "admin"));
        }

        [TestMethod]
        public void Does_Password_Contain_Digit()
        {
            Assert.Throws<ArgumentException>(() => _userService.RegisterUser("testuser", "Password!", "admin"));
        }

        [TestMethod]
        public void Does_Password_Contain_Special_Character()
        {
            Assert.Throws<ArgumentException>(() => _userService.RegisterUser("testuser", "Password1", "admin"));
        }

        [TestMethod]
        public void Is_User_Role_Valid()
        {
            Assert.Throws<ArgumentException>(() => _userService.RegisterUser("testuser", "Password1!", "admiral"));
        }

        [TestMethod]
        public void Can_Register_Valid_User()
        {
            _userService.RegisterUser("validuser", "Password1!", "admin");

            User user = _userService.getUser("validuser");

            Assert.IsNotNull(user);
        }

    }

    [TestClass]
    public sealed class LoginUserTests
    {
        private UserService _userService;
        public LoginUserTests()
        {
            _userService = new UserService();
        }

        [TestMethod]
        public void Can_Login_Valid_User()
        {
            _userService.RegisterUser("validuser", "Password1!", "admin");
            Assert.IsTrue(_userService.LoginUser("validuser", "Password1!"));
        }

        [TestMethod]
        public void Password_Mismatch_Fails_Login()
        {
            _userService.RegisterUser("validuser", "Password1!", "admin");
            Assert.IsFalse(_userService.LoginUser("validuser", "Password"));
        }


        [TestMethod]
        public void Cannot_Login_Non_Existent_User()
        {
            Assert.Throws<KeyNotFoundException>(() => _userService.LoginUser("invaliduser", "Password1!"));
        }
    }


    [TestClass]
    public sealed class HasAccessTests
    {
        private UserService _userService;
        public HasAccessTests()
        {
            _userService = new UserService();
        }

        [TestMethod]
        public void Does_Username_Have_Value()
        {
            Assert.Throws<ArgumentException>(() => _userService.HasAccess("", "admin"));
        }


        [TestMethod]
        public void Does_Role_Have_Value()
        {
            Assert.Throws<ArgumentException>(() => _userService.HasAccess("user", ""));
        }

        [TestMethod]
        public void User_Matched_Role_Returns_True()
        {
            _userService.RegisterUser("validuser", "Password1!", "admin");
            User user = _userService.getUser("validuser");

            Assert.IsTrue(_userService.HasAccess("validuser", "admin"));

        }

        [TestMethod]
        public void User_Mismatched_Role_Returns_False()
        {
            _userService.RegisterUser("validuser", "Password1!", "admin");
            User user = _userService.getUser("validuser");

            Assert.IsFalse(_userService.HasAccess("validuser", "trainer"));

        }
    }   

}
