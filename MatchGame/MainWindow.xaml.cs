using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MatchGame
{
    using System.Windows.Threading;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int tenthsOfSecondsElapsed;
        int matchesFound;

        public MainWindow()
        {
            InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += Timer_Tick;
            SetUpGame();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            tenthsOfSecondsElapsed++;
            timeTextBlock.Text = (tenthsOfSecondsElapsed / 10F).ToString("0.0s");
            if (matchesFound == 8)
            {
                timer.Stop();
                timeTextBlock.Text += " - Play again?";
            }
        }

        private void SetUpGame()
        {
            //Create a list of eight pairs of emoji
            List<string> animalEmoji = new List<string>()
           {
               "🐶", "🐶",
               "🐱", "🐱",
               "🦁", "🦁",
               "🐯", "🐯",
               "🦒", "🦒",
               "🦊", "🦊",
               "🦝", "🦝",
               "🐮", "🐮",
           };

            //Create a new random number generator
            Random random = new Random();

            //Find every TextBlock in the main grid and repeat the following statments for each of them
            foreach (TextBlock textBlock in mainGrid.Children.OfType<TextBlock>())
            {
                //This if statment makes sure that foreach loop skips the timeTextBlock
                if (textBlock.Name != "timeTextBlock")
                {
                    textBlock.Visibility = Visibility.Visible;
                    //Pick a random number between 0 and the number of emoji left in the list and call it "index"
                    int index = random.Next(animalEmoji.Count);
                    //Use the random number called "index" to get a random emoji from the list               
                    string nextEmoji = animalEmoji[index];
                    //Update the TextBlock with the random emoji from the list
                    textBlock.Text = nextEmoji;
                    //Remove the random emoji from the list
                    animalEmoji.RemoveAt(index);
                }     
            }

            timer.Start();
            tenthsOfSecondsElapsed = 0;
            matchesFound = 0;
        }

        TextBlock lastTextBlockClicked;
        bool findingMatch = false;

        /*If it's the first in the pair being clocked,
         keep track of which TextBlock was clicked and make animal disappear.
        If it's the second one, either make it disappear ( if it's a match)
        or bring back the first one (if it's not).*/

        private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;
            if (findingMatch == false)
            {
                textBlock.Visibility = Visibility.Hidden;
                lastTextBlockClicked = textBlock;
                findingMatch = true;
            }

            else if (textBlock.Text == lastTextBlockClicked.Text)
            {
                matchesFound++;
                textBlock.Visibility = Visibility.Hidden;
                findingMatch = false;
            }

            else
            {
                lastTextBlockClicked.Visibility = Visibility.Visible;
                findingMatch = false;
            }
        }

        private void TimeTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            /*Resets the game if all 8 matched pairs have been found 
            (otherwise it does nothing because the game is still running).*/
            if (matchesFound == 8)
            {
                SetUpGame();
            }
        }

    }
}
