using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XRPGLibrary.Collisions
{
    public class PathNode : SettlersEngine.IPathNode<Object>
    {
        #region Field Region

        private Int32 x;
        private Int32 y;
        private Boolean isWall;

        #endregion

        #region Property Region

        public Int32 X
        {
            get { return x; }
            set { x = value; }
        }

        public Int32 Y
        {
            get { return y; }
            set { y = value; }
        }

        public Boolean IsWall
        {
            get { return isWall; }
            set { isWall = value; }
        }

        #endregion

        #region Method Region

        public bool IsWalkable(Object unused)
        {
            return !IsWall;
        }

        #endregion
    }
}
