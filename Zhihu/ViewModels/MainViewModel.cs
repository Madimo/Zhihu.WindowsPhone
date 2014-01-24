using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Zhihu.Resources;

namespace Zhihu.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.timeline = new ObservableCollection<TimelineViewModel>();
        }

        /// <summary>
        /// ItemViewModel 对象的集合。
        /// </summary>
        public ObservableCollection<TimelineViewModel> timeline { get; private set; }

        /// <summary>
        /// 返回本地化字符串的示例属性
        /// </summary>
        public string LocalizedSampleProperty
        {
            get
            {
                return AppResources.SampleProperty;
            }
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// 创建一些 ItemViewModel 对象并将其添加到 Items 集合中。
        /// </summary>
        public void LoadData()
        {
            this.IsDataLoaded = true;
        }

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