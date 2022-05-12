
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TrueketeaApp.MyControls;
using Application = Android.App.Application;
using Android.Graphics.Drawables;

[assembly: ExportRenderer(typeof(PlainEntry), typeof(TrueketeaApp.Droid.PlainRenderer))]
namespace TrueketeaApp.Droid
{
    public class PlainRenderer : EntryRenderer
    {

        public PlainRenderer()
            : base(Application.Context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);
           

            if (e.OldElement == null)
            {
                //Control.SetBackgroundResource(Resource.Layout.rounded_shape);
                var gradientDrawable = new GradientDrawable();
                gradientDrawable.SetCornerRadius(70f);
                gradientDrawable.SetStroke(5, Android.Graphics.Color.Rgb(11,191,47));
                gradientDrawable.SetColor(Android.Graphics.Color.Transparent);
                Control.SetBackground(gradientDrawable);

                Control.SetPadding(50, Control.PaddingTop, Control.PaddingRight,
                Control.PaddingBottom);
            }
        }

    }
}