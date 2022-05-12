using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using TrueketeaApp.Models;


//coigo poporciondo por https://github.com/xamarin/Essentials/tree/develop/Samples/Sample.Server.WebAuthenticator & https://www.youtube.com/watch?v=ZCPENg5wP8o
namespace TrueketeaApp.AppConstant
{
	public class Constants  
	{
		public static string AppName = "OAuthNativeFlow";
		public static string FBIdApp = "432593718656396";
		public static string UserLoginId = "";
		public static string UserLoginEmail = "";
		public static string UserDirection = "";
		public static string UserProfilePhoto = "";
		public static string UserName = "";
		public static bool _isVisbleOptions;
		public static string UserIdInfo;
		public static string ActualView;
		

		// OAuth
		// For Google login, configure at https://console.developers.google.com/
		public static string iOSClientId = "365832877438-3i6pp19v9eaq7i1hngk3qhm61d1je1dr.apps.googleusercontent.com";
		public static string AndroidClientId = "365832877438-7emiqvq0t61nqshae8mm5is4pkal1h04.apps.googleusercontent.com";

		// These values do not need changing
		public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
		public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
		public static string AccessTokenUrl = "https://www.googleapis.com/oauth2/v4/token";
		public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

		public static string FBScope = "email";
		public static string FBAuthorizeUrl = "https://www.facebook.com/dialog/oauth?client_id="
				+ FBIdApp
				+ "&display=popup&response_type=token&redirect_uri=http://www.facebook.com/connect/login_success.html";
		
		public static string FBAccessTokenUrl = "https://www.facebook.com/connect/login_success.html";
		public static string FBUserInfoUrl = "https://graph.facebook.com/me?fields=email&access_token={accessToken}";

		// Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
		public static string iOSRedirectUrl = "com.googleusercontent.apps.365832877438-3i6pp19v9eaq7i1hngk3qhm61d1je1dr:/oauth2redirect";
		public static string AndroidRedirectUrl = "com.googleusercontent.apps.365832877438-7emiqvq0t61nqshae8mm5is4pkal1h04:/oauth2redirect";

		public static string FBRedirectUrl = "https://www.facebook.com/connect/login_success.html";


		public static ObjectId ProductId;



	}
}
