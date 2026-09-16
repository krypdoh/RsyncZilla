namespace RsyncZilla.Models
{
    public class ConnectionProfile
    {
        public string Host { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
            public string SshKeyPath { get; set; } = string.Empty;
            public string KeyPassphrase { get; set; } = string.Empty;
        public int Port { get; set; } = 22;
        public string RemoteInitialPath { get; set; } = string.Empty;
    }
}
