using Microsoft.Phone.Controls;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Resources;
using Zhihu.Library.ZhihuAPI;

namespace Zhihu
{
    public partial class StartPage : PhoneApplicationPage
    {
        bool isLoginTextBoxFadeIn = false;
        bool isLogining = false;

        public StartPage()
        {
            InitializeComponent();
        }

        // 登陆
        private async void LoginZH()
        {
            try
            {
                TopCoverFadeIn();
                isLogining = true;
                ZHAccount account = new ZHAccount();
                await account.Login(Mail.Text, PasswordBox.Password);
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
            }
            catch
            {
                MessageBox.Show("", "登陆失败，请重试", MessageBoxButton.OK);
            }
            finally
            {
                TopCoverFadeOut();
                isLogining = false;
            }
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            base.OnBackKeyPress(e);

            if (isLogining)
            {
                e.Cancel = true;
                return;
            }
            // 如果登陆框显示了，隐藏登陆框并让两个按钮显示
            if (isLoginTextBoxFadeIn)
            {
                LoginTextBoxFadeOut();
                ButtonFadeIn();
                isLoginTextBoxFadeIn = false;
                e.Cancel = true;
                return;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // 如果是注销然后跳转到这个页面，先清空后退栈
            string type;
            if (NavigationContext.QueryString.TryGetValue("type", out type))
            {
                if (type == "logout")
                {
                    while (NavigationService.RemoveBackEntry() != null)
                    {
                        ; // do nothing
                    }
                }
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            isLoginTextBoxFadeIn = true;
            ButtonFadeOut();
            LoginTextBoxFadeIn();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("暂不支持在客户端内注册，是否跳转到知乎注册账号的网页？", "抱歉", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                // to-do: 跳转到知乎注册的网页
                // NavigationService.Navigate(new Uri("http://", UriKind.Absolute));
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            // 如果已经登陆，直接跳转到主页面
            ZHAccount account = new ZHAccount();
            if (account.isLogined())
            {
                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                return;
            }

            FadeIn();
        }

        #region 各种动画
        private void FadeIn()
        {
            Storyboard storyBoard = new Storyboard();

            CubicEase ease = new CubicEase();
            ease.EasingMode = EasingMode.EaseOut;

            Logo.RenderTransform = new TranslateTransform();
            DoubleAnimation logoTransformY = new DoubleAnimation();
            logoTransformY.BeginTime = TimeSpan.FromMilliseconds(1000);
            logoTransformY.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
            logoTransformY.From = Logo.RenderTransformOrigin.Y;
            logoTransformY.To = Logo.RenderTransformOrigin.Y - 200;
            logoTransformY.EasingFunction = ease;
            Storyboard.SetTarget(logoTransformY, Logo);
            Storyboard.SetTargetProperty(logoTransformY, new PropertyPath("(Image.RenderTransform).(TranslateTransform.Y)"));
            storyBoard.Children.Add(logoTransformY);

            Login.Visibility = System.Windows.Visibility.Visible;
            Login.RenderTransform = new TranslateTransform();
            DoubleAnimation loginTransformY = new DoubleAnimation();
            loginTransformY.BeginTime = TimeSpan.FromMilliseconds(2000);
            loginTransformY.Duration = new Duration(TimeSpan.FromMilliseconds(750));
            loginTransformY.From = Login.RenderTransformOrigin.Y;
            loginTransformY.To = Login.RenderTransformOrigin.Y + 80;
            loginTransformY.EasingFunction = ease;
            Storyboard.SetTarget(loginTransformY, Login);
            Storyboard.SetTargetProperty(loginTransformY, new PropertyPath("(Button.RenderTransform).(TranslateTransform.Y)"));
            storyBoard.Children.Add(loginTransformY);

            DoubleAnimation loginTransformOpacity = new DoubleAnimation();
            loginTransformOpacity.BeginTime = TimeSpan.FromMilliseconds(2000);
            loginTransformOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(750));
            loginTransformOpacity.From = 0;
            loginTransformOpacity.To = 1;
            loginTransformOpacity.EasingFunction = ease;
            Storyboard.SetTarget(loginTransformOpacity, Login);
            Storyboard.SetTargetProperty(loginTransformOpacity, new PropertyPath(Button.OpacityProperty));
            storyBoard.Children.Add(loginTransformOpacity);

            Register.Visibility = System.Windows.Visibility.Visible;
            Register.RenderTransform = new TranslateTransform();
            DoubleAnimation registerTransformY = new DoubleAnimation();
            registerTransformY.BeginTime = TimeSpan.FromMilliseconds(2500);
            registerTransformY.Duration = new Duration(TimeSpan.FromMilliseconds(750));
            registerTransformY.From = Register.RenderTransformOrigin.Y;
            registerTransformY.To = Register.RenderTransformOrigin.Y + 80;
            registerTransformY.EasingFunction = ease;
            Storyboard.SetTarget(registerTransformY, Register);
            Storyboard.SetTargetProperty(registerTransformY, new PropertyPath("(Button.RenderTransform).(TranslateTransform.Y)"));
            storyBoard.Children.Add(registerTransformY);

            DoubleAnimation registerTransformOpacity = new DoubleAnimation();
            registerTransformOpacity.BeginTime = TimeSpan.FromMilliseconds(2500);
            registerTransformOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(750));
            registerTransformOpacity.From = 0;
            registerTransformOpacity.To = 1;
            registerTransformOpacity.EasingFunction = ease;
            Storyboard.SetTarget(registerTransformOpacity, Register);
            Storyboard.SetTargetProperty(registerTransformOpacity, new PropertyPath(Button.OpacityProperty));
            storyBoard.Children.Add(registerTransformOpacity);

            storyBoard.Begin();
        }

        private void ButtonFadeIn()
        {
            Storyboard storyBoard = new Storyboard();

            CubicEase ease = new CubicEase();
            ease.EasingMode = EasingMode.EaseOut;

            Login.Visibility = System.Windows.Visibility.Visible;
            Login.RenderTransform = new TranslateTransform();
            DoubleAnimation loginTransformY = new DoubleAnimation();
            loginTransformY.BeginTime = TimeSpan.FromMilliseconds(500);
            loginTransformY.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            loginTransformY.From = Login.RenderTransformOrigin.Y + 160;
            loginTransformY.To = Login.RenderTransformOrigin.Y + 80;
            loginTransformY.EasingFunction = ease;
            Storyboard.SetTarget(loginTransformY, Login);
            Storyboard.SetTargetProperty(loginTransformY, new PropertyPath("(Button.RenderTransform).(TranslateTransform.Y)"));
            storyBoard.Children.Add(loginTransformY);

            DoubleAnimation loginTransformOpacity = new DoubleAnimation();
            loginTransformOpacity.BeginTime = TimeSpan.FromMilliseconds(500);
            loginTransformOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            loginTransformOpacity.From = 0;
            loginTransformOpacity.To = 1;
            loginTransformOpacity.EasingFunction = ease;
            Storyboard.SetTarget(loginTransformOpacity, Login);
            Storyboard.SetTargetProperty(loginTransformOpacity, new PropertyPath(Button.OpacityProperty));
            storyBoard.Children.Add(loginTransformOpacity);

            Register.Visibility = System.Windows.Visibility.Visible;
            Register.RenderTransform = new TranslateTransform();
            DoubleAnimation registerTransformY = new DoubleAnimation();
            registerTransformY.BeginTime = TimeSpan.FromMilliseconds(750);
            registerTransformY.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            registerTransformY.From = Register.RenderTransformOrigin.Y + 160;
            registerTransformY.To = Register.RenderTransformOrigin.Y + 80;
            registerTransformY.EasingFunction = ease;
            Storyboard.SetTarget(registerTransformY, Register);
            Storyboard.SetTargetProperty(registerTransformY, new PropertyPath("(Button.RenderTransform).(TranslateTransform.Y)"));
            storyBoard.Children.Add(registerTransformY);

            DoubleAnimation registerTransformOpacity = new DoubleAnimation();
            registerTransformOpacity.BeginTime = TimeSpan.FromMilliseconds(750);
            registerTransformOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            registerTransformOpacity.From = 0;
            registerTransformOpacity.To = 1;
            registerTransformOpacity.EasingFunction = ease;
            Storyboard.SetTarget(registerTransformOpacity, Register);
            Storyboard.SetTargetProperty(registerTransformOpacity, new PropertyPath(Button.OpacityProperty));
            storyBoard.Children.Add(registerTransformOpacity);

            storyBoard.Begin();
        }

        private void ButtonFadeOut()
        {
            Storyboard storyBoard = new Storyboard();

            CubicEase ease = new CubicEase();
            ease.EasingMode = EasingMode.EaseOut;

            Login.RenderTransform = new TranslateTransform();
            DoubleAnimation loginTransformY = new DoubleAnimation();
            loginTransformY.BeginTime = TimeSpan.FromMilliseconds(0);
            loginTransformY.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            loginTransformY.From = Login.RenderTransformOrigin.Y + 80;
            loginTransformY.To = Login.RenderTransformOrigin.Y + 160;
            loginTransformY.EasingFunction = ease;
            Storyboard.SetTarget(loginTransformY, Login);
            Storyboard.SetTargetProperty(loginTransformY, new PropertyPath("(Button.RenderTransform).(TranslateTransform.Y)"));
            storyBoard.Children.Add(loginTransformY);

            DoubleAnimation loginTransformOpacity = new DoubleAnimation();
            loginTransformOpacity.BeginTime = TimeSpan.FromMilliseconds(0);
            loginTransformOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            loginTransformOpacity.From = 1;
            loginTransformOpacity.To = 0;
            loginTransformOpacity.EasingFunction = ease;
            loginTransformOpacity.Completed += (a, b) =>
            {
                Login.Visibility = System.Windows.Visibility.Collapsed;
            };
            Storyboard.SetTarget(loginTransformOpacity, Login);
            Storyboard.SetTargetProperty(loginTransformOpacity, new PropertyPath(Button.OpacityProperty));
            storyBoard.Children.Add(loginTransformOpacity);

            Register.RenderTransform = new TranslateTransform();
            DoubleAnimation registerTransformY = new DoubleAnimation();
            registerTransformY.BeginTime = TimeSpan.FromMilliseconds(0);
            registerTransformY.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            registerTransformY.From = Register.RenderTransformOrigin.Y + 80;
            registerTransformY.To = Register.RenderTransformOrigin.Y + 160;
            registerTransformY.EasingFunction = ease;
            Storyboard.SetTarget(registerTransformY, Register);
            Storyboard.SetTargetProperty(registerTransformY, new PropertyPath("(Button.RenderTransform).(TranslateTransform.Y)"));
            storyBoard.Children.Add(registerTransformY);

            DoubleAnimation registerTransformOpacity = new DoubleAnimation();
            registerTransformOpacity.BeginTime = TimeSpan.FromMilliseconds(0);
            registerTransformOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            registerTransformOpacity.From = 1;
            registerTransformOpacity.To = 0;
            registerTransformOpacity.EasingFunction = ease;
            registerTransformOpacity.Completed += (a, b) =>
            {
                Register.Visibility = System.Windows.Visibility.Collapsed;
            };
            Storyboard.SetTarget(registerTransformOpacity, Register);
            Storyboard.SetTargetProperty(registerTransformOpacity, new PropertyPath(Button.OpacityProperty));
            storyBoard.Children.Add(registerTransformOpacity);

            storyBoard.Begin();
        }

        private void LoginTextBoxFadeIn()
        {
            Storyboard storyBoard = new Storyboard();

            CubicEase ease = new CubicEase();
            ease.EasingMode = EasingMode.EaseOut;

            Mail.Visibility = System.Windows.Visibility.Visible;
            Mail.RenderTransform = new TranslateTransform();
            DoubleAnimation mailTransformY = new DoubleAnimation();
            mailTransformY.BeginTime = TimeSpan.FromMilliseconds(500);
            mailTransformY.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            mailTransformY.From = Mail.RenderTransformOrigin.Y;
            mailTransformY.To = Mail.RenderTransformOrigin.Y + 80;
            mailTransformY.EasingFunction = ease;
            Storyboard.SetTarget(mailTransformY, Mail);
            Storyboard.SetTargetProperty(mailTransformY, new PropertyPath("(TextBox.RenderTransform).(TranslateTransform.Y)"));
            storyBoard.Children.Add(mailTransformY);

            DoubleAnimation mailTransformOpacity = new DoubleAnimation();
            mailTransformOpacity.BeginTime = TimeSpan.FromMilliseconds(500);
            mailTransformOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            mailTransformOpacity.From = 0;
            mailTransformOpacity.To = 1;
            mailTransformOpacity.EasingFunction = ease;
            Storyboard.SetTarget(mailTransformOpacity, Mail);
            Storyboard.SetTargetProperty(mailTransformOpacity, new PropertyPath(TextBox.OpacityProperty));
            storyBoard.Children.Add(mailTransformOpacity);

            if (PasswordBox.Password == "")
                Password.Visibility = System.Windows.Visibility.Visible;
            else
                PasswordBox.Visibility = System.Windows.Visibility.Visible;
            Password.RenderTransform = new TranslateTransform();
            DoubleAnimation passwordTransformY = new DoubleAnimation();
            passwordTransformY.BeginTime = TimeSpan.FromMilliseconds(750);
            passwordTransformY.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            passwordTransformY.From = Password.RenderTransformOrigin.Y;
            passwordTransformY.To = Password.RenderTransformOrigin.Y + 80;
            passwordTransformY.EasingFunction = ease;
            Storyboard.SetTarget(passwordTransformY, Password);
            Storyboard.SetTargetProperty(passwordTransformY, new PropertyPath("(TextBox.RenderTransform).(TranslateTransform.Y)"));
            storyBoard.Children.Add(passwordTransformY);

            DoubleAnimation passwordTransformOpacity = new DoubleAnimation();
            passwordTransformOpacity.BeginTime = TimeSpan.FromMilliseconds(750);
            passwordTransformOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            passwordTransformOpacity.From = 0;
            passwordTransformOpacity.To = 1;
            passwordTransformOpacity.EasingFunction = ease;
            Storyboard.SetTarget(passwordTransformOpacity, Password);
            Storyboard.SetTargetProperty(passwordTransformOpacity, new PropertyPath(TextBox.OpacityProperty));
            storyBoard.Children.Add(passwordTransformOpacity);

            PasswordBox.RenderTransform = new TranslateTransform();
            DoubleAnimation passwordBoxTransformY = new DoubleAnimation();
            passwordBoxTransformY.BeginTime = TimeSpan.FromMilliseconds(750);
            passwordBoxTransformY.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            passwordBoxTransformY.From = PasswordBox.RenderTransformOrigin.Y;
            passwordBoxTransformY.To = PasswordBox.RenderTransformOrigin.Y + 80;
            passwordBoxTransformY.EasingFunction = ease;
            Storyboard.SetTarget(passwordBoxTransformY, PasswordBox);
            Storyboard.SetTargetProperty(passwordBoxTransformY, new PropertyPath("(PasswordBox.RenderTransform).(TranslateTransform.Y)"));
            storyBoard.Children.Add(passwordBoxTransformY);

            DoubleAnimation passwordBoxTransformOpacity = new DoubleAnimation();
            passwordBoxTransformOpacity.BeginTime = TimeSpan.FromMilliseconds(750);
            passwordBoxTransformOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            passwordBoxTransformOpacity.From = 0;
            passwordBoxTransformOpacity.To = 1;
            passwordBoxTransformOpacity.EasingFunction = ease;
            Storyboard.SetTarget(passwordBoxTransformOpacity, PasswordBox);
            Storyboard.SetTargetProperty(passwordBoxTransformOpacity, new PropertyPath(PasswordBox.OpacityProperty));
            storyBoard.Children.Add(passwordBoxTransformOpacity);

            storyBoard.Begin();
        }

        private void LoginTextBoxFadeOut()
        {
            Storyboard storyBoard = new Storyboard();

            CubicEase ease = new CubicEase();
            ease.EasingMode = EasingMode.EaseOut;

            Mail.Visibility = System.Windows.Visibility.Visible;
            Mail.RenderTransform = new TranslateTransform();
            DoubleAnimation mailTransformY = new DoubleAnimation();
            mailTransformY.BeginTime = TimeSpan.FromMilliseconds(0);
            mailTransformY.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            mailTransformY.From = Mail.RenderTransformOrigin.Y + 80;
            mailTransformY.To = Mail.RenderTransformOrigin.Y;
            mailTransformY.EasingFunction = ease;
            Storyboard.SetTarget(mailTransformY, Mail);
            Storyboard.SetTargetProperty(mailTransformY, new PropertyPath("(TextBox.RenderTransform).(TranslateTransform.Y)"));
            storyBoard.Children.Add(mailTransformY);

            DoubleAnimation mailTransformOpacity = new DoubleAnimation();
            mailTransformOpacity.BeginTime = TimeSpan.FromMilliseconds(0);
            mailTransformOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            mailTransformOpacity.From = 1;
            mailTransformOpacity.To = 0;
            mailTransformOpacity.EasingFunction = ease;
            mailTransformOpacity.Completed += (a, b) =>
            {
                Mail.Visibility = System.Windows.Visibility.Collapsed;
            };
            Storyboard.SetTarget(mailTransformOpacity, Mail);
            Storyboard.SetTargetProperty(mailTransformOpacity, new PropertyPath(TextBox.OpacityProperty));
            storyBoard.Children.Add(mailTransformOpacity);


            if (PasswordBox.Password == "")
            {
                Password.Visibility = System.Windows.Visibility.Visible;
                Password.RenderTransform = new TranslateTransform();
                DoubleAnimation passwordTransformY = new DoubleAnimation();
                passwordTransformY.BeginTime = TimeSpan.FromMilliseconds(0);
                passwordTransformY.Duration = new Duration(TimeSpan.FromMilliseconds(500));
                passwordTransformY.From = Password.RenderTransformOrigin.Y + 80;
                passwordTransformY.To = Password.RenderTransformOrigin.Y;
                passwordTransformY.EasingFunction = ease;
                Storyboard.SetTarget(passwordTransformY, Password);
                Storyboard.SetTargetProperty(passwordTransformY, new PropertyPath("(TextBox.RenderTransform).(TranslateTransform.Y)"));
                storyBoard.Children.Add(passwordTransformY);

                DoubleAnimation passwordTransformOpacity = new DoubleAnimation();
                passwordTransformOpacity.BeginTime = TimeSpan.FromMilliseconds(0);
                passwordTransformOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(500));
                passwordTransformOpacity.From = 1;
                passwordTransformOpacity.To = 0;
                passwordTransformOpacity.EasingFunction = ease;
                passwordTransformOpacity.Completed += (a, b) =>
                {
                    Password.Visibility = System.Windows.Visibility.Collapsed;
                };
                Storyboard.SetTarget(passwordTransformOpacity, Password);
                Storyboard.SetTargetProperty(passwordTransformOpacity, new PropertyPath(TextBox.OpacityProperty));
                storyBoard.Children.Add(passwordTransformOpacity);
            }
            else
            {
                PasswordBox.RenderTransform = new TranslateTransform();
                DoubleAnimation passwordBoxTransformY = new DoubleAnimation();
                passwordBoxTransformY.BeginTime = TimeSpan.FromMilliseconds(0);
                passwordBoxTransformY.Duration = new Duration(TimeSpan.FromMilliseconds(500));
                passwordBoxTransformY.From = PasswordBox.RenderTransformOrigin.Y + 80;
                passwordBoxTransformY.To = PasswordBox.RenderTransformOrigin.Y;
                passwordBoxTransformY.EasingFunction = ease;
                Storyboard.SetTarget(passwordBoxTransformY, PasswordBox);
                Storyboard.SetTargetProperty(passwordBoxTransformY, new PropertyPath("(PasswordBox.RenderTransform).(TranslateTransform.Y)"));
                storyBoard.Children.Add(passwordBoxTransformY);

                DoubleAnimation passwordBoxTransformOpacity = new DoubleAnimation();
                passwordBoxTransformOpacity.BeginTime = TimeSpan.FromMilliseconds(0);
                passwordBoxTransformOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(500));
                passwordBoxTransformOpacity.From = 1;
                passwordBoxTransformOpacity.To = 0;
                passwordBoxTransformOpacity.EasingFunction = ease;
                passwordBoxTransformOpacity.Completed += (a, b) =>
                {
                    PasswordBox.Visibility = System.Windows.Visibility.Collapsed;
                };
                Storyboard.SetTarget(passwordBoxTransformOpacity, PasswordBox);
                Storyboard.SetTargetProperty(passwordBoxTransformOpacity, new PropertyPath(PasswordBox.OpacityProperty));
                storyBoard.Children.Add(passwordBoxTransformOpacity);
            }
            storyBoard.Begin();
        }

