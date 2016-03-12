using System;
using System.Collections.Generic;
using System.Configuration;

namespace Shackal.Gui
{
    internal class Configuration
    {
        /// <summary>
        /// Set of known image file extensions.
        /// </summary>
        public static HashSet<string> ImageExtensions
        {
            get
            {
                var extensionsString = ConfigurationManager.AppSettings[nameof(ImageExtensions)] ?? ".png;.jpg";
                return new HashSet<string>(extensionsString.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries), StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}
