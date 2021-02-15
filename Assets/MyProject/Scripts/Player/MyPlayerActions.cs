namespace Bindings
{
	using InControl;
	using UnityEngine;


	public class MyPlayerActions : PlayerActionSet
	{
		public readonly PlayerAction Fire;
		public readonly PlayerAction JumpGlide;
		public readonly PlayerAction Left;
		public readonly PlayerAction Right;
		public readonly PlayerAction Up;
		public readonly PlayerAction Down;
		public readonly PlayerAction DiveUp;
		public readonly PlayerAction DiveDown;
		public readonly PlayerTwoAxisAction Move;


		public MyPlayerActions()
		{
			Fire = CreatePlayerAction( "Fire" );
			JumpGlide = CreatePlayerAction( "JumpGlide" );
			Left = CreatePlayerAction( "Move Left" );
			Right = CreatePlayerAction( "Move Right" );
			Up = CreatePlayerAction( "Move Up" );
			Down = CreatePlayerAction( "Move Down" );
			DiveUp = CreatePlayerAction( "DiveUp" );
			DiveDown = CreatePlayerAction( "DiveDown" );
			Move = CreateTwoAxisPlayerAction( Left, Right, Down, Up );
		}


		public static MyPlayerActions CreateWithDefaultBindings()
		{
			var playerActions = new MyPlayerActions();

			// How to set up mutually exclusive keyboard bindings with a modifier key.
			// playerActions.Back.AddDefaultBinding( Key.Shift, Key.Tab );
			// playerActions.Next.AddDefaultBinding( KeyCombo.With( Key.Tab ).AndNot( Key.Shift ) );

			playerActions.Fire.AddDefaultBinding( Key.E );
			playerActions.Fire.AddDefaultBinding( InputControlType.Action1 );
			// playerActions.Fire.AddDefaultBinding( Mouse.LeftButton );

			playerActions.JumpGlide.AddDefaultBinding( Key.Space );
			playerActions.JumpGlide.AddDefaultBinding( InputControlType.Action3 );
			playerActions.JumpGlide.AddDefaultBinding( InputControlType.Back );

			playerActions.Up.AddDefaultBinding(Key.W);
			playerActions.Down.AddDefaultBinding(Key.S);
			playerActions.Left.AddDefaultBinding(Key.A);
			playerActions.Right.AddDefaultBinding(Key.D);

			playerActions.Up.AddDefaultBinding( Key.UpArrow );
			playerActions.Down.AddDefaultBinding( Key.DownArrow );
			playerActions.Left.AddDefaultBinding( Key.LeftArrow );
			playerActions.Right.AddDefaultBinding( Key.RightArrow );

			playerActions.Left.AddDefaultBinding( InputControlType.LeftStickLeft );
			playerActions.Right.AddDefaultBinding( InputControlType.LeftStickRight );
			playerActions.Up.AddDefaultBinding( InputControlType.LeftStickUp );
			playerActions.Down.AddDefaultBinding( InputControlType.LeftStickDown );

			playerActions.Left.AddDefaultBinding( InputControlType.DPadLeft );
			playerActions.Right.AddDefaultBinding( InputControlType.DPadRight );
			playerActions.Up.AddDefaultBinding( InputControlType.DPadUp );
			playerActions.Down.AddDefaultBinding( InputControlType.DPadDown );

			playerActions.DiveUp.AddDefaultBinding(Mouse.LeftButton);
			playerActions.DiveDown.AddDefaultBinding(Mouse.RightButton);

			// Esto pilla el ratón
			/*playerActions.Up.AddDefaultBinding( Mouse.PositiveY );
			playerActions.Down.AddDefaultBinding( Mouse.NegativeY );
			playerActions.Left.AddDefaultBinding( Mouse.NegativeX );
			playerActions.Right.AddDefaultBinding( Mouse.PositiveX );*/

			playerActions.ListenOptions.IncludeUnknownControllers = true;
			playerActions.ListenOptions.MaxAllowedBindings = 4;
			//playerActions.ListenOptions.MaxAllowedBindingsPerType = 1;
			//playerActions.ListenOptions.AllowDuplicateBindingsPerSet = true;
			playerActions.ListenOptions.UnsetDuplicateBindingsOnSet = true;
			//playerActions.ListenOptions.IncludeMouseButtons = true;
			//playerActions.ListenOptions.IncludeModifiersAsFirstClassKeys = true;
			//playerActions.ListenOptions.IncludeMouseScrollWheel = true;

			playerActions.ListenOptions.OnBindingFound = ( action, binding ) =>
			{
				if (binding == new KeyBindingSource( Key.Escape ))
				{
					action.StopListeningForBinding();
					return false;
				}

				return true;
			};

			playerActions.ListenOptions.OnBindingAdded += ( action, binding ) => { Debug.Log( "Binding added... " + binding.DeviceName + ": " + binding.Name ); };

			playerActions.ListenOptions.OnBindingRejected += ( action, binding, reason ) => { Debug.Log( "Binding rejected... " + reason ); };

			return playerActions;
		}
	}
}
