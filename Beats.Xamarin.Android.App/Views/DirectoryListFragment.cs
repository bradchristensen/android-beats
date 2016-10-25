using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Beats.Xamarin.Datastore;
using Beats.Xamarin.WebApiClient;
using Beats.Xamarin.WebApiClient.Contracts.Response;
using System.Collections.Generic;
using static Android.Support.V7.Widget.RecyclerView;

namespace Beats.Xamarin.Android.App.Views
{
    public class DirectoryListFragment : Fragment
    {
        private Repository _repository;
        private CherryMusicClient _cherryMusicClient;
        private View _rootView;

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

            _cherryMusicClient.GetAllDirectories().ContinueWith(task =>
            {
                // TODO: Only run if the UI still exists
                Activity.RunOnUiThread(() =>
                {
                    recycler.SetAdapter(new DirectoryAdapter(task.Result));
                });
            });

            return _rootView;
        }
    }

    public class DirectoryAdapter : RecyclerView.Adapter
    {
        public List<DirectoryListingItem> _directoryListing;

        public DirectoryAdapter(List<DirectoryListingItem> directoryListing)
        {
            _directoryListing = directoryListing;
        }

        public override int ItemCount
        {
            get
            {
                return _directoryListing.Count;
            }
        }

        public override void OnBindViewHolder(ViewHolder holderRaw, int position)
        {
            var holder = (DirectoryAdapterViewHolder)holderRaw;
            holder.TextView.Text = _directoryListing[position].Label;
        }

        public override ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var vi = LayoutInflater.From(parent.Context);
            var v = vi.Inflate(Resource.Layout.DirectoryListListItem, parent, false);
            var tv = v.FindViewById<TextView>(Resource.Id.DirectoryListItemText);
            return new DirectoryAdapterViewHolder(tv);
        }

        public class DirectoryAdapterViewHolder : ViewHolder
        {
            public readonly TextView TextView;
            public DirectoryAdapterViewHolder(TextView v) : base(v)
            {
                TextView = v;
            }
        }
    }
}