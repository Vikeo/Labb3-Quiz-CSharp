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
        private string _themeName;
        //public event Action SelectedChanged;

        public string ThemeName
        {
            get { return _themeName; }
            set { _themeName = value; }
        }

        private bool _selected;

        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                //OnSelectedChanged();
            }
        }

        //private void OnSelectedChanged()
        //{
        //    SelectedChanged?.Invoke();
        //}

        public Theme(string themeName, bool selected)
        {
            _themeName = themeName;
            _selected = selected;
        }
    }
}
