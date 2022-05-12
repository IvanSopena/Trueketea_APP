using Syncfusion.ListView.XForms;
using System.Threading.Tasks;
using TrueketeaApp.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TrueketeaApp.MyControls
{
    public class SearchableChatList : SearchableListView
    {
        public SearchableChatList()
        {
            this.SelectionChanged -= this.CustomListView_SelectionChanged;
            this.SelectionChanged += this.CustomListView_SelectionChanged;
        }


        public override bool FilterContacts(object obj)
        {
            if (base.FilterContacts(obj))
            {
                var taskInfo = obj as ChatDetail;
                if (taskInfo == null || string.IsNullOrEmpty(taskInfo.SenderName) || string.IsNullOrEmpty(taskInfo.Message))
                {
                    return false;
                }

                var message = taskInfo.Message;
                if (taskInfo.MessageType != "Text")
                {
                    message = string.Empty;
                }

                return taskInfo.SenderName.ToUpperInvariant().Contains(this.SearchText.ToUpperInvariant())
                       || message.ToUpperInvariant().Contains(this.SearchText.ToUpperInvariant());
            }

            return false;
        }

        private async void CustomListView_SelectionChanged(object sender, ItemSelectionChangedEventArgs e)
        {
            Application.Current.Resources.TryGetValue("Gray-100", out var retVal);
            this.SelectionBackgroundColor = (Color)retVal;
            await Task.Delay(100).ConfigureAwait(true);
            this.SelectionBackgroundColor = Color.Transparent;
            this.SelectedItems.Clear();
        }




    }
}
