using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csnake.Models
{
    internal class Snake
    {
        int direction = 4;
        int length = 3;
        Tuple<int, int>[] snake_body = [new Tuple<int,int>(7, 9), new Tuple<int, int>(7, 10), new Tuple<int, int>(7,11)];

       public bool ChangeDirection(int direction)
        {
            if (direction == this.direction || direction + this.direction == 12) {
            return false;
                }


        else
        {
            this.direction = direction;
            return true;
        }

        }

        public Tuple<int, int> GetHead()
        {
            return this.snake_body[0];
        }

        public Tuple<int, int>[] GetTail() {
            return this.snake_body[1..];
        }


        public int GetDirection()
        {
            return this.direction;
        }

        // Runs when apple has been eaten, adds 1 to length and repositions the body.
        public bool EatApple(int apple_y, int apple_x) {
            this.length++;
            Tuple<int, int>[] new_tail_positions = new Tuple<int, int>[this.length];
            new_tail_positions[0] = new Tuple<int, int>(apple_y, apple_x);
            
            this.snake_body.CopyTo(new_tail_positions, 1);

            this.snake_body = new_tail_positions;

            return true;

        }

        // Checks if head has hit tail
        public bool HitTail()
        {
            var snake_tail = this.snake_body[1..];
            var head_position = this.snake_body[0];
            return snake_tail.Contains(head_position);
        }

        // Returns the new location of the snake
        private (int, int) NewLocation()
        {
            int current_y = this.snake_body[0].Item1;
            int current_x = this.snake_body[0].Item2;

            switch (this.direction)
            {

                case 4:
                current_x--;

                break;

                case 3:
                current_y--;
                break;

                case 8:
                current_x++;
                break;

                case 9:
                current_y++;
                break;

                default:
                break;
            }

            return (current_y, current_x);
        }

        // Handles the movement of the snake
        public bool Move(int food_y, int food_x)
        {

            var (current_y, current_x) = NewLocation();

            if (this.HitTail())
            {
                throw new Exception();
            }

            if (current_y == food_y && current_x == food_x)
                {
                this.EatApple(food_y, food_x);
                return true;
            }

            Tuple<int, int>[] new_tail_positions = new Tuple<int, int>[this.length];
            new_tail_positions[0] = new Tuple<int, int>(current_y, current_x);
            var sliced_tail = this.snake_body.Take(this.length - 1).ToArray();
            sliced_tail.CopyTo(new_tail_positions, 1);

            this.snake_body = new_tail_positions;

            return false;

        }
 
    }
}
