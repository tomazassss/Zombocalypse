namespace MapEditor.Forms
{
    partial class FormDeleteTileSet
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
            this.cbTileSetSelect = new System.Windows.Forms.ComboBox();
            this.lblText = new System.Windows.Forms.Label();
            this.bDelete = new System.Windows.Forms.Button();
            this.bCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbTileSetSelect
            // 
            this.cbTileSetSelect.FormattingEnabled = true;
            this.cbTileSetSelect.Location = new System.Drawing.Point(100, 13);
            this.cbTileSetSelect.Name = "cbTileSetSelect";
            this.cbTileSetSelect.Size = new System.Drawing.Size(120, 21);
            this.cbTileSetSelect.TabIndex = 0;
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(15, 15);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(76, 13);
            this.lblText.TabIndex = 1;
            this.lblText.Text = "Select TileSet:";
            // 
            // bDelete
            // 
            this.bDelete.Location = new System.Drawing.Point(20, 50);
            this.bDelete.Name = "bDelete";
            this.bDelete.Size = new System.Drawing.Size(75, 23);
            this.bDelete.TabIndex = 2;
            this.bDelete.Text = "Delete";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bCancel
            // 
            this.bCancel.Location = new System.Drawing.Point(140, 50);
            this.bCancel.Name = "bCancel";
            this.bCancel.Size = new System.Drawing.Size(75, 23);
            this.bCancel.TabIndex = 3;
            this.bCancel.Text = "Cancel";
            this.bCancel.UseVisualStyleBackColor = true;
            this.bCancel.Click += new System.EventHandler(this.bCancel_Click);
            // 
            // FormDeleteTileSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(234, 82);
            this.Controls.Add(this.bCancel);
            this.Controls.Add(this.bDelete);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.cbTileSetSelect);
            this.Name = "FormDeleteTileSet";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormDeleteTileSet";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbTileSetSelect;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.Button bCancel;
    }
}