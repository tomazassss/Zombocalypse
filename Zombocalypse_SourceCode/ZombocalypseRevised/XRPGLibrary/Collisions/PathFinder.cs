using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Collections;

namespace XRPGLibrary.Collisions
{
    public class PathFinder
    {
        #region Field Region

        private PathNode[,] grid;
        private Point playerPosition;
        private Point enemyPosition;

        private Point previousLastPosition;
        private Point lastPosition;

        #endregion

        #region Property Region

        public Point PlayerPosition
        {
            get { return playerPosition; }
            set { playerPosition = value; }
        }

        public Point EnemyPosition
        {
            get { return enemyPosition; }
            set { enemyPosition = value; }
        }

        public Point PreviousLastPosition
        {
            get { return previousLastPosition; }
            set { previousLastPosition = value; }
        }

        public Point LastPosition
        {
            get { return lastPosition; }
            set { lastPosition = value; }
        }

        #endregion

        #region Constructor Region

        public PathFinder(PathNode[,] grid)
        {
            this.grid = grid;
        }

        #endregion

        #region Method Region

        public void FindPath()
        {
            previousLastPosition.X = -1;
            previousLastPosition.Y = -1;
            MySolver<PathNode, Object> aStar = new MySolver<PathNode, Object>(grid);
            LinkedList<PathNode> path = aStar.Search(new System.Drawing.Point(playerPosition.X, playerPosition.Y), new System.Drawing.Point(EnemyPosition.X, EnemyPosition.Y), null);
            PathNode lastPathNode;
            PathNode previousLastPathNode;
            if (path != null)
            {
                if (path.Last.List.Count > 2)
                {
                    previousLastPathNode = path.Last.Previous.Previous.Value;
                    previousLastPosition.X = previousLastPathNode.X;
                    previousLastPosition.Y = previousLastPathNode.Y;
                }
                if (path.Last.List.Count > 1)
                    lastPathNode = path.Last.Previous.Value;
                else
                    lastPathNode = path.Last.Value;
                lastPosition.X = lastPathNode.X;
                lastPosition.Y = lastPathNode.Y;
            }        
        }


        #endregion

        public class MySolver<TPathNode, TUserContext> : SettlersEngine.SpatialAStar<TPathNode, TUserContext> where TPathNode : SettlersEngine.IPathNode<TUserContext>
        {
            protected override Double Heuristic(PathNode inStart, PathNode inEnd)
            {
                return Math.Abs(inStart.X - inEnd.X) + Math.Abs(inStart.Y - inEnd.Y);
            }

            protected override Double NeighborDistance(PathNode inStart, PathNode inEnd)
            {
                return Heuristic(inStart, inEnd);
            }

            public MySolver(TPathNode[,] inGrid)
                : base(inGrid)
            {
            }
        }

    }
}
