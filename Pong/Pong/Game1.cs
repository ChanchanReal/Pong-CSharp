using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private int _screenHeight = 1024, _screenWidth = 1280;
        private Texture2D _playerPaddle;
        private Texture2D _aiPaddle;
        private Texture2D ball;
        private SpriteFont font; // you wont get score because of logic
        private int _aiScore;
        private Vector2 _paddlePosition = new Vector2(100, 0);
        private Vector2 _ballPosition = new Vector2(400, 500);
        private Vector2 _aiPaddlePos = new Vector2(1140, 0);
        private float VelocityX = 1;
        private float VelocityY = 1;
        private bool outBounced = false;
        private bool startGame = false;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.PreferredBackBufferHeight = _screenHeight;
            _graphics.PreferredBackBufferWidth = _screenWidth;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _playerPaddle = Content.Load<Texture2D>("Rectangle");
            _aiPaddle = Content.Load<Texture2D>("Rectangle");
            ball = Content.Load<Texture2D>("ball");
            font = Content.Load<SpriteFont>("File");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            // ball collision logic
            // ball continuous moving
            _ballPosition.X += VelocityX * (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            _ballPosition.Y += VelocityY * (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            //ai paddle 
            if (_ballPosition.Y < _aiPaddlePos.Y)
                _aiPaddlePos = new Vector2(_aiPaddlePos.X, _aiPaddlePos.Y - (int)gameTime.ElapsedGameTime.TotalMilliseconds);
            if (_ballPosition.Y > _aiPaddlePos.Y)
                _aiPaddlePos = new Vector2(_aiPaddlePos.X, _aiPaddlePos.Y + (int)gameTime.ElapsedGameTime.TotalMilliseconds);
            if (_ballPosition.X > _aiPaddlePos.X - _aiPaddle.Width)
                VelocityX = -VelocityX;
            // if paddle with is greater than ball flip velocity
            // and if ball position is greater than paddle y and ball position is less paddle height it should stay within the texture
            if (_ballPosition.X < _paddlePosition.X + _playerPaddle.Width &&
                _ballPosition.Y > _paddlePosition.Y && _ballPosition.Y < _paddlePosition.Y + _playerPaddle.Height)
                VelocityX = -VelocityX;
            // ball hit left
            if (_ballPosition.X < 0)
            {
                if (!outBounced)
                {
                    outBounced = true;
                    Scored();
                }
            }
                
           // ball hit top bototm
            if (_ballPosition.Y < 0 || _ballPosition.Y > _graphics.PreferredBackBufferHeight - ball.Height)
                VelocityY = -VelocityY;
            // TODO: Add your update logic here
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                _paddlePosition = new Vector2(_paddlePosition.X, _paddlePosition.Y + (int)gameTime.ElapsedGameTime.TotalMilliseconds);
            else if (Keyboard.GetState().IsKeyDown(Keys.Up))
                _paddlePosition = new Vector2(_paddlePosition.X, _paddlePosition.Y - (int)gameTime.ElapsedGameTime.TotalMilliseconds);
            else if (Keyboard.GetState().IsKeyDown(Keys.Space))
                _ballPosition = new Vector2(400, 500);
            else if (Keyboard.GetState().IsKeyDown(Keys.G) && !startGame)
                startGame = true;
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            if (startGame)
            {
                _spriteBatch.Draw(_aiPaddle, _aiPaddlePos, new Rectangle(0, 0, _aiPaddle.Width, _aiPaddle.Height), Color.White);
                _spriteBatch.Draw(_playerPaddle, _paddlePosition, new Rectangle(0, 0, _playerPaddle.Width, _playerPaddle.Height), Color.White);
                _spriteBatch.Draw(ball, _ballPosition, new Rectangle(0, 0, ball.Width, ball.Height), Color.White);
                _spriteBatch.DrawString(font, "0", new Vector2(400, 0), Color.White);
                _spriteBatch.DrawString(font, $"{_aiScore}", new Vector2(800, 0), Color.White);
            }
            else if (!startGame)
            {
                _spriteBatch.DrawString(font, "Controls: up down left right", new Vector2((_graphics.PreferredBackBufferWidth / 2f) - 200, _graphics.PreferredBackBufferHeight / 2f), Color.Yellow);
                _spriteBatch.DrawString(font, "Press P to pause and G to start", new Vector2((_graphics.PreferredBackBufferWidth / 2f) - 200, (_graphics.PreferredBackBufferHeight / 2f) +40), Color.White);
            }
            
            _spriteBatch.End();
            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        void Scored()
        {
            _aiScore++;
            _ballPosition = new Vector2(400, 500);
            outBounced = false;
            VelocityX = -VelocityX;
        }
    }
}
