using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlameBadge;
namespace FlameBadge
{
    public partial class Form1 : Form
    {
        public FlameBadge game;
        public static Image[] textures;
        public static GameBoard board;
        public Form1(FlameBadge game)
        {
            InitializeComponent();
            textures = new Image[10000];
            textures[(int)'%'] = Image.FromFile("Art/grass.png");
            textures[(int)'^'] = Image.FromFile("Art/mountain.png");
            textures[(int)'~'] = Image.FromFile("Art/water.png");
            textures[(int)'z'] = Image.FromFile("Art/selected.png");
            textures[(int)'='] = Image.FromFile("Art/bridge.png");
            textures[(int)'#'] = Image.FromFile("Art/tree.png");
            textures[(int)'&'] = Image.FromFile("Art/road.png");
            textures[(int)'+'] = Image.FromFile("Art/castle1.png");
            textures[(int)'*'] = Image.FromFile("Art/castle2.png");
            textures[(int)'<'] = Image.FromFile("Art/enemy.png");
            textures[(int)'>'] = Image.FromFile("Art/player.png");
            //textures[4] = Image.FromFile("../Art/soldier");
            //textures[5] = Image.FromFile("../Art/archer");

            this.game = game;
            Invalidate();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Click(Object sender, MouseEventArgs e)
        {
            Invalidate();
            Point point = panel1.PointToClient(Cursor.Position);
            panel1.CreateGraphics().DrawImage(textures[(int)'z'], new Point((point.X / 32) * 32, (point.Y / 32) * 32));
        }
        private void panel1_Click(Object sender, EventArgs e)
        {
            Invalidate();
            Point point = panel1.PointToClient(Cursor.Position);
            panel1.CreateGraphics().DrawImage(textures[(int)'z'], new Point((point.X / 32) * 32, (point.Y / 32) * 32));
        }

        //public override void takeTurn(PlayerCharacter p)
        //{
        //    Sidebar.announce(String.Format(@"{0} taking turn...", this.id.ToString()), true);
        //    Point point = panel1.PointToClient(Cursor.Position);
        //    panel1.CreateGraphics().DrawImage(textures[(int)'z'], new Point((point.X / 32) * 32, (point.Y / 32) * 32));

        //    KeyPress s = new k
        //    while (true)
        //    {
        //        cmd = Form1.();

        //        if (cmd.KeyChar == 's' || cmd.KeyChar == 'S')
        //        {
        //            GameBoard.saveGame(p.id);
        //            continue;
        //        }

        //        if (p.makeMove(cmd.KeyChar))
        //        {
        //            Invalidate();
        //            break;
        //        }
        //        else
        //        {
        //            label1.Text = (String.Format(@"Invalid move, try again {0}.", p.id.ToString()));
        //        }
        //    }
        //}

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            try
            {
                board = game.getGameBoard();
            }
            catch
            {
            }
            int text = 0;
            
            for (int i = 0; i < 20  ; i++)
            {
                for (int j = 0; j < 20; j++)
                {
                    if(board!=null && textures[(int)board.getTextureAtLocation(i,j)]!=null)
                        g.DrawImage(textures[(int)board.getTextureAtLocation(i,j)], new Point(j * 32, i * 32));
                    else
                        g.DrawImage(textures[(int)'%'], new Point(j * 32, i * 32));
                }

                
            }
           
            foreach(PlayerCharacter p in game.getPlayerCharacters())
            {
                g.DrawImage(textures[(int)'>'], new Point(p.xPos*32, p.yPos*32));
            }
           
            foreach(EnemyCharacter p in game.getEnemyCharacters())
            {
                g.DrawImage(textures[(int)'<'], new Point(p.xPos*32, p.yPos*32));
            }

        }
    }
}
