using CoreGraphics;
using TrueketeaApp.iOS.Renderers;
using TrueketeaApp.MyControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomShadowFrame), typeof(FrameShadowRenderer))]

namespace TrueketeaApp.iOS.Renderers
{
	public class FrameShadowRenderer : FrameRenderer
	{
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> element)
        {
            base.OnElementChanged(element);
            var customShadowFrame = (CustomShadowFrame)this.Element;
            if (customShadowFrame != null)
            {
                this.Layer.CornerRadius = customShadowFrame.Radius;
                this.Layer.ShadowOpacity = customShadowFrame.ShadowOpacity;
                this.Layer.ShadowOffset = new CGSize(customShadowFrame.ShadowOffsetWidth, customShadowFrame.ShadowOffSetHeight);
                this.Layer.Bounds.Inset(customShadowFrame.BorderWidth, customShadowFrame.BorderWidth);
                this.Layer.BorderColor = customShadowFrame.CustomBorderColor.ToCGColor();
                this.Layer.BorderWidth = (float)customShadowFrame.BorderWidth;
            }
        }
    }
}

