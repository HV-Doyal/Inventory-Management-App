using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UndergradProject
{
   public static class Constants
   {
        public static readonly Color darkGrayColor = Color.FromArgb("#1E1D1D");
        public static readonly Color backgroundColor = Color.FromArgb("#121212");
        public static readonly Color whiteColor = Color.FromArgb("#FFFFFF");
        public static readonly string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "users.db3");
    }
}
