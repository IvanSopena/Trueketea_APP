using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace TrueketeaApp.MyControls
{
    public class TitleView : Grid
    {

        private Label titleLabel;


        public View LeadingView
        {
            get { return (View)this.GetValue(LeadingViewProperty); }
            set { this.SetValue(LeadingViewProperty, value); }
        }

        public View TrailingView
        {
            get { return (View)this.GetValue(TrailingViewProperty); }
            set { this.SetValue(TrailingViewProperty, value); }
        }

        public View Content
        {
            get { return (View)this.GetValue(ContentProperty); }
            set { this.SetValue(ContentProperty, value); }
        }

        public string Title
        {
            get { return (string)this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }

        public string FontFamily
        {
            get { return (string)this.GetValue(FontFamilyProperty); }
            set { this.SetValue(FontFamilyProperty, value); }
        }

        public FontAttributes FontAttributes
        {
            get { return (FontAttributes)this.GetValue(FontAttributesProperty); }
            set { this.SetValue(FontAttributesProperty, value); }
        }

        public double FontSize
        {
            get { return (double)this.GetValue(FontSizeProperty); }
            set { this.SetValue(FontSizeProperty, value); }
        }



        public TitleView()
        {
            this.ColumnSpacing = 2;
            this.RowSpacing = 8;
            this.Padding = new Thickness(0, 8, 0, 0);
            this.BackgroundColor = (Color)Application.Current.Resources["Gray-Bg"];

            this.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition { Width = 4 },
                new ColumnDefinition(),
                new ColumnDefinition { Width = GridLength.Auto },
                new ColumnDefinition(),
                new ColumnDefinition { Width = 4 },
            };

            this.RowDefinitions = new RowDefinitionCollection
            {
                new RowDefinition { Height = GridLength.Auto },
                new RowDefinition { Height = 1 },
            };

            var boxView = new BoxView { Color = (Color)Application.Current.Resources["Gray-200"] };

            this.Children.Add(this.LeadingView, 1, 0);
            this.Children.Add(this.Content, 2, 0);
            this.Children.Add(this.TrailingView, 3, 0);
            this.Children.Add(boxView, 0, 1);
            SetColumnSpan(boxView, 5);
        }

        public static readonly BindableProperty LeadingViewProperty = BindableProperty.Create(nameof(LeadingView), typeof(View), typeof(TitleView), new ContentView(), BindingMode.Default, null, OnLeadingViewPropertyChanged);
        public static readonly BindableProperty TrailingViewProperty = BindableProperty.Create(nameof(TrailingView), typeof(View), typeof(TitleView), new ContentView(), BindingMode.Default, null, OnTrailingViewPropertyChanged);
        public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(Content), typeof(View), typeof(TitleView), new ContentView(), BindingMode.Default, null, OnContentPropertyChanged);
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(TitleView), string.Empty, BindingMode.Default, null, OnTitlePropertyChanged);
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(TitleView), string.Empty, BindingMode.Default, null, OnFontFamilyPropertyChanged);
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(TitleView), FontAttributes.None, BindingMode.Default, null, OnFontAttributesPropertyChanged);
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(TitleView), 16d, BindingMode.Default, null, OnFontSizePropertyChanged);
        


        private static void OnLeadingViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var titleView = bindable as TitleView;
            var newView = (View)newValue;
            newView.HorizontalOptions = LayoutOptions.Start;
            titleView.Children.Add(newView, 1, 0);
        }

        private static void OnTrailingViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var titleView = bindable as TitleView;
            var newView = (View)newValue;
            newView.HorizontalOptions = LayoutOptions.End;
            titleView.Children.Add(newView, 3, 0);
        }

        private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var titleView = bindable as TitleView;
            var newView = (View)newValue;

            if (!string.IsNullOrEmpty(titleView.Title))
            {
                titleView.Children.Remove(titleView.titleLabel);
            }

            titleView.Children.Add(newView, 2, 0);
        }

        private static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var titleView = bindable as TitleView;
            var newText = (string)newValue;

            if (!string.IsNullOrEmpty(newText))
            {
                titleView.titleLabel = new Label
                {
                    Text = newText,
                    TextColor = Color.White,//(Color)Application.Current.Resources["TextColor"],
                    FontSize = 16,
                    Margin = new Thickness(0, 8),
                    FontFamily = "Montserrat-Medium",
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                };

                if (Device.RuntimePlatform == Device.Android)
                {
                    titleView.titleLabel.LineHeight = 1.5;
                }

                titleView.Children.Remove(titleView.Content);
                titleView.Children.Add(titleView.titleLabel, 2, 0);
            }
        }

        private static void OnFontFamilyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var titleView = bindable as TitleView;

            if (titleView.titleLabel != null)
            {
                titleView.titleLabel.FontFamily = (string)newValue;
            }
        }

        private static void OnFontAttributesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var titleView = bindable as TitleView;

            if (titleView.titleLabel != null)
            {
                titleView.titleLabel.FontAttributes = (FontAttributes)newValue;
            }
        }

        private static void OnFontSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var titleView = bindable as TitleView;

            if (titleView.titleLabel != null)
            {
                titleView.titleLabel.FontSize = (double)newValue;
            }
        }
    }
}
