﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TGP.States
{
	public class Characters
	{
		// Set differnet character types with descriptions and options

		public class CharacterType
		{
			public string Line1, Line2, Line3;
			public Color Line1Color;
			public float VelocityLimit, JumpHeightLimit;
			public int Lifes;
		}

		public class Leaper : CharacterType
		{
			public Leaper()
			{
				Line1 = "Leaper";
				Line2 = "Jumps very high, but runs very slow.";
				Line3 = "Has 3 lifes.";

				Line1Color = Color.GreenYellow;

				VelocityLimit = 4.5f;
				JumpHeightLimit = 7f;
				Lifes = 3;
			}
		}

		public class Muller : CharacterType
		{
			public Muller()
			{
				Line1 = "Muller";
				Line2 = "Runs very fast, but very jumps low.";
				Line3 = "Has 3 lifes.";

				Line1Color = Color.MediumSpringGreen;

				VelocityLimit = 9f;
				JumpHeightLimit = 3.5f;
				Lifes = 3;
			}
		}

		public class Survivalist : CharacterType
		{
			public Survivalist()
			{
				Line1 = "Survivalist";
				Line2 = "Does not jump high or run fast.";
				Line3 = "Has 5 lifes.";

				Line1Color = Color.MediumVioletRed;

				VelocityLimit = 6f;
				JumpHeightLimit = 4.5f;
				Lifes = 5;
			}
		}

		public class Tryhard : CharacterType
		{
			public Tryhard()
			{
				Line1 = "Tryhard";
				Line2 = "Does not jump high or run fast.";
				Line3 = "Has 1 life.";

				Line1Color = Color.DodgerBlue;

				VelocityLimit = 6f;
				JumpHeightLimit = 4.5f;
				Lifes = 1;
			}
		}
	}

	class Character : State
	{
		// List of character types

		private static List<Characters.CharacterType> CharacterTypes = new List<Characters.CharacterType>() { new Characters.Leaper(), new Characters.Muller(), new Characters.Survivalist(), new Characters.Tryhard() };

		// Character index in character type list (also matches their position in texture)

		private static int CharacterIndex = 2;

		// Character preview entity

		private static Map.Entity Preview = new Map.Entity(new Point(0,0));

		private class Next : Button
		{
			public Next(string text, Vector2 position, Color color) : base(text, position, color, -1) { }

			public override void Action()
			{
				// Increace or reset CharacterIndex

				CharacterIndex = (CharacterIndex == 3) ? 0 : CharacterIndex + 1;

				base.Action();
			}
		}

		private class Previous : Button
		{
			public Previous(string text, Vector2 position, Color color) : base(text, position, color, -1) { }

			public override void Action()
			{
				// Decreace or reset CharacterIndex

				CharacterIndex = (CharacterIndex == 0) ? 3 : CharacterIndex - 1;

				base.Action();
			}
		}

		private class Select : Button
		{
			public Select(string text, Vector2 position, Color color) : base(text, position, color, 4) { }

			public override void Action()
			{
				// Logic for selection

				Map.Map.InitializeMap();

				Map.Map.Player.Lifes = CharacterTypes[CharacterIndex].Lifes;
				Map.Map.Player.EntityTypeIndex = CharacterIndex;
				Map.Map.Player.MaximumHorizontalVelocity = CharacterTypes[CharacterIndex].VelocityLimit;
				Map.Map.Player.MaximumJumpingHeight = CharacterTypes[CharacterIndex].JumpHeightLimit;

				Map.Map.GameOn = true;

				base.Action();
			}
		}

		public Character()
		{
			// Add texts

			Texts.Add(new Text(Color.White, new Vector2(0, -280), "Choose your character:"));
			Texts.Add(new Text(Color.White, new Vector2(0, -160), "NaN"));
			Texts.Add(new Text(Color.LightSlateGray, new Vector2(0, -130), "NaN"));
			Texts.Add(new Text(Color.LightSlateGray, new Vector2(0, -100), "NaN"));

			// Add buttons

			Buttons.Add(new Next(">", new Vector2(50, 0), Color.PaleVioletRed));
			Buttons.Add(new Previous("<", new Vector2(-54, 0), Color.PaleVioletRed));
			Buttons.Add(new Select("Select", new Vector2(0, 100), Color.PaleVioletRed));
			Buttons.Add(new Button("Return", new Vector2(0, 180), Color.LightSlateGray, 0));
		}

		public override void Update(GameTime gameTime)
		{
			// Update character preview entity

			Preview.EntityTypeIndex = CharacterIndex;
			Preview.Position.Location = new Point(Main.Graphics.PreferredBackBufferWidth / 2 - 24, Main.Graphics.PreferredBackBufferHeight / 2 - 8);
			Preview.MoveDown = true;

			// Update character specific description text

			Texts[1].TextString = CharacterTypes[CharacterIndex].Line1;
			Texts[2].TextString = CharacterTypes[CharacterIndex].Line2;
			Texts[3].TextString = CharacterTypes[CharacterIndex].Line3;

			Texts[1].Color = CharacterTypes[CharacterIndex].Line1Color;

			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			// Draw background

			DrawMenuBackground();

			// Draw character preview

			Preview.Draw(gameTime);

			base.Draw(gameTime);
		}
	}
}
