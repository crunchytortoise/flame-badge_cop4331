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
        public static char[,] board;
        public Pen blackPen = new Pen(Color.Black, 3);
        public Pen redPen = new Pen(Color.Red, 3);
        public Pen bluePen = new Pen(Color.Blue, 3);
        public SolidBrush redbrush = new SolidBrush(Color.Red);

        public Form1()
        {
            FlameBadge game = new FlameBadge(this);
            InitializeComponent();

            textures = new Image[10000];
            textures[(int)'b'] = Bitmap.FromFile("Art/background.png");
            textures[(int)'B'] = Bitmap.FromFile("Art/in_game_button.png");
            textures[(int)'%'] = Bitmap.FromFile("Art/grass.png");
            textures[(int)'^'] = Bitmap.FromFile("Art/mountain.png");
            textures[(int)'~'] = Bitmap.FromFile("Art/water.png");
            textures[(int)'z'] = Bitmap.FromFile("Art/selected.png");
            textures[(int)'='] = Bitmap.FromFile("Art/bridge.png");
            textures[(int)'#'] = Bitmap.FromFile("Art/tree.png");
            textures[(int)'&'] = Bitmap.FromFile("Art/road.png");
            textures[(int)'+'] = Bitmap.FromFile("Art/castle1.png");
            textures[(int)'*'] = Bitmap.FromFile("Art/castle2.png");
            textures[(int)'<'] = Bitmap.FromFile("Art/enemy.png");
            textures[(int)'>'] = Bitmap.FromFile("Art/player.png");
            textures[(int)'p'] = Bitmap.FromFile("Art/possibleMove.png");
            textures[(int)'P'] = Bitmap.FromFile("Art/possibleAttack.png");
            textures[(int)'@'] = Bitmap.FromFile("Art/border.png");
            textures[(int)'S'] = Bitmap.FromFile("Art/mazeEntrance.png");
            textures[(int)'E'] = Bitmap.FromFile("Art/mazeEntrance.png");
            textures[(int)'h'] = Bitmap.FromFile("Art/health_tick.png");
            textures[(int)'H'] = Bitmap.FromFile("Art/health_bar.png");

            //Debug to check that all images are same dimensions
            foreach (Image x in textures)
            {
                if(x!=null)
                {
                    Console.Write(x.VerticalResolution + "\n");
                    Console.Write(x.HorizontalResolution + "\n");
                    Console.Write(x.HorizontalResolution + "\n");
                }
            }
            //textures[4] = Image.FromFile("../Art/soldier");
            //textures[5] = Image.FromFile("../Art/archer");

            this.game = game;
            this.BackgroundImage = textures[(int)'b'];
            Load.CreateGraphics().DrawImage(textures[(int)'B'], new Point(0, 0));
            Save.CreateGraphics().DrawImage(textures[(int)'B'], new Point(0, 0));
            panel1.Click += new EventHandler(panel1_Click);
            Invalidate();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Click(Object sender, MouseEventArgs e)
        {
        }
        private void panel1_Click(Object sender, EventArgs e)
        {
            Point point = panel1.PointToClient(Cursor.Position);
            int x = (point.X/32);
            int y = (point.Y/32);
            if(game.selectUnit()!=null && game.selectUnit().validMovePerformed(x,y) && GameBoard.update(game.selectUnit(),(short)x,(short)y) )
            {
                game.selectUnit().ActionTaken();
                game.unselectUnit();
                game.checkForTurnChange();
            }
            else if(game.selectUnit()!=null && game.selectUnit().attackUnit( x, y, this.game.getEnemyCharacters()))
            {
                game.selectUnit().ActionTaken();
                game.unselectUnit();
                game.checkForTurnChange();
            }
            else  
                game.selectUnit((point.X / 32), (point.Y / 32));

            panel1.Invalidate();
        }

        public Point[] createPoints(int x, int y)
        {
            Point[] p = new Point[4];
            p[0] = new Point(x*32,y*32);
            p[1] = new Point(x * 32, y * 32 + 32);
            p[2] = new Point(x*32+32,y*32); 
            p[3] = new Point(x*32+32,y*32+32); 
            return p;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            System.Drawing.Drawing2D.FillMode fill = System.Drawing.Drawing2D.FillMode.Winding;
            //Gets gameboard
            try
            {
                board = GameBoard.board;
            }
            catch
            {
            }
            int text = 0;

            //HealthIcon.CreateGraphics().DrawImage(textures[(int)'h'], new Point(0,0));
            Healthbar.CreateGraphics().DrawImage(textures[(int)'H'], new Point(0,0));
            Load.CreateGraphics().DrawImage(textures[(int)'B'], new Point(0, 0));
            Save.CreateGraphics().DrawImage(textures[(int)'B'], new Point(0, 0));
            //Draws tiles to screen. This should be changed to represent the boards size
            for (int i = 0; i < 21  ; i++)
            {
                for (int j = 0; j < 21; j++)
                {
                    if(board!=null && textures[(int)board[i,j]]!=null)
                        g.DrawImageUnscaled(textures[(int)board[i,j]], new Point(j * 32, i * 32));
                    else if(board[i,j]!='@')
                        g.DrawImageUnscaled(textures[(int)'%'], new Point(j * 32, i * 32));
                }

                
            }

            //Handles drawing for the selected playerCharacter
            if(null!=game.selectUnit())
            {
                Console.Write("Drawing\n");
                g.DrawImageUnscaled(textures[(int)'z'], new Point(game.selectUnit().xPos*32, game.selectUnit().yPos*32));
                HealthNum.Text = game.selectUnit().health.ToString();
                
                foreach(Tuple<int,int> x in game.selectUnit().getPossibleMoves())
                {
                    g.DrawImageUnscaled(textures[(int)'p'], new Point(x.Item1*32, x.Item2*32));
                };    
            
                foreach(Character x in GameBoard.getAttackableUnits(game.selectUnit().xPos, game.selectUnit().yPos,game.getEnemyCharacters().Cast<Character>().ToList()))
                {
                    g.DrawImageUnscaled(textures[(int)'P'], new Point(x.xPos*32, x.yPos*32));
                };    
            }
         
            //Draws characters to screen
            foreach(PlayerCharacter p in game.getPlayerCharacters())
            {
                g.DrawImageUnscaled(textures[(int)'>'], new Point(p.xPos*32, p.yPos*32));
                g.FillPolygon(redbrush, new Point[]  { new Point( p.xPos*32+29, p.yPos*32),
                                                          new Point( p.xPos*32+29, p.yPos*32+p.health*2),
                                                          new Point(p.xPos*32+32, p.yPos*32+p.health*2),
                                                          new Point( p.xPos*32+32, p.yPos*32) }, fill );
            }
            foreach(EnemyCharacter p in game.getEnemyCharacters())
            {
                g.DrawImageUnscaled(textures[(int)'<'], new Point(p.xPos*32, p.yPos*32));
                g.FillPolygon(redbrush, new Point[]  { new Point( p.xPos*32+29, p.yPos*32),
                                                          new Point( p.xPos*32+29, p.yPos*32+p.health*2),
                                                          new Point(p.xPos*32+32, p.yPos*32+p.health*2),
                                                          new Point( p.xPos*32+32, p.yPos*32) }, fill );
            }

        }
    }
}
