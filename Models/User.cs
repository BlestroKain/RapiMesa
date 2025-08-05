namespace Rapimesa
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public string NombreCompleto { get; set; }
        public int FailedAttempts { get; set; }
        public bool IsLocked { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
    }
}
