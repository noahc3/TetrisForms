namespace TetrisForms {
    partial class Form1 {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.gridGameBoard = new System.Windows.Forms.DataGridView();
            this.lblDebug = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridGameBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // gridGameBoard
            // 
            this.gridGameBoard.AllowUserToAddRows = false;
            this.gridGameBoard.AllowUserToDeleteRows = false;
            this.gridGameBoard.AllowUserToResizeColumns = false;
            this.gridGameBoard.AllowUserToResizeRows = false;
            this.gridGameBoard.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridGameBoard.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gridGameBoard.ColumnHeadersVisible = false;
            this.gridGameBoard.Location = new System.Drawing.Point(21, 28);
            this.gridGameBoard.MultiSelect = false;
            this.gridGameBoard.Name = "gridGameBoard";
            this.gridGameBoard.ReadOnly = true;
            this.gridGameBoard.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.gridGameBoard.RowHeadersVisible = false;
            this.gridGameBoard.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gridGameBoard.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gridGameBoard.Size = new System.Drawing.Size(275, 495);
            this.gridGameBoard.TabIndex = 1;
            // 
            // lblDebug
            // 
            this.lblDebug.AutoSize = true;
            this.lblDebug.Location = new System.Drawing.Point(370, 193);
            this.lblDebug.Name = "lblDebug";
            this.lblDebug.Size = new System.Drawing.Size(35, 13);
            this.lblDebug.TabIndex = 2;
            this.lblDebug.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 585);
            this.Controls.Add(this.lblDebug);
            this.Controls.Add(this.gridGameBoard);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.gridGameBoard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView gridGameBoard;
        private System.Windows.Forms.Label lblDebug;
    }
}

