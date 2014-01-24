using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Zhihu.Library.ZhihuAPI.Models;

namespace Zhihu.ViewModels
{
    public class TimelineViewModel : INotifyPropertyChanged
    {
        private string _showTitle;
        public string showTitle
        {
            get
            {
                return _showTitle;
            }
            set
            {
                if (value != _showTitle)
                {
                    _showTitle = value;
                    NotifyPropertyChanged("showTitle");
                }
            }
        }

        private string _showContent;
        public string showContent
        {
            get
            {
                return _showContent;
            }
            set
            {
                if (value != _showContent)
                {
                    _showContent = value;
                    NotifyPropertyChanged("showTitle");
                }
            }
        }

        private string _showName;
        public string showName
        {
            get
            {
                return _showName;
            }
            set
            {
                if (value != _showName)
                {
                    _showName = value;
                    NotifyPropertyChanged("showName");
                }
            }
        }

        private System.Windows.Visibility _visibility;
        public System.Windows.Visibility visibility
        {
            get
            {
                return _visibility;
            }
            set
            {
                if (value != _visibility)
                {
                    _visibility = value;
                    NotifyPropertyChanged("visibility");
                }
            }
        }

        private string _showVoteupCount;
        public string showVoteupCount
        {
            get
            {
                return _showVoteupCount;
            }
            set
            {
                if (value != _showVoteupCount)
                {
                    _showVoteupCount = value;
                    NotifyPropertyChanged("showVoteupCount");
                }
            }
        }

        public ZHMFeed feed = new ZHMFeed();

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

}