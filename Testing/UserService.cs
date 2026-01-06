namespace Testing
{
    public class UserService
    {
        private Dictionary<String, User> users = new Dictionary<String, User>();

        private string[] passwordSpecialChars = new string[] { "!", "@", "#", "$", "%", "^", "&", "*" };
        private string[] specialRoles = new string[] { "manager", "admin", "support", "trainer" };
        public int PasswordMinLength { get; } = 8;

        public void RegisterUser(String username, String password, String role)
        {
            //check username has value
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be null or empty");
            }

            //check username is unique
            if (users.ContainsKey(username))
            {
                throw new ArgumentException("Username already exists");
            }

            //check password length
            if (string.IsNullOrWhiteSpace(password) || password.Length < PasswordMinLength)
            {
                throw new ArgumentException("Password must be at least " + PasswordMinLength + " characters long");
            }

            //check password contains digit
            if (!password.Any(char.IsDigit))
            {
                throw new ArgumentException("Password must contain at least one number");
            }

            //check role has value
            if (string.IsNullOrWhiteSpace(role))
            {
                throw new ArgumentException("Role cannot be null or empty");
            }

            //check role is one of the special roles
            if (!specialRoles.Contains(role))
            {
                throw new ArgumentException("Role must be one of the special roles: " + string.Join(", ", specialRoles));
            }

            //check password contains special character
            if (!passwordSpecialChars.Any(password.Contains))
            {
                throw new ArgumentException($"Password must contain at least one special character from the following: {string.Join(", ", passwordSpecialChars)}");
            }
            
            users.Add(username, new User(username, password, role));
            
            // 	Then test username is added
            if (!users.ContainsKey(username))
            {
                throw new Exception("User registration failed");
            }
        }

        public bool LoginUser(string username, string password)
        {
            //check username has value
            if (string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be null or empty");
            }

            //get the user
            User user = getUser(username);

            //check password matches
            if (user.Password == password)
            {
                return true;
            }

            //return false if password does not match
            return false;
        }

        public bool HasAccess(string username, string requiredRole)
        {

            //check username has value
            if(string.IsNullOrWhiteSpace(username))
            {
                throw new ArgumentException("Username cannot be null or empty");
            }

            //check requiredRole has value  
            if (string.IsNullOrWhiteSpace(requiredRole))
            {
                throw new ArgumentException("Required role cannot be null or empty");
            }

            User user = getUser(username);

            //check that user role matches required role
            return user.Role == requiredRole;
        }

        public User getUser(string username)
        {
            if (users.ContainsKey(username))
            {
                return users[username];
            }
            else
            {
                throw new KeyNotFoundException("User not found");
            }
        }
    }


    public class User
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public string Role { get; private set; }

        public User(String username, String password, String role)
        {
            this.Username = username;
            this.Password = password;
            this.Role = role;
        }
    }

}
