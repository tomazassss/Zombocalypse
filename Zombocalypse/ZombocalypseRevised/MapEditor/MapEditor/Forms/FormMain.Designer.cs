namespace MapEditor.Forms
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tilesetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newTilesetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteTilesetToolStripMenuItemripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteLayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.mapDisplay = new MapEditor.MapDisplay();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabTiles = new System.Windows.Forms.TabPage();
            this.tbMapLocation = new System.Windows.Forms.TextBox();
            this.lbTileset = new System.Windows.Forms.ListBox();
            this.lblTilesets = new System.Windows.Forms.Label();
            this.pbTilesetPreview = new System.Windows.Forms.PictureBox();
            this.lblCurrentTileset = new System.Windows.Forms.Label();
            this.nudCurrentTile = new System.Windows.Forms.NumericUpDown();
            this.gbDrawMode = new System.Windows.Forms.GroupBox();
            this.rbErase = new System.Windows.Forms.RadioButton();
            this.rbDraw = new System.Windows.Forms.RadioButton();
            this.pbTilePreview = new System.Windows.Forms.PictureBox();
            this.lb1Tile = new System.Windows.Forms.Label();
            this.tabLayers = new System.Windows.Forms.TabPage();
            this.clbLayers = new System.Windows.Forms.CheckedListBox();
            this.controlTimer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabTiles.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTilesetPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCurrentTile)).BeginInit();
            this.gbDrawMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTilePreview)).BeginInit();
            this.tabLayers.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.tilesetToolStripMenuItem,
            this.mapLayerToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newMapToolStripMenuItem,
            this.openMapToolStripMenuItem,
            this.saveMapToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newMapToolStripMenuItem
            // 
            this.newMapToolStripMenuItem.Name = "newMapToolStripMenuItem";
            this.newMapToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.newMapToolStripMenuItem.Text = "New Map";
            // 
            // openMapToolStripMenuItem
            // 
            this.openMapToolStripMenuItem.Name = "openMapToolStripMenuItem";
            this.openMapToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.openMapToolStripMenuItem.Text = "Open Map";
            // 
            // saveMapToolStripMenuItem
            // 
            this.saveMapToolStripMenuItem.Name = "saveMapToolStripMenuItem";
            this.saveMapToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.saveMapToolStripMenuItem.Text = "Save Map";
            // 
            // tilesetToolStripMenuItem
            // 
            this.tilesetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newTilesetToolStripMenuItem,
            this.deleteTilesetToolStripMenuItemripMenuItem});
            this.tilesetToolStripMenuItem.Name = "tilesetToolStripMenuItem";
            this.tilesetToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.tilesetToolStripMenuItem.Text = "Tileset";
            // 
            // newTilesetToolStripMenuItem
            // 
            this.newTilesetToolStripMenuItem.Name = "newTilesetToolStripMenuItem";
            this.newTilesetToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.newTilesetToolStripMenuItem.Text = "New Tileset";
            // 
            // deleteTilesetToolStripMenuItemripMenuItem
            // 
            this.deleteTilesetToolStripMenuItemripMenuItem.Name = "deleteTilesetToolStripMenuItemripMenuItem";
            this.deleteTilesetToolStripMenuItemripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.deleteTilesetToolStripMenuItemripMenuItem.Text = "Delete Tileset";
            this.deleteTilesetToolStripMenuItemripMenuItem.Click += new System.EventHandler(this.deleteTilesetToolStripMenuItemripMenuItem_Click);
            // 
            // mapLayerToolStripMenuItem
            // 
            this.mapLayerToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newLayerToolStripMenuItem,
            this.deleteLayerToolStripMenuItem});
            this.mapLayerToolStripMenuItem.Name = "mapLayerToolStripMenuItem";
            this.mapLayerToolStripMenuItem.Size = new System.Drawing.Size(74, 20);
            this.mapLayerToolStripMenuItem.Text = "Map Layer";
            // 
            // newLayerToolStripMenuItem
            // 
            this.newLayerToolStripMenuItem.Name = "newLayerToolStripMenuItem";
            this.newLayerToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.newLayerToolStripMenuItem.Text = "New Layer";
            // 
            // deleteLayerToolStripMenuItem
            // 
            this.deleteLayerToolStripMenuItem.Name = "deleteLayerToolStripMenuItem";
            this.deleteLayerToolStripMenuItem.Size = new System.Drawing.Size(138, 22);
            this.deleteLayerToolStripMenuItem.Text = "Delete Layer";
            this.deleteLayerToolStripMenuItem.Click += new System.EventHandler(this.deleteLayerToolStripMenuItem_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.mapDisplay);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 658);
            this.splitContainer1.SplitterDistance = 800;
            this.splitContainer1.TabIndex = 1;
            // 
            // mapDisplay
            // 
            this.mapDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapDisplay.Location = new System.Drawing.Point(0, 0);
            this.mapDisplay.Name = "mapDisplay";
            this.mapDisplay.Size = new System.Drawing.Size(800, 658);
            this.mapDisplay.TabIndex = 0;
            this.mapDisplay.Text = "mapDisplay1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabTiles);
            this.tabControl1.Controls.Add(this.tabLayers);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(204, 658);
            this.tabControl1.TabIndex = 1;
            // 
            // tabTiles
            // 
            this.tabTiles.Controls.Add(this.tbMapLocation);
            this.tabTiles.Controls.Add(this.lbTileset);
            this.tabTiles.Controls.Add(this.lblTilesets);
            this.tabTiles.Controls.Add(this.pbTilesetPreview);
            this.tabTiles.Controls.Add(this.lblCurrentTileset);
            this.tabTiles.Controls.Add(this.nudCurrentTile);
            this.tabTiles.Controls.Add(this.gbDrawMode);
            this.tabTiles.Controls.Add(this.pbTilePreview);
            this.tabTiles.Controls.Add(this.lb1Tile);
            this.tabTiles.Location = new System.Drawing.Point(4, 22);
            this.tabTiles.Name = "tabTiles";
            this.tabTiles.Padding = new System.Windows.Forms.Padding(3);
            this.tabTiles.Size = new System.Drawing.Size(196, 632);
            this.tabTiles.TabIndex = 0;
            this.tabTiles.Text = "Tiles";
            this.tabTiles.UseVisualStyleBackColor = true;
            // 
            // tbMapLocation
            // 
            this.tbMapLocation.Location = new System.Drawing.Point(7, 609);
            this.tbMapLocation.Name = "tbMapLocation";
            this.tbMapLocation.Size = new System.Drawing.Size(180, 20);
            this.tbMapLocation.TabIndex = 8;
            // 
            // lbTileset
            // 
            this.lbTileset.FormattingEnabled = true;
            this.lbTileset.Location = new System.Drawing.Point(7, 352);
            this.lbTileset.Name = "lbTileset";
            this.lbTileset.Size = new System.Drawing.Size(180, 251);
            this.lbTileset.TabIndex = 7;
            // 
            // lblTilesets
            // 
            this.lblTilesets.Location = new System.Drawing.Point(7, 321);
            this.lblTilesets.Name = "lblTilesets";
            this.lblTilesets.Size = new System.Drawing.Size(180, 23);
            this.lblTilesets.TabIndex = 6;
            this.lblTilesets.Text = "Tilesets";
            this.lblTilesets.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pbTilesetPreview
            // 
            this.pbTilesetPreview.Location = new System.Drawing.Point(7, 138);
            this.pbTilesetPreview.Name = "pbTilesetPreview";
            this.pbTilesetPreview.Size = new System.Drawing.Size(180, 180);
            this.pbTilesetPreview.TabIndex = 5;
            this.pbTilesetPreview.TabStop = false;
            // 
            // lblCurrentTileset
            // 
            this.lblCurrentTileset.Location = new System.Drawing.Point(7, 112);
            this.lblCurrentTileset.Name = "lblCurrentTileset";
            this.lblCurrentTileset.Size = new System.Drawing.Size(180, 23);
            this.lblCurrentTileset.TabIndex = 4;
            this.lblCurrentTileset.Text = "Current Tileset";
            this.lblCurrentTileset.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // nudCurrentTile
            // 
            this.nudCurrentTile.Location = new System.Drawing.Point(7, 83);
            this.nudCurrentTile.Name = "nudCurrentTile";
            this.nudCurrentTile.Size = new System.Drawing.Size(180, 20);
            this.nudCurrentTile.TabIndex = 3;
            // 
            // gbDrawMode
            // 
            this.gbDrawMode.Controls.Add(this.rbErase);
            this.gbDrawMode.Controls.Add(this.rbDraw);
            this.gbDrawMode.Location = new System.Drawing.Point(100, 7);
            this.gbDrawMode.Name = "gbDrawMode";
            this.gbDrawMode.Size = new System.Drawing.Size(85, 70);
            this.gbDrawMode.TabIndex = 2;
            this.gbDrawMode.TabStop = false;
            this.gbDrawMode.Text = "Draw Mode";
            // 
            // rbErase
            // 
            this.rbErase.AutoSize = true;
            this.rbErase.Location = new System.Drawing.Point(7, 43);
            this.rbErase.Name = "rbErase";
            this.rbErase.Size = new System.Drawing.Size(52, 17);
            this.rbErase.TabIndex = 1;
            this.rbErase.TabStop = true;
            this.rbErase.Text = "Erase";
            this.rbErase.UseVisualStyleBackColor = true;
            // 
            // rbDraw
            // 
            this.rbDraw.AutoSize = true;
            this.rbDraw.Checked = true;
            this.rbDraw.Location = new System.Drawing.Point(7, 20);
            this.rbDraw.Name = "rbDraw";
            this.rbDraw.Size = new System.Drawing.Size(50, 17);
            this.rbDraw.TabIndex = 0;
            this.rbDraw.TabStop = true;
            this.rbDraw.Text = "Draw";
            this.rbDraw.UseVisualStyleBackColor = true;
            // 
            // pbTilePreview
            // 
            this.pbTilePreview.Location = new System.Drawing.Point(7, 27);
            this.pbTilePreview.Name = "pbTilePreview";
            this.pbTilePreview.Size = new System.Drawing.Size(80, 50);
            this.pbTilePreview.TabIndex = 1;
            this.pbTilePreview.TabStop = false;
            // 
            // lb1Tile
            // 
            this.lb1Tile.Location = new System.Drawing.Point(7, 7);
            this.lb1Tile.Name = "lb1Tile";
            this.lb1Tile.Size = new System.Drawing.Size(50, 17);
            this.lb1Tile.TabIndex = 0;
            this.lb1Tile.Text = "Tile";
            this.lb1Tile.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tabLayers
            // 
            this.tabLayers.Controls.Add(this.clbLayers);
            this.tabLayers.Location = new System.Drawing.Point(4, 22);
            this.tabLayers.Name = "tabLayers";
            this.tabLayers.Padding = new System.Windows.Forms.Padding(3);
            this.tabLayers.Size = new System.Drawing.Size(196, 632);
            this.tabLayers.TabIndex = 1;
            this.tabLayers.Text = "Map Layers";
            this.tabLayers.UseVisualStyleBackColor = true;
            // 
            // clbLayers
            // 
            this.clbLayers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbLayers.FormattingEnabled = true;
            this.clbLayers.Location = new System.Drawing.Point(3, 3);
            this.clbLayers.Name = "clbLayers";
            this.clbLayers.Size = new System.Drawing.Size(190, 626);
            this.clbLayers.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 682);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "Map Editor";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabTiles.ResumeLayout(false);
            this.tabTiles.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTilesetPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCurrentTile)).EndInit();
            this.gbDrawMode.ResumeLayout(false);
            this.gbDrawMode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTilePreview)).EndInit();
            this.tabLayers.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveMapToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private MapDisplay mapDisplay;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabTiles;
        private System.Windows.Forms.TabPage tabLayers;
        private System.Windows.Forms.ToolStripMenuItem tilesetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newTilesetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteTilesetToolStripMenuItemripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapLayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newLayerToolStripMenuItem;
        private System.Windows.Forms.ListBox lbTileset;
        private System.Windows.Forms.Label lblTilesets;
        private System.Windows.Forms.PictureBox pbTilesetPreview;
        private System.Windows.Forms.Label lblCurrentTileset;
        private System.Windows.Forms.NumericUpDown nudCurrentTile;
        private System.Windows.Forms.GroupBox gbDrawMode;
        private System.Windows.Forms.RadioButton rbErase;
        private System.Windows.Forms.RadioButton rbDraw;
        private System.Windows.Forms.PictureBox pbTilePreview;
        private System.Windows.Forms.Label lb1Tile;
        private System.Windows.Forms.CheckedListBox clbLayers;
        private System.Windows.Forms.Timer controlTimer;
        private System.Windows.Forms.TextBox tbMapLocation;
        private System.Windows.Forms.ToolStripMenuItem deleteLayerToolStripMenuItem;
    }
}