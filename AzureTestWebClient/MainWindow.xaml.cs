using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AzureTestWebClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            var authContext = new AuthenticationContext("https://login.windows.net/microsoft.com");
            AuthenticationResult result = null;
            try
            {
               result = authContext.AcquireToken("http://anandmsraazuretest.azurewebsites.net/", "ba1166f6-1dba-4794-8e7b-1e326c63085e", new Uri("https://MSRA"), PromptBehavior.Auto);

                //// A valid token is in the cache - get the To Do list.
                HttpClient httpClient = new HttpClient();
              httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);
               HttpResponseMessage response = await httpClient.GetAsync("http://anandmsraazuretest.azurewebsites.net/odata/MSRAQuery");
            }
            catch (AdalException ex)
            {
                if (ex.ErrorCode == "user_interaction_required")
                {
                    // There are no tokens in the cache.  Proceed without calling the To Do list service.
                }
                else
                {
                    // An unexpected error occurred.
                    string message = ex.Message;
                    if (ex.InnerException != null)
                    {
                        message += "Inner Exception : " + ex.InnerException.Message;
                    }
                    MessageBox.Show(message);
                }
                return;
            }
        }
    }
}
