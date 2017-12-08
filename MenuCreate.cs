/* Class to create menu for the forms
 * 
 * AddMenu() --> Adds the menu to the form
 */

using System.Windows.Forms;
using System.Collections.Generic;

namespace MyCalculator
{
    class MenuCreate
    {
        // Adds a menu to the form
        public List<MenuItem> AddMenu(MainMenu mainMenu)
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            MenuItem menuItem1 = new MenuItem();
            menuItems.Add(menuItem1);
            MenuItem menuItem2 = new MenuItem();
            menuItems.Add(menuItem2);
            menuItem1.Text = "Standard";
            menuItem2.Text = "Scientific";
            mainMenu.MenuItems.Add(menuItem1);
            mainMenu.MenuItems.Add(menuItem2);
            return menuItems;
        }
    }
}
