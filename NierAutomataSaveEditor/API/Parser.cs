using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NierAutomataSaveEditor.API.Objects;

namespace NierAutomataSaveEditor.API
{
    public static class Parser // Fake name the SaveFile class is responsible for the parsing
    {
        #region Public Functions
        public static bool CheckValid(string file)
        {
            if (!File.Exists(file))
                return false;

            FileInfo fi = new FileInfo(file);

            return (fi.Length == 235980);
        }

        public static SaveFile Parse(string file)
        {
            return new SaveFile(file);
        }
        #endregion
    }
}
