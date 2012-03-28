using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ARDroneKinect
{
    public class LogManager : DrawableGameComponent
    {
        
		private SpriteBatch spriteBatch;
		private SpriteFont font;
        private bool drawLog;
        private int max;

        private List<string> logs;
                               
		public LogManager(Game game, int max, SpriteBatch sp)
			: base(game)
		{
            logs = new List<string>();
            drawLog = true;
            this.max = max;
            spriteBatch = sp;

            font = game.Content.Load<SpriteFont>("Font");
		}

        public void addLog(string text)
        {
            if (logs.Count < max)
            {
                logs.Add(text);
            }
            else
            {
                logs.RemoveAt(0);
                logs.Add(text);
            }


        }

        protected override void LoadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {

            
            base.Update(gameTime);
        }

		public override void Draw(GameTime gameTime)
		{
            if (drawLog)
            {
                spriteBatch.Begin();

                spriteBatch.DrawString(font, "Last " + logs.Count + " logs: ", new Vector2(10f, 12f), Color.White);
                int i = 2;
                foreach (string text in logs)
                {
                    spriteBatch.DrawString(font, text, new Vector2(10f, 12f*i), Color.White);
                    i++;
                }
                spriteBatch.End();
            }
		}
    
    }
}
