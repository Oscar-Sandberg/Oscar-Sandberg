using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace SnakeGame
{
    class Program
    {
        static int screenWidth = 50;
        static int screenHeight = 20;
        static int snakeInitialLength = 3;
        static int gameSpeed = 200; // Milliseconds

        static int score = 0;
        static bool isGameOver = false;
        static bool restartRequested = false;
        static Direction direction = Direction.Right;

        static List<Position> snake = new List<Position>();
        static Position food;

        static void Main(string[] args)
        {
            Console.Title = "Snake Game";
            Console.CursorVisible = false;
            Console.SetWindowSize(screenWidth + 2, screenHeight + 4);
            Console.SetBufferSize(screenWidth + 2, screenHeight + 4);

            Console.ForegroundColor = ConsoleColor.Blue; // Set line color to blue

            while (true)
            {
                InitializeGame();

                while (!isGameOver)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        HandleInput(key);
                    }

                    MoveSnake();
                    DrawSnake();
                    DrawFood();
                    CheckCollision();
                    Thread.Sleep(gameSpeed);
                }

                GameOver();
                HandleRestart();
            }
        }

        static void InitializeGame()
        {
            score = 0;
            isGameOver = false;
            direction = Direction.Right;

            // Set the initial position of the snake
            snake.Clear();
            for (int i = snakeInitialLength - 1; i >= 0; i--)
            {
                snake.Add(new Position(i, 0));
            }

            // Generate the initial food position
            GenerateFood();
        }

        static void GenerateFood()
        {
            int x = new Random().Next(0, screenWidth);
            int y = new Random().Next(0, screenHeight);
            food = new Position(x, y);
        }

        static void HandleInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.LeftArrow:
                    if (direction != Direction.Right)
                        direction = Direction.Left;
                    break;
                case ConsoleKey.RightArrow:
                    if (direction != Direction.Left)
                        direction = Direction.Right;
                    break;
                case ConsoleKey.UpArrow:
                    if (direction != Direction.Down)
                        direction = Direction.Up;
                    break;
                case ConsoleKey.DownArrow:
                    if (direction != Direction.Up)
                        direction = Direction.Down;
                    break;
                case ConsoleKey.R:
                    if (isGameOver)
                        restartRequested = true;
                    break;
            }
        }

        static void HandleRestart()
        {
            while (restartRequested)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.R)
                    {
                        restartRequested = false;
                    }
                }
            }
        }

        static void MoveSnake()
        {
            Position snakeHead = snake.First();
            Position nextPosition = new Position(snakeHead.X, snakeHead.Y);

            switch (direction)
            {
                case Direction.Left:
                    nextPosition.X--;
                    break;
                case Direction.Right:
                    nextPosition.X++;
                    break;
                case Direction.Up:
                    nextPosition.Y--;
                    break;
                case Direction.Down:
                    nextPosition.Y++;
                    break;
            }

            snake.Insert(0, nextPosition);

            if (nextPosition.X == food.X && nextPosition.Y == food.Y)
            {
                // The snake has eaten the food
                score++;
                GenerateFood();
            }
            else
            {
                // Remove the tail segment
                snake.RemoveAt(snake.Count - 1);
            }
        }

        static void CheckCollision()
        {
            Position snakeHead = snake.First();

            // Check if the snake has collided with the walls
            if (snakeHead.X < 0 || snakeHead.X >= screenWidth ||
                snakeHead.Y < 0 || snakeHead.Y >= screenHeight)
            {
                isGameOver = true;
                return;
            }

            // Check if the snake has collided with its own body
            for (int i = 1; i < snake.Count; i++)
            {
                if (snakeHead.X == snake[i].X && snakeHead.Y == snake[i].Y)
                {
                    isGameOver = true;
                    return;
                }
            }
        }

        static void DrawSnake()
        {
            foreach (var segment in snake)
            {
                Console.SetCursorPosition(segment.X + 1, segment.Y + 2);
                Console.ForegroundColor = ConsoleColor.White; // Set snake color to white
                Console.Write("■");
            }
        }

        static void DrawFood()
        {
            Console.SetCursorPosition(food.X + 1, food.Y + 2);
            Console.ForegroundColor = ConsoleColor.Green; // Set food color to green
            Console.Write("@");
        }

        static void GameOver()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Blue; // Set line color to blue
            Console.SetCursorPosition(screenWidth / 2 - 4, screenHeight / 2);
            Console.WriteLine("Game Over");
            Console.SetCursorPosition(screenWidth / 2 - 7, screenHeight / 2 + 1);
            Console.WriteLine("Your score: " + score);
            Console.SetCursorPosition(screenWidth / 2 - 8, screenHeight / 2 + 3);
            Console.WriteLine("Press 'R' to restart");
        }
    }

    enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
