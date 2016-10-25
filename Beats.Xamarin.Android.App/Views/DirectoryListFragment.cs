using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Beats.Xamarin.Datastore;
using Beats.Xamarin.WebApiClient;
using Beats.Xamarin.WebApiClient.Contracts.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Android.Support.V7.Widget.RecyclerView;

namespace Beats.Xamarin.Android.App.Views
{
    public class DirectoryListFragment : Fragment
    {
        private Repository _repository;
        private CherryMusicClient _cherryMusicClient;
        private View _rootView;
        private string _currentDirectory = null;

        public DirectoryListFragment() { }

        public DirectoryListFragment(string currentDirectory)
        {
            _currentDirectory = currentDirectory;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            _rootView = inflater.Inflate(Resource.Layout.DirectoryListFragment, container, false);

            var recycler = _rootView.FindViewById<RecyclerView>(Resource.Id.DirectoryListRecycler);
            recycler.HasFixedSize = true;
            recycler.SetLayoutManager(new LinearLayoutManager(Activity));

            _repository = new Repository();
            var deets = _repository.LoginDetails;
            _cherryMusicClient = new CherryMusicClient(deets.SessionId, deets.ServerUrl);

            Task<List<DirectoryListingItem>> task = _currentDirectory == null ?
                _cherryMusicClient.GetAllDirectories() :
                _cherryMusicClient.GetDirectoryListing(_currentDirectory, "");

            task.ContinueWith(t =>
            {
                // TODO: Only run if the UI still exists
                Activity.RunOnUiThread(() =>
                {
                    var adapter = new DirectoryAdapter(t.Result, Activity);
                    recycler.SetAdapter(adapter);

                    adapter.ItemClick += (sender, position) =>
                    {
                        var item = t.Result[position];
                        if (item.Type == "dir")
                        {
                            ((MainActivity)Activity).ReplaceFragment(new DirectoryListFragment(item.Path));
                        }
                    };
                });
            });

            return _rootView;
        }
    }

    public class DirectoryAdapter : RecyclerView.Adapter
    {
        private List<DirectoryListingItem> _directoryListing;
        private Activity _activity;

        public DirectoryAdapter(List<DirectoryListingItem> directoryListing, Activity activity)
        {
            _directoryListing = directoryListing;
            _activity = activity;
        }

        public override int ItemCount => _directoryListing.Count;

        public event EventHandler<int> ItemClick;

        public override void OnBindViewHolder(ViewHolder holderRaw, int position)
        {
            var item = _directoryListing[position];
            var holder = (DirectoryAdapterViewHolder)holderRaw;
            holder.TextView.Text = item.Label;
        }

        private void OnClick(int position)
        {
            ItemClick?.Invoke(this, position);
        }

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var vi = LayoutInflater.From(parent.Context);
            var v = vi.Inflate(Resource.Layout.DirectoryListListItem, parent, false);
            var tv = v.FindViewById<TextView>(Resource.Id.DirectoryListItemText);
            return new DirectoryAdapterViewHolder(tv, OnClick);
        }

        public class DirectoryAdapterViewHolder : ViewHolder
        {
            public readonly TextView TextView;
            public DirectoryAdapterViewHolder(TextView v, Action<int> listener) : base(v)
            {
                TextView = v;
                v.Click += (sender, e) => listener(AdapterPosition);
            }
        }
    }
}