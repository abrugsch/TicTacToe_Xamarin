using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Widget;
using Android.Content;
using Android.Views.Animations;

namespace TicTacToe
{
	//game state enums
	enum ePlayers
	{
		NO_PLAYER,Player_X,Player_O
	};
	enum eGameState
	{
		Player_X_Turn, Player_O_Turn, Player_X_Win, Player_O_Win, Draw
	};

	[Activity (Label = "@string/TicTacToe")]			
	public class GameScreen : Activity
	{
		//init member vars
		private const int GRID_SIZE = 3;

		//starting player choice is arbitrary as it should be selected by the main scren
		private ePlayers _nextPlayer = ePlayers.Player_O;
		private eGameState _curState = eGameState.Player_O_Turn;

		//keep the result message as a member
		private AlertDialog.Builder _ResultDlg = null;

		//reference to the text view containing the "next player message"
		private TextView _playerText;

		//the internal representation of the board and each place's 3 possible states
		private ePlayers[,] _gameBoard = new ePlayers[GRID_SIZE,GRID_SIZE]{
			{ePlayers.NO_PLAYER,ePlayers.NO_PLAYER,ePlayers.NO_PLAYER},
			{ePlayers.NO_PLAYER,ePlayers.NO_PLAYER,ePlayers.NO_PLAYER},
			{ePlayers.NO_PLAYER,ePlayers.NO_PLAYER,ePlayers.NO_PLAYER}};

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);
			//associate the view with the class
			SetContentView (Resource.Layout.GameScreen);

			//init the result dialog
			_ResultDlg = new AlertDialog.Builder(this);
			_ResultDlg.SetPositiveButton("OK", delegate { 
				Finish ();
			});
			//disallow the back button
			_ResultDlg.SetCancelable(false);
			//pick up the passed in data
			int iPlayer = Intent.GetIntExtra ("Player",1);
			_nextPlayer = (ePlayers)iPlayer;

			_playerText = FindViewById<TextView> (Resource.Id.textPlayerTurn);

			ImageButton butTmp;
			//restore transient change data 
			if (bundle != null)
			{
				//fetch persisted data
				_nextPlayer = (ePlayers)bundle.GetInt("nextPlayer",(int)ePlayers.Player_O);
				_curState = (eGameState)bundle.GetInt("CurState", (int)eGameState.Player_O_Turn);

				//the board has to map board states with view cell id's to set the graphic and clickableness
				for (int x = 0; x<GRID_SIZE;x++)
				{
					for (int y=0;y<GRID_SIZE; y++)
					{
						string xy = x.ToString()+y.ToString();
						_gameBoard[x,y] = (ePlayers)bundle.GetInt("Board"+xy,(int)ePlayers.NO_PLAYER);
						if(_gameBoard[x,y] != ePlayers.NO_PLAYER)
						{
							//Convert numeric x,y positions into Cell view ID's
							int cellID = 0;
							switch(xy)
							{
							case "00":
								cellID = Resource.Id.Cell00;
								break;
							case "01":
								cellID = Resource.Id.Cell01;
								break;
							case "02":
								cellID = Resource.Id.Cell02;
								break;
							case "10":
								cellID = Resource.Id.Cell10;
								break;
							case "11":
								cellID = Resource.Id.Cell11;
								break;
							case "12":
								cellID = Resource.Id.Cell12;
								break;
							case "20":
								cellID = Resource.Id.Cell20;
								break;
							case "21":
								cellID = Resource.Id.Cell21;
								break;
							case "22":
								cellID = Resource.Id.Cell22;
								break;
							default:
								cellID=0;
								break;
							}
							if(cellID != 0)
							{
								//set the data to the button states
								butTmp = FindViewById<ImageButton>(cellID);
								butTmp.SetImageResource( _gameBoard[x,y] == ePlayers.Player_O?(Resource.Drawable.omark):(Resource.Drawable.xmark));
								butTmp.Clickable = false;
							}
						}
					}
				}

			}

			//set the click handlers to each of the buttons
			butTmp = FindViewById<ImageButton>(Resource.Id.Cell00);
			butTmp.Click += (o, e) => {
				Clicked(0,0, o);
			};
			butTmp = FindViewById<ImageButton>(Resource.Id.Cell01);
			butTmp.Click += (o, e) => {
				Clicked(0,1, o);
			};
			butTmp = FindViewById<ImageButton>(Resource.Id.Cell02);
			butTmp.Click += (o, e) => {
				Clicked(0,2, o);
			};
			butTmp = FindViewById<ImageButton>(Resource.Id.Cell10);
			butTmp.Click += (o, e) => {
				Clicked(1,0, o);
			};
			butTmp = FindViewById<ImageButton> (Resource.Id.Cell11);
			butTmp.Click += (o, e) => {
				Clicked(1,1, o);
			};
			butTmp = FindViewById<ImageButton>(Resource.Id.Cell12);
			butTmp.Click += (o, e) => {
				Clicked(1,2, o);
			};
			butTmp = FindViewById<ImageButton>(Resource.Id.Cell20);
			butTmp.Click += (o, e) => {
				Clicked(2,0, o);
			};				
			butTmp = FindViewById<ImageButton>(Resource.Id.Cell21);
			butTmp.Click += (o, e) => {
				Clicked(2,1, o);
			};
			butTmp = FindViewById<ImageButton>(Resource.Id.Cell22);
			butTmp.Click += (o, e) => {
				Clicked(2,2, o);
			};
			//set the label on the gameboard to display the current player
			_playerText.Text = (_nextPlayer == ePlayers.Player_O)?Resources.GetString(Resource.String.OsTurn):Resources.GetString(Resource.String.XsTurn);

		}
		//this will save transient states for rotation, backgrounding etc.
		protected override void OnSaveInstanceState (Bundle outState)
		{
			//outState.PutInt ("click_count", _counter);
			//Log.Debug(GetType().FullName, "Activity A - Saving instance state");
			outState.PutInt("nextPlayer",(int)_nextPlayer);
			outState.PutInt("CurState", (int)_curState);
			for (int x = 0; x<GRID_SIZE;x++)
			{
				for (int y=0;y<GRID_SIZE; y++)
				{
					outState.PutInt("Board"+x.ToString()+y.ToString(),(int)_gameBoard[x,y]);
				}
			}

			// always call the base implementation!
			base.OnSaveInstanceState (outState);    
		}
		protected override void OnRestoreInstanceState(Bundle bundle) 
		{
			
			base.OnRestoreInstanceState(bundle);

			//fetch persisted data
			_nextPlayer = (ePlayers)bundle.GetInt("nextPlayer",(int)ePlayers.Player_O);
			_curState = (eGameState)bundle.GetInt("CurState", (int)eGameState.Player_O_Turn);

			//the board has to map board states with view cell id's to set the graphic and clickableness
			for (int x = 0; x<GRID_SIZE;x++)
			{
				for (int y=0;y<GRID_SIZE; y++)
				{
					string xy = x.ToString()+y.ToString();
					_gameBoard[x,y] = (ePlayers)bundle.GetInt("Board"+xy,(int)ePlayers.NO_PLAYER);
					if(_gameBoard[x,y] != ePlayers.NO_PLAYER)
					{
						//Convert numeric x,y positions into Cell view ID's
						int cellID = 0;
						switch(xy)
						{
						case "00":
							cellID = Resource.Id.Cell00;
							break;
						case "01":
							cellID = Resource.Id.Cell01;
							break;
						case "02":
							cellID = Resource.Id.Cell02;
							break;
						case "10":
							cellID = Resource.Id.Cell10;
							break;
						case "11":
							cellID = Resource.Id.Cell11;
							break;
						case "12":
							cellID = Resource.Id.Cell12;
							break;
						case "20":
							cellID = Resource.Id.Cell20;
							break;
						case "21":
							cellID = Resource.Id.Cell21;
							break;
						case "22":
							cellID = Resource.Id.Cell22;
							break;
						default:
							cellID=0;
							break;
						}
						if(cellID != 0)
						{
							//set the data to the button states
							ImageButton butTmp = FindViewById<ImageButton>(cellID);
							butTmp.SetImageResource( _gameBoard[x,y] == ePlayers.Player_O?(Resource.Drawable.omark):(Resource.Drawable.xmark));
							butTmp.Clickable = false;
						}
					}
				}
			}
		}
		//the generic click handler, takes the grid x and y position and the button object sending it
		protected void Clicked(int x, int y, object o)
		{
			var butRef = (ImageButton)o;

			//set the appropriate image into the button and then disable from further turns
			butRef.SetImageResource( _nextPlayer == ePlayers.Player_O?(Resource.Drawable.omark):(Resource.Drawable.xmark));
			butRef.Clickable = false;

			//Animate the action!
			Animation Ani = AnimationUtils.LoadAnimation(this, Resource.Animation.fade_in);
			butRef.StartAnimation(Ani);

			//set the player info into the internal board representation
			_gameBoard [x, y] = _nextPlayer;

			//update the player
			_nextPlayer = (_nextPlayer == ePlayers.Player_X) ? ePlayers.Player_O : ePlayers.Player_X;

			//check for a game over/win condition
			EvaluateBoard ();

			//display the appropriate message if the game has finished
			if (_curState == eGameState.Draw || _curState == eGameState.Player_O_Win || _curState == eGameState.Player_X_Win) 
			{
				string Message = "";
				switch (_curState) {
				case eGameState.Draw:
					{
						Message = Resources.GetString (Resource.String.DrawGame);
						break;
					}
				case eGameState.Player_X_Win:
					{
						Message = Resources.GetString (Resource.String.XWins);
						//add the share/brag buttton to the outcome dialog
						_ResultDlg.SetNeutralButton (Resources.GetString (Resource.String.ShareLabel), delegate {
							Intent shareIntent = new Intent (Intent.ActionSend);
							shareIntent.SetType ("text/plain");
							shareIntent.PutExtra (Intent.ExtraSubject, Resources.GetString (Resource.String.IWon));
							shareIntent.PutExtra (Intent.ExtraText, Resources.GetString (Resource.String.XBragMsg));    

							StartActivity (Intent.CreateChooser (shareIntent, Resources.GetString (Resource.String.SharingTitle)));
							Finish ();
						});
						break;
					}
				case eGameState.Player_O_Win:
					{
						Message = Resources.GetString (Resource.String.OWins);
						//add the share/brag buttton to the outcome dialog
						_ResultDlg.SetNeutralButton (Resources.GetString (Resource.String.ShareLabel), delegate {
							Intent shareIntent = new Intent (Intent.ActionSend);
							shareIntent.SetType ("text/plain");
							shareIntent.PutExtra (Intent.ExtraSubject, Resources.GetString (Resource.String.IWon));
							shareIntent.PutExtra (Intent.ExtraText, Resources.GetString (Resource.String.OBragMsg));    

							StartActivity (Intent.CreateChooser (shareIntent, Resources.GetString (Resource.String.SharingTitle)));
							Finish ();
						});
						break;
					}
				default:
					Message = Resources.GetString (Resource.String.GameErr) + _curState.ToString ();
					break;
				}
				_ResultDlg.SetMessage (Message);
				_ResultDlg.Show ();	
				_playerText.Text = Resources.GetString (Resource.String.GameOver);
			}
			else 
			{
				//update next player label
				_playerText.Text = _nextPlayer == ePlayers.Player_O ? Resources.GetString (Resource.String.OsTurn) : Resources.GetString (Resource.String.XsTurn);
			}
		}
		//run the evaluations for a game complete cndition
		protected void EvaluateBoard()
		{

			bool Win = false;
			//evaluate each row and column for wins
			for (int index = 0; index < GRID_SIZE; index++) 
			{
				Win = EvaluateRow (index);
				if (Win)
					break;
				Win = EvaluateCol (index);
				if (Win)
					break;
			}
			//then the diagonals for wins
			if (!Win) 
			{
				Win = EvaluateDiags ();
			}
			//if no winning conditions are found test if we're full and have a draw
			if(!Win && IsBoardFull())
				_curState = eGameState.Draw;

		}
		//return true if the board is full
		protected bool IsBoardFull()
		{
			bool bFoundEmpty = false;

			//check each cell. stop on first empty that gets found
			foreach (ePlayers cel in _gameBoard) 
			{
				if (cel == ePlayers.NO_PLAYER) 
				{
					bFoundEmpty = true;
					break;
				}

				if (bFoundEmpty)
					break;
			}
			return !bFoundEmpty;
		}
		//return true if a win state is detected. curState is updated with the winner
		protected bool EvaluateRow(int RowNum)
		{
			int xCount = 0;
			int oCount = 0;
			//count each O and X in the Row
			for (int cel = 0; cel < GRID_SIZE; cel++) {
				if (_gameBoard [cel,RowNum] == ePlayers.Player_O)
					oCount++;
				else if(_gameBoard [cel, RowNum] == ePlayers.Player_X)
					xCount++;
			}
			//mark the win if any rows match a full count of player pieces
			if (xCount == GRID_SIZE) {
				_curState = eGameState.Player_X_Win;
				return true;
			} else if (oCount == GRID_SIZE) {
				_curState = eGameState.Player_O_Win;
				return true;
			}
			return false;
		}
		//return true if a win state is detected. curState is updated with the winner
		protected bool EvaluateCol(int ColNum)
		{
			int xCount = 0;
			int oCount = 0;
			//count each O and X in the Row
			for (int cel = 0; cel < GRID_SIZE; cel++) {
				if (_gameBoard [ColNum,cel ] == ePlayers.Player_O)
					oCount++;
				else if(_gameBoard [ColNum,cel] == ePlayers.Player_X)
					xCount++;
			}
			//mark the win if any rows match a full count of player pieces
			if (xCount == GRID_SIZE) {
				_curState = eGameState.Player_X_Win;
				return true;
			} else if (oCount == GRID_SIZE) {
				_curState = eGameState.Player_O_Win;
				return true;
			}
			return false;
		}
		//return true if a win state is detected. curState is updated with the winner
		protected bool EvaluateDiags()
		{
			int xCount1 = 0;
			int oCount1 = 0;
			int xCount2 = 0;
			int oCount2 = 0;
			for (int cel = 0; cel < GRID_SIZE; cel++) 
			{
				//diag 1 (top left to bottom right)
				if (_gameBoard [cel, cel ] == ePlayers.Player_O)
					oCount1++;
				else if(_gameBoard [cel, cel] == ePlayers.Player_X)
					xCount1++;
				//diag 2 (bottom left to top right)
				if (_gameBoard [cel, GRID_SIZE-cel-1 ] == ePlayers.Player_O)
					oCount2++;
				else if(_gameBoard [cel, GRID_SIZE-cel-1] == ePlayers.Player_X)
					xCount2++;				
			}
			//check for a winner
			if (xCount1 == GRID_SIZE || xCount2 == GRID_SIZE)
			{
				_curState = eGameState.Player_X_Win;
				return true;
			} 
			else if (oCount1 == GRID_SIZE||oCount2 == GRID_SIZE) 
			{
				_curState = eGameState.Player_O_Win;
				return true;
			}
	
			return false;
		}
	}
}
