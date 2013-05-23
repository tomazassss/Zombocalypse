﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using XRPGLibrary.WorldClasses;

namespace MapEditor.Forms
{
    public partial class FormNewLayer : Form
    {
        #region Field Region

        private bool okPressed;

        private int LayerWidth;
        private int LayerHeight;

        private MapLayerData mapLayerData;

        #endregion

        #region Property Region

        public bool OKPressed
        {
            get { return okPressed; }
        }

        public MapLayerData MapLayerData
        {
            get { return mapLayerData; }
        }

        #endregion

        #region Constructor Region

        public FormNewLayer(int width, int height)
        {
            InitializeComponent();

            LayerWidth = width;
            LayerHeight = height;
            btnOK.Click += new EventHandler(btnOK_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        #endregion

        #region Button Event Handler Region

        void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbLayerName.Text))
            {
                MessageBox.Show("You must enter a name for the layer.", "Missing Layer Name");
                return;
            }

            if (cbFill.Checked)
            {
                mapLayerData = new MapLayerData(
                    tbLayerName.Text,
                    LayerWidth,
                    LayerHeight,
                    (int)nudLayerLevel.Value,
                    (int)nudTileset.Value,
                    (int)nudTileset.Value);
            }
            else
            {
                mapLayerData = new MapLayerData(
                    tbLayerName.Text,
                    LayerWidth,
                    LayerHeight,
                    (int)nudLayerLevel.Value);
            }

            okPressed = true;
            this.Close();
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            okPressed = false;
            this.Close();
        }

        #endregion

    }
}