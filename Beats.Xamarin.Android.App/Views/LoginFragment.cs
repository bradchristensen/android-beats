using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Beats.Xamarin.Datastore;
using Beats.Xamarin.Datastore.Models;
using Beats.Xamarin.WebApiClient;
using Beats.Xamarin.WebApiClient.Exceptions;
using System;

namespace Beats.Xamarin.Droid.App.Views
{
    public class LoginFragment : Fragment
    {
        private string _server;
        private string _username;
        private string _password;
        private string _sessionId;
        private DateTimeOffset _sessionExpires;

        private Repository _repository;
        private CherryMusicClient _cherryMusicClient;

        private View _rootView;

        public LoginFragment()
        {
            _repository = new Repository();
            _cherryMusicClient = new CherryMusicClient();
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle bundle)
        {
            _rootView = inflater.Inflate(Resource.Layout.LoginFragment, container, false);

            Button loginButton = _rootView.FindViewById<Button>(Resource.Id.ButtonLogin);
            EditText serverField = _rootView.FindViewById<EditText>(Resource.Id.EditTextServer);
            EditText usernameField = _rootView.FindViewById<EditText>(Resource.Id.EditTextUsername);
            EditText passwordField = _rootView.FindViewById<EditText>(Resource.Id.EditTextPassword);
            TextView textViewLoginStatus = _rootView.FindViewById<TextView>(Resource.Id.TextViewLoginStatus);

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
                    var cookie = await _cherryMusicClient.Authenticate(_server, _username, _password);

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

                    ((App.MainActivity)Activity).ReplaceFragment(new DirectoryListFragment());
                }
                catch (LoginFailedException)
                {
                    EnableLoginButton();
                    textViewLoginStatus.Text = "";
                    textViewLoginStatus.Text = "Failed to login.";
                }
            };

            return _rootView;
        }

        private void EnableLoginButton()
        {
            Button loginButton = _rootView.FindViewById<Button>(Resource.Id.ButtonLogin);
            loginButton.Text = GetString(Resource.String.LoginButton);
            loginButton.Enabled = true;
        }
    }
}