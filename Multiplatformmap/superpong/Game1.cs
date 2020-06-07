#undef CONTROLLER

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;


namespace superpong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
            
            GraphicsDeviceManager graphics;
            SpriteBatch spriteBatch;
            
            // declares object sprites and positions
            Texture2D paddle1, paddle2, ball, pongstart, linkswint, rechtswint, CPU, Player, VersusCPU, control, xboximage;
            Vector2 paddle1position, paddle2position, ballposition, ballspeed, pongstartposition, linkswintposition, 
            rechtswintposition, CPUposition, Playerposition, VersusCPUposition, controlposition, xboximageposition;

            // declares player lives
            int Lives1 = 3;
            int Lives2 = 3;

            // declares variables used in resetcount
            bool start = false;
            int counter = 0;

            // declares several gamestates as well as sets initial gamestate 
            enum gamestates { title, versusplayer, versusCPU, linkswint, rechtswint};
            gamestates state = gamestates.title;

            // basic control inputs for multiplatform handler
            bool paddle1up, paddle1down, paddle2up, paddle2down, startgame1, startgame2;

        public Game1()
            {
                graphics = new GraphicsDeviceManager(this);
                Content.RootDirectory = "Content";
            }

            protected override void Initialize()
            {

            base.Initialize();
            
            // make mouse visible
            this.IsMouseVisible = true;

            // set initial object positions
            ballposition = new Vector2(GraphicsDevice.Viewport.Width / 2 - (ball.Width / 2), GraphicsDevice.Viewport.Height / 2 - (ball.Height / 2));
            paddle1position = new Vector2(30, GraphicsDevice.Viewport.Height / 2 - (paddle1.Height / 2) );
            paddle2position = new Vector2(GraphicsDevice.Viewport.Width - paddle1.Width - 30, GraphicsDevice.Viewport.Height / 2 - (paddle1.Height / 2));
            pongstartposition = new Vector2(GraphicsDevice.Viewport.Width / 2 - (pongstart.Width / 2), GraphicsDevice.Viewport.Height / 2 - (pongstart.Height / 2));
            linkswintposition = new Vector2(GraphicsDevice.Viewport.Width / 2 - (linkswint.Width / 2), GraphicsDevice.Viewport.Height / 4 - (linkswint.Height / 2));
            rechtswintposition  = new Vector2(GraphicsDevice.Viewport.Width / 2 - (rechtswint.Width / 2), GraphicsDevice.Viewport.Height / 4 - (rechtswint.Height / 2));
            VersusCPUposition = new Vector2(GraphicsDevice.Viewport.Width / 2 - (VersusCPU.Width / 2), GraphicsDevice.Viewport.Height - (VersusCPU.Height * 2));
            controlposition = new Vector2(GraphicsDevice.Viewport.Width / 2 - (control.Width / 2), GraphicsDevice.Viewport.Height / 4 - (control.Height / 2));
            xboximageposition = new Vector2(GraphicsDevice.Viewport.Width / 2 - (control.Width / 2) + 150, GraphicsDevice.Viewport.Height / 4 - (control.Height / 2) + 200);
            Playerposition = new Vector2(0, ball.Height);
            CPUposition = new Vector2(GraphicsDevice.Viewport.Width - CPU.Width, ball.Height);

            // set initial ballspeed
            ballspeed.X = 300;
            ballspeed.Y = 100;
            
            }

            protected override void LoadContent()
            {
                // Create a new SpriteBatch, which can be used to draw textures.
                spriteBatch = new SpriteBatch(GraphicsDevice);

                //loads several sprites
                paddle1 = Content.Load<Texture2D>("Paddle");
                paddle2 = Content.Load<Texture2D>("Paddle2");
                ball = Content.Load<Texture2D>("Newball");
                pongstart = Content.Load<Texture2D>("Pongstart");
                linkswint = Content.Load<Texture2D>("Linkswint");
                rechtswint = Content.Load<Texture2D>("Rechtswint");
                VersusCPU = Content.Load<Texture2D>("VersusCPU");
                control = Content.Load<Texture2D>("control");
                Player = Content.Load<Texture2D>("Player");
                CPU = Content.Load<Texture2D>("CPU");
                xboximage = Content.Load<Texture2D>("Xboxcontrolimage");
        }

            protected override void UnloadContent()
            {
                // Is never used
            }

            protected override void Update(GameTime gameTime)
            {
            //handles various methods based on gamestate

            // Handles the multiple input methods
            MultiplatformInputHandler();

                if (state == gamestates.title)
                {
                    startgame();
                }
                else if (state == gamestates.versusplayer)
                {
                    Playermovement();
                    PaddleReflect();
                    BallMovement(gameTime);
                    resetcount();
                    gameover();
                }
                else if (state == gamestates.versusCPU)
                {
                    Playermovement();
                    PaddleReflect();
                    BallMovement(gameTime);
                    resetcount();
                    gameover();
                }
                else if (state == gamestates.linkswint || state == gamestates.rechtswint)
                {
                    gameover();
                }
                base.Update(gameTime);
            }

            public void startgame()
                {
            
            // allows player to choose game mode

                    if (startgame1)
                        state = gamestates.versusplayer;
                    else if (startgame2)
                        state = gamestates.versusCPU;
                }

            public void gameover()
            {

            // allows game to end as well as restart based on gamestate
            if (Lives2 == 0)
                {
                    state = gamestates.linkswint;
                }
            if (Lives1 == 0)
                {
                    state = gamestates.rechtswint;
                }

            if ( state == gamestates.linkswint || state == gamestates.rechtswint)
                if (startgame1)
                {
                    Lives1 = 3;
                    Lives2 = 3;
                    state = gamestates.versusplayer;
                    Initialize();
                }
            if (state == gamestates.linkswint || state == gamestates.rechtswint)
                if (startgame2)
                {
                    Lives1 = 3;
                    Lives2 = 3;
                    state = gamestates.versusCPU;
                    Initialize();
                }

        }

            public void BallMovement(GameTime gameTime)
            {

                // declares various neccesary variables
                Random r = new Random();
                int randomNumber = r.Next(-500, 500);

                int paddleminY = 0;
                int paddlemaxY = GraphicsDevice.Viewport.Height - paddle1.Height;

                int ballmiddleX = (GraphicsDevice.Viewport.Width / 2 - (ball.Width / 2));
                int ballmiddleY = (GraphicsDevice.Viewport.Height / 2 - (ball.Height / 2));

                int ballminX = 0 - ball.Width;
                int ballmaxX = GraphicsDevice.Viewport.Width;

                int ballminY = 0;
                int ballmaxY = GraphicsDevice.Viewport.Height - ball.Height;

            // the following will only occur after the 90 frames resetcount
            if (start)
            {
                // makes the ball move
                ballposition += ballspeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

                // makes ball bounce of walls
                if (ballposition.Y > ballmaxY && ballspeed.Y > 0 || ballposition.Y < ballminY && ballspeed.Y < 0)
                    ballspeed.Y *= -1;

                // makes the ball reset its position, speed and activates the 90 frames wait, as well as make the player lose a live
                if (ballposition.X < ballminX)
                {
                    ballspeed.Y = randomNumber;
                    ballspeed.X = -300;
                    ballposition.X = ballmiddleX;
                    ballposition.Y = ballmiddleY;
                    Lives1 -= 1;
                    counter = 0;
                    start = false;
                    resetcount();
                }
                if (ballposition.X > ballmaxX)
                {
                    ballspeed.Y = randomNumber;
                    ballspeed.X = 300;
                    ballposition.X = ballmiddleX;
                    ballposition.Y = ballmiddleY;
                    Lives2 -= 1;
                    counter = 0;
                    start = false;
                    resetcount();
                }

            }
                // set maximum vertical ballspeeds
                if (ballspeed.Y > 600)
                   ballspeed.Y = 600;    
                if (ballspeed.Y < -600)
                   ballspeed.Y = -00;
        }

            public void resetcount()
            {
                // Makes the ball wait 90 frames (1,5 seconds) in the middle before moving again after a player loses a live
                if (counter < 90)
                {
                    counter += 1;
                } 
                else 
                {
                    start = true;
                }
        
            }

        public void PaddleReflect()
        {
            // declares midpoints of hitboxes
            float ballmiddleY = ballposition.Y + ball.Height / 2;
            float paddle1middleY = paddle1position.Y + paddle1.Height / 2;
            float paddle2middleY = paddle2position.Y + paddle2.Height / 2;

            // declares several hitboxes
            Rectangle paddle1rect = new Rectangle((int)paddle1position.X, (int)paddle1position.Y, paddle1.Width, paddle1.Height);
            Rectangle paddle2rect = new Rectangle((int)paddle2position.X, (int)paddle2position.Y, paddle2.Width, paddle2.Height);
            Rectangle ballrect = new Rectangle((int)ballposition.X, (int)ballposition.Y, ball.Width, ball.Height);

            // makes the ball reflect off the paddle with a speed based on their hit positions
            if (paddle1rect.Intersects(ballrect))
            {
                ballspeed.X *= -1;
                ballspeed.X += 20;
                if (paddle1middleY > ballmiddleY)
                    ballspeed.Y = (paddle1middleY - ballmiddleY) * -10;
                else if (paddle1middleY < ballmiddleY)
                    ballspeed.Y = (paddle1middleY - ballmiddleY) * -10;
            }
            if (paddle2rect.Intersects(ballrect))
            {
                ballspeed.X *= -1;
                ballspeed.X -= 20;
                if (paddle2middleY > ballmiddleY)
                    ballspeed.Y = (paddle2middleY - ballmiddleY) * -10;
                else if (paddle2middleY < ballmiddleY)
                    ballspeed.Y = (paddle2middleY - ballmiddleY) * -10;
            }
        }

        public void MultiplatformInputHandler()
        {

            // Allows for both keyboard and Xbox controller support
            // Supports future optional control inputs as well

#if CONTROLLER
            GamePadState p1gamepad = GamePad.GetState(PlayerIndex.One);

            // defines movement inputs for left and right paddles
            if (p1gamepad.ThumbSticks.Left.Y > 0)
                paddle1up = true;
            else
                paddle1up = false;

            if (p1gamepad.ThumbSticks.Left.Y < 0)
                paddle1down = true;
            else
                paddle1down = false;

            if (p1gamepad.ThumbSticks.Right.Y > 0)
                paddle2up = true;
            else
                paddle2up = false;

            if (p1gamepad.ThumbSticks.Right.Y < 0)
                paddle2down = true;
            else
                paddle2down = false;

            // defines menu inputs
            if ( p1gamepad.Buttons.A == ButtonState.Pressed)
                startgame1 = true;
            else
                startgame1 = false;

            if (p1gamepad.Buttons.B == ButtonState.Pressed)
                startgame2 = true;
            else
                startgame2 = false;

            // allows game to exit
            if (p1gamepad.Buttons.Back == ButtonState.Pressed)
                Exit();
#else
            
            KeyboardState keyState = Keyboard.GetState();

            // defines movement inputs for left and right paddles
            if (keyState.IsKeyDown(Keys.A))
                paddle1up = true;
            else
                paddle1up = false;

            if (keyState.IsKeyDown(Keys.Z))
                paddle1down = true;
            else
                paddle1down = false;

            if (keyState.IsKeyDown(Keys.K))
                paddle2up = true;
            else
                paddle2up = false;

            if (keyState.IsKeyDown(Keys.M))
                paddle2down = true;
            else
                paddle2down = false;

            // defines menu inputs
            if (keyState.IsKeyDown(Keys.Space))
                startgame1 = true;
            else
                startgame1 = false;

            if (keyState.IsKeyDown(Keys.Enter))
                startgame2 = true;
            else
                startgame2 = false;

            // allows game to exit
            if (keyState.IsKeyDown(Keys.Escape))
                Exit();
#endif
        }

        public void Playermovement()
        {

            // define paddle movement boundaries
            int paddleminY = 0;
            int paddlemaxY = GraphicsDevice.Viewport.Height - paddle1.Height;

            // set movement controls for left paddle
            if (paddle1up && paddle1position.Y > paddleminY)
                paddle1position.Y -= 10;
            if (paddle1down && paddle1position.Y < paddlemaxY)
                paddle1position.Y += 10;

            // set movements controls for right paddle
            if (state == gamestates.versusplayer)
            {
                if (paddle2up && paddle2position.Y > paddleminY)
                    paddle2position.Y -= 10;
                if (paddle2down && paddle2position.Y < paddlemaxY)
                    paddle2position.Y += 10;
            }

            // set movement for right paddle vs CPU
            else if (state == gamestates.versusCPU && ballspeed.X > 0 )
                {
                    if (paddle2position.Y + paddle2.Height / 2 > ballposition.Y + ball.Height / 2 && paddle2position.Y > 0)
                        paddle2position.Y -= 6;
                    if (paddle2position.Y + paddle2.Height / 2 < ballposition.Y + ball.Height / 2 && paddle2position.Y < paddlemaxY)
                        paddle2position.Y += 6;
                }
        }

            public void Drawlives()
        {
            // draws the correct amount of lives on screen
            int i = 0; while (i < Lives1)
            {
                spriteBatch.Draw(ball, new Vector2(i * ball.Width, 0), Color.White);
                i = i + 1;
            }
            int j = 0; while (j < Lives2)
            {
                spriteBatch.Draw(ball, new Vector2(GraphicsDevice.Viewport.Width - ball.Width - j * ball.Width, 0), Color.White);
                j = j + 1;
            }
        }

            protected override void Draw(GameTime gameTime)
            {
                // set background color
                GraphicsDevice.Clear(Color.White);
                
                // draws various sprites on screen based on gamestate
                spriteBatch.Begin();

            if (state == gamestates.title)
            {
                spriteBatch.Draw(xboximage, xboximageposition, Color.White);
                spriteBatch.Draw(pongstart, pongstartposition, Color.White);
                spriteBatch.Draw(VersusCPU, VersusCPUposition, Color.White);
                spriteBatch.Draw(control, controlposition, Color.White);
                
            }
            if (state == gamestates.versusplayer || state == gamestates.versusCPU)
            {
                Drawlives();
                if ( state == gamestates.versusCPU)
                {
                    spriteBatch.Draw(Player, Playerposition, Color.White);
                    spriteBatch.Draw(CPU, CPUposition, Color.White);
                }
                spriteBatch.Draw(ball, ballposition, Color.White);
                spriteBatch.Draw(paddle1, paddle1position, Color.White);
                spriteBatch.Draw(paddle2, paddle2position, Color.White);
            }
            if (state == gamestates.rechtswint)
            {
                spriteBatch.Draw(rechtswint, rechtswintposition, Color.White);
                spriteBatch.Draw(pongstart, pongstartposition, Color.White);
                spriteBatch.Draw(VersusCPU, VersusCPUposition, Color.White);
            }
            if (state == gamestates.linkswint)
            {
                spriteBatch.Draw(linkswint, linkswintposition, Color.White);
                spriteBatch.Draw(pongstart, pongstartposition, Color.White);
                spriteBatch.Draw(VersusCPU, VersusCPUposition, Color.White);
            }

            spriteBatch.End();

                base.Draw(gameTime);
            }
        }
    }
