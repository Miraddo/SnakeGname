using System;
using System.Threading;

namespace SnakeGame
{
    class SnakeGame
    {
        private const int GameHeight = 20;
        private const int GameWidth = 30;
        private readonly int[] SnakeXPositions = new int[50];
        private readonly int[] SnakeYPositions = new int[50];
        private int FruitXPosition, FruitYPosition;
        private int SnakeLength = 3;
        private ConsoleKeyInfo keyInfo = new ConsoleKeyInfo();
        private char CurrentDirection = 'W'; // Default direction
        private readonly Random random = new Random();

        public SnakeGame()
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            Console.CursorVisible = false;
            SnakeXPositions[0] = 5;
            SnakeYPositions[0] = 5;
            PlaceFruitOnBoard();
        }

        private void PlaceFruitOnBoard()
        {
            FruitXPosition = random.Next(2, GameWidth - 2);
            FruitYPosition = random.Next(2, GameHeight - 2);
        }

        public void DrawGameBoard()
        {
            Console.Clear();
            for (int i = 0; i <= GameWidth + 2; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
            for (int i = 0; i < GameHeight; i++)
            {
                Console.Write("|");
                for (int j = 0; j < GameWidth; j++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine("|");
            }
            for (int i = 0; i <= GameWidth + 2; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
        }

        public void ProcessInput()
        {
            if (Console.KeyAvailable)
            {
                keyInfo = Console.ReadKey(true);
                CurrentDirection = char.ToUpper(keyInfo.KeyChar);
            }
        }

        public void UpdateGameState()
        {
            int previousX = SnakeXPositions[0];
            int previousY = SnakeYPositions[0];
            int tempX, tempY;
            switch (CurrentDirection)
            {
                case 'W': SnakeYPositions[0]--; break;
                case 'S': SnakeYPositions[0]++; break;
                case 'D': SnakeXPositions[0]++; break;
                case 'A': SnakeXPositions[0]--; break;
            }

            for (int i = 1; i < SnakeLength; i++)
            {
                tempX = SnakeXPositions[i];
                tempY = SnakeYPositions[i];
                SnakeXPositions[i] = previousX;
                SnakeYPositions[i] = previousY;
                previousX = tempX;
                previousY = tempY;
            }

            if (SnakeXPositions[0] == FruitXPosition && SnakeYPositions[0] == FruitYPosition)
            {
                SnakeLength++;
                PlaceFruitOnBoard();
            }

            DrawSnakeAndFruit();

            CheckForGameOver();
        }

        private void DrawSnakeAndFruit()
        {
            for (int i = 0; i < SnakeLength; i++)
            {
                DrawPoint(SnakeXPositions[i], SnakeYPositions[i], i == 0 ? 'O' : 'o'); // 'O' for head, 'o' for body
            }

            DrawPoint(FruitXPosition, FruitYPosition, '$'); // '$' for fruit
        }

        private void CheckForGameOver()
        {
            // Check for collision with walls
            if (SnakeXPositions[0] < 0 || SnakeXPositions[0] >= GameWidth || SnakeYPositions[0] < 0 || SnakeYPositions[0] >= GameHeight)
            {
                EndGame();
            }

            // Check for collision with itself
            for (int i = 1; i < SnakeLength; i++)
            {
                if (SnakeXPositions[i] == SnakeXPositions[0] && SnakeYPositions[i] == SnakeYPositions[0])
                {
                    EndGame();
                }
            }

            Thread.Sleep(100 - (SnakeLength - 3) * 2); // Increase game speed as the snake grows
        }

        private void DrawPoint(int x, int y, char symbol)
        {
            Console.SetCursorPosition(x + 2, y + 2); // Adjusted for border
            Console.Write(symbol);
        }

        private void EndGame()
        {
            Console.Clear();
            Console.WriteLine("Game over! Press any key to exit.");
            Console.ReadKey();
            Environment.Exit(0);
        }

        static void Main(string[] args)
        {
            SnakeGame game = new SnakeGame();
            while (true)
            {
                game.DrawGameBoard();
                game.ProcessInput();
                game.UpdateGameState();
            }
        }
    }
}