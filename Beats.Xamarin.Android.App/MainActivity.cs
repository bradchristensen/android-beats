using Android.App;
using Android.Content.Res;
using Android.OS;
using Android.Support.V4.App;
using Android.Support.V4.Widget;
using Android.Views;
using Beats.Xamarin.Droid.App.Views;
using Fragment = Android.App.Fragment;

namespace Beats.Xamarin.Droid.App
{
    [Activity(Label = "@string/ApplicationName", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private DrawerLayout _drawerLayout;
        private ActionBarDrawerToggle _drawerToggle;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.DrawerLayout);

            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.ContentFrame, new LoginFragment())
                .Commit();

            _drawerLayout = FindViewById<DrawerLayout>(Resource.Id.DrawerLayout);

            ActionBar.SetDisplayHomeAsUpEnabled(true);
            ActionBar.SetHomeButtonEnabled(true);

            _drawerToggle = new MyActionBarDrawerToggle(this, _drawerLayout,
                Resource.Drawable.ic_menu_white_24dp,
                Resource.String.DrawerOpen,
                Resource.String.DrawerClosed
            );

            _drawerLayout.AddDrawerListener(_drawerToggle);
        }

        private class MyActionBarDrawerToggle : ActionBarDrawerToggle
        {
            MainActivity owner;

            public MyActionBarDrawerToggle(MainActivity activity, DrawerLayout layout, int imgRes, int openRes, int closeRes)
                : base(activity, layout, imgRes, openRes, closeRes)
            {
                owner = activity;
            }

            public override void OnDrawerClosed(View drawerView)
            {
                owner.ActionBar.Title = owner.Title;
                owner.InvalidateOptionsMenu();
            }

            public override void OnDrawerOpened(View drawerView)
            {
                owner.ActionBar.Title = owner.Title;
                owner.InvalidateOptionsMenu();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // The action bar home/up action should open or close the drawer.
            // ActionBarDrawerToggle will take care of this.
            if (_drawerToggle.OnOptionsItemSelected(item))
            {
                return true;
            }

            return true;
        }

        protected override void OnPostCreate(Bundle savedInstanceState)
        {
            base.OnPostCreate(savedInstanceState);
            // Sync the toggle state after onRestoreInstanceState has occurred.
            _drawerToggle.SyncState();
        }

        public override void OnConfigurationChanged(Configuration newConfig)
        {
            base.OnConfigurationChanged(newConfig);
            // Pass any configuration change to the drawer toggls
            _drawerToggle.OnConfigurationChanged(newConfig);
        }

        public void ReplaceFragment(Fragment fragment)
        {
            // Insert the fragment by replacing any existing fragment
            FragmentManager.BeginTransaction()
                .Replace(Resource.Id.ContentFrame, fragment)
                .AddToBackStack("dir")
                .Commit();
        }
    }
}
