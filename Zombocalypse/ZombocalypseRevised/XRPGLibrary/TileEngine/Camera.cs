using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

using XRPGLibrary.TileEngine.TileMaps;
using XRPGLibrary.SpriteClasses;


namespace XRPGLibrary.TileEngine
{
    public enum CameraMode { Free, Follow }

    public class Camera
    {
        #region Field Region

        private Vector2 position;
        private float speed;
        private float zoom;
        Rectangle viewportRectangle;
        CameraMode mode;

        #endregion

        #region Property Region

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public float Speed
        {
            get { return speed; }
            set
            {
                speed = (float)MathHelper.Clamp(value, 1f, 16f);
            }
        }

        public float Zoom
        {
            get { return zoom; }
        }

        public CameraMode CameraMode
        {
            get { return mode; }
        }

        public Rectangle ViewportRectangle
        {
            get
            {
                return new Rectangle(
                    viewportRectangle.X,
                    viewportRectangle.Y,
                    viewportRectangle.Width,
                    viewportRectangle.Height);
            }
        }

        public Matrix Transformation
        {
            get
            {
                return Matrix.CreateScale(zoom) *
                    Matrix.CreateTranslation(new Vector3(-Position, 0f));
            }
        }

        #endregion

        #region Constructor Region

        public Camera(Rectangle viewportRectangle)
        {
            //TODO: hardcoded speed and zoom
            Speed = 40f;
            zoom = 1f;
            this.viewportRectangle = viewportRectangle;
            this.mode = CameraMode.Follow;
        }

        public Camera(Rectangle viewportRectangle, Vector2 position)
        {
            //TODO: hardcoded speed and zoom
            Speed = 16f;
            zoom = 1f;
            this.viewportRectangle = viewportRectangle;
            this.position = position;
            this.mode = CameraMode.Follow;
        }

        #endregion

        #region Method Region

        public void Update(GameTime gameTime)
        {
            if (mode == CameraMode.Follow)
                return;

            Vector2 motion = Vector2.Zero;

            if (InputHandler.KeyDown(Keys.Left))
            {
                motion.X -= speed;
            }
            else if (InputHandler.KeyDown(Keys.Right))
            {
                motion.X += speed;
            }

            if (InputHandler.KeyDown(Keys.Up))
            {
                motion.Y -= speed;
            }
            else if (InputHandler.KeyDown(Keys.Down))
            {
                motion.Y += speed;
            }

            if (motion != Vector2.Zero)
            {
                motion.Normalize();
            }

            position += motion * speed;

            LockCamera();
        }

        public void LockCamera()
        {
            position.X = MathHelper.Clamp(position.X,
                0,
                ATileMap.WidthInPixels - viewportRectangle.Width);

            position.Y = MathHelper.Clamp(position.Y,
                0,
                ATileMap.HeightInPixels - viewportRectangle.Height);
        }

        public void LockToSprite(AnimatedSprite sprite)
        {
            position.X = sprite.Position.X + sprite.Width / 2 - (viewportRectangle.Width / 2);
            position.Y = sprite.Position.Y + sprite.Height / 2 - (viewportRectangle.Height / 2);
            LockCamera();
        }

        public void ToggleCameraMode()
        {
            if (mode == CameraMode.Follow)
                mode = CameraMode.Free;
            else if (mode == CameraMode.Free)
                mode = CameraMode.Follow;
        }

        #endregion
    }
}
