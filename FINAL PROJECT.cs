using System;

namespace ConnectFour
{
    class Program
    {
        static void Main(string[] args)
        {
            ConnectFourGame game = new ConnectFourGame();
            game.Play();
        }
    }

    public abstract class Player
    {
        public abstract int GetNextMove();
    }

    public class HumanPlayer : Player
    {
        public override int GetNextMove()
        {
            while (true)
            {
                Console.WriteLine("Enter the column number (1-7):");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int col) && col >= 1 && col <= 7)
                {
                    return col;
                }
                else
                {
                    Console.WriteLine("Invalid input. Try again.");
                }
            }
        }
    }

    public class ComputerPlayer : Player
    {
        private Random random;

        public ComputerPlayer()
        {
            random = new Random();
        }

        public override int GetNextMove()
        {
            return random.Next(1, 8);
        }
    }

    public class ConnectFourGame
    {
        private int[,] board;
        private Player player1;
        private Player player2;
        private Player currentPlayer;

        public ConnectFourGame()
        {
            board = new int[6, 7];
            player1 = new HumanPlayer();
            player2 = new ComputerPlayer();
            currentPlayer = player1;
        }

        public void Play()
        {
            bool gameOver = false;

            while (!gameOver)
            {
                PrintBoard();
                int col = currentPlayer.GetNextMove();
                MakeMove(col);

                if (CheckWinner())
                {
                    PrintBoard();
                    Console.WriteLine($"Player {GetCurrentPlayerSymbol()} wins!");
                    gameOver = true;
                }
                else if (IsBoardFull())
                {
                    PrintBoard();
                    Console.WriteLine("It's a draw!");
                    gameOver = true;
                }

                SwitchPlayer();
            }
        }

        private void PrintBoard()
        {
            Console.Clear();
            Console.WriteLine("Connect Four Game");
            Console.WriteLine();

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    char symbol = GetSymbol(board[row, col]);
                    Console.Write("| " + symbol + " ");
                }
                Console.WriteLine("|");
                Console.WriteLine("-----------------------------");
            }
            Console.WriteLine("  1   2   3   4   5   6   7");
            Console.WriteLine();
        }

        private char GetSymbol(int player)
        {
            return player == 1 ? 'X' : player == 2 ? 'O' : ' ';
        }

        private void MakeMove(int col)
        {
            col--;
            for (int row = 5; row >= 0; row--)
            {
                if (board[row, col] == 0)
                {
                    board[row, col] = GetPlayerIndex(currentPlayer);
                    break;
                }
            }
        }

        private int GetPlayerIndex(Player player)
        {
            return player == player1 ? 1 : 2;
        }

        private string GetCurrentPlayerSymbol()
        {
            return GetSymbol(GetPlayerIndex(currentPlayer)).ToString();
        }

        private bool CheckWinner()
        {
            // Check rows
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (board[row, col] != 0 && board[row, col] == board[row, col + 1] &&
                        board[row, col] == board[row, col + 2] && board[row, col] == board[row, col + 3])
                    {
                        return true;
                    }
                }
            }

            // Check columns
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (board[row, col] != 0 && board[row, col] == board[row + 1, col] &&
                        board[row, col] == board[row + 2, col] && board[row, col] == board[row + 3, col])
                    {
                        return true;
                    }
                }
            }

            // Check diagonals (top-left to bottom-right)
            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (board[row, col] != 0 && board[row, col] == board[row + 1, col + 1] &&
                        board[row, col] == board[row + 2, col + 2] && board[row, col] == board[row + 3, col + 3])
                    {
                        return true;
                    }
                }
            }

            // Check diagonals (top-right to bottom-left)
            for (int row = 0; row < 3; row++)
            {
                for (int col = 3; col < 7; col++)
                {
                    if (board[row, col] != 0 && board[row, col] == board[row + 1, col - 1] &&
                        board[row, col] == board[row + 2, col - 2] && board[row, col] == board[row + 3, col - 3])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool IsBoardFull()
        {
            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 7; col++)
                {
                    if (board[row, col] == 0)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void SwitchPlayer()
        {
            currentPlayer = currentPlayer == player1 ? player2 : player1;
        }
    }
}
