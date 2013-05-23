using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MapEditor.Forms
{
    public partial class FormDeleteLayer : Form
    {
        #region Field Region

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
        public FormDeleteLayer(System.Windows.Forms.CheckedListBox.ObjectCollection mapLayers)
        {
            InitializeComponent();
            cbSetData(mapLayers);
        }

        #endregion

        #region Method Region

        private void cbSetData(System.Windows.Forms.CheckedListBox.ObjectCollection mapLayers)
        {
            foreach (string layer in mapLayers)
            {
                cbSelectLayer.Items.Add(layer);
            }
        }

        private void bDelete_Click(object sender, EventArgs e)
        {
            //if (tileSets.Count <= 1)
            //{
            //    MessageBox.Show("There must be two or more TileSets to delete");
            //    return;
            //}
            if (cbSelectLayer.SelectedItem != null)
            {
                itemSelected = cbSelectLayer.SelectedIndex;
            }
            else
            {
                MessageBox.Show("You must select Layer.");
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
