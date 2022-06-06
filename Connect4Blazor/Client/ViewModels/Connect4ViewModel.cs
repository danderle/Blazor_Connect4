using Connect4Blazor.Client.Data;
using System.Collections.ObjectModel;

namespace Connect4Blazor.Client.ViewModels
{
    /// <summary>
    /// The view model for the Connect4 page
    /// </summary>
    public class Connect4ViewModel
    {
        #region Properties

        /// <summary>
        /// The number of rows of the field
        /// </summary>
        public int Rows => 6;

        /// <summary>
        /// The number of columns of the field
        /// </summary>
        public int Columns => 7;

        /// <summary>
        /// The current clicked column, if greater than max columns none is clicked
        /// </summary>
        public int ClickedColumn = 8;

        /// <summary>
        /// The player colors
        /// </summary>
        public string[] Players = { "red", "yellow" };

        /// <summary>
        /// Current player index
        /// </summary>
        public int ActivePlayer = 0;

        /// <summary>
        /// Flag to let us know if the game is over
        /// </summary>
        public bool GameOver { get; set; } = false;

        /// <summary>
        /// The chips
        /// </summary>
        public ObservableCollection<Chip> Chips { get; set; } = new ObservableCollection<Chip>();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Connect4ViewModel()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    var chip = new Chip
                    {
                        Row = row,
                        Column = col
                    };
                    Chips.Add(chip);
                }
            }
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Clicking on a chip
        /// </summary>
        /// <param name="chip"></param>
        public void MouseClick(Chip chip)
        {
            if (GameOver)
            {
                return;
            }

            ClickedColumn = chip.Column;
            if (Chips[ClickedColumn].Color.Equals(Chip.DefaultColor))
            {
                for (int row = 0; row < Rows; row++)
                {
                    int index = Columns * row + ClickedColumn;
                    Chips[index].Color = Players[ActivePlayer];
                    int next = Columns * (row + 1) + ClickedColumn;
                    if (next < Rows * Columns && Chips[next].Color.Equals(Chip.DefaultColor))
                    {
                        Chips[index].Color = Chip.DefaultColor;
                    }
                    else
                    {
                        break;
                    }
                }
                if (CheckForWinner())
                {
                    GameOver = true;
                    return;
                }

                SwitchPlayer();
            }
        }

        /// <summary>
        /// Reset the game after game over
        /// </summary>
        public void Reset()
        {
            foreach (Chip chip in Chips)
            {
                chip.Color = Chip.DefaultColor;
            }

            ClickedColumn = 8;
            GameOver = false;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Switch the active player
        /// </summary>
        private void SwitchPlayer()
        {
            ActivePlayer++;
            ActivePlayer %= 2;
        }

        /// <summary>
        /// Check if the current player has won the game
        /// </summary>
        /// <returns>true if winner</returns>
        private bool CheckForWinner()
        {
            if (HorizontalCheck() || VerticalCheck() || DiagonalRightCheck() || DiagonalLeftCheck())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check horizontal for win
        /// </summary>
        /// <returns>true if winner</returns>
        private bool HorizontalCheck()
        {
            var currentPlayerColor = Players[ActivePlayer];
            for (int row = 0; row < Rows; row++)
            {
                int chipCount = 0;
                for (int col = 0; col < Columns; col++)
                {
                    int index = GetChipIndex(row, col);
                    if (Chips[index].Color.Equals(currentPlayerColor))
                    {
                        chipCount++;
                    }
                    else
                    {
                        chipCount = 0;
                    }

                    if (chipCount == 4)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check vertical for win
        /// </summary>
        /// <returns>true if winner</returns>
        private bool VerticalCheck()
        {
            var currentPlayerColor = Players[ActivePlayer];
            for (int col = 0; col < Columns; col++)
            {
                int chipCount = 0;
                for (int row = 0; row < Rows; row++)
                {
                    int index = GetChipIndex(row, col);
                    if (Chips[index].Color.Equals(currentPlayerColor))
                    {
                        chipCount++;
                    }
                    else
                    {
                        chipCount = 0;
                    }

                    if (chipCount == 4)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check diagonal right for win
        /// </summary>
        /// <returns>true if winner</returns>
        private bool DiagonalRightCheck()
        {
            for (int row = Rows - 1; row >= 0; row--)
            {
                int chipCount = 0;
                for (int col = 0; col < Columns; col++)
                {
                    chipCount = NextDiagRight(row, col, chipCount);
                    if (chipCount == 4)
                    {
                        return true;
                    }
                    else
                    {
                        chipCount = 0;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Check the next diag up right
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="chipCount"></param>
        /// <returns>The number of consecutive active player chips</returns>
        private int NextDiagRight(int row, int col, int chipCount)
        {
            var currentPlayerColor = Players[ActivePlayer];
            if (row >= 0 && row < Rows && col >= 0 && col < Columns)
            {
                int index = GetChipIndex(row, col);
                if (Chips[index].Color.Equals(currentPlayerColor))
                {
                    chipCount++;
                    return NextDiagRight(row - 1, col + 1, chipCount);
                }
            }

            return chipCount;
        }

        /// <summary>
        /// Check for diagonal left winner
        /// </summary>
        /// <returns>True if winner</returns>
        private bool DiagonalLeftCheck()
        {
            for (int row = Rows - 1; row >= 0; row--)
            {
                int chipCount = 0;
                for (int col = Columns - 1; col >= 0; col--)
                {
                    chipCount = NextDiagLeft(row, col, chipCount);
                    if (chipCount == 4)
                    {
                        return true;
                    }
                    else
                    {
                        chipCount = 0;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Check the next diag up left
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="chipCount"></param>
        /// <returns>The number of consecutive active player chips</returns>
        private int NextDiagLeft(int row, int col, int chipCount)
        {
            var currentPlayerColor = Players[ActivePlayer];
            if (row >= 0 && row < Rows && col >= 0 && col < Columns)
            {
                int index = GetChipIndex(row, col);
                if (Chips[index].Color.Equals(currentPlayerColor))
                {
                    chipCount++;
                    return NextDiagLeft(row - 1, col - 1, chipCount);
                }
            }

            return chipCount;
        }

        /// <summary>
        /// Returns the index of grid
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <returns></returns>
        private int GetChipIndex(int row, int col)
        {
            return Columns * row + col;
        } 
        #endregion
    }
}