        private void TopCoverFadeIn()
        {
            Storyboard storyBoard = new Storyboard();

            CubicEase ease = new CubicEase();
            ease.EasingMode = EasingMode.EaseOut;

            TopCover.Visibility = System.Windows.Visibility.Visible;
            DoubleAnimation topCoverTransformOpacity = new DoubleAnimation();
            topCoverTransformOpacity.BeginTime = TimeSpan.FromMilliseconds(0);
            topCoverTransformOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
            topCoverTransformOpacity.From = 0;
            topCoverTransformOpacity.To = 0.5;
            topCoverTransformOpacity.EasingFunction = ease;
            Storyboard.SetTarget(topCoverTransformOpacity, TopCover);
            Storyboard.SetTargetProperty(topCoverTransformOpacity, new PropertyPath(Grid.OpacityProperty));
            storyBoard.Children.Add(topCoverTransformOpacity);

            storyBoard.Begin();
        }

        private void TopCoverFadeOut()
        {
            Storyboard storyBoard = new Storyboard();

            CubicEase ease = new CubicEase();
            ease.EasingMode = EasingMode.EaseOut;

            TopCover.Visibility = System.Windows.Visibility.Visible;
            DoubleAnimation topCoverTransformOpacity = new DoubleAnimation();
            topCoverTransformOpacity.BeginTime = TimeSpan.FromMilliseconds(0);
            topCoverTransformOpacity.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
            topCoverTransformOpacity.From = 0.5;
            topCoverTransformOpacity.To = 0;
            topCoverTransformOpacity.EasingFunction = ease;
            topCoverTransformOpacity.Completed += (a, b) =>
            {
                TopCover.Visibility = System.Windows.Visibility.Collapsed;
            };
            Storyboard.SetTarget(topCoverTransformOpacity, TopCover);
            Storyboard.SetTargetProperty(topCoverTransformOpacity, new PropertyPath(Grid.OpacityProperty));
            storyBoard.Children.Add(topCoverTransformOpacity);

            storyBoard.Begin();
        }
        #endregion

        #region 事件处理
        private void Mail_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Mail.Text == "邮箱")
            {
                Mail.Text = "";
                Mail.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void Mail_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Mail.Text == "")
            {
                Mail.Text = "邮箱";
                Mail.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void Mail_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                Password.Focus();
        }

        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox.Visibility = System.Windows.Visibility.Visible;
            Password.Visibility = System.Windows.Visibility.Collapsed;
            PasswordBox.Focus();
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Password == "")
            {
                Password.Visibility = System.Windows.Visibility.Visible;
                PasswordBox.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void PasswordBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Focus();
                LoginZH();
            }
        }
        #endregion
    }
}