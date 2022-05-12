using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using UIKit;
using TrueketeaApp.MyControls;
using TrueketeaApp.iOS.Renderers;
using System.Runtime.Remoting.Contexts;
using CoreGraphics;

[assembly: ExportRenderer(typeof(PlainEntry), typeof(Plainrenderer))]
namespace TrueketeaApp.iOS.Renderers
{
    public class Plainrenderer : EntryRenderer
    {

        //public MyEntryRenderer(Context context) : base(context)
        //{
        //}

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Layer.CornerRadius = 20;
                Control.Layer.BorderWidth = 3f;
                Control.Layer.BorderColor = Color.FromHex("#0BBF2F").ToCGColor();
                Control.Layer.BackgroundColor = Color.Transparent.ToCGColor();

                Control.LeftView = new UIKit.UIView(new CGRect(0, 0, 10, 0));
                Control.LeftViewMode = UIKit.UITextFieldViewMode.Always;
            }
        }


    }
}