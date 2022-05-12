using System;
using Xamarin.Forms;

namespace TrueketeaApp.MyControls
{
    public class BorderlessEditor : Editor
    {
        public BorderlessEditor()
        {
            this.TextChanged += this.ExtendableEditor_TextChanged;
        }


        private void ExtendableEditor_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.InvalidateMeasure();
        }

    }
}
