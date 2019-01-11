using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TetrisForms {
    public partial class Form1 : Form {

        Random rng = new Random(); //random num generator

        Timer updateTimer = new Timer();

        //number of rows and columns
        private int rows = 24;
        private int columns = 10;

        //size of the cells
        private int cellWidth = 20;
        private int cellHeight = 20;

        //array of colors representing the game board
        //the question mark means the spaces in the game board are allowed to be null (meaning there is no value)
        private Color?[,] gameBoard = new Color?[10, 24];

        bool BoardHasChanged = false;

        //array of possible shape pieces, the game will pick randomly which one to give the player
        //a game piece is a list of points relative to its own origin (0,0)
        List<Point>[] GamePieces = {
            //I
            new List<Point> {
                new Point(0,0),
                new Point(0,1),
                new Point(0,2),
                new Point(0,3)
            },
            // L
            new List<Point> {
                new Point(0, 0),
                new Point(0, 1),
                new Point(0, 2),
                new Point(1, 2)
            },
            //J
            new List<Point> {
                new Point(1, 0),
                new Point(1, 1),
                new Point(1, 2),
                new Point(0, 2)
            },
            //O
            new List<Point> {
                new Point(0, 0),
                new Point(0, 1),
                new Point(1, 0),
                new Point(1, 1)
            },
            //S
            new List<Point> {
                new Point(0, 1),
                new Point(1, 1),
                new Point(1, 0),
                new Point(2, 0)
            },
            //Z
            new List<Point> {
                new Point(0, 0),
                new Point(1, 0),
                new Point(1, 1),
                new Point(2, 1)
            },
            //T
            new List<Point> {
                new Point(0, 0),
                new Point(1, 0),
                new Point(2, 0),
                new Point(1, 1)
            }
        };

        Dictionary<List<Point>, Color> GamePieceColors;

        private List<Point> gamePiece;
        
        private int x_gamePiece = 0;
        private int y_gamePiece = 0;

        private int shiftPieceCounter = 0;

        private int shiftCounterMax = 20;

        public Dictionary<Keys, bool> keyStorage = new Dictionary<Keys, bool>();

        public Form1() {
            InitializeComponent();

            //fill the gameboard with nothing as by default it actually contains colors (which we do not want)
            for(int x = 0; x < columns; x++) {
                for (int y = 0; y < rows; y++) {
                    gameBoard[x, y] = null;
                }
            }

            SetupGrid();

            //set all of the game piece colors because you cannot do this in an instance variable
            //color values based on puyo puyo tetris
            GamePieceColors = new Dictionary<List<Point>, Color> {
                { GamePieces[0], Color.FromArgb(255, 4, 157, 217) },
                { GamePieces[1], Color.FromArgb(255, 254, 112, 2) },
                { GamePieces[2], Color.FromArgb(255, 1, 75, 182) },
                { GamePieces[3], Color.FromArgb(255, 255, 200, 0) },
                { GamePieces[4], Color.FromArgb(255, 100, 185, 40) },
                { GamePieces[5], Color.FromArgb(255, 222, 27, 33) },
                { GamePieces[6], Color.FromArgb(255, 151, 44, 150) }
            };

            gamePiece = GamePieces[1];

            //set the timer interval to 40ms = 25 ticks per second
            updateTimer.Interval = 40;

            updateTimer.Tick += Update;

            //start the timer
            updateTimer.Start();
        }

        ///////////////////////
        // actual game logic //
        ///////////////////////

        //game loop, this will obscure the default update function but we dont care 
        //(we need to use the word "new" to tell it to piss off)
        private new void Update(object sender, EventArgs e) {

            lblDebug.Text = "" + keyStorage.Count();

            //this will be used to track if any changes to the game board were made, meaning it needs to be redrawn

            shiftPieceCounter++;

            //if the counter is greater than 20 = 1 second (or whatever shiftCounterMax is), shift the piece down (or check if it can be)
            if (shiftPieceCounter > (shiftCounterMax / (keyStorage.ContainsKey(Keys.S) && keyStorage[Keys.S] ? 16 : 1))) {
                bool canShiftDown = PieceCanShiftDown(gamePiece);

                //if the piece can be shifted down, do so
                if (canShiftDown) {
                    shiftPieceCounter = 0;
                    y_gamePiece++;
                } else { //otherwise we need to set the piece onto the gameboard.

                    //foreach point on the gamepiece, set the color of that spot on the gameboard to the color of the gamePiece.
                    foreach (Point point in gamePiece) {
                        gameBoard[x_gamePiece + point.X, y_gamePiece + point.Y] = GamePieceColors[gamePiece];
                    }

                    gamePiece = GamePieces[rng.Next(GamePieces.Length)];
                    x_gamePiece = 0;
                    y_gamePiece = 0;
                    shiftPieceCounter = 0;

                    ScanGameBoardForRows();
                }

                BoardHasChanged = true;
            }


            //if the board has changed, redraw it
            if (BoardHasChanged) DrawGameBoard();

            BoardHasChanged = false;
        }

        private void ScanGameBoardForRows() {
            List<int> rowsDeleted = new List<int>();
            //for each row
            for (int y = 0; y < rows; y++) {
                //count how many blocks there are in the row
                int blocks = 0;
                for (int x = 0; x < columns; x++) {
                    if (gameBoard[x, y].HasValue) blocks++;
                }

                //if the blocks span the whole row (each column has a block)
                if (blocks == columns) {
                    //clear each block from that row
                    DeleteRow(y);
                    rowsDeleted.Add(y);
                }
            }
            ShiftGameBoard(rowsDeleted);
        }

        private void ShiftGameBoard(List<int> rows) {
            foreach(int k in rows) {
                for (int y = k - 1; y >= 0; y--) {
                    for (int x = 0; x < columns; x++) {
                        gameBoard[x, y + 1] = gameBoard[x, y];
                    }
                    DeleteRow(y);
                }
            }
        }

        private void DeleteRow(int y) {
            for (int x = 0; x < columns; x++) {
                if (gameBoard[x, y].HasValue) gameBoard[x, y] = null;
            }
        }

        private void DrawGameBoard() {
            ClearGameBoard();

            //for each cell in the gameboard
            for (int x = 0; x < columns; x++) {
                for (int y = 0; y < rows; y++) {
                    //if the cell exists (has a color)
                    if (gameBoard[x, y].HasValue) {
                        SetBoxColor(x, y, gameBoard[x,y].Value);
                    }
                }
            }

            //for each cell in the current gamepiece
            foreach (Point point in gamePiece) {
                SetBoxColor(point.X + x_gamePiece, point.Y + y_gamePiece, GamePieceColors[gamePiece]);
            }
        }

        private bool PieceCanShiftLeft(List<Point> gamePiece) {
            bool canShiftLeft = true;

            foreach (Point point in gamePiece) {
                //foreach point on the gamepiece, 

                //check if there is even any rows below the piece, set canShiftDown to false and break
                if (point.X + x_gamePiece - 1 < 0) {
                    canShiftLeft = false;
                    break;
                }

                //check if there is a block below any of those points, set canShiftDown to false and break
                if (gameBoard[point.X + x_gamePiece - 1, point.Y + y_gamePiece].HasValue) {
                    canShiftLeft = false;
                    break;
                }
            }

            return canShiftLeft;
        }

        private bool PieceCanShiftRight(List<Point> gamePiece) {
            bool canShiftRight = true;

            foreach (Point point in gamePiece) {
                //foreach point on the gamepiece, 

                //check if there is even any rows below the piece, set canShiftDown to false and break
                if (point.X + x_gamePiece + 1 >= columns) {
                    canShiftRight = false;
                    break;
                }

                //check if there is a block below any of those points, set canShiftDown to false and break
                if (gameBoard[point.X + x_gamePiece + 1, point.Y + y_gamePiece].HasValue) {
                    canShiftRight = false;
                    break;
                }
            }

            return canShiftRight;
        }

        private bool PieceCanShiftDown(List<Point> gamePiece) {
            bool canShiftDown = true;

            foreach (Point point in gamePiece) {
                //foreach point on the gamepiece, 

                //check if there is even any rows below the piece, set canShiftDown to false and break
                if (point.Y + y_gamePiece + 1 >= rows) {
                    canShiftDown = false;
                    break;
                }

                //check if there is a block below any of those points, set canShiftDown to false and break
                if (gameBoard[point.X + x_gamePiece, point.Y + y_gamePiece + 1].HasValue) {
                    canShiftDown = false;
                    break;
                }
            }

            return canShiftDown;
        }

        /////////////////////////////////////////////////////////////////////////////////////
        // low level methods, nothing here has anything to do with the game logic directly //
        /////////////////////////////////////////////////////////////////////////////////////

        private void SetBoxColor(int x, int y, Color color) {
            gridGameBoard[x, y].Style.BackColor = color;
        }

        //clears the game board (sets every cell to gray)
        private void ClearGameBoard() {
            for (int x = 0; x < columns; x++) {
                for (int y = 0; y < rows; y++) {
                    SetBoxColor(x, y, Color.Gray);
                }
            }
        }

        //creates the grid, creates its cells and background color

        private void SetupGrid() {

            //make the gameboard the correct size for the cells
            gridGameBoard.Width = columns * cellWidth;
            gridGameBoard.Height = rows * cellHeight;

            //clear all of the rows and columns (just in case)
            gridGameBoard.Rows.Clear();
            gridGameBoard.Columns.Clear();


            //create a template cell that every other cell in the game board will be based on
            DataGridViewCell templateCell = new DataGridViewTextBoxCell();

            //add the number of rows and columns specified
            for (int i = 0; i < columns; i++) {
                //create the new column
                DataGridViewColumn column = new DataGridViewColumn(templateCell);

                //set the width of the column
                column.Width = cellWidth;

                //add the column to the game board
                gridGameBoard.Columns.Add(column);
            }

            for (int i = 0; i < rows; i++) {
                //create the new row
                DataGridViewRow row = new DataGridViewRow();

                //set the height of the row
                row.Height = cellHeight;

                //change the selected color to transpartent so the user cant "select" cells on the board
                row.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.Transparent;

                //add the row to the game board
                gridGameBoard.Rows.Add(row);
            }

            //make sure the grid knows how many columns and rows it has
            gridGameBoard.ColumnCount = columns;
            gridGameBoard.RowCount = rows;

            //the grid will have a default selection, clear it
            gridGameBoard[2,2].Selected = true;

            ClearGameBoard();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e) {
            keyStorage[e.KeyCode] = true;
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e) {
            keyStorage[e.KeyCode] = false;

            if (e.KeyCode == Keys.A) {
                if (PieceCanShiftLeft(gamePiece)) {
                    BoardHasChanged = true;
                    x_gamePiece--;
                } 
            }

            if (e.KeyCode == Keys.D) {
                if (PieceCanShiftRight(gamePiece)) {
                    BoardHasChanged = true;
                    x_gamePiece++;
                }
            }
        }
    }
}
