using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csnake.Models
{
    internal class Snake
    {
        int direction = 0;
        int length = 3;
        Tuple<int, int>[] snake_body = [new Tuple<int,int>(5, 5), new Tuple<int, int>(6, 5), new Tuple<int, int>(7,5)];

       public void ChangeDirection(int direction)
        {
            this.direction = direction;

        }

        public Tuple<int, int> GetHead()
        {
            return this.snake_body[0];
        }

        public Tuple<int, int>[] GetTail() {
            return this.snake_body[1..];
        }

        void AddFood(int y, int x) {
            this.length++;
            Tuple<int, int>[] new_tail_positions = new Tuple<int, int>[this.length];
            new_tail_positions[0] = new Tuple<int, int>(y, x);
            
            this.snake_body.CopyTo(new_tail_positions, 1);

            this.snake_body = new_tail_positions;

        }

        // Checks if head has hit tail
        public bool HitTail()
        {
            var snake_tail = this.snake_body[1..];
            var head_position = this.snake_body[0];
            return snake_tail.Contains(head_position);
        }

        public void Move(int food_y, int food_x)
        {
            int y = this.snake_body[0].Item1;
            int x = this.snake_body[0].Item2;

            switch (this.direction)
            {
               
                case 0:
                x--;

                break;

                case 1:
                y--;
                break;

                case 2:
                x++;
                break;

                case 3:
                y++;
                break;

                default:
                break;
            }

            if (this.HitTail())
            {
                throw new Exception();
            }

            if (this.snake_body[0].Item1 == food_y &&  this.snake_body[0].Item2 == food_x)
                {
                this.AddFood(food_y, food_x);
            }

            Tuple<int, int>[] new_tail_positions = new Tuple<int, int>[this.length];
            new_tail_positions[0] = new Tuple<int, int>(y, x);
            var sliced_tail = this.snake_body.Take(this.length-1).ToArray();
            sliced_tail.CopyTo(new_tail_positions, 1);

            this.snake_body = new_tail_positions;

        }
 
    }
}
