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

using System.Windows.Threading; //необходимо для таймера

namespace GameBird2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer playTimer = new DispatcherTimer(); // Выделяем память для таймера

        double score;   //
        int gravity = 8;
        bool gameOver;
        Rect birdHitPipe; // описывает ширину, высоту и расположение прямоугольника (по идее птица будет понимать, что ударилась об трубу)

        public MainWindow()
        {
            InitializeComponent();

            playTimer.Tick += MainEventTimer; // tick - происходит по истечению интервала таймера
            playTimer.Interval = TimeSpan.FromMilliseconds(20); // получает или задает период времени между тактами таймера
            StartGame();

        }

        private void MainEventTimer(object sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;

            birdHitPipe = new Rect(Canvas.GetLeft(Bird), Canvas.GetTop(Bird), Bird.Width - 5, Bird.Height);

            Canvas.SetTop(Bird, Canvas.GetTop(Bird) + gravity);


            if (Canvas.GetTop(Bird) < -10 || Canvas.GetTop(Bird) > 458) // если птица поднимается выше или ниже ограничения, то игра заканчивается
            {
                EndGame();
            }


            foreach (var x in MyCanvas.Children.OfType<Image>()) //происходит движение
            {
                if ((string)x.Tag == "truba1" || (string)x.Tag == "truba2" || (string)x.Tag == "truba3")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 5);

                    if (Canvas.GetLeft(x) < -100)
                    {
                        Canvas.SetLeft(x, 800);

                        score += .5; 
                    }

                    Rect pipeHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (birdHitPipe.IntersectsWith(pipeHitBox))
                    {
                        EndGame();
                    }
                }

                if ((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, Canvas.GetLeft(x) - 1);

                    if (Canvas.GetLeft(x) <-250)
                    {
                        Canvas.SetLeft(x, 550);
                    }
                }

            }

        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) // если нажат пробел, то 
            {
                Bird.RenderTransform = new RotateTransform(-20, Bird.Width / 2, Bird.Height/2); // птица наклоняется вверх 
                gravity = -8;   
            }
            
            if (e.Key == Key.R && gameOver == true) // если нажата R и игра окончена
            {
                StartGame(); // запускаем игру заново
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e) // ничего не нажато, птица падает
        {
            Bird.RenderTransform = new RotateTransform(5, Bird.Width / 2, Bird.Height / 2); // при падении птица накланяется вниз
            gravity = 8;
        }

        private void StartGame()
        {
            MyCanvas.Focus(); //функция будеи держать элемент в фокусе

            int temp = 300;
            gameOver = false;
            score = 0;

            Canvas.SetTop(Bird, 200); //задал положение птички по умолчанию

            foreach (var x in MyCanvas.Children.OfType<Image>()) //foreach - предназначен для перебора элементов в контейнерах, в том числе в массивах
            {
                if ((string)x.Tag == "truba1") // первые трубы отдалены на 500 от левого края
                {
                    Canvas.SetLeft(x, 500);
                }
                if ((string)x.Tag == "truba2") // вторые трубы отдалены на 800 от левого края
                {
                    Canvas.SetLeft(x, 800);
                }
                if ((string)x.Tag == "truba3") // третьи трубы отдалены на 1100 от левого края
                {
                    Canvas.SetLeft(x, 1100); 
                }

                if ((string)x.Tag == "cloud")
                {
                    Canvas.SetLeft(x, 200 + temp); // отдаляет первое облако на 200. Второе облако когда находит его(+temp) отдалет на 700.  
                    temp = 700;
                }
            }
            playTimer.Start(); // запуск таймера игры
        }

        private void EndGame()
        {
            playTimer.Stop();
            gameOver = true;
            txtScore.Content += "    Game Over, R - restart";
        }
    }
}
