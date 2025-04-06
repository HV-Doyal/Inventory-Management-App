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
        public static readonly string usersTableName = "users";
        public static readonly string itemsTableName = "items";
        public static readonly string salesTableName = "sales";
        public static readonly string usersDatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{usersTableName}.db3");
        public static readonly string itemsDatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{itemsTableName}.db3");
        public static readonly string salesDatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), $"{salesTableName}.db3");
    }
}
