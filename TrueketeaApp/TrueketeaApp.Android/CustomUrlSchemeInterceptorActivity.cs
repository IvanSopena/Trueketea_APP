using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using TrueketeaApp.AuthHelpers;


//coigo poporciondo por https://github.com/xamarin/Essentials/tree/develop/Samples/Sample.Server.WebAuthenticator & https://www.youtube.com/watch?v=ZCPENg5wP8o
namespace TrueketeaApp.Droid
{
	[Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
	[IntentFilter(
	new[] { Intent.ActionView },
	Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
	DataSchemes = new[] { "com.googleusercontent.apps.365832877438-7emiqvq0t61nqshae8mm5is4pkal1h04" },
	DataPath = "/oauth2redirect")]
	public class CustomUrlSchemeInterceptorActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Convert Android.Net.Url to Uri
			var uri = new Uri(Intent.Data.ToString());

			// Load redirectUrl page
			AuthenticationState.Authenticator.OnPageLoading(uri);

			//Finish();
			var intent = new Intent(this, typeof(MainActivity));

			intent.SetFlags(ActivityFlags.ClearTop | ActivityFlags.SingleTop);

			StartActivity(intent);

			this.Finish();
		}
	}
}