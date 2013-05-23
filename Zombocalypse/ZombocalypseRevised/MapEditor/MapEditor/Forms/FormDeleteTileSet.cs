using System;
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
    public partial class FormDeleteTileSet : Form
    {
        #region Field Region

        private List<TilesetData> tileSets = new List<TilesetData>();
        private int itemSelected;
        private bool okPressed;

        #endregion

        #region Property Region

        public bool OKPressed
        {
            get { return okPressed; }
        }

        public int ItemSelected
        {
            get { return itemSelected; }
        }

        #endregion

        #region Constructor Region

        public FormDeleteTileSet(List<TilesetData> tileSets)
        {
            this.tileSets = tileSets;
            InitializeComponent();
            cbSetData();
        }

        #endregion

        #region Method Region

        private void cbSetData()
        {
            foreach (TilesetData data in tileSets)
            {
                cbTileSetSelect.Items.Add(data.TilesetName);
            }
        }  

        private void bDelete_Click(object sender, EventArgs e)
        {
            if (tileSets.Count <= 1)
            {
                MessageBox.Show("There must be two or more TileSets to delete");
                return;
            }
            if (cbTileSetSelect.SelectedItem != null)
            {
                itemSelected = cbTileSetSelect.SelectedIndex;
            }
            else
            {
                MessageBox.Show("You must select TileSet.");
                return;
            }
            okPressed = true;
            this.Close();
        }

        private void bCancel_Click(object sender, EventArgs e)
        {
            okPressed = false;
            this.Close();
        }
        #endregion


    }
}
