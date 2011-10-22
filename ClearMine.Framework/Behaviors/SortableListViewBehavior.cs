namespace ClearMine.Framework.Behaviors
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Threading;

    using ClearMine.Common.Utilities;
    using ClearMine.Framework.Controls;

    public class SortInfo
    {
        public GridViewColumnHeader LastSortColumn { get; set; }

        public VisualAdorner CurrentAdorner { get; set; }
    }

    public static class SortableListViewBehavior
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty HeaderSortProperty =
            DependencyProperty.RegisterAttached("HeaderSort", typeof(bool), typeof(SortableListViewBehavior), new UIPropertyMetadata(new PropertyChangedCallback(OnHeaderSortPropertyChanged)));

        /// <summary>
        /// 
        /// </summary>
        internal static readonly DependencyPropertyKey SortInfoProperty =
            DependencyProperty.RegisterAttachedReadOnly("SortInfo", typeof(SortInfo), typeof(SortableListViewBehavior), new PropertyMetadata());

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty SortFieldProperty =
            DependencyProperty.RegisterAttached("SortField", typeof(string), typeof(SortableListViewBehavior));

        public static bool GetHeaderSort(DependencyObject obj)
        {
            return (bool)obj.GetValue(HeaderSortProperty);
        }

        public static void SetHeaderSort(DependencyObject obj, bool value)
        {
            obj.SetValue(HeaderSortProperty, value);
        }

        public static SortInfo GetSortInfo(DependencyObject obj)
        {
            return (SortInfo)obj.GetValue(SortInfoProperty.DependencyProperty);
        }

        internal static void SetSortInfo(DependencyObject obj, SortInfo value)
        {
            obj.SetValue(SortInfoProperty.DependencyProperty, value);
        }

        public static string GetSortField(DependencyObject obj)
        {
            return (string)obj.GetValue(SortFieldProperty);
        }

        public static void SetSortField(DependencyObject obj, string value)
        {
            obj.SetValue(SortFieldProperty, value);
        }

        private static void OnHeaderSortPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView == null)
                throw new InvalidOperationException("HeaderSort Property can only be set on a ListView");

            if ((bool)e.NewValue)
            {
                listView.AddHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(OnListViewHeaderClick));
                listView.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(() => InitializeSortDirection(listView)));
            }
            else
            {
                listView.RemoveHandler(GridViewColumnHeader.ClickEvent, new RoutedEventHandler(OnListViewHeaderClick));
            }
        }

        private static void OnListViewHeaderClick(object sender, RoutedEventArgs e)
        {
            ListView listView = e.Source as ListView;
            GridViewColumnHeader header = e.OriginalSource as GridViewColumnHeader;
            if (header == null || header.Role == GridViewColumnHeaderRole.Padding)
            {
                return;
            }

            SortInfo sortInfo = listView.GetValue(SortInfoProperty.DependencyProperty) as SortInfo;

            if (sortInfo != null)
            {
                AdornerLayer.GetAdornerLayer(sortInfo.LastSortColumn).Remove(sortInfo.CurrentAdorner);
                listView.Items.SortDescriptions.Clear();
            }
            else
                sortInfo = new SortInfo();

            if (sortInfo.LastSortColumn == header)
                (sortInfo.CurrentAdorner.Child as ListSortDecorator).SortDirection = (sortInfo.CurrentAdorner.Child as ListSortDecorator).SortDirection == ListSortDirection.Ascending ? ListSortDirection.Descending : ListSortDirection.Ascending;
            else
                sortInfo.CurrentAdorner = new VisualAdorner(header, new ListSortDecorator());

            sortInfo.LastSortColumn = header;
            listView.SetValue(SortInfoProperty, sortInfo);

            AdornerLayer.GetAdornerLayer(header).Add(sortInfo.CurrentAdorner);
            SortDescription sortDescriptioin = new SortDescription()
            {
                Direction = (sortInfo.CurrentAdorner.Child as ListSortDecorator).SortDirection,
                PropertyName = header.Column.GetValue(SortFieldProperty) as string ?? header.Column.Header as string
            };
            listView.Items.SortDescriptions.Add(sortDescriptioin);
        }

        private static void InitializeSortDirection(ListView listView)
        {
            #region Guards
            var dataSource = listView.ItemsSource;
            if (dataSource == null)
                return;

            var dataview = CollectionViewSource.GetDefaultView(dataSource);
            if (dataview == null || dataview.SortDescriptions.Count == 0)
                return;

            SortInfo sortInfo = listView.GetValue(SortInfoProperty.DependencyProperty) as SortInfo;
            if (sortInfo != null)
                return;
            #endregion

            foreach (var columnHeader in listView.FindChildren<GridViewColumnHeader>(h => h.Role != GridViewColumnHeaderRole.Padding))
            {
                if (GetSortField(columnHeader.Column).Equals(dataview.SortDescriptions[0].PropertyName))
                {
                    sortInfo = new SortInfo();
                    sortInfo.LastSortColumn = columnHeader;
                    sortInfo.CurrentAdorner = new VisualAdorner(columnHeader, new ListSortDecorator());
                    AdornerLayer.GetAdornerLayer(columnHeader).Add(sortInfo.CurrentAdorner);
                    listView.SetValue(SortInfoProperty, sortInfo);
                    break;
                }
            }
        }
    }
}
