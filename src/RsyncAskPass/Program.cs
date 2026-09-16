using System;

namespace RsyncAskPass
{
    internal class Program
    {
        static int Main(string[] args)
        {
                var passphrase = Environment.GetEnvironmentVariable("SSH_KEY_PASSPHRASE");
                if (!string.IsNullOrEmpty(passphrase))
                {
                    Console.WriteLine(passphrase);
                    return 0;
                }

                var password = Environment.GetEnvironmentVariable("RSYNC_PASSWORD");
                if (!string.IsNullOrEmpty(password))
            {
                Console.WriteLine(password);
                return 0;
            }
            return 1;
        }
    }
}
