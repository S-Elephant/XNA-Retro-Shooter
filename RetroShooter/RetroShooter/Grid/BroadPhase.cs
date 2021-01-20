using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNALib;
using System;

namespace RetroShooter
{
    public class BroadPhase
    {
        public static BroadPhase Instance;

        /// <summary>
        /// Do not assign to from another class. It's not implemented as a member because of the cpu usage increase that it causes.
        /// </summary>
        public int GridSize;

        public const int GRID_CNT = 17;
        public Block[,] Blocks = new Block[GRID_CNT, GRID_CNT];
        
        // Offset
        public readonly Point Location = new Point(-512, -512);
        readonly Point LocationIdx = new Point(-4, -4);

        public BroadPhase(int gridSize)
        {
            GridSize = gridSize;
        }

        public void Init()
        {
            for (int y = 0; y < GRID_CNT; y++)
            {
                for (int x = 0; x < GRID_CNT; x++)
                {
                    Blocks[x, y] = new Block(new Point(x, y));

                }
            }
        }

        /* // outcommented because its not used.
        public Point LocationToGridIdx(Vector2 location)
        {
            return new Point(((int)location.X / GridSize) - LocationIdx.X, ((int)location.Y / GridSize) - LocationIdx.Y);
        }*/

        public void AddEntity(IEntity entity, Rectangle aabb)
        {
            Point start, end;
            start = new Point(((int)aabb.X / GridSize)-LocationIdx.X, ((int)aabb.Y / GridSize)-LocationIdx.Y);
            end = new Point((aabb.Right / GridSize)-LocationIdx.X, (aabb.Bottom / GridSize)-LocationIdx.Y);

            for (int y = start.Y; y <= end.Y; y++)
            {
                for (int x = start.X; x <= end.X; x++)
                    Blocks[x, y].Entities.Add(entity);
            }
        }

        public void ClearEntities()
        {
            for (int y = 0; y < GRID_CNT; y++)
            {
                for (int x = 0; x < GRID_CNT; x++)
                    Blocks[x, y].Entities.Clear();
            }
        }

        public void DebugDraw()
        {
            for (int y = 0; y < GRID_CNT; y++)
            {
                for (int x = 0; x < GRID_CNT; x++)
                    Blocks[x, y].DebugDraw();
            }
        }
    }
}
