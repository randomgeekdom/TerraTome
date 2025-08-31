using System;
using System.Collections.Generic;
using System.Text;

namespace TerraTome.Events
{
    public class TabCloseEventArgs : EventArgs
    {
        public bool IsSaving { get; set; }
    }
}
