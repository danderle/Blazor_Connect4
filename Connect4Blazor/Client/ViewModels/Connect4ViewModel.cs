using Connect4Blazor.Client.Data;
using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;

namespace Connect4Blazor.Client.ViewModels
{
    public class Connect4ViewModel : IConnect4ViewModel
    {
        public int Rows => 6;
        public int Columns => 7;

        public int ClickedColumn = 8;

        public string[] Players = { "red", "yellow" };

        public int ActivePlayer = 0;

        public bool GameOver { get; set; } = false;

        public ObservableCollection<Chip> Chips { get; set; } = new ObservableCollection<Chip>();

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



        public void Reset()
        {
            foreach (Chip chip in Chips)
            {
                chip.Color = Chip.DefaultColor;
            }

            GameOver = false;
        }

        private void SwitchPlayer()
        {
            ActivePlayer++;
            ActivePlayer = ActivePlayer % 2;
        }

        private bool CheckForWinner()
        {
            if (HorizontalCheck() || VerticalCheck() || DiagonalRightCheck() || DiagonalLeftCheck())
            {
                return true;
            }

            return false;
        }

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

        private int NextDiagLeft(int row, int col, int chipCount)
        {
            var currentPlayerColor = Players[ActivePlayer];
            if (row >= 0 && row < Rows && col >= 0 && col < Columns)
            {
                int index = GetChipIndex(row, col);
                if (Chips[index].Color.Equals(currentPlayerColor))
                {
                    chipCount++;
                    Console.WriteLine($"Row {row} Col {col} ChipCount {chipCount}");
                    return NextDiagLeft(row - 1, col - 1, chipCount);
                }
            }

            return chipCount;
        }

        private int GetChipIndex(int row, int col)
        {
            return Columns * row + col;
        }
    }
}
