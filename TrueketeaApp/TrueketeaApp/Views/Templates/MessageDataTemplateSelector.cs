using TrueketeaApp.Models;
using TrueketeaApp.Views.Templates;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace TrueketeaApp.Views
{
    
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class MessageDataTemplateSelector : DataTemplateSelector
    {
        
        public MessageDataTemplateSelector()
        {
            this.CardTextTemplate = new DataTemplate(typeof(CardTextTemplate));
            this.OutCardTextTemplate = new DataTemplate(typeof(OutCardTextTemplate));
            this.CardImageTemplate = new DataTemplate(typeof(CardImageTemplate));
            this.OutCardImageTemplate = new DataTemplate(typeof(OutCardImageTemplate));
        }
        
        public DataTemplate CardTextTemplate { get; set; }
        public DataTemplate OutCardTextTemplate { get; set; }
        public DataTemplate CardImageTemplate { get; set; }
        public DataTemplate OutCardImageTemplate { get; set; }


        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            
            if (item != null && ((ChatMessage)item).User_Send.Equals(AppConstant.Constants.UserLoginId))
            {
                if (string.IsNullOrEmpty(((ChatMessage)item).ImagePath))
                {
                    return this.CardTextTemplate;
                }
                else
                {
                    return this.CardImageTemplate;
                }
            }
            else
            {
                if (item != null && string.IsNullOrEmpty(((ChatMessage)item).ImagePath))
                {
                    return this.OutCardTextTemplate;
                }
                else
                {
                    return this.OutCardImageTemplate;
                }
            }
        }

    
    }
}
