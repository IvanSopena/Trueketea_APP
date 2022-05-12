using System;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;

namespace TrueketeaApp.Behaviors
{
    public class ChatMessageListViewBehavior : Behavior<SfListView>
    {
        private SfListView listView;
        public ChatMessageListViewBehavior()
        {
        }

        protected override void OnAttachedTo(SfListView bindable)
        {
            if (bindable != null)
            {
                base.OnAttachedTo(bindable);
                this.listView = bindable;
                this.listView.Loaded += this.ListView_Loaded;
                this.listView.DataSource.SourceCollectionChanged += this.DataSource_SourceCollectionChanged;
            }
        }

        protected override void OnDetachingFrom(SfListView bindable)
        {
            this.listView = null;
            base.OnDetachingFrom(bindable);
        }

        private void DataSource_SourceCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ((LinearLayout)this.listView.LayoutManager).ScrollToRowIndex(
                this.listView.DataSource.DisplayItems.Count - 1, Syncfusion.ListView.XForms.ScrollToPosition.End, true);
        }


        private void ListView_Loaded(object sender, ListViewLoadedEventArgs e)
        {
            ScrollView scrollView = this.listView.Parent as ScrollView;
            this.listView.HeightRequest = scrollView.Height;

            ((LinearLayout)this.listView.LayoutManager).ScrollToRowIndex(
                this.listView.DataSource.DisplayItems.Count - 1, Syncfusion.ListView.XForms.ScrollToPosition.End, true);
        }

    }
}
