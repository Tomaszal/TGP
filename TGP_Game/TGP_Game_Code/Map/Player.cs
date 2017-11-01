﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace TGP_Game_Code.Map
{
    public class Player : Entity
    {
        public int Lifes;

        public Player(Point startingPosition) : base(startingPosition) { }

        private bool TouchBottomOf(Rectangle r1, Rectangle r2)
        {
            return (r1.Top <= r2.Bottom + (r2.Height / 3) &&
                    r1.Top >= r2.Bottom - 1 &&
                    r1.Right >= r2.Left + (r2.Width / 5) &&
                    r1.Left <= r2.Right - (r2.Width / 5));
        }

        private bool TouchRightOf(Rectangle r1, Rectangle r2)
        {
            return (r1.Left >= r2.Left &&
                    r1.Left + r1.Width / 4 <= r2.Right + 5 &&
                    r1.Top <= r2.Bottom - (r2.Width / 4) &&
                    r1.Bottom >= r2.Top + (r2.Width / 4));
        }

        public void Respawn()
        {
            Active = true;

            Position.Location = StartingPosition;

            IsJumping = false;
            JumpingHeight = 0f;

            Velocity = Vector2.Zero;
        }

        public void Die()
        {
            if (!Active) return;

            Active = false;

            Lifes--;

            Main.NewStateIndex = 5;
        }

        public override void FloorCollision()
        {
            if (!Active) return;

            // Die if player is in water (bad implementation)

            if (Map.TileMap[(int)Math.Floor((float)Position.Center.Y / Map.TileDestinationRectangle.Height)][(int)Math.Floor((float)Position.Center.X / Map.TileDestinationRectangle.Width)].TexturePosition.X / 3 == Map.TileSourceRectangle.X)
            {
                Die();

                Main.DrownSound.Play();
            }
        }

        public override void Update(GameTime gameTime)
        {
            // Update movement directions

            MoveRight = Keyboard.GetState().IsKeyDown(Keys.D);
            MoveLeft = Keyboard.GetState().IsKeyDown(Keys.A);
            MoveUp = Keyboard.GetState().IsKeyDown(Keys.W);
            MoveDown = Keyboard.GetState().IsKeyDown(Keys.S);

            // Update jump state

            Jump = Keyboard.GetState().IsKeyDown(Keys.Space);

            // Update entity

            base.Update(gameTime);

            // Check for collisions with enemies

            foreach (Enemy enemy in Map.Enemies)
            {
                if (!Active) break;

                if (!enemy.Active) continue;

                if (MovedDownDuringLastUpdate && TouchBottomOf(enemy.Position, Position))
                {
                    enemy.Active = false;

                    Main.EnemyDeathSound.Play();
                }
                else if (TouchRightOf(Position, enemy.Position) || TouchRightOf(enemy.Position, Position))
                {
                    Die();

                    Main.PlayerDeathSound.Play();
                }
            }
        }
    }
}
