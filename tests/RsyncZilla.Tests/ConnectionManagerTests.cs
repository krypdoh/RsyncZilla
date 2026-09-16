using System;
using System.IO;
using RsyncZilla.Services;
using Xunit;

namespace RsyncZilla.Tests
{
    public class ConnectionManagerTests
    {
        [Fact]
        public void SaveOrUpdate_ShouldPersistConnectionWithoutPassword()
        {
            var service = new ConnectionManagerService();
            var testHost = "test-server.org";
            var testUser = "deployuser";
            var testPort = 2222;

            service.SaveOrUpdate(testHost, testUser, testPort);

            var list = service.LoadConnections();
            var found = list.Find(c => c.Host == testHost && c.Username == testUser && c.Port == testPort);

            Assert.NotNull(found);
            Assert.Equal(testHost, found.Host);
            Assert.Equal(testUser, found.Username);
            Assert.Equal(testPort, found.Port);

            // Clean up
            service.DeleteConnection(found.Id);
            var afterDelete = service.LoadConnections();
            Assert.DoesNotContain(afterDelete, c => c.Id == found.Id);
        }

        [Fact]
        public void UpdatePaths_ShouldPersistLocalAndRemotePathsImmediately()
        {
            var service = new ConnectionManagerService();
            var testHost = "paths-test.org";
            var testUser = "pathuser";
            var testPort = 22;

            service.SaveOrUpdate(testHost, testUser, testPort);

            var initial = service.FindConnection(testHost, testUser, testPort);
            Assert.NotNull(initial);

            // Update paths immediately
            service.UpdatePaths(testHost, testUser, testPort, @"C:\MyLocal\Project", "/var/www/remote");

            var updated = service.FindConnection(testHost, testUser, testPort);
            Assert.NotNull(updated);
            Assert.Equal(@"C:\MyLocal\Project", updated.LastLocalPath);
            Assert.Equal("/var/www/remote", updated.LastRemotePath);

            // Clean up
            service.DeleteConnection(updated.Id);
        }

            [Fact]
            public void SaveOrUpdate_ShouldPersistAndClearSshKeyPath()
            {
                var service = new ConnectionManagerService();
                var testHost = "key-test.org";
                var testUser = "keyuser";
                const int testPort = 22;
                var keyPath = Path.Combine(Path.GetTempPath(), "id_test_key");

                try
                {
                    service.SaveOrUpdate(testHost, testUser, testPort, sshKeyPath: keyPath);

                    var saved = service.FindConnection(testHost, testUser, testPort);
                    Assert.NotNull(saved);
                    Assert.Equal(keyPath, saved.SshKeyPath);

                    service.SaveOrUpdate(testHost, testUser, testPort, sshKeyPath: string.Empty);

                    var cleared = service.FindConnection(testHost, testUser, testPort);
                    Assert.NotNull(cleared);
                    Assert.Equal(string.Empty, cleared.SshKeyPath);
                }
                finally
                {
                    var connection = service.FindConnection(testHost, testUser, testPort);
                    if (connection != null) service.DeleteConnection(connection.Id);
                }
            }
    }
}
