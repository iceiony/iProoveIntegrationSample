using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using Sample.Properties;

namespace Sample
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public partial class AuthenticateForm : Form
    {
        private readonly string _cmsUrl;
        private readonly string _pageBody;
        private readonly AsyncSubject<AuthenticationResult> _authenticationSubject = new AsyncSubject<AuthenticationResult>(); 

        public AuthenticateForm()
        {
            InitializeComponent();
            _cmsUrl = ConfigurationManager.AppSettings["CmsUrl"];

            var lastUser = ((string)@settings.Default["last_iproove_user"]);
            if (string.IsNullOrEmpty(lastUser)) lastUser = "";

           

            var iProoveUrl = ConfigurationManager.AppSettings["iProoveUrl"];
            _pageBody = File.ReadAllText("./iProovePage.html")
                            .Replace("%IPROOV_URL%", iProoveUrl)
                            .Replace("%LAST_IPROOV_USER%", lastUser);

            AutenticationBrowser.Hide();
            AutenticationBrowser.ObjectForScripting = this;

            this.FormClosing += OnClosing;
        }

        private void Authenticate_Load(object sender, EventArgs e)
        {
            AutenticationBrowser.Navigate(_cmsUrl);


            Observable.FromEventPattern(AutenticationBrowser, "DocumentCompleted")
                .ObserveOn(SynchronizationContext.Current)
                .Take(1)
                .Subscribe((t) =>
                {
                    AutenticationBrowser.Document.Body.InnerHtml = _pageBody;

                    AutenticationBrowser.Document.InvokeScript("eval", new object[] { File.ReadAllText("./JavaScript/OverrideConsoleLog.js") });
                    AutenticationBrowser.Document.InvokeScript("eval", new object[] { File.ReadAllText("./JavaScript/RemoveHeaderTags.js") });
                    AutenticationBrowser.Document.InvokeScript("eval", new object[] { File.ReadAllText("./JavaScript/SetUpiProoveMessaging.js") });

                    AutenticationBrowser.Show();
                });
        }

        public void Log(string logMessage)
        {
            Debugger.Log(1, "JS LOG",  logMessage + "\n");
        }

        public void NotifyAuthenticationResult(string result, string token, string authenticatedUser, string userAgent)
        {
            Debugger.Log(1, "Authentication Result ", "Authentication:"+ result + "\n");
            if (result.ToLower().Equals("passed"))
            {
                @settings.Default["last_iproove_user"] = authenticatedUser;

                _authenticationSubject.OnNext(new AuthenticationResult()
                {
                    Token = token,
                    iProoveUserId = authenticatedUser,
                    MachineIP = GetLocalIp(),
                    UserAgent = userAgent
                });
                _authenticationSubject.OnCompleted();
                Close();
            }
        }

        private string GetLocalIp()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork && ip.ToString().StartsWith("10"))
                {
                    return ip.ToString();
                }
            }

            return "";
        }

        public IObservable<AuthenticationResult> AuthenticationResult()
        {
            return _authenticationSubject;
        }

        private void OnClosing(object sender, FormClosingEventArgs e)
        {
            @settings.Default.Save();

            AutenticationBrowser.Dispose();

            if (!_authenticationSubject.IsCompleted)
            {
                _authenticationSubject.OnNext(new AuthenticationResult(false));
                _authenticationSubject.OnCompleted();
            }
        }
    }
}
