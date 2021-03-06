using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Abimn
{
    /// <summary>
    /// Classe Game m�re
    /// C'est elle qui switch entre tous les �l�ments de la pile de jeu
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        Entity black, white;

        public Game()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Rand.Init();
            G.currentGame = new Stack<GameType>();
            G.content = new Hashtable();

            this.graphics.IsFullScreen = false;
            this.graphics.PreferredBackBufferWidth = C.Screen.Width;
            this.graphics.PreferredBackBufferHeight = C.Screen.Height;
            this.graphics.ApplyChanges();
            this.Window.Title = "Abimn";
            this.Window.AllowUserResizing = true;

            base.Initialize();

            Cursor.Initialize();
            Cursor.SetCursor("cursor", "default", "clicked", new Pos(-15, -5));

            Music.Volume = 7;
            G.brightness = 50;

            black = new Entity();
            black.LoadContent("black");
            black.Opacity = 0;
            white = new Entity();
            white.LoadContent("white");
            white.Opacity = 0;

            G.currentGame.Push(new Menu());
        }

        protected override void LoadContent()
        {
            G.spriteBatch = new SpriteBatch(GraphicsDevice);

            Ressources.Load(Content);
        }

        protected override void UnloadContent() {}

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || G.currentGame.Count == 0)
            {
                this.Exit();
                return;
            }

            E.Update();

            if (G.brightness >= 50)
            {
                black.Opacity = 0;
                white.Opacity = ((float)G.brightness - 50) / 50;
            }
            if (G.brightness < 50)
            {
                white.Opacity = 0;
                black.Opacity = (50 - (float)G.brightness) / 50;
            }

            G.currentGame.Peek().Update(gameTime);

            while (G.currentGame.Count != 0 && G.currentGame.Peek().State == State.Exit)
                G.currentGame.Pop();
            if (G.currentGame.Count == 0)
                return;
            if (G.currentGame.Peek().State == State.Reload)
                G.currentGame.Peek().Initialize();

            Cursor.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            if (G.currentGame.Count == 0)
                return;

            Stack<GameType> buffer = new Stack<GameType>();

            GraphicsDevice.Clear(Color.Black);

            while (G.currentGame.Count != 1 && !G.currentGame.Peek().IsFullScreen)
                buffer.Push(G.currentGame.Pop());
            buffer.Push(G.currentGame.Pop());
            G.spriteBatch.Begin();
            while (buffer.Count != 0)
            {
                buffer.Peek().Draw();
                G.currentGame.Push(buffer.Pop());
            }
            Cursor.Draw();

            black.Draw();
            white.Draw();

            G.spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
