using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace FlickrClient.Controls
{
    public class GridViewWithAutoLoading : GridView
    {
        private readonly double eps = 2.0000001;
        private readonly double lowEps = 0.0000001;
        private const int offset = 550;
        public event EventHandler LoadMore;
        public event EventHandler ReachTop;
        private ScrollViewer scrollViewer;
        private ProgressRing progressRing;

        public static DependencyProperty IsLoadingMoreItemsProperty =
            DependencyProperty.Register("IsLoadingMoreItems",
                                        typeof(bool),
                                        typeof(GridViewWithAutoLoading),
                                        new PropertyMetadata(false, OnIsLoadingPropertyChanged));

        public bool IsLoadingMoreItems
        {
            get { return (bool)GetValue(IsLoadingMoreItemsProperty); }
            set { SetValue(IsLoadingMoreItemsProperty, value); }
        }

        public static DependencyProperty VerticalOffsetProperty =
        DependencyProperty.Register("VerticalOffset",
                                    typeof(double),
                                    typeof(GridViewWithAutoLoading),
                                    new PropertyMetadata(double.MinValue, OnVerticalOffsetPropertyChanged));

        public double VerticalOffset
        {
            get { return (double)GetValue(VerticalOffsetProperty); }
            set { SetValue(VerticalOffsetProperty, value); }
        }

        public GridViewWithAutoLoading()
        {
            DefaultStyleKey = typeof(GridViewWithAutoLoading);
        }

        private static void OnIsLoadingPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as GridViewWithAutoLoading;

            if (ctrl == null || ctrl.progressRing == null)
            {
                return;
            }

            var val = (bool)e.NewValue;

            ctrl.progressRing.IsActive = val;
            ctrl.progressRing.Visibility = val ? Visibility.Visible : Visibility.Collapsed;
        }

        private static void OnVerticalOffsetPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ctrl = d as GridViewWithAutoLoading;

            if (ctrl == null)
            {
                return;
            }

            ctrl.OnVerticalOffsetPropertyChanged(e);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            scrollViewer = GetTemplateChild("ScrollViewer") as ScrollViewer;
            progressRing = GetTemplateChild("ProgressRing") as ProgressRing;
            if (scrollViewer != null)
            {
                scrollViewer.Loaded += ScrollViewerLoaded;
            }
        }

        void ScrollViewerLoaded(object sender, RoutedEventArgs e)
        {
            var scrolViewer = sender as ScrollViewer;
            if (scrolViewer == null)
            {
                return;
            }
            var binding = new Binding
            {
                Source = scrolViewer,
                Path = new PropertyPath("VerticalOffset"),
                Mode = BindingMode.OneWay
            };

            SetBinding(VerticalOffsetProperty, binding);
        }

        protected virtual void OnVerticalOffsetPropertyChanged(DependencyPropertyChangedEventArgs e)
        {

            if (scrollViewer == null)
            {
                return;
            }

            if (scrollViewer.ScrollableHeight > 0)
            {
                if (Items != null && Items.Count != 0 && (scrollViewer.ScrollableHeight - scrollViewer.VerticalOffset) < offset)
                {
                    if (LoadMore != null)
                    {
                        LoadMore(this, null);
                    }
                }
            }
        }
    }
}
