using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;


namespace TrueketeaApp.MyControls
{
    [Preserve(AllMembers = true)]
    public class TapAnimationGrid : Grid
    {
        public TapAnimationGrid() => Initialize();

        public static readonly BindableProperty CommandProperty =
           BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TapAnimationGrid), null);

        public static readonly BindableProperty CommandParameterProperty =
           BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(TapAnimationGrid), null);

        public static readonly BindableProperty TappedProperty =
           BindableProperty.Create(
               nameof(Tapped), typeof(bool), typeof(TapAnimationGrid), false, BindingMode.TwoWay, null, propertyChanged: OnTapped);


        private ICommand tappedCommand;

        public ICommand Command
        {
            get => (ICommand)this.GetValue(CommandProperty);
            set => this.SetValue(CommandProperty, value);
        }
        public object CommandParameter
        {
            get => this.GetValue(CommandParameterProperty);
            set => this.SetValue(CommandParameterProperty, value);
        }

        public bool Tapped
        {
            get => (bool)this.GetValue(TappedProperty);
            set => this.SetValue(TappedProperty, value);
        }

        public ICommand TappedCommand => this.tappedCommand
                ?? (this.tappedCommand = new Command(() =>
                {
                    if (this.Tapped)
                    {
                        this.Tapped = false;
                    }
                    else
                    {
                        this.Tapped = true;
                    }

                    if (this.Command != null)
                    {
                        this.Command.Execute(this.CommandParameter);
                    }
                }));
        private static async void OnTapped(BindableObject bindable, object oldValue, object newValue)
        {
            var grid = (TapAnimationGrid)bindable;
            Application.Current.Resources.TryGetValue("Gray-100", out var retVal);
            grid.BackgroundColor = (Color)retVal;

            await Task.Delay(100).ConfigureAwait(true);
            Application.Current.Resources.TryGetValue("Gray-Bg", out var retValue);
            grid.BackgroundColor = (Color)retValue;
        }

        private void Initialize()
        {
            GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = TappedCommand,
            });
        }
    }
}
