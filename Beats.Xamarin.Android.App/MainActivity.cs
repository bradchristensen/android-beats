using Android.App;
using Android.OS;
using Android.Widget;
using Beats.Xamarin.Android.App.Exceptions;
using Beats.Xamarin.Android.App.Helpers;
using Beats.Xamarin.Datastore;
using Beats.Xamarin.Datastore.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Beats.Xamarin.Android.App
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private string _server;
        private string _username;
        private string _password;
        private string _sessionId;
        private DateTimeOffset _sessionExpires;

        private Repository _repository;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            _repository = new Repository();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            Button loginButton = FindViewById<Button>(Resource.Id.ButtonLogin);
            EditText serverField = FindViewById<EditText>(Resource.Id.EditTextServer);
            EditText usernameField = FindViewById<EditText>(Resource.Id.EditTextUsername);
            EditText passwordField = FindViewById<EditText>(Resource.Id.EditTextPassword);
            TextView textViewLoginStatus = FindViewById<TextView>(Resource.Id.TextViewLoginStatus);

            serverField.Text = _repository.LoginDetails?.ServerUrl ?? "";
            usernameField.Text = _repository.LoginDetails?.Username ?? "";
            passwordField.Text = _repository.LoginDetails?.Password ?? "";
            _sessionId = _repository.LoginDetails?.SessionId;

            _server = serverField.Text;
            _username = usernameField.Text;
            _password = passwordField.Text;

            serverField.TextChanged += delegate
            {
                _server = serverField.Text;
                EnableLoginButton();
            };
            usernameField.TextChanged += delegate
            {
                _username = usernameField.Text;
                EnableLoginButton();
            };
            passwordField.TextChanged += delegate
            {
                _password = passwordField.Text;
                EnableLoginButton();
            };

            loginButton.Click += async delegate
            {
                loginButton.Text = "Logging in...";
                loginButton.Enabled = false;
                textViewLoginStatus.Text = "";

                try
                {
                    var cookie = await Authenticate(_server, _username, _password);

                    _sessionId = cookie.Value;
                    _sessionExpires = new DateTimeOffset(cookie.Expires);

                    _repository.UpdateLoginDetails(new LoginDetails
                    {
                        Username = _username,
                        Password = _password,
                        ServerUrl = _server,
                        SessionExpires = _sessionExpires,
                        SessionId = _sessionId,
                    });

                    loginButton.Text = "Logged in!";
                    loginButton.Enabled = false;
                    textViewLoginStatus.Text = _sessionId;
                }
                catch (LoginFailedException)
                {
                    EnableLoginButton();
                    textViewLoginStatus.Text = "";
                    textViewLoginStatus.Text = "Failed to login.";
                }
            };
        }

        private void EnableLoginButton()
        {
            Button loginButton = FindViewById<Button>(Resource.Id.ButtonLogin);
            loginButton.Text = GetString(Resource.String.LoginButton);
            loginButton.Enabled = true;
        }

        private static async Task<Cookie> Authenticate(string server, string username, string password)
        {
            using (var client = new HttpServiceHelper())
            {
                var response = await client.PostAsync(server, new Dictionary<string, string>
                {
                    ["username"] = username,
                    ["password"] = password,
                    ["login"] = "login",
                });

                var content = await response.Content.ReadAsStringAsync();

                if (content.Contains("<title>CherryMusic | Login</title>"))
                {
                    throw new LoginFailedException();
                }

                return response.Cookies["session_id"];
            }
        }
    }
}
