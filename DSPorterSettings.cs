using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSRPorter
{
    public static class DSPorterSettings
    {
        public static bool Is_SOTE = false;

        public static bool CompileLua = true;
        public static bool SlimeCeilingFix = true;
        public static bool MiscCollisionFixes = true;
        public static bool RenderGroupImprovements = true;
        //public static bool DrawGroup_ChooseLeastShiny = true;

        /// <summary>
        /// DSR has different FFX for empty estus vs full estus.
        /// </summary>
        public static bool EmptyEstusFFX = true;

        public static bool m12_01_AddExtraDSRNavmesh = true;
    }
}
