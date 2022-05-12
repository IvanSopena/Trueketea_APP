using System;
using Syncfusion.ListView.XForms;
using Xamarin.Forms;

namespace TrueketeaApp.MyControls
{
    public class SearchableListView : SfListView
    {


        public static readonly BindableProperty SearchTextProperty =
            BindableProperty.Create(nameof(SearchText), typeof(string), typeof(SearchableListView), null, BindingMode.Default, null, OnSearchTextChanged);

        private string searchText;

        public string SearchText
        {
            get { return (string)this.GetValue(SearchTextProperty); }
            set { this.SetValue(SearchTextProperty, value); }
        }

        public virtual bool FilterContacts(object obj)
        {
            if (this.SearchText == null)
            {
                return false;
            }

            return true;
        }

        private static void OnSearchTextChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var listView = bindable as SearchableListView;
            if (newValue != null && listView.DataSource != null)
            {
                listView.searchText = (string)newValue;
                listView.DataSource.Filter = listView.FilterContacts;
                listView.DataSource.RefreshFilter();
            }

            listView.RefreshView();
        }
    }
}
