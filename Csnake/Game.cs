using Csnake.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Csnake
{
    internal class Game
    {
        private bool running = true;

        private Snake snake = new Snake();

        private string[,] field = new string[18, 19];

        private readonly int tick = 200;

        private Tuple<int, int> point = new Tuple<int, int>(0, 0);

        private bool can_move = true;

        private int points = 0;

        private List<Tuple<int, int>> empty_spaces = new List<Tuple<int, int>>();

        // Runs the game with two threads
        public void Run_game()
        {

            if (running)
            {
                StartKeyPressed();
                StartRenderGame();

            }

        }

        // Creates, starts the function RenderGame and returns it
        private Thread StartRenderGame()
        {
            var t = new Thread(() => { RenderGame(); });
            t.Start();
            return t;
        }

        // Renders the field
        private bool RenderField()
        {
            int vertical_length = this.field.GetLength(0);
            int horizontal_length = this.field.GetLength(1);

            // Render top and bottom
            for (int i = 0; i < horizontal_length; i++)
            {
                this.field[1, i] = "-";
                this.field[vertical_length - 1, i] = "_";
            }

            // Renders the sides
            for (int i = 2; i < vertical_length - 1; i++)
            {
                this.field[i, 0] = "|";
                this.field[i, horizontal_length - 1] = "|";
            }

            // Renders the inside
            for (int y = 2; y < vertical_length - 1; y++)
            {
                for (int x = 1; x < horizontal_length - 1; x++)
                {
                    this.field[y, x] = " ";
                }
            }

            return true;

        }

        // Creates the positions for empty spaces
        private void CreateEmptySpaces()
        {
            List<Tuple<int, int>> new_empty_spaces = new List<Tuple<int, int>>();

            for (int y = 2; y < field.GetLength(0) -1; y++)
            {
                for (int x = 1; x < field.GetLength(0) - 1; x++)
                {
                    new_empty_spaces.Add(new Tuple<int, int>(y, x));
                }
            }

            this.empty_spaces = new_empty_spaces;
        }

        // Creates the apple
        private void CreateApple()
        {
            Random rnd = new Random();
            this.CreateEmptySpaces();

                int index = rnd.Next(0, this.empty_spaces.Count());

                this.point = this.empty_spaces[index];

        }

        // Replaces the empty positions with the tail and returns the field
        private bool RenderTail()
        {
            var snake_tail = snake.GetTail();

            for (int i = 0; i < snake_tail.Length; i++)
            {
                var tail_row = field[snake_tail[i].Item1, snake_tail[i].Item2] = "#";

            }

            return true;
        }

        // Checks if the apple has been eaten and creates a new
        private bool CheckApple()
        {
            bool eatenApple = snake.Move(this.point.Item1, this.point.Item2);

            if (eatenApple)
            {

                CreateApple();

                return true;
            }

            return false;
        }

        // Renders the game to the console
        private void RenderGame()
        {
            Console.CursorVisible = false;
            CreateApple();
            while (running)
            {
                this.RenderField();

                Tuple<int, int> head_position = snake.GetHead();

                this.RenderTail();

                this.field[head_position.Item1, head_position.Item2] = "C";

                this.field[this.point.Item1, this.point.Item2] = "@";

                Thread.Sleep(this.tick);
                
                Console.SetCursorPosition(0, 0);
                var field_string_array = this.BuildField();

                string field_string = string.Join(System.Environment.NewLine, field_string_array);

                Console.Write(field_string);


                if (this.CheckApple())
                {
                    this.points++;
                };

                can_move = true;

            }
        }

        // Takes the two dimensional array and turns into a single array while combining the characters
        private string[] BuildField()
        {
            string[] field_string_array = new string[this.field.GetLength(0)];
            field_string_array[0] = $"Points: {this.points}";

            for (int y = 1; y < field_string_array.Length; y++)
            {
                StringBuilder sb = new StringBuilder("", this.field.GetLength(1));
                for (int x = 0; x < this.field.GetLength(1); x++)
                {
                    sb.Append(this.field[y, x]);

                }
                field_string_array[y] = sb.ToString();
            }

            return field_string_array;
        }


        // Creates a thread with the function KeyPressed and returns it
        private Thread StartKeyPressed()
        {
            var t = new Thread(() => { KeyPressed(); });
            t.Start();
            return t;
        }


        // Checks for arrow key pressed and changes the number of the direction
        private void KeyPressed()
        {
            ConsoleKeyInfo keyinfo;
            do
            {
                keyinfo = Console.ReadKey();
                if (can_move)
                {

                    switch (keyinfo.Key)
                    {
                        case ConsoleKey.LeftArrow:
                        this.snake.ChangeDirection(4);
                        break;
                        case ConsoleKey.UpArrow:
                        this.snake.ChangeDirection(3);
                        break;
                        case ConsoleKey.RightArrow:
                        this.snake.ChangeDirection(8);
                        break;
                        case ConsoleKey.DownArrow:
                        this.snake.ChangeDirection(9);
                        break;

                        default:
                        break;
                    }
                    can_move = false;
                }

            }
            while (true);
        }

    }
}
