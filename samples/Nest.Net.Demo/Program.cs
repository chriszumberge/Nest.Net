using Microsoft.Win32;
using Nest.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nest.Net.Demo
{
    class Program
    {
        const string PRODUCT_ID = "";
        const string PRODUCT_SECRET = "";

        static void Main(string[] args)
        {
            var accessToken = String.Empty;

            // Get location of executable
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "accesstoken.txt");

            if (File.Exists(file))
                accessToken = File.ReadAllText(file);

            var client = new NestClient(PRODUCT_ID, PRODUCT_SECRET);

            if (!String.IsNullOrEmpty(accessToken))
                client.SetAccessToken(accessToken, DateTime.UtcNow.AddYears(1));

            // TODO load access token

            if (!client.IsAuthValid())
            {
                GetAndStoreAccessToken(file, client).Wait();
            }

            NestDataModel result = client.GetNestData().Result;

        }

        private static async Task GetAndStoreAccessToken(string file, NestClient client)
        {
            try
            {
                Console.WriteLine("Login to authorize and enter your pin here.");

                // Get the URl to load in a browser for the PIN
                var authUrl = client.GetAuthorizationUrl();

                string browser = GetDefaultBrowser();
                Process processHandle;
                if (!String.IsNullOrEmpty(browser))
                    processHandle = Process.Start(browser, authUrl);
                else
                    processHandle = Process.Start(authUrl);

                Console.Write("Enter your PIN:");

                // Read back the pin
                var authToken = Console.ReadLine();

                // Close the started process
                if (processHandle != null && !processHandle.HasExited)
                {
                    processHandle.Kill();
                }

                // get an access token to use with the API
                await client.GetAccessToken(authToken);

                File.WriteAllText(file, client.AccessToken);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }

        private static string GetDefaultBrowser()
        {
            string browser = string.Empty;
            RegistryKey key = null;
            try
            {
                key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command");

                //trim off quotes
                if (key != null)
                {
                    browser = key.GetValue(null).ToString().ToLower().Trim(new[] { '"' });
                }
                if (!browser.EndsWith("exe"))
                {
                    //get rid of everything after the ".exe"
                    browser = browser.Substring(0, browser.LastIndexOf(".exe", StringComparison.InvariantCultureIgnoreCase) + 4);
                }
            }
            finally
            {
                if (key != null)
                {
                    key.Close();
                }
            }
            return browser;
        }
    }
}
