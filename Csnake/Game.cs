using Csnake.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csnake
{
    internal class Game
    {
        private bool running = true;

        private Snake snake = new Snake();

        private string[,] field = new string[17, 19];

        private int tick = 200;

        private bool apple_collected = true;

        private bool apple_created = false;

        private Tuple<int, int> point = new Tuple<int, int>(0, 0);

        private bool can_move = true;

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
                this.field[0, i] = "-";
                this.field[vertical_length - 1, i] = "_";
            }

            // Renders the sides
            for (int i = 1; i < vertical_length -1; i++)
            {
                this.field[i, 0] = "|";
                this.field[i, horizontal_length-1] = "|";
            }

            // Renders the inside
            for (int y = 1; y < vertical_length-1; y++)
            {
                for (int x = 1; x < horizontal_length - 1; x++)
                {
                    this.field[y, x] = " ";
                }
            }

            return true;

        }


        // Create point
        private void CreateApple()
        {
            Random rnd = new Random();
            if (!apple_created) {
                int y_pos = rnd.Next(1, this.field.GetLength(0));
                int x_pos = rnd.Next(1, this.field.GetLength(1));

                this.point = new Tuple<int, int>(y_pos, x_pos);

                this.apple_created = true;
                this.apple_collected = false;
            }

            else if (this.apple_collected)
            {
                int y_pos = rnd.Next(1, this.field.GetLength(0));
                int x_pos = rnd.Next(1, this.field.GetLength(1));

                this.point = new Tuple<int, int>(y_pos, x_pos);

                this.apple_collected = false;
            }
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

                apple_created = false;
                apple_collected = true;

                CreateApple();

                return true;
            }

            return false;
        }

        // Renders the game to the console
        private void RenderGame()
        {
            CreateApple();
            while (running)
            {
                this.RenderField();

                Tuple<int, int> head_position = snake.GetHead();

                this.RenderTail();

                this.field[head_position.Item1, head_position.Item2] = "C";

                this.field[this.point.Item1, this.point.Item2] = "@";

                Thread.Sleep(this.tick);
                Console.Clear();

                var field_string_array = this.BuildField();
                foreach (var row in field_string_array)
                {
                    Console.WriteLine(row.ToString());
                }


                this.CheckApple();

                can_move = true;

            }
        }

        // Takes the two dimensional array and turns into a single array while combining the characters
        private string[] BuildField()
        {
            string[] field_string_array = new string[this.field.GetLength(0)];

            for (int y = 0; y < field_string_array.Length; y++)
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
                    can_move = false;
                    break;

                    case ConsoleKey.UpArrow:
                    this.snake.ChangeDirection(3);
                    can_move = false;
                    break;
                    case ConsoleKey.RightArrow:
                    this.snake.ChangeDirection(8);
                    can_move = false;
                    break;
                    case ConsoleKey.DownArrow:
                    this.snake.ChangeDirection(9);
                    can_move = false;
                    break;

                    default:
                    break;
                }

                }

            }
            while (true);
        }

    }
}
