using Beats.Xamarin.Datastore.Models;
using Realms;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Beats.Xamarin.Datastore
{
    public class Repository
    {
        private Realm _realm;

        private IEnumerable<LoginDetails> _loginDetails;

        public LoginDetails LoginDetails
        {
            get
            {
                return _loginDetails.FirstOrDefault();
            }
        }

        public Repository()
        {
            var config = new RealmConfiguration("myAppDb.realm");
            System.Diagnostics.Debug.WriteLine(config.DatabasePath);
            _realm = Realm.GetInstance(config);

            _loginDetails = _realm.All<LoginDetails>()
                .ToNotifyCollectionChanged(e =>
                {
                    System.Diagnostics.Debug.WriteLine(e);
                }) as IEnumerable<LoginDetails>;
        }

        public void UpdateLoginDetails(LoginDetails newLoginDetails)
        {
            using (var transaction = _realm.BeginWrite())
            {
                var loginDetails = LoginDetails ?? _realm.CreateObject<LoginDetails>();

                loginDetails.Username = newLoginDetails.Username ?? loginDetails.Username;
                loginDetails.Password = newLoginDetails.Password ?? loginDetails.Password;
                loginDetails.ServerUrl = newLoginDetails.ServerUrl ?? loginDetails.ServerUrl;
                loginDetails.SessionId = newLoginDetails.SessionId ?? loginDetails.SessionId;

                transaction.Commit();
            }
        }
    }
}
