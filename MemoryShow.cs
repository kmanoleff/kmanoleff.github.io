/* "Form" for when MS button is clicked
 * Basically a pop-up that shows the memory storage
 * 
 * MemoryShow() --> takes a list of doubles and add those items to MemoryListBox
 * MemoryShow_Deactivate() --> closes the MemoryShow form any time user clicks outside of the box
 * MemoryItem_Click() --> Bring memory value back into calculators DisplayTextBox
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyCalculator
{
    public partial class MemoryShow : Form
    {
        Form ownerform;
        TextBox ownerformDisplayTextBox;
        public MemoryShow(List<double> memoryValues, Form standard, TextBox displayTextBox)
        {
            ownerform = standard;
            ownerformDisplayTextBox = displayTextBox;
            InitializeComponent();

            // Ensures that memory show form appears "inside" calculator
            int windowX = ownerform.Location.X + 7;
            int windowY = ownerform.Location.Y + 150;
            Point windowLocation = new Point(windowX, windowY);
            this.Location = (windowLocation);

            // Input values into ListBox
            for (int i = 0; i < memoryValues.Count; i++)
            {
                MemoryListBox.Items.Add(memoryValues[i]);
            }
        }

        private void MemoryShow_Deactivate(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MemoryItem_Click(object sender, EventArgs e)
        {
            string text = MemoryListBox.GetItemText(MemoryListBox.SelectedItem);
            ownerformDisplayTextBox.Text = text;
            this.Close();
        }
    }
}
