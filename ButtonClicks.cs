/* Class for button clicks
 * 
 * Number_Click() --> user clicks 0 - 9, decimal buttons
 * PosNeg_Click() --> user clicks +/- button
 * Equals_Click() --> user clicks = button
 * SSI_Click() --> used for functions that don't require two numbers to calculate (i.e. x^2)
 * Percentage_Click --> user clicks % button
 */

using System;
using System.Text;

namespace MyCalculator
{
    class ButtonClicks
    {
        public string Number_Click(string numberAsString, string buttonText, 
            bool isOperationPerformed, bool isEqualsPerformed, bool isMemoryStoredPerformed)
        {
            // User wants to backspace 
            if(buttonText == "DEL")
            {
                if(numberAsString == "Invalid input")
                {
                    numberAsString = "";
                }
                if (!isEqualsPerformed) // Don't allow backspacing after equation is complete
                {
                    if (numberAsString != "")
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append(numberAsString);
                        sb.Length--;
                        numberAsString = sb.ToString();
                    }
                }
            }

            // User wants any other number 0-9, decimal
            else
            {
                // Erase leading zero and clear text box after operation button is clicked
                if ((numberAsString == "0") || (isOperationPerformed) || (isMemoryStoredPerformed)
                    || (isEqualsPerformed) || numberAsString == "Invalid input")
                {
                    numberAsString = "";
                }

                // Prohibit the entering of more than one decimal
                if (buttonText == ".")
                {
                    if (!numberAsString.Contains("."))
                    {
                        numberAsString += buttonText;
                    }
                }
                else
                {
                    numberAsString += buttonText;
                }
            }         
            return numberAsString;
        }

        public string PosNeg_Click(string numberAsString)
        {
            // Instantiate StringBuilder to add or remove negative sign from number
            StringBuilder sb = new StringBuilder();

            // Compare number to determine pos/neg
            double numberAsDouble = Convert.ToDouble(numberAsString);
            int result = numberAsDouble.CompareTo(0);

            // StringBuilder has value of number...
            sb.Append(numberAsString);

            // ...if positive, make negative
            if (result == 1)
            {
                sb.Insert(0, "-");
            }

            // ...if negative, make positive
            else
            {
                sb.Remove(0, 1);
            }
            return sb.ToString();
        }

        public string Equals_Click(string operation, string numberAsString, double value)
        {
            // Calculate result based on which operation was clicked
            switch (operation)
            {
                case "+":
                    numberAsString = (value + double.Parse(numberAsString)).ToString();
                    break;
                case "-":
                    numberAsString = (value - double.Parse(numberAsString)).ToString();
                    break;
                case "*":
                    numberAsString = (value * double.Parse(numberAsString)).ToString();
                    break;
                case "/":
                    numberAsString = (value / double.Parse(numberAsString)).ToString();
                    break;
                case "x\x207F": // x ^ n
                    numberAsString = (Math.Pow(value, double.Parse(numberAsString)).ToString());
                    break;
                case "\x207F√x": // n sqrt x
                    numberAsString = (Math.Pow(value, 1.0 / double.Parse(numberAsString)).ToString());
                    break;
                case "Exp":
                    double exp = 1;
                    for(int i = 0; i < double.Parse(numberAsString); i++)
                    {
                        exp *= 10;
                    }
                    numberAsString = (value * exp).ToString();
                    break;
                case "Mod":
                    numberAsString = (value % double.Parse(numberAsString)).ToString();
                    break;
                default:
                    break;
            }
            return numberAsString;
        }

