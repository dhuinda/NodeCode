namespace CodeDesigner.UI.Forms
{
    partial class ProjectManager
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
            this.neoPanel3 = new CodeDesigner.UI.Controls.Panels.NeoPanel();
            this.neoPanel1 = new CodeDesigner.UI.Controls.Panels.NeoPanel();
            this.SuspendLayout();
            // 
            // neoPanel3
            // 
            this.neoPanel3.BorderGradientAngle = 0F;
            this.neoPanel3.BorderGradientOne = System.Drawing.Color.Empty;
            this.neoPanel3.BorderGradientTwo = System.Drawing.Color.Empty;
            this.neoPanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.neoPanel3.DragControl = true;
            this.neoPanel3.DragForm = this;
            this.neoPanel3.GradientAngle = 45F;
            this.neoPanel3.GradientOne = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(19)))), ((int)(((byte)(29)))));
            this.neoPanel3.GradientTwo = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(19)))), ((int)(((byte)(29)))));
            this.neoPanel3.Location = new System.Drawing.Point(0, 0);
            this.neoPanel3.Name = "neoPanel3";
            this.neoPanel3.Size = new System.Drawing.Size(322, 23);
            this.neoPanel3.TabIndex = 2;
            this.neoPanel3.Text = "neoPanel3";
            // 
            // neoPanel1
            // 
            this.neoPanel1.BackColor = System.Drawing.Color.Transparent;
            this.neoPanel1.BorderGradientAngle = 0F;
            this.neoPanel1.BorderGradientOne = System.Drawing.Color.Empty;
            this.neoPanel1.BorderGradientTwo = System.Drawing.Color.Empty;
            this.neoPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.neoPanel1.DragControl = false;
            this.neoPanel1.DragForm = null;
            this.neoPanel1.GradientAngle = 45F;
            this.neoPanel1.GradientOne = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(29)))), ((int)(((byte)(39)))));
            this.neoPanel1.GradientTwo = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(29)))), ((int)(((byte)(39)))));
            this.neoPanel1.Location = new System.Drawing.Point(0, 23);
            this.neoPanel1.Name = "neoPanel1";
            this.neoPanel1.Size = new System.Drawing.Size(322, 397);
            this.neoPanel1.TabIndex = 0;
            this.neoPanel1.Text = "neoPanel1";
            // 
            // ProjectManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(322, 420);
            this.Controls.Add(this.neoPanel1);
            this.Controls.Add(this.neoPanel3);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProjectManager";
            this.Text = "ProjectManager";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Panels.NeoPanel neoPanel3;
        private Controls.Panels.NeoPanel neoPanel1;
    }
}