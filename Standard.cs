/* This is the form for the basic calculator.  Basic calculator DOES NOT implement the 
 * shunting yard algorithm, so it does not see order of precedence
 * (i.e. 1 + 2 * 3 will equal 9 since it doesnt do multiplication first)
 * 
 * Standard() --> initializing form and creating menu - creating/utilizing menu handled by MenuCreate.cs
 * MenuItem2_Click() --> clicking to switch to scientific calculator
 * KeepFocus() --> keeping cursor focus in the text box so numbers can be entered any time
 * DisplayTextBox_KeyPresses() --> keyboard entries - handled by KeyPresses.cs
 * Number_Click() --> 0 - 9, decimal clicks - handled by ButtonClicks.cs
 * PosNeg_Click() --> user presses +/- button
 * Operator_Click() --> operator clicks - handled in method
 * Equals_Click() --> equal button clicks - handled in ButtonClicks.cs
 * Clear_Click() --> C and CE clicks - handled in method
 * Percent_Click() --> % clicks - handled in ButtonClicks.cs
 * SSI_Click() --> used for functions that don't require two numbers to calculate (i.e. x^2)
 * Memory_Click() --> MS, MR, etc. - showing memory values is handled by MemoryShow.cs (Form)
 */

using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MyCalculator
{
    public partial class Standard : Form
    {
        MenuCreate menuCreate = new MenuCreate(); // Create top menu
        List<MenuItem> menuItems = new List<MenuItem>(); // List of menu items to display in menu
        ButtonClicks buttonClick = new ButtonClicks(); // Call methods when buttons are clicked
        KeyPresses keyPresses = new KeyPresses(); // Call methods when keys are pressed
        List<double> memoryValues = new List<double>(); // List for memory values to be stored
        double numberValue = 0; // Double representation of number taken from DisplayTextBox
        string operation = ""; // String representation of operator
        bool isOperationPerformed = false; // Bool based on whether operation buttons were pressed
        bool isEqualsPerformed = false; // Bool based on whether equals button was clicked
        bool isMemoryStoredPerformed = false; // Bool based on whether MS button was clicked
        double newNumberValue = 0; // Double representation of number to keep adding every time equals is clicked
        string arbTxt = null; // Aribtrary string to satisfy SSI_Click method.. one more arg needed for sci calc

        // Initializes form and creates menu - menu implements MenuCreate.cs
        public Standard()
        {
            InitializeComponent();
            MainMenu mainMenu = new MainMenu(); 
            Menu = mainMenu;
            menuItems = menuCreate.AddMenu(mainMenu);  

            // Disable menu item 1 - already in standard calculator
            menuItems[0].Enabled = false;

            // Menu item to change to scientific calculator implements menuItem2_Click (below)
            menuItems[1].Click += new EventHandler(this.menuItem2_Click);

            // Disable these buttons on start since no values exist in memory
            MemoryViewButton.Enabled = false;
            MemoryRecallButton.Enabled = false;
            MemoryClearButton.Enabled = false;
        }

        // Change to scientific
        private void menuItem2_Click (object sender, EventArgs e)
        {
            Scientific scientific = new Scientific(this, memoryValues);
            Standard standard = this;
            scientific.Show();
            standard.Hide();
        }

        // Method to keep cursor in text box - call after button clicks
        private void Keep_Focus()
        {
            DisplayTextBox.Focus();
            DisplayTextBox.SelectionStart = DisplayTextBox.Text.Length;
        }

        // User inputs from keyboard - implement KeyPresses.cs
        private void DisplayTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(DisplayTextBox.Text == "0" || isOperationPerformed == true)
            {
                DisplayTextBox.Text = "";
            }
            keyPresses.Check_KeyPress(sender, e);
            isOperationPerformed = false;
        }

        // User clicks 0-9, decimal - implement ButtonClicks.cs
        private void Number_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            DisplayTextBox.Text = buttonClick.
                Number_Click(DisplayTextBox.Text, button.Text, isOperationPerformed, isEqualsPerformed, isMemoryStoredPerformed);
            isOperationPerformed = false;
            isMemoryStoredPerformed = false;
            isEqualsPerformed = false;
            Keep_Focus();
        }

        // User clicks pos/neg button - implement ButtonClicks.cs
        private void PosNeg_Click(object sender, EventArgs e)
        {
            if(DisplayTextBox.Text != "")
            {
                DisplayTextBox.Text = buttonClick.PosNeg_Click(DisplayTextBox.Text);
            }           
            Keep_Focus();
        }

        // User clicks +,-,*,/ - handled in method
        private void Operator_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if(!isEqualsPerformed)
            {
                if (numberValue != 0)
                {
                    EqualsButton.PerformClick();
                    operation = button.Text;
                    EquationTextBox.Text = (numberValue + " " + operation);
                    isOperationPerformed = true;

                    // Setting this to false will allow continuing equation after = is clicked
                    isEqualsPerformed = false;
                }
                else
                {
                    operation = button.Text;
                    numberValue = double.Parse(DisplayTextBox.Text);
                    EquationTextBox.Text = (numberValue + " " + operation);
                    isOperationPerformed = true;
                }
            }
            else
            {
                numberValue = 0;
                newNumberValue = Convert.ToDouble(DisplayTextBox.Text);
                operation = button.Text;
                numberValue = double.Parse(DisplayTextBox.Text);
                EquationTextBox.Text = (numberValue + " " + operation);
                isOperationPerformed = true;
            }
            Keep_Focus();
        }

        // User clicks the equals button - implement ButtonClicks.cs
        private void Equals_Click(object sender, EventArgs e)
        {
            if (DisplayTextBox.Text != "Invalid input")
            {
                if (!isEqualsPerformed)
                {
                    newNumberValue = double.Parse(DisplayTextBox.Text);
                    DisplayTextBox.Text = buttonClick.Equals_Click(operation, DisplayTextBox.Text, numberValue);
                    numberValue = double.Parse(DisplayTextBox.Text);
                    EquationTextBox.Clear();
                    isEqualsPerformed = true;
                }
                else
                {
                    DisplayTextBox.Text = buttonClick.Equals_Click(operation, DisplayTextBox.Text, newNumberValue);
                    EquationTextBox.Clear();
                }
            }
            else
            {
                DisplayTextBox.Text = "0";
            }
        }

        // User clicks CE (clear entry) or C (clear all) - handled in method
        private void Clear_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if(button.Text == "CE")
            {
                DisplayTextBox.Text = "0";
            }
            else
            {
                DisplayTextBox.Text = "0";
                EquationTextBox.Text = "";
                numberValue = 0;
                operation = "";
                isOperationPerformed = false;
                newNumberValue = 0;
                isEqualsPerformed = false;
            }
            Keep_Focus();
        }

        // User clicks the % button - implement ButtonClicks.cs
        private void Percent_Click(object sender, EventArgs e)
        {
            double newNumberValue = (numberValue * (Convert.ToDouble(DisplayTextBox.Text) / 100));
            DisplayTextBox.Text = buttonClick.Percentage_Click(operation, DisplayTextBox.Text, numberValue);
            EquationTextBox.Text += (" " + newNumberValue.ToString());
            numberValue = newNumberValue;
            isEqualsPerformed = false;
        }

        // User clicks square, square root, inverse buttons - implement ButtonClicks.cs
        private void SSI_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            numberValue = double.Parse(DisplayTextBox.Text);
            string[] values = buttonClick.SSI_Click(button.Text, DisplayTextBox.Text, numberValue, arbTxt);
            DisplayTextBox.Text = values[0];
            EquationTextBox.Text = values[1];
        }

        // User clicks mem clear, mem recall, add mem value, subtract mem value, mem store, mem view
        // Memory show calls MemoryShow Form
        private void Memory_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if(button.Text == "MC")
            {
                memoryValues.Clear();

                // Disable these buttons since memory has been cleared
                MemoryViewButton.Enabled = false;
                MemoryRecallButton.Enabled = false;
                MemoryClearButton.Enabled = false;
            }
            if(button.Text == "MR")
            {
                // Brings MOST RECENT memory value into DisplayTextBox
                DisplayTextBox.Text = memoryValues[memoryValues.Count - 1].ToString();
            }
            if(button.Text == "M+")
            {
                if(memoryValues.Count == 0)
                {
                    memoryValues.Add(0);
                    memoryValues[0] = (memoryValues[0] + double.Parse(DisplayTextBox.Text));
                }
                else
                {
                    memoryValues[memoryValues.Count - 1] += double.Parse(DisplayTextBox.Text);
                }
                MemoryViewButton.Enabled = true;
                MemoryRecallButton.Enabled = true;
                MemoryClearButton.Enabled = true;
                isMemoryStoredPerformed = true;
            }
            if(button.Text == "M-")
            {
                if (memoryValues.Count == 0)
                {
                    memoryValues.Add(0);
                    memoryValues[0] = 0 - double.Parse(DisplayTextBox.Text);
                }
                else
                {
                    memoryValues[memoryValues.Count - 1] -= double.Parse(DisplayTextBox.Text);
                }
                MemoryViewButton.Enabled = true;
                MemoryRecallButton.Enabled = true;
                MemoryClearButton.Enabled = true;
                isMemoryStoredPerformed = true;
            }
            if(button.Text == "MS")
            {
                memoryValues.Add(double.Parse(DisplayTextBox.Text));
                MemoryViewButton.Enabled = true;
                MemoryRecallButton.Enabled = true;
                MemoryClearButton.Enabled = true;
                isMemoryStoredPerformed = true;
            }
            if(button.Text == "M ▼")
            {
                MemoryShow memoryShow = new MemoryShow(memoryValues, this, DisplayTextBox);
                memoryShow.Show();
            }
        } 
    }
}