        public string[] SSI_Click(string buttonText, string numberAsString, double numberValue, string angularUnit)
        {
            string[] values = new string[2];
            string equationTextBoxText = "";
            switch (buttonText)
            {
                case "√":
                    numberAsString = (Math.Sqrt(numberValue)).ToString();
                    equationTextBoxText = "√(" + numberValue + ")"; 
                    break;
                case "x²":
                    numberAsString = (numberValue * numberValue).ToString();
                    equationTextBoxText = numberValue + "²";
                    break;
                case "x³":
                    numberAsString = (numberValue * numberValue * numberValue).ToString();
                    equationTextBoxText = numberValue + "³";
                    break;
                case "1/x":
                    numberAsString = (1 / numberValue).ToString();
                    equationTextBoxText = "1/" + numberValue;
                    break;
                case "10\x207F": // 10 ^ x
                    numberAsString = Math.Pow(10, numberValue).ToString();
                    equationTextBoxText = "10^" + numberValue.ToString();
                    break;
                case "e\x207F": // e ^ x
                    numberAsString = Math.Pow(2.71828, numberValue).ToString();
                    equationTextBoxText = "e^" + numberValue.ToString();
                    break;
                case "log":
                    numberAsString = Math.Log10(numberValue).ToString();
                    equationTextBoxText = "log(" + numberValue + ")";
                    break;
                case "ln":
                    numberAsString = Math.Log(numberValue).ToString();
                    equationTextBoxText = "ln(" + numberValue + ")";
                    break;

                // Sin cos tan uses Angular Unit button text to determine operation
                case "sin":

                    // Degrees
                    if(angularUnit == "DEG")
                    {
                        numberAsString = (Math.Sin(numberValue / 180 * Math.PI)).ToString();
                    }

                    // Radians
                    else
                    {
                        numberAsString = Math.Sin(numberValue).ToString();
                    }
                    equationTextBoxText = "sin(" + numberValue.ToString() + ")";
                    break;
                case "sin\x207B\x00B9": // sin ^ -1
                    if (angularUnit == "DEG")
                    {
                        numberAsString = (Math.Asin(numberValue / 180 * Math.PI)).ToString();
                    }
                    else
                    {
                        numberAsString = Math.Asin(numberValue).ToString();
                    }
                    equationTextBoxText = "sin\x207B\x00B9(" + numberValue.ToString() + ")";
                    break;
                case "cos":
                    if(angularUnit == "DEG")
                    {
                        numberAsString = (Math.Cos(numberValue / 180 * Math.PI)).ToString();
                    }
                    else
                    {
                        numberAsString = Math.Cos(numberValue).ToString();
                    }
                    equationTextBoxText = "cos(" + numberValue.ToString() + ")";
                    break;
                case "cos\x207B\x00B9": // sin ^ -1
                    if(angularUnit == "DEG")
                    {
                        numberAsString = (Math.Acos(numberValue / 180 * Math.PI)).ToString();

                    }
                    else
                    {
                        numberAsString = Math.Acos(numberValue).ToString();
                    }
                    equationTextBoxText = "cos\x207B\x00B9(" + numberValue.ToString() + ")";
                    break;
                case "tan":
                    if(angularUnit == "DEG")
                    {
                        numberAsString = (Math.Tan(numberValue / 180 * Math.PI)).ToString();
                    }
                    else
                    {
                        numberAsString = Math.Tan(numberValue).ToString();
                    }
                    equationTextBoxText = "tan(" + numberValue.ToString() + ")";
                    break;
                case "tan\x207B\x00B9": // sin ^ -1
                    if(angularUnit == "DEG")
                    {
                        numberAsString = (Math.Atan(numberValue / 180 * Math.PI)).ToString();
                    }
                    else
                    {
                        numberAsString = Math.Atan(numberValue).ToString();
                    }
                    equationTextBoxText = "tan\x207B\x00B9(" + numberValue.ToString() + ")";
                    break;
                case "n!":
                    if(numberValue > 0 && numberValue % 1 == 0)
                    {
                        double result = 1;
                        string holder = numberValue.ToString();
                        while (numberValue != 0)
                        {
                            result *= numberValue;
                            numberValue--;
                        }
                        numberAsString = result.ToString();
                        equationTextBoxText = holder + "!";                       
                    }
                    else
                    {
                        numberAsString = "Invalid input";
                    }
                    break;

                case "dms":
                    double remainder = numberValue % 1;
                    double degrees = numberValue - remainder;
                    double minutesR = remainder * 60;
                    double remainder2 = minutesR % 1;
                    double minutes = minutesR - remainder2;
                    double secondsR = remainder2 * 60;
                    double remainder3 = secondsR % 1;
                    double seconds = secondsR - remainder3;
                    numberAsString = (degrees.ToString()) + "." + (minutes.ToString()) + (seconds.ToString());
                    equationTextBoxText = (numberValue.ToString() + " deg -> dms");
                    break;

                case "ABS":
                    numberAsString = (Math.Abs(numberValue)).ToString();
                    equationTextBoxText = "|" + numberValue.ToString() + "|";
                    break;
            }
            values[0] = numberAsString;
            values[1] = equationTextBoxText;
            return values;
        } 

        public string Percentage_Click(string operation, string numberAsString, double numberValue)
        {
            if (operation == "+")
            {
                double percentValue = Convert.ToDouble(numberAsString);
                numberAsString = (numberValue + (numberValue * (percentValue / 100))).ToString();

            }
            if (operation == "-")
            {
                double percentValue = Convert.ToDouble(numberAsString);
                numberAsString = (numberValue - (numberValue * (percentValue / 100))).ToString();

            }
            if (operation == "*")
            {
                double percentValue = Convert.ToDouble(numberAsString);
                numberAsString = (numberValue * (numberValue * (percentValue / 100))).ToString();

            }
            if (operation == "/")
            {
                double percentValue = Convert.ToDouble(numberAsString);
                numberAsString = (numberValue / (numberValue * (percentValue / 100))).ToString();

            }
            return numberAsString;
        }
    }
}
