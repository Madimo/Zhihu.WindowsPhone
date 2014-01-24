using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Zhihu.Resources;
using Zhihu.Library.ZhihuAPI;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using Zhihu.Library.ZhihuAPI.Models;

namespace Zhihu
{
    public partial class MainPage : PhoneApplicationPage
    {
        ZHTimeline timeline;

        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            // 将 listbox 控件的数据上下文设置为示例数据
            DataContext = App.ViewModel;

            // 全局变量初始化
            timeline = new ZHTimeline();
        }

        // 为 ViewModel 项加载数据
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            Thread thread = new Thread(LoadTimeline);
            thread.Start(true);
        }

        private async void LoadTimeline(object isLoadFirstPage)
        {
            try
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    TimelineProgressBar.Visibility = System.Windows.Visibility.Visible;
                });
                if ((bool)isLoadFirstPage)
                {
                    await timeline.GetFirstPage();
                    this.Dispatcher.BeginInvoke(delegate()
                    {
                        App.ViewModel.timeline.Clear();
                    });
                }
                else
                {
                    await timeline.GetNextPage();
                }
                foreach (ZHMFeed item in timeline.timeline)
                {
                    ViewModels.TimelineViewModel tvm = new ViewModels.TimelineViewModel();
                    switch (item.verb)
                    {
                        case ZHMTarget.ZHMTARGET_TYPE_ANSWER_CREATE:
                            tvm.showTitle       = item.target.question.title;
                            tvm.showContent     = item.target.excerpt;
                            tvm.showName        = item.actors[0].name + " 回答了该问题";
                            tvm.showVoteupCount = item.target.voteupCount.ToString();
                            tvm.visibility      = System.Windows.Visibility.Visible;
                            break;
                        case ZHMTarget.ZHMTARGET_TYPE_ANSWER_VOTE_UP:
                            tvm.showTitle       = item.target.question.title;
                            tvm.showContent     = item.target.excerpt;
                            tvm.showName        = item.actors[0].name + " 赞同该回答";
                            tvm.showVoteupCount = item.target.voteupCount.ToString();
                            tvm.visibility      = System.Windows.Visibility.Visible;
                            break;
                        case ZHMTarget.ZHMTARGET_TYPE_QUESTION_FOLLOW:
                            tvm.showTitle       = item.target.title;
                            tvm.showContent     = "";
                            tvm.showName        = item.actors[0].name + " 关注该问题";
                            tvm.showVoteupCount = "0";
                            tvm.visibility      = System.Windows.Visibility.Collapsed;
                            break;
                        case ZHMTarget.ZHMTARGET_TYPE_QUESTION_CREATE:
                            tvm.showTitle       = item.target.title;
                            tvm.showContent     = "";
                            tvm.showName        = item.actors[0].name + " 提了一个问题";
                            tvm.showVoteupCount = "0";
                            tvm.visibility      = System.Windows.Visibility.Collapsed;
                            break;
                        default:
                            tvm.showTitle       = item.target.question.title;
                            tvm.showContent     = "未定义";
                            tvm.showName        = item.verb;
                            tvm.showVoteupCount = "0";
                            tvm.visibility      = System.Windows.Visibility.Collapsed;
                            break;
                    }
                    tvm.feed = item;
                    this.Dispatcher.BeginInvoke(delegate()
                    {
                        App.ViewModel.timeline.Add(tvm);
                    });
                }
            }
            catch
            {

            }
            finally
            {
                this.Dispatcher.BeginInvoke(delegate()
                {
                    TimelineProgressBar.Visibility = System.Windows.Visibility.Collapsed;
                });
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as Pivot).SelectedIndex)
            {
                case 0:
                    ApplicationBar = (ApplicationBar)Resources["AppBar0"];
                    break;
                case 1:
                    ApplicationBar = (ApplicationBar)Resources["AppBar1"];
                    break;
            }
        }
    }
}