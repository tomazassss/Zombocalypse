using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using GDIBitmap = System.Drawing.Bitmap;
using GDIColor = System.Drawing.Color;
using GDIImage = System.Drawing.Image;
using GDIGraphics = System.Drawing.Graphics;
using GDIGraphicsUnit = System.Drawing.GraphicsUnit;
using GDIRectangle = System.Drawing.Rectangle;

using XRPGLibrary.WorldClasses;
using XRPGLibrary.TileEngine;
using XRPGLibrary.TileEngine.TileMaps;

namespace MapEditor.Forms
{
    public partial class FormMain : Form
    {
        #region Field Region

        private SpriteBatch spriteBatch;

        private MapData mData;

        private List<Tileset> tileSets = new List<Tileset>();
        private List<TilesetData> tileSetData = new List<TilesetData>();
        private List<MapLayer> layers = new List<MapLayer>();
        private List<GDIImage> tileSetImages = new List<GDIImage>();

        private TileMapIso map;

        private Camera camera;
        private Engine engine;

        private Point mouse = new Point();

        private bool isMouseDown = false;
        private bool trackMouse = false;

        private Texture2D grid;

        private int frameCount = 0;
        private int brushWidth = 1;
        private Color gridColor = Color.White;

        private Texture2D shadow;
        private Vector2 shadowPosition = Vector2.Zero;

        #endregion

        #region Property Region

        public GraphicsDevice GraphicsDevice
        {
            get { return mapDisplay.GraphicsDevice; }
        }

        #endregion

        #region Constructor Region

        public FormMain()
        {
            InitializeComponent();

            this.Load += new EventHandler(FormMain_Load);
            this.FormClosing += new FormClosingEventHandler(FormMain_FormClosing);

            tilesetToolStripMenuItem.Enabled = false;
            mapLayerToolStripMenuItem.Enabled = false;

            newMapToolStripMenuItem.Click += new EventHandler(newMapToolStripMenuItem_Click);
            newTilesetToolStripMenuItem.Click += new EventHandler(newTilesetToolStripMenuItem_Click);
            newLayerToolStripMenuItem.Click += new EventHandler(newLayerToolStripMenuItem_Click);

            saveMapToolStripMenuItem.Click += new EventHandler(saveMapToolStripMenuItem_Click);

            openMapToolStripMenuItem.Click += new EventHandler(openMapToolStripMenuItem_Click);

            mapDisplay.OnInitialize += new EventHandler(mapDisplay_OnInitialize);
            mapDisplay.OnDraw += new EventHandler(mapDisplay_OnDraw);
        }

        #endregion

        #region Form Event Handler Region

        void FormMain_Load(object sender, EventArgs e)
        {
            lbTileset.SelectedIndexChanged += new EventHandler(lbTileset_SelectedIndexChanged);
            nudCurrentTile.ValueChanged += new EventHandler(nudCurrentTile_ValueChanged);

            Rectangle viewPort = new Rectangle(0, 0, mapDisplay.Width, mapDisplay.Height);
            camera = new Camera(viewPort);

            engine = new Engine(64, 32);

            controlTimer.Tick += new EventHandler(controlTimer_Tick);
            controlTimer.Enabled = true;
            controlTimer.Interval = 17;

            tbMapLocation.TextAlign = HorizontalAlignment.Center;
            pbTilesetPreview.MouseDown += new MouseEventHandler(pbTilesetPreview_MouseDown);

            mapDisplay.SizeChanged += new EventHandler(mapDisplay_SizeChanged);
        }

        void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
        }

        void mapDisplay_SizeChanged(object sender, EventArgs e)
        {
            Rectangle viewPort = new Rectangle(0, 0, mapDisplay.Width, mapDisplay.Height);
            Vector2 cameraPosition = camera.Position;

            camera = new Camera(viewPort, cameraPosition);
            camera.LockCamera();

            mapDisplay.Invalidate();
        }

        void controlTimer_Tick(object sender, EventArgs e)
        {
            frameCount = ++frameCount % 6;
            mapDisplay.Invalidate();
            Logic();
        }

        #endregion

        #region Tile Tab Event Handler Region

        void pbTilesetPreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (lbTileset.Items.Count == 0)
                return;

            if (e.Button != System.Windows.Forms.MouseButtons.Left)
                return;

            int index = lbTileset.SelectedIndex;

            double div = tileSetImages[index].Width / pbTilesetPreview.Width;
            int height = Convert.ToInt32(tileSetImages[index].Height / div);

            float xScale = (float)tileSetImages[index].Width /
                pbTilesetPreview.Width;
            
            float yScale = (float)tileSetImages[index].Height /
                height;

            //float yScale = (float)tileSets[index].TileHeight / xScale;

            Point previewPoint = new Point(e.X, e.Y);

            Point tilesetPoint = new Point((int)(previewPoint.X * xScale), (int)(previewPoint.Y * yScale));

            Point tile = new Point(
                tilesetPoint.X / tileSets[index].TileWidth,
                tilesetPoint.Y / tileSets[index].TileHeight);
            if (tile.Y < tileSets[index].TilesHigh)
            {
                nudCurrentTile.Value = tile.Y * tileSets[index].TilesWide + tile.X;
            }
        }

        void lbTileset_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbTileset.SelectedItem != null)
            {
                nudCurrentTile.Value = 0;
                nudCurrentTile.Maximum = tileSets[lbTileset.SelectedIndex].SourceRectangles.Length - 1;
                FillPreviews();
            }
        }

        void nudCurrentTile_ValueChanged(object sender, EventArgs e)
        {
            if (lbTileset.SelectedItem != null)
            {
                FillPreviews();
            }
        }

        private void FillPreviews()
        {
            int selected = lbTileset.SelectedIndex;
            int tile = (int)nudCurrentTile.Value;
            
            GDIImage preview = (GDIImage)new GDIBitmap(pbTilePreview.Width, pbTilePreview.Height);
            GDIImage setPreview = (GDIImage)new GDIBitmap(pbTilesetPreview.Width, pbTilesetPreview.Height);

            double divv = 1;
            if (tileSets[selected].TileWidth > tileSets[selected].TileHeight)
            {
                if (tileSets[selected].TileWidth > preview.Width)
                {
                    divv = (double)tileSets[selected].TileWidth / preview.Width;
                }
            }
            else
            {
                if (tileSets[selected].TileHeight > preview.Height)
                {
                    divv = (double)tileSets[selected].TileHeight / preview.Height;
                }
            }

            GDIRectangle dest = new GDIRectangle(0, 0, (int)(tileSets[selected].TileWidth / divv), (int)(tileSets[selected].TileHeight / divv));
            
            double div = tileSetImages[selected].Width / pbTilesetPreview.Width;

            GDIRectangle dest1 = new GDIRectangle(0, 0, pbTilesetPreview.Width, (int)(tileSetImages[selected].Height / div));
            GDIRectangle dest2 = new GDIRectangle(0, 0, tileSetImages[selected].Width, tileSetImages[selected].Height);

            GDIRectangle source = new GDIRectangle(
                tileSets[selected].SourceRectangles[tile].X,
                tileSets[selected].SourceRectangles[tile].Y,
                tileSets[selected].SourceRectangles[tile].Width,
                tileSets[selected].SourceRectangles[tile].Height);

            GDIGraphics g = GDIGraphics.FromImage(preview);
            g.DrawImage(tileSetImages[selected], dest, source, GDIGraphicsUnit.Pixel);

            GDIGraphics g1 = GDIGraphics.FromImage(setPreview);
            g1.DrawImage(tileSetImages[selected], dest1, dest2, GDIGraphicsUnit.Pixel);
            pbTilePreview.Image = preview;
            pbTilesetPreview.Image = setPreview;

        }

        #endregion

        #region New Menu Item Event Handler Region

        void newMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormNewMap frmNewMap = new FormNewMap())
            {
                frmNewMap.ShowDialog();

                if (frmNewMap.OKPressed)
                {
                    mData = frmNewMap.MapData;
                    tilesetToolStripMenuItem.Enabled = true;

                    if (frmNewMap.CbCheked)
                        LoadDefault();
                    
                }
            }
        }

        void newTilesetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormNewTileset frmNewTileset = new FormNewTileset())
            {
                frmNewTileset.ShowDialog();

                if (frmNewTileset.OKPressed)
                {
                    TilesetData data = frmNewTileset.TilesetData;

                    try
                    {
                        GDIImage image = (GDIImage)GDIBitmap.FromFile(data.TilesetImageName);
                        tileSetImages.Add(image);

                        Stream stream = new FileStream(data.TilesetImageName, FileMode.Open, FileAccess.Read);

                        Texture2D texture = Texture2D.FromStream(GraphicsDevice, stream);

                        Tileset tileset = new Tileset(
                                texture,
                                data.TilesWide,
                                data.TilesHigh,
                                data.TileWidthInPixels,
                                data.TileHeightInPixels);
                        tileSets.Add(tileset);
                        tileSetData.Add(data);

                        if (map != null)
                            map.AddTileset(tileset);

                        stream.Close();
                        stream.Dispose();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error reading file.\n" + ex.Message, "Error reading image");
                        return;
                    }

                    lbTileset.Items.Add(data.TilesetName);

                    if (lbTileset.SelectedItem == null)
                        lbTileset.SelectedIndex = 0;

                    mapLayerToolStripMenuItem.Enabled = true;
                }
            }
        }

        void newLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormNewLayer frmNewLayer = new FormNewLayer(mData.MapWidth, mData.MapHeight))
            {
                frmNewLayer.ShowDialog();

                if (frmNewLayer.OKPressed)
                {
                    MapLayerData data = frmNewLayer.MapLayerData;

                    if (clbLayers.Items.Contains(data.MapLayerName))
                    {
                        MessageBox.Show("Layer with name " + data.MapLayerName + " exists.", "Existing layer");
                        return;
                    }

                    MapLayer layer = MapLayer.FromMapLayerData(data);
                    clbLayers.Items.Add(data.MapLayerName, true);
                    clbLayers.SelectedIndex = clbLayers.Items.Count - 1;

                    layers.Add(layer);

                    if (map == null)
                    {
                        map = new TileMapIso(tileSets[0], (MapLayer)layers[0]);

                        for (int i = 1; i < tileSets.Count; i++)
                            map.AddTileset(tileSets[1]);

                        for (int i = 1; i < layers.Count; i++)
                            map.AddLayer(layers[1]);
                    }
                }
            }
        }

        #endregion

        #region Delete Menu Item Event Handler Region

        void deleteTilesetToolStripMenuItemripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormDeleteTileSet frmDeleteTileSet = new FormDeleteTileSet(tileSetData))
            {
                frmDeleteTileSet.ShowDialog();

                if (frmDeleteTileSet.OKPressed)
                {
                    tileSets.RemoveAt(frmDeleteTileSet.ItemSelected);
                    tileSetData.RemoveAt(frmDeleteTileSet.ItemSelected);
                    tileSetImages.RemoveAt(frmDeleteTileSet.ItemSelected);
                    lbTileset.Items.RemoveAt(frmDeleteTileSet.ItemSelected);

                    if (lbTileset.SelectedItem == null)
                        lbTileset.SelectedIndex = 0;

                    foreach (MapLayer layer in layers)
                    {
                        for (int x = 0; x < layer.Height; x++)
                        {
                            for (int y = 0; y < layer.Width; y++)
                            {
                                int tileset = layer.GetTileTileset(x, y);
                                if (tileset == frmDeleteTileSet.ItemSelected)
                                {
                                    layer.SetTile(x, y, -1, -1);
                                }
                                if (tileset > frmDeleteTileSet.ItemSelected)
                                {
                                    layer.SetTileTileset(x, y, tileset - 1);
                                }
                            }
                        }
                    }
                }
            }
        }

        void deleteLayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormDeleteLayer frmDeleteLayer = new FormDeleteLayer(clbLayers.Items))
            {
                frmDeleteLayer.ShowDialog();

                if (frmDeleteLayer.OKPressed)
                {
                    layers.RemoveAt(frmDeleteLayer.ItemSelected);
                    clbLayers.Items.RemoveAt(frmDeleteLayer.ItemSelected);

                    if (clbLayers.Items.Count > 0)
                        clbLayers.SelectedIndex = 0;
                }
            }
        }

        #endregion

        #region Map Display Event Handler Region

        void mapDisplay_OnInitialize(object sender, EventArgs e)
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            shadow = new Texture2D(GraphicsDevice, 20, 20, false, SurfaceFormat.Color);

            Color[] data = new Color[shadow.Width * shadow.Height];
            Color tint = Color.Blue;
            tint.A = 25;

            for (int i = 0; i < shadow.Width * shadow.Height; i++)
                data[i] = tint;

            shadow.SetData<Color>(data);

            mapDisplay.MouseEnter += new EventHandler(mapDisplay_MouseEnter);
            mapDisplay.MouseLeave += new EventHandler(mapDisplay_MouseLeave);
            mapDisplay.MouseMove += new MouseEventHandler(mapDisplay_MouseMove);
            mapDisplay.MouseDown += new MouseEventHandler(mapDisplay_MouseDown);
            mapDisplay.MouseUp += new MouseEventHandler(mapDisplay_MouseUp);

            try
            {
                using (Stream stream = new FileStream(@"Content\gridIso.png", FileMode.Open, FileAccess.Read))
                {
                    grid = Texture2D.FromStream(GraphicsDevice, stream);
                    stream.Close();
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error reading images");
                grid = null;
            }
        }

        void mapDisplay_OnDraw(object sender, EventArgs e)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Render();
        }

        void mapDisplay_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                isMouseDown = false;
        }

        void mapDisplay_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                isMouseDown = true;
        }

        void mapDisplay_MouseMove(object sender, MouseEventArgs e)
        {
            mouse.X = e.X;
            mouse.Y = e.Y;
        }

        void mapDisplay_MouseLeave(object sender, EventArgs e)
        {
            trackMouse = false;
        }

        void mapDisplay_MouseEnter(object sender, EventArgs e)
        {
            trackMouse = true;
        }

        #endregion

        #region Display Rendering and Logic Region

        private void Render()
        {
            for (int i = 0; i < layers.Count; i++)
            {
                spriteBatch.Begin(
                    SpriteSortMode.Deferred,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    camera.Transformation);

                if (clbLayers.GetItemChecked(i))
                    layers[i].Draw(spriteBatch, camera, tileSets);

                Rectangle destination = new Rectangle(
                    (int)shadowPosition.X * Engine.TileWidth,
                    (int)shadowPosition.Y * Engine.TileHeight,
                    brushWidth * Engine.TileWidth,
                    brushWidth * Engine.TileHeight);

                Color tint = Color.White;
                tint.A = 1;

                //spriteBatch.Draw(shadow, destination, tint);
                spriteBatch.End();
            }

            DrawDisplay();
        }

        private void DrawDisplay()
        {
            if (map == null)
                return;

            Rectangle destination = new Rectangle(
                0,
                0,
                Engine.TileWidth,
                Engine.TileHeight);

            //if (displayGridToolStripMenuItem.Checked)
            //{
                int maxX = mapDisplay.Width / Engine.TileWidth + 1;
                int maxY = mapDisplay.Height / Engine.TileHeight + 1;

                spriteBatch.Begin();

                for (int y = 0; y < maxY; y++)
                {
                    destination.Y = y * Engine.TileHeight;

                    for (int x = 0; x < maxX; x++)
                    {
                        destination.X = x * Engine.TileWidth;

                        spriteBatch.Draw(grid, destination, gridColor);
                    }
                }

                spriteBatch.End();
            //}

            spriteBatch.Begin();

            destination.X = mouse.X - Engine.TileWidth / 2;
            destination.Y = mouse.Y - Engine.TileHeight / 2;
            Rectangle destinationCursor = new Rectangle(
                                        destination.X,
                                        destination.Y,
                                        tileSets[lbTileset.SelectedIndex].TileWidth,
                                        tileSets[lbTileset.SelectedIndex].TileHeight);
            if (rbDraw.Checked)
            {
                if (tileSets[lbTileset.SelectedIndex].TilesHigh * tileSets[lbTileset.SelectedIndex].TilesWide > (int)nudCurrentTile.Value)
                {
                    spriteBatch.Draw(
                        tileSets[lbTileset.SelectedIndex].Image,
                        destinationCursor,
                        tileSets[lbTileset.SelectedIndex].SourceRectangles[(int)nudCurrentTile.Value],
                        Color.White);
                }

            }

            Color lowAlpha = Color.White;
            lowAlpha.A = 5;


            spriteBatch.Draw(grid, destination, lowAlpha);

            spriteBatch.End();
        }

        private void Logic()
        {
            int baseOffsetX = -Engine.TileWidth;
            int baseOffsetY = -Engine.TileHeight;
            int tileStepX = Engine.TileWidth;
            int tileStepY = Engine.TileHeight / 2;
            int oddRowOffset = Engine.TileWidth / 2;

            if (layers.Count == 0)
                return;

            Vector2 position = camera.Position;

            if (trackMouse)
            {
                if (frameCount == 0)
                {
                    if (mouse.X < Engine.TileWidth)                
                        position.X -= Engine.TileWidth;

                    if (mouse.X > mapDisplay.Width - Engine.TileWidth)
                        position.X += Engine.TileWidth;

                    if (mouse.Y < Engine.TileHeight)
                        position.Y -= Engine.TileHeight;

                    if (mouse.Y > mapDisplay.Height - Engine.TileHeight)
                        position.Y += Engine.TileHeight;

                    camera.Position = position;
                    camera.LockCamera();
                }
                position.X = mouse.X + camera.Position.X * 2;
                position.Y = mouse.Y + camera.Position.Y * 2;
                Point tile = Engine.VectorToCellIso(position);
                //shadowPosition = new Vector2(tile.X, tile.Y);

                tbMapLocation.Text = "( " + tile.X.ToString() + ", " + tile.Y.ToString() + " )";

                if (isMouseDown)
                {
                    if (rbDraw.Checked)
                        SetTiles(tile, (int)nudCurrentTile.Value, lbTileset.SelectedIndex);

                    if (rbErase.Checked)
                        SetTiles(tile, -1, -1);
                }
            }
        }

        private void SetTiles(Point tile, int tileIndex, int tileset)
        {
            int selected = clbLayers.SelectedIndex;

            if (layers[selected] is MapLayer)
            {
                for (int y = 0; y < brushWidth; y++)
                {
                    if (tile.Y + y >= ((MapLayer)layers[selected]).Height)
                        break;

                    for (int x = 0; x < brushWidth; x++)
                    {
                        if (tile.X + x < ((MapLayer)layers[selected]).Width)
                            ((MapLayer)layers[selected]).SetTile(
                                tile.X + x,
                                tile.Y + y,
                                tileIndex,
                                tileset);
                    }
                }
            }
        }

        #endregion

        #region Save Menu Item Event Handler Region

        void saveMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (map == null)
                return;

            List<MapLayerData> mapLayerData = new List<MapLayerData>();

            for (int i = 0; i < clbLayers.Items.Count; i++)
            {
                if (layers[i] is MapLayer)
                {
                    MapLayerData data = new MapLayerData(
                        clbLayers.Items[i].ToString(),
                        ((MapLayer)layers[i]).Width,
                        ((MapLayer)layers[i]).Height,
                        ((MapLayer)layers[i]).LayerLevel);
                    for (int y = 0; y < ((MapLayer)layers[i]).Height; y++)
                        for (int x = 0; x < ((MapLayer)layers[i]).Width; x++)
                            data.SetTile(
                                x,
                                y,
                                ((MapLayer)layers[i]).GetTile(x, y).TileIndex,
                                ((MapLayer)layers[i]).GetTile(x, y).Tileset);

                    mapLayerData.Add(data);
                }
            }

            mData.setAdditionalData(tileSetData, mapLayerData);

            FolderBrowserDialog fbDialog = new FolderBrowserDialog();

            fbDialog.Description = "Select Folder";
            fbDialog.SelectedPath = Application.StartupPath;

            DialogResult result = fbDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                XnaSerializer.Serialize<MapData>(fbDialog.SelectedPath + "\\" + mData.MapName + ".xml", mData);
            }
        }

        #endregion

        #region Open Menu Event Handler Region
        
        void openMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofDialog = new OpenFileDialog();
            ofDialog.Filter = "Level Files (*.xml)|*.xml";
            ofDialog.CheckFileExists = true;
            ofDialog.CheckPathExists = true;

            DialogResult result = ofDialog.ShowDialog();

            if (result != DialogResult.OK)
                return;

            string path = Path.GetDirectoryName(ofDialog.FileName);

            MapData mapData = null;

            try
            {
                mapData = XnaSerializer.Deserialize<MapData>(ofDialog.FileName);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, "Error reading level");
                return;
            }

            tileSetImages.Clear();
            tileSetData.Clear();
            tileSets.Clear();
            layers.Clear();
            lbTileset.Items.Clear();
            clbLayers.Items.Clear();

            mData = new MapData(mapData.MapName, mapData.MapWidth, mapData.MapHeight);

            string directory = System.IO.Directory.GetCurrentDirectory();
            int pos = directory.IndexOf("ZombocalypseRevised\\") + 20;
            directory = directory.Substring(0, pos).Trim() + "ZombocalypseRevised\\ZombocalypseRevisedContent\\";

            foreach (TilesetData data in mapData.Tilesets)
            {
                Texture2D texture = null;

                tileSetData.Add(data);

                int posLoadLoc = data.TilesetImageName.IndexOf("TileSets\\");
                int lenghtFull = data.TilesetImageName.Length - posLoadLoc;
                string fileDirectoryFull = directory + data.TilesetImageName.Substring(posLoadLoc, lenghtFull).Trim();
                lbTileset.Items.Add(data.TilesetName);

                GDIImage image = (GDIImage)GDIBitmap.FromFile(fileDirectoryFull);
                tileSetImages.Add(image);

                using (Stream stream = new FileStream(fileDirectoryFull, FileMode.Open, FileAccess.Read))
                {
                    texture = Texture2D.FromStream(GraphicsDevice, stream);
                    tileSets.Add(
                        new Tileset(
                            texture,
                            data.TilesWide,
                            data.TilesHigh,
                            data.TileWidthInPixels,
                            data.TileHeightInPixels));
                }
            }

            foreach (MapLayerData data in mapData.Layers)
            {
                clbLayers.Items.Add(data.MapLayerName, true);
                layers.Add(MapLayer.FromMapLayerData(data));
            }

            lbTileset.SelectedIndex = 0;
            clbLayers.SelectedIndex = 0;
            nudCurrentTile.Value = 0;

            map = new TileMapIso(tileSets[0], (MapLayer)layers[0]);

            for (int i = 1; i < tileSets.Count; i++)
                map.AddTileset(tileSets[i]);

            for (int i = 1; i < layers.Count; i++)
                map.AddLayer(layers[i]);

            tilesetToolStripMenuItem.Enabled = true;
            mapLayerToolStripMenuItem.Enabled = true;
        }

        #endregion

        #region Load Default Event Handler Region

        private void LoadDefault()
        {
            LoudDefaultTileSets();
            LoudDefaultLayers();
        }

        private void LoudDefaultTileSets()
        {
            string directory = System.IO.Directory.GetCurrentDirectory();
            int pos = directory.IndexOf("ZombocalypseRevised\\") + 20;
            directory = directory.Substring(0, pos).Trim() + @"ZombocalypseRevised\ZombocalypseRevisedContent\TileSets\";

            TilesetData tilesetData1 = new TilesetData();
            tilesetData1.TilesetName = "Default Tileset 1";
            tilesetData1.TilesetImageName = directory + "TileSet_Grass_64_32.png";
            tilesetData1.TileWidthInPixels = 64;
            tilesetData1.TileHeightInPixels = 32;
            tilesetData1.TilesWide = 16;
            tilesetData1.TilesHigh = 2;

            GDIImage image = (GDIImage)GDIBitmap.FromFile(tilesetData1.TilesetImageName);
            tileSetImages.Add(image);

            Stream stream = new FileStream(tilesetData1.TilesetImageName, FileMode.Open, FileAccess.Read);

            Texture2D texture = Texture2D.FromStream(GraphicsDevice, stream);

            Tileset tileset = new Tileset(texture, tilesetData1.TilesWide, tilesetData1.TilesHigh, tilesetData1.TileWidthInPixels, tilesetData1.TileHeightInPixels);
            tileSets.Add(tileset);
            tileSetData.Add(tilesetData1);

            if (map != null)
                map.AddTileset(tileset);

            stream.Close();
            stream.Dispose();

            lbTileset.Items.Add(tilesetData1.TilesetName);

            TilesetData tilesetData2 = new TilesetData();
            tilesetData2.TilesetName = "Default Tileset 2";
            tilesetData2.TilesetImageName = directory + "TileSet_Objects_64_64.png";
            tilesetData2.TileWidthInPixels = 64;
            tilesetData2.TileHeightInPixels = 64;
            tilesetData2.TilesWide = 16;
            tilesetData2.TilesHigh = 2;

            image = (GDIImage)GDIBitmap.FromFile(tilesetData2.TilesetImageName);
            tileSetImages.Add(image);

            stream = new FileStream(tilesetData2.TilesetImageName, FileMode.Open, FileAccess.Read);

            texture = Texture2D.FromStream(GraphicsDevice, stream);

            tileset = new Tileset(texture, tilesetData2.TilesWide, tilesetData2.TilesHigh, tilesetData2.TileWidthInPixels, tilesetData2.TileHeightInPixels);
            tileSets.Add(tileset);
            tileSetData.Add(tilesetData2);

            if (map != null)
                map.AddTileset(tileset);

            stream.Close();
            stream.Dispose();

            lbTileset.Items.Add(tilesetData2.TilesetName);

            TilesetData tilesetData3 = new TilesetData();
            tilesetData3.TilesetName = "Default Tileset 3";
            tilesetData3.TilesetImageName = directory + "TileSet_Objects2_64_96.png";
            tilesetData3.TileWidthInPixels = 64;
            tilesetData3.TileHeightInPixels = 96;
            tilesetData3.TilesWide = 16;
            tilesetData3.TilesHigh = 1;

            image = (GDIImage)GDIBitmap.FromFile(tilesetData3.TilesetImageName);
            tileSetImages.Add(image);

            stream = new FileStream(tilesetData3.TilesetImageName, FileMode.Open, FileAccess.Read);

            texture = Texture2D.FromStream(GraphicsDevice, stream);

            tileset = new Tileset(texture, tilesetData3.TilesWide, tilesetData3.TilesHigh, tilesetData3.TileWidthInPixels, tilesetData3.TileHeightInPixels);
            tileSets.Add(tileset);
            tileSetData.Add(tilesetData3);

            if (map != null)
                map.AddTileset(tileset);

            stream.Close();
            stream.Dispose();

            lbTileset.Items.Add(tilesetData3.TilesetName);

            TilesetData tilesetData4 = new TilesetData();
            tilesetData4.TilesetName = "Default Tileset 4";
            tilesetData4.TilesetImageName = directory + "TileSet_Platform_64_64.png";
            tilesetData4.TileWidthInPixels = 64;
            tilesetData4.TileHeightInPixels = 64;
            tilesetData4.TilesWide = 16;
            tilesetData4.TilesHigh = 1;

            image = (GDIImage)GDIBitmap.FromFile(tilesetData4.TilesetImageName);
            tileSetImages.Add(image);

            stream = new FileStream(tilesetData4.TilesetImageName, FileMode.Open, FileAccess.Read);

            texture = Texture2D.FromStream(GraphicsDevice, stream);

            tileset = new Tileset(texture, tilesetData4.TilesWide, tilesetData4.TilesHigh, tilesetData4.TileWidthInPixels, tilesetData4.TileHeightInPixels);
            tileSets.Add(tileset);
            tileSetData.Add(tilesetData4);

            if (map != null)
                map.AddTileset(tileset);

            stream.Close();
            stream.Dispose();

            lbTileset.Items.Add(tilesetData4.TilesetName);

            TilesetData tilesetData5 = new TilesetData();
            tilesetData5.TilesetName = "Default Tileset 5";
            tilesetData5.TilesetImageName = directory + "TileSet_Wall_64_96.png";
            tilesetData5.TileWidthInPixels = 64;
            tilesetData5.TileHeightInPixels = 96;
            tilesetData5.TilesWide = 16;
            tilesetData5.TilesHigh = 2;

            image = (GDIImage)GDIBitmap.FromFile(tilesetData5.TilesetImageName);
            tileSetImages.Add(image);

            stream = new FileStream(tilesetData5.TilesetImageName, FileMode.Open, FileAccess.Read);

            texture = Texture2D.FromStream(GraphicsDevice, stream);

            tileset = new Tileset(texture, tilesetData5.TilesWide, tilesetData5.TilesHigh, tilesetData5.TileWidthInPixels, tilesetData5.TileHeightInPixels);
            tileSets.Add(tileset);
            tileSetData.Add(tilesetData5);

            if (map != null)
                map.AddTileset(tileset);

            stream.Close();
            stream.Dispose();

            lbTileset.Items.Add(tilesetData5.TilesetName);

            TilesetData tilesetData6 = new TilesetData();
            tilesetData6.TilesetName = "Default Tileset 6";
            tilesetData6.TilesetImageName = directory + "TileSet_WallWater_64_64.png";
            tilesetData6.TileWidthInPixels = 64;
            tilesetData6.TileHeightInPixels = 64;
            tilesetData6.TilesWide = 16;
            tilesetData6.TilesHigh = 2;

            image = (GDIImage)GDIBitmap.FromFile(tilesetData6.TilesetImageName);
            tileSetImages.Add(image);

            stream = new FileStream(tilesetData6.TilesetImageName, FileMode.Open, FileAccess.Read);

            texture = Texture2D.FromStream(GraphicsDevice, stream);

            tileset = new Tileset(texture, tilesetData6.TilesWide, tilesetData6.TilesHigh, tilesetData6.TileWidthInPixels, tilesetData6.TileHeightInPixels);
            tileSets.Add(tileset);
            tileSetData.Add(tilesetData6);

            if (map != null)
                map.AddTileset(tileset);

            stream.Close();
            stream.Dispose();

            lbTileset.Items.Add(tilesetData6.TilesetName);

            TilesetData tilesetData7 = new TilesetData();
            tilesetData7.TilesetName = "Default Tileset 7";
            tilesetData7.TilesetImageName = directory + "TileSet_Water_64_32.png";
            tilesetData7.TileWidthInPixels = 64;
            tilesetData7.TileHeightInPixels = 32;
            tilesetData7.TilesWide = 16;
            tilesetData7.TilesHigh = 1;

            image = (GDIImage)GDIBitmap.FromFile(tilesetData7.TilesetImageName);
            tileSetImages.Add(image);

            stream = new FileStream(tilesetData7.TilesetImageName, FileMode.Open, FileAccess.Read);

            texture = Texture2D.FromStream(GraphicsDevice, stream);

            tileset = new Tileset(texture, tilesetData7.TilesWide, tilesetData7.TilesHigh, tilesetData7.TileWidthInPixels, tilesetData7.TileHeightInPixels);
            tileSets.Add(tileset);
            tileSetData.Add(tilesetData7);

            if (map != null)
                map.AddTileset(tileset);

            stream.Close();
            stream.Dispose();

            lbTileset.Items.Add(tilesetData7.TilesetName);


            if (lbTileset.SelectedItem == null)
                lbTileset.SelectedIndex = 0;

            mapLayerToolStripMenuItem.Enabled = true;
        }

        private void LoudDefaultLayers()
        {
            MapLayerData mapLayerData1 = new MapLayerData("Water", mData.MapWidth, mData.MapHeight, 0, 0, 6);

            MapLayer layer = MapLayer.FromMapLayerData(mapLayerData1);
            clbLayers.Items.Add(mapLayerData1.MapLayerName, true);
            clbLayers.SelectedIndex = clbLayers.Items.Count - 1;

            layers.Add(layer);

            MapLayerData mapLayerData2 = new MapLayerData("Level 1", mData.MapWidth, mData.MapHeight, 1, -1, -1);

            layer = MapLayer.FromMapLayerData(mapLayerData2);
            clbLayers.Items.Add(mapLayerData2.MapLayerName, true);
            clbLayers.SelectedIndex = clbLayers.Items.Count - 1;

            layers.Add(layer);

            MapLayerData mapLayerData3 = new MapLayerData("Level 2", mData.MapWidth, mData.MapHeight, 2, -1, -1);

            layer = MapLayer.FromMapLayerData(mapLayerData3);
            clbLayers.Items.Add(mapLayerData3.MapLayerName, true);
            clbLayers.SelectedIndex = clbLayers.Items.Count - 1;

            layers.Add(layer);
            if (map == null)
            {
                map = new TileMapIso(tileSets[0], (MapLayer)layers[0]);

                for (int i = 1; i < tileSets.Count; i++)
                    map.AddTileset(tileSets[1]);

                for (int i = 1; i < layers.Count; i++)
                    map.AddLayer(layers[1]);
            }
        }

        #endregion
    }
}