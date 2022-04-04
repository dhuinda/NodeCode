using CodeDesigner.UI.Designer.Canvas;

namespace CodeDesigner.UI.Windows
{
    partial class Designer
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.DesignerCanvas = new CodeDesigner.UI.Windows.Resources.Controls.Panels.NeoPanel();
            this.NodePanel = new CodeDesigner.UI.Windows.Resources.Controls.Panels.NeoPanel();
            this.BlockPanel = new CodeDesigner.UI.Windows.Resources.Controls.Panels.InfoPanel();
            this.BlockTypePanel = new CodeDesigner.UI.Windows.Resources.Controls.Panels.InfoPanel();
            this.pictureBoxButton6 = new CodeDesigner.UI.Windows.Resources.Controls.Buttons.PictureBoxButton();
            this.pictureBoxButton5 = new CodeDesigner.UI.Windows.Resources.Controls.Buttons.PictureBoxButton();
            this.DockPanel = new CodeDesigner.UI.Windows.Resources.Controls.Panels.NeoPanel();
            this.pictureBoxButton4 = new CodeDesigner.UI.Windows.Resources.Controls.Buttons.PictureBoxButton();
            this.pictureBoxButton3 = new CodeDesigner.UI.Windows.Resources.Controls.Buttons.PictureBoxButton();
            this.pictureBoxButton2 = new CodeDesigner.UI.Windows.Resources.Controls.Buttons.PictureBoxButton();
            this.pictureBoxButton1 = new CodeDesigner.UI.Windows.Resources.Controls.Buttons.PictureBoxButton();
            this.panel1.SuspendLayout();
            this.NodePanel.SuspendLayout();
            this.BlockTypePanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxButton6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxButton5)).BeginInit();
            this.DockPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxButton4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxButton3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxButton2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxButton1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(39)))), ((int)(((byte)(49)))));
            this.panel1.Controls.Add(this.DesignerCanvas);
            this.panel1.Controls.Add(this.NodePanel);
            this.panel1.Controls.Add(this.DockPanel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(898, 478);
            this.panel1.TabIndex = 0;
            // 
            // DesignerCanvas
            // 
            this.DesignerCanvas.AutoScroll = true;
            this.DesignerCanvas.BorderGradientAngle = 0F;
            this.DesignerCanvas.BorderGradientOne = System.Drawing.Color.Empty;
            this.DesignerCanvas.BorderGradientTwo = System.Drawing.Color.Empty;
            this.DesignerCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DesignerCanvas.DragControl = false;
            this.DesignerCanvas.DragForm = null;
            this.DesignerCanvas.GradientAngle = 45F;
            this.DesignerCanvas.GradientOne = System.Drawing.Color.Empty;
            this.DesignerCanvas.GradientTwo = System.Drawing.Color.Empty;
            this.DesignerCanvas.Location = new System.Drawing.Point(229, 0);
            this.DesignerCanvas.Name = "DesignerCanvas";
            this.DesignerCanvas.Size = new System.Drawing.Size(669, 478);
            this.DesignerCanvas.TabIndex = 2;
            // 
            // NodePanel
            // 
            this.NodePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(29)))), ((int)(((byte)(39)))));
            this.NodePanel.BorderGradientAngle = 0F;
            this.NodePanel.BorderGradientOne = System.Drawing.Color.Empty;
            this.NodePanel.BorderGradientTwo = System.Drawing.Color.Empty;
            this.NodePanel.Controls.Add(this.BlockPanel);
            this.NodePanel.Controls.Add(this.BlockTypePanel);
            this.NodePanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.NodePanel.DragControl = false;
            this.NodePanel.DragForm = null;
            this.NodePanel.Font = new System.Drawing.Font("Montserrat", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.NodePanel.GradientAngle = 45F;
            this.NodePanel.GradientOne = System.Drawing.Color.Empty;
            this.NodePanel.GradientTwo = System.Drawing.Color.Empty;
            this.NodePanel.Location = new System.Drawing.Point(43, 0);
            this.NodePanel.Name = "NodePanel";
            this.NodePanel.Size = new System.Drawing.Size(186, 478);
            this.NodePanel.TabIndex = 1;
            // 
            // BlockPanel
            // 
            this.BlockPanel.BackColor = System.Drawing.Color.Transparent;
            this.BlockPanel.BorderGradientAngle = 0F;
            this.BlockPanel.BorderGradientOne = System.Drawing.Color.Empty;
            this.BlockPanel.BorderGradientTwo = System.Drawing.Color.Empty;
            this.BlockPanel.BorderRadius = 10;
            this.BlockPanel.Content = null;
            this.BlockPanel.DragControl = false;
            this.BlockPanel.DragForm = null;
            this.BlockPanel.GradientAngle = 45F;
            this.BlockPanel.GradientOne = System.Drawing.Color.Transparent;
            this.BlockPanel.GradientTwo = System.Drawing.Color.Transparent;
            this.BlockPanel.Location = new System.Drawing.Point(16, 56);
            this.BlockPanel.Name = "BlockPanel";
            this.BlockPanel.Size = new System.Drawing.Size(152, 405);
            this.BlockPanel.TabIndex = 1;
            this.BlockPanel.TextColor = System.Drawing.Color.Empty;
            // 
            // BlockTypePanel
            // 
            this.BlockTypePanel.BackColor = System.Drawing.Color.Transparent;
            this.BlockTypePanel.BorderGradientAngle = 0F;
            this.BlockTypePanel.BorderGradientOne = System.Drawing.Color.Empty;
            this.BlockTypePanel.BorderGradientTwo = System.Drawing.Color.Empty;
            this.BlockTypePanel.BorderRadius = 10;
            this.BlockTypePanel.Content = "LOGIC";
            this.BlockTypePanel.Controls.Add(this.pictureBoxButton6);
            this.BlockTypePanel.Controls.Add(this.pictureBoxButton5);
            this.BlockTypePanel.DragControl = false;
            this.BlockTypePanel.DragForm = null;
            this.BlockTypePanel.Font = new System.Drawing.Font("Montserrat ExtraBold", 8.999999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.BlockTypePanel.GradientAngle = 45F;
            this.BlockTypePanel.GradientOne = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(39)))), ((int)(((byte)(49)))));
            this.BlockTypePanel.GradientTwo = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(39)))), ((int)(((byte)(49)))));
            this.BlockTypePanel.Location = new System.Drawing.Point(16, 10);
            this.BlockTypePanel.Name = "BlockTypePanel";
            this.BlockTypePanel.Size = new System.Drawing.Size(152, 36);
            this.BlockTypePanel.TabIndex = 0;
            this.BlockTypePanel.TextColor = System.Drawing.Color.White;
            // 
            // pictureBoxButton6
            // 
            this.pictureBoxButton6.Image = global::CodeDesigner.UI.Properties.Resources.More_Than_35px;
            this.pictureBoxButton6.Location = new System.Drawing.Point(132, 10);
            this.pictureBoxButton6.Name = "pictureBoxButton6";
            this.pictureBoxButton6.ScaleFactorOnHover = 1.1D;
            this.pictureBoxButton6.Size = new System.Drawing.Size(15, 15);
            this.pictureBoxButton6.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxButton6.TabIndex = 1;
            this.pictureBoxButton6.TabStop = false;
            this.pictureBoxButton6.Click += new System.EventHandler(this.pictureBoxButton6_Click);
            // 
            // pictureBoxButton5
            // 
            this.pictureBoxButton5.Image = global::CodeDesigner.UI.Properties.Resources.Less_Than_35px;
            this.pictureBoxButton5.Location = new System.Drawing.Point(6, 10);
            this.pictureBoxButton5.Name = "pictureBoxButton5";
            this.pictureBoxButton5.ScaleFactorOnHover = 1.1D;
            this.pictureBoxButton5.Size = new System.Drawing.Size(15, 15);
            this.pictureBoxButton5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxButton5.TabIndex = 0;
            this.pictureBoxButton5.TabStop = false;
            this.pictureBoxButton5.Click += new System.EventHandler(this.pictureBoxButton5_Click);
            // 
            // DockPanel
            // 
            this.DockPanel.BorderGradientAngle = 0F;
            this.DockPanel.BorderGradientOne = System.Drawing.Color.Empty;
            this.DockPanel.BorderGradientTwo = System.Drawing.Color.Empty;
            this.DockPanel.Controls.Add(this.pictureBoxButton4);
            this.DockPanel.Controls.Add(this.pictureBoxButton3);
            this.DockPanel.Controls.Add(this.pictureBoxButton2);
            this.DockPanel.Controls.Add(this.pictureBoxButton1);
            this.DockPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.DockPanel.DragControl = false;
            this.DockPanel.DragForm = null;
            this.DockPanel.GradientAngle = 45F;
            this.DockPanel.GradientOne = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(19)))), ((int)(((byte)(29)))));
            this.DockPanel.GradientTwo = System.Drawing.Color.FromArgb(((int)(((byte)(14)))), ((int)(((byte)(19)))), ((int)(((byte)(29)))));
            this.DockPanel.Location = new System.Drawing.Point(0, 0);
            this.DockPanel.Name = "DockPanel";
            this.DockPanel.Size = new System.Drawing.Size(43, 478);
            this.DockPanel.TabIndex = 0;
            // 
            // pictureBoxButton4
            // 
            this.pictureBoxButton4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBoxButton4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxButton4.Image = global::CodeDesigner.UI.Properties.Resources.Exit_35px_RED;
            this.pictureBoxButton4.Location = new System.Drawing.Point(10, 446);
            this.pictureBoxButton4.Name = "pictureBoxButton4";
            this.pictureBoxButton4.ScaleFactorOnHover = 1.2D;
            this.pictureBoxButton4.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxButton4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxButton4.TabIndex = 3;
            this.pictureBoxButton4.TabStop = false;
            this.pictureBoxButton4.Click += new System.EventHandler(this.ExitBtnClicked);
            // 
            // pictureBoxButton3
            // 
            this.pictureBoxButton3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxButton3.Image = global::CodeDesigner.UI.Properties.Resources.Settings_35px;
            this.pictureBoxButton3.Location = new System.Drawing.Point(10, 72);
            this.pictureBoxButton3.Name = "pictureBoxButton3";
            this.pictureBoxButton3.ScaleFactorOnHover = 1.2D;
            this.pictureBoxButton3.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxButton3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxButton3.TabIndex = 2;
            this.pictureBoxButton3.TabStop = false;
            // 
            // pictureBoxButton2
            // 
            this.pictureBoxButton2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxButton2.Image = global::CodeDesigner.UI.Properties.Resources.Save_35px;
            this.pictureBoxButton2.Location = new System.Drawing.Point(10, 40);
            this.pictureBoxButton2.Name = "pictureBoxButton2";
            this.pictureBoxButton2.ScaleFactorOnHover = 1.2D;
            this.pictureBoxButton2.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxButton2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxButton2.TabIndex = 1;
            this.pictureBoxButton2.TabStop = false;
            // 
            // pictureBoxButton1
            // 
            this.pictureBoxButton1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxButton1.Image = global::CodeDesigner.UI.Properties.Resources.Play_35px_GREEN;
            this.pictureBoxButton1.Location = new System.Drawing.Point(10, 10);
            this.pictureBoxButton1.Name = "pictureBoxButton1";
            this.pictureBoxButton1.ScaleFactorOnHover = 1.2D;
            this.pictureBoxButton1.Size = new System.Drawing.Size(22, 22);
            this.pictureBoxButton1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxButton1.TabIndex = 0;
            this.pictureBoxButton1.TabStop = false;
            this.pictureBoxButton1.Click += (sender, args) =>
            {
                NodeConverter.CompileNodes(Core.Canvas.Nodes);
                // ConsoleHelper.StartProcess(null);
            };
            // 
            // Designer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(898, 478);
            this.Controls.Add(this.panel1);
            this.Name = "Designer";
            this.Text = "Designer";
            this.panel1.ResumeLayout(false);
            this.NodePanel.ResumeLayout(false);
            this.BlockTypePanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxButton6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxButton5)).EndInit();
            this.DockPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxButton4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxButton3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxButton2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxButton1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Resources.Controls.Panels.NeoPanel DockPanel;
        private Resources.Controls.Buttons.PictureBoxButton pictureBoxButton1;
        private Resources.Controls.Buttons.PictureBoxButton pictureBoxButton4;
        private Resources.Controls.Buttons.PictureBoxButton pictureBoxButton3;
        private Resources.Controls.Buttons.PictureBoxButton pictureBoxButton2;
        private Resources.Controls.Panels.NeoPanel NodePanel;
        private Resources.Controls.Buttons.PictureBoxButton pictureBoxButton6;
        private Resources.Controls.Buttons.PictureBoxButton pictureBoxButton5;
        public Resources.Controls.Panels.InfoPanel BlockTypePanel;
        public Resources.Controls.Panels.InfoPanel BlockPanel;
        public Resources.Controls.Panels.NeoPanel DesignerCanvas;
    }
}