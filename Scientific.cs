/* Form for scientific calculator
 * 
 * Scientific() --> initializing form and creating menu - creating/utilizing menu handled by MenuCreate.cs
 * MenuItem1_Click() --> clicking to switch to scientific calculator
 * Scientific_FormClosed() --> makes sure standard calculator is closed if app is closed from this form
 * KeepFocus() --> keeping cursor focus in the text box so numbers can be entered any time
 * DisplayTextBox_TextChanged --> checks to see if "Invalid input" and if so, disable any buttons that parse that string
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
using System.Text;
using System.Windows.Forms;

namespace MyCalculator
{
    public partial class Scientific : Form
    {
        Standard ownerForm = new Standard(); // Represents form that user was in previously
        ChangeFunction changeFunction = new ChangeFunction(); // Call methods when change function button is clicked
        MenuCreate menuCreate = new MenuCreate(); // Create top menu
        List<MenuItem> menuItems = new List<MenuItem>(); // List of menu items to display
        ButtonClicks buttonClick = new ButtonClicks(); // Call methods when buttons are clicked
        KeyPresses keyPresses = new KeyPresses(); // Call methods when keys are pressed
        List<double> memoryValues = new List<double>(); // List for memory values to be stored
        double numberValue = 0; // Double representation of number taken from DisplayTextBox
        string operation = ""; // String representation of operator
        bool isOperationPerformed = false; // Bool based on whether operation buttons were pressed
        bool isEqualsPerformed = false; // Bool based on whether equals button was clicked
        bool isMemoryStoredPerformed = false; // Bool based on whether MS button was clicked
        double newNumberValue = 0; // Double representation of number to keep adding every time equals is clicked

        public Scientific(Standard standard, List<double> pMemoryValues)
        {
            InitializeComponent();
            ownerForm = standard;
            this.Location = standard.Location;
            MainMenu mainMenu = new MainMenu();
            Menu = mainMenu;
            menuItems = menuCreate.AddMenu(mainMenu);
            menuItems[0].Click += new EventHandler(this.menuItem1_Click);
            menuItems[1].Enabled = false; ;           
            standard.Hide();

            // Assigns existing memory from prior mode into this mode
            memoryValues = pMemoryValues;
        }

        // Go back to standard calculator
        private void menuItem1_Click(object sender, EventArgs e)
        {
            ownerForm.Show();
            this.Hide();
        }

        // Makes sure standard calculator is closed if app is closed from this form
        private void Scientific_FormClosed(object sender, FormClosedEventArgs e)
        {
            ownerForm.Close();
        }

        // Method to keep cursor in text box - call after button clicks
        private void Keep_Focus()
        {
            DisplayTextBox.Focus();
            DisplayTextBox.SelectionStart = DisplayTextBox.Text.Length;
        }

        // Checks to see if "Invalid input" and if so, disable any buttons that parse that string
        private void DisplayTextBox_TextChanged(object sender, EventArgs e)
        {
            Button[] invalidButtons = {RightParenthesisButton, LeftParenthesisButton, PosNegButton, FactorialButton, PiButton,
                FunctionButton, SquareRootAndInverseButton, SquareAndCubeButton, TenExponentialButton, XToTheYAndYRootButton,
                LogarithmicButton, SineButton, ExponentButton, CosineButton, AddButton, SubtractButton, MultiplyButton, DivideButton,
                DegAndDmsButton, TangentButton, MemoryClearButton, MemoryRecallButton, AddMemoryValueButton,SubtractMemoryValueButton,
                MemoryStoreButton, MemoryViewButton, AngularUnitButton, HyperbolicFunctionButton, ForceExponentialButton,
                DecimalButton};
            if (DisplayTextBox.Text == "Invalid input")
            {
                for (int i = 0; i < invalidButtons.Length; i++)
                {
                    invalidButtons[i].Enabled = false;
                }
            }
            else
            {
                for (int i = 0; i < invalidButtons.Length; i++)
                {
                    invalidButtons[i].Enabled = true;
                }
                if(memoryValues.Count == 0)
                {
                    MemoryViewButton.Enabled = false;
                    MemoryRecallButton.Enabled = false;
                    MemoryClearButton.Enabled = false;
                }
            }

        }

        // User inputs from keyboard - implement KeyPresses.cs
        private void DisplayTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (DisplayTextBox.Text == "0" || isOperationPerformed == true || DisplayTextBox.Text == "Invalid input")
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
            if (DisplayTextBox.Text != "")
            {
                DisplayTextBox.Text = buttonClick.PosNeg_Click(DisplayTextBox.Text);
            }
            Keep_Focus();
        }

        // User clicks +,-,*,/,x^y - handled in method
        private void Operator_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (!isEqualsPerformed)
            {
                if (numberValue != 0)
                {
                    EqualsButton.PerformClick();
                    operation = button.Text;
                    if (button.Text == "x^y")
                    {
                        EquationTextBox.Text = (numberValue + " ^ ");
                    }
                    else
                    {
                        EquationTextBox.Text = (numberValue + " " + operation);
                    }
                    isOperationPerformed = true;
                    isEqualsPerformed = false;
                }
                else
                {
                    operation = button.Text;
                    numberValue = double.Parse(DisplayTextBox.Text);
                    if (button.Text == "x^y")
                    {
                        EquationTextBox.Text = (numberValue + " ^ ");
                    }
                    else if (button.Text == "Exp")
                    {
                        EquationTextBox.Text = (numberValue + ".e+");
                    }
                    else
                    {
                        EquationTextBox.Text = (numberValue + " " + operation);
                    }
                    isOperationPerformed = true;
                }
            }
            else
            {
                numberValue = 0;
                newNumberValue = Convert.ToDouble(DisplayTextBox.Text);
                operation = button.Text;
                numberValue = double.Parse(DisplayTextBox.Text);
                if (button.Text == "x^y")
                {
                    EquationTextBox.Text = (numberValue + " ^ ");
                }
                else if (button.Text == "Exp")
                {
                    EquationTextBox.Text = (numberValue + ".e+");
                }
                else
                {
                    EquationTextBox.Text = (numberValue + " " + operation);
                }
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
            if (button.Text == "CE")
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

        // User clicks square, square root, inverse buttons - implement ButtonClicks.cs
        private void SSI_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            numberValue = double.Parse(DisplayTextBox.Text);
            string[] values = buttonClick.SSI_Click(button.Text, DisplayTextBox.Text,
                numberValue, AngularUnitButton.Text);
            DisplayTextBox.Text = values[0];
            EquationTextBox.Text = values[1];
        }

        // Changes the text on buttons when the function button is clicked
        private void Function_Click(object sender, EventArgs e)
        {
            changeFunction.ChangeButton(SquareAndCubeButton, SquareRootAndInverseButton, XToTheYAndYRootButton,
                TenExponentialButton, SineButton, CosineButton, TangentButton, LogarithmicButton,
                ExponentButton, DegAndDmsButton);
        }

        // Switch from degrees to radians
        private void AngularUnit_Click(object sender, EventArgs e)
        {
            if (AngularUnitButton.Text == "DEG")
            {
                AngularUnitButton.Text = "RAD";
            }
            else
            {
                AngularUnitButton.Text = "DEG";
            }
        }



        // 3.1415
        private void PiButton_Click(object sender, EventArgs e)
        {
            DisplayTextBox.Text = "3.14159265359";
        }

        // User clicks mem clear, mem recall, add mem value, subtract mem value, mem store, mem view
        // Memory show calls MemoryShow Form
        private void Memory_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            if (button.Text == "MC")
            {
                memoryValues.Clear();

                // Disable these buttons since memory has been cleared
                MemoryViewButton.Enabled = false;
                MemoryRecallButton.Enabled = false;
                MemoryClearButton.Enabled = false;
            }
            if (button.Text == "MR")
            {
                // Brings MOST RECENT memory value into DisplayTextBox
                DisplayTextBox.Text = memoryValues[memoryValues.Count - 1].ToString();
            }
            if (button.Text == "M+")
            {
                if (memoryValues.Count == 0)
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
            if (button.Text == "M-")
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
            if (button.Text == "MS")
            {
                memoryValues.Add(double.Parse(DisplayTextBox.Text));
                MemoryViewButton.Enabled = true;
                MemoryRecallButton.Enabled = true;
                MemoryClearButton.Enabled = true;
                isMemoryStoredPerformed = true;
            }
            if (button.Text == "M ▼")
            {
                MemoryShow memoryShow = new MemoryShow(memoryValues, this, DisplayTextBox);
                memoryShow.Show();
            }
        } // End memory click

        private void Parenthesis_Click(object sender, EventArgs e)
        {
            StringBuilder pStringBuilder = new StringBuilder();
            pStringBuilder.Append(EquationTextBox.Text);
            pStringBuilder.Insert(0, "(");
            EquationTextBox.Text = pStringBuilder.ToString();

        }
    }
}
