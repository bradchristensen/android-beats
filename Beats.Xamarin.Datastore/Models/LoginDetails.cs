using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Realms;
using System.ComponentModel;

namespace Beats.Xamarin.Datastore.Models
{
    public class LoginDetails : RealmObject, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string Username { get; set; }
        public string Password { get; set; }
        public string ServerUrl { get; set; }
        public string SessionId { get; set; }
        public DateTimeOffset SessionExpires { get; set; }
    }
}