using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TicTacToe
{
	[Activity (Label = "TicTacToe", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our buttons from the layout resource,
			// and attach events
			Button buttonX = FindViewById<Button> (Resource.Id.StartX);
			
			buttonX.Click += delegate {
				//create the activity intent
				var Game = new Intent( this, typeof(GameScreen));
				//insert the player choice selection
				Game.PutExtra ("Player", (int)ePlayers.Player_X);
				//start
				StartActivity(Game);
			};
			Button buttonO = FindViewById<Button> (Resource.Id.StartO);

			buttonO.Click += delegate {
				//create the activity intent
				var Game = new Intent(this, typeof(GameScreen));
				//insert the player choice selection
				Game.PutExtra ("Player", (int)ePlayers.Player_O);
				//start
				StartActivity(Game);
			};
		}
	}
}


