using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3.Models
{
    public class Theme
    {
        public string ThemeName { get; set; }

        public bool Selected { get; set; }

        public Theme(string themeName, bool selected)
        {
            ThemeName = themeName;
            Selected = selected;
        }
    }
}
