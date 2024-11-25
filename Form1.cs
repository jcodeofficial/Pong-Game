using System; // Import the System namespace
using System.Collections.Generic; // Import collections
using System.ComponentModel; // Import components
using System.Data; // Import data handling
using System.Drawing; // Import drawing utilities
using System.Linq; // Import LINQ utilities
using System.Text; // Import text handling
using System.Threading.Tasks; // Import threading and tasks
using System.Windows.Forms; // Import Windows Forms components

namespace Pong_Game
{
    public partial class Form1 : Form
    {
        // Variables for ball speed
        int ballXspeed = 4;
        int ballYspeed = 4;

        // Speed for computer paddle
        int speed = 2;

        // Random number generator
        Random rand = new Random();

        // Flags for player paddle movement
        bool goDown, goUp;

        // Counter to change computer paddle speed
        int computer_speed_change = 50;

        // Scores for player and computer
        int playerScore = 0;
        int computerScore = 0;

        // Speed of the player paddle
        int playerSpeed = 8;

        // Arrays for random speeds
        int[] i = { 5, 6, 8, 9, };
        int[] j = { 10, 9, 8, 11, 12 };

        // Constructor for the form
        public Form1()
        {
            InitializeComponent(); // Initialize form components
        }

        // Event handler for the game timer
        private void GameTimerEvent(object sender, EventArgs e)
        {
            // Move the ball
            ball.Top -= ballYspeed;
            ball.Left -= ballXspeed;

            // Update the window title with the current scores
            this.Text = "Player Score : " + playerScore + " -- Computer Score: " + computerScore;

            // Check if the ball hits the top or bottom of the form
            if (ball.Top < 0 || ball.Bottom > this.ClientSize.Height)
            {
                ballYspeed = -ballYspeed; // Reverse ball's Y direction
            }
            // Check if the ball goes out of bounds on the left side
            if (ball.Left < -2)
            {
                ball.Left = 300; // Reset ball position
                ballXspeed = -ballXspeed; // Reverse ball's X direction
                computerScore++; // Increment computer's score
            }
            // Check if the ball goes out of bounds on the right side
            if (ball.Right > this.ClientSize.Width + 2)
            {
                ball.Left = 300; // Reset ball position
                ballXspeed = -ballXspeed; // Reverse ball's X direction
                playerScore++; // Increment player's score
            }

            // Ensure the computer paddle doesn't move out of the form
            if (computer.Top <= 1)
            {
                computer.Top = 0;
            }
            else if (computer.Bottom >= this.ClientSize.Height)
            {
                computer.Top = this.ClientSize.Height - computer.Height;
            }

            // Move the computer paddle towards the ball
            if (ball.Top < computer.Top + (computer.Height / 2) && ball.Left > 300)
            {
                computer.Top -= speed;
            }
            if (ball.Top > computer.Top - (computer.Height / 2) && ball.Left > 300)
            {
                computer.Top += speed;
            }

            // Decrease the counter for changing computer speed
            computer_speed_change -= 1;

            // Change computer speed when the counter reaches 0
            if (computer_speed_change < 0)
            {
                speed = i[rand.Next(i.Length)]; // Pick a random speed
                computer_speed_change = 50; // Reset the counter
            }

            // Move player paddle down if the down arrow key is pressed
            if (goDown && player.Top + player.Height < this.ClientSize.Height)
            {
                player.Top += playerSpeed;
            }

            // Move player paddle up if the up arrow key is pressed
            if (goUp && player.Top > 0)
            {
                player.Top -= playerSpeed;
            }

            // Check for collisions between the ball and paddles
            CheckCollision(ball, player, player.Right + 5);
            CheckCollision(ball, computer, computer.Left - 35);

            // End the game if either player or computer scores more than 5 points
            if (computerScore > 5)
            {
                GameOver("Sorry you lost the game");
            }
            else if (playerScore > 5)
            {
                GameOver("You won this game");
            }
        }

        // Event handler for key down events
        private void KeyisDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                goDown = true; // Set flag to move paddle down
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = true; // Set flag to move paddle up
            }
        }

        // Event handler for key up events
        private void KeyisUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                goDown = false; // Clear flag to stop moving paddle down
            }
            if (e.KeyCode == Keys.Up)
            {
                goUp = false; // Clear flag to stop moving paddle up
            }
        }

        // Method to check for collisions between the ball and paddles
        private void CheckCollision(PictureBox PicOne, PictureBox PicTwo, int offset)
        {
            if (PicOne.Bounds.IntersectsWith(PicTwo.Bounds))
            {
                PicOne.Left = offset; // Adjust ball's position

                int x = j[rand.Next(j.Length)]; // Pick a random speed for X direction
                int y = j[rand.Next(j.Length)]; // Pick a random speed for Y direction

                if (ballXspeed < 0)
                {
                    ballXspeed = x; // If ball was moving left, set new speed to the right
                }
                else
                {
                    ballXspeed = -x; // If ball was moving right, set new speed to the left
                }

                if (ballYspeed < 0)
                {
                    ballYspeed = -y; // If ball was moving up, set new speed to down
                }
                else
                {
                    ballYspeed = y; // If ball was moving down, set new speed to up
                }
            }
        }

        // Method to handle game over scenarios
        private void GameOver(string message)
        {
            gameTimer.Stop(); // Stop the game timer
            MessageBox.Show(message, "Game Over"); // Show game over message
            computerScore = 0; // Reset computer score
            playerScore = 0; // Reset player score
            ballXspeed = ballYspeed = 4; // Reset ball speed
            computer_speed_change = 50; // Reset computer speed change counter
            gameTimer.Start(); // Restart the game timer
        }
    }
}
