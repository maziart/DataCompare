namespace DBCompare.UI
{
    partial class MainForm
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.compareProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.monitorProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.openRecentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.projectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeConnectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionBToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.projectToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(844, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.toolStripMenuItem1,
            this.openRecentToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.compareProjectToolStripMenuItem,
            this.monitorProjectToolStripMenuItem});
            this.newToolStripMenuItem.Image = global::DBCompare.Properties.Resources.New;
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.newToolStripMenuItem.Text = "&New Project";
            // 
            // compareProjectToolStripMenuItem
            // 
            this.compareProjectToolStripMenuItem.Image = global::DBCompare.Properties.Resources.Compare16;
            this.compareProjectToolStripMenuItem.Name = "compareProjectToolStripMenuItem";
            this.compareProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.compareProjectToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.compareProjectToolStripMenuItem.Text = "&Compare Project";
            this.compareProjectToolStripMenuItem.Click += new System.EventHandler(this.compareProjectToolStripMenuItem_Click);
            // 
            // monitorProjectToolStripMenuItem
            // 
            this.monitorProjectToolStripMenuItem.Image = global::DBCompare.Properties.Resources.Monitor16;
            this.monitorProjectToolStripMenuItem.Name = "monitorProjectToolStripMenuItem";
            this.monitorProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.monitorProjectToolStripMenuItem.Size = new System.Drawing.Size(206, 22);
            this.monitorProjectToolStripMenuItem.Text = "&Monitor Project";
            this.monitorProjectToolStripMenuItem.Click += new System.EventHandler(this.monitorProjectToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = global::DBCompare.Properties.Resources.Open;
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.openToolStripMenuItem.Text = "&Open Project";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = global::DBCompare.Properties.Resources.Close;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.W)));
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.closeToolStripMenuItem.Text = "&Close Project";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::DBCompare.Properties.Resources.Save;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.saveToolStripMenuItem.Text = "&Save Project";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(185, 6);
            // 
            // openRecentToolStripMenuItem
            // 
            this.openRecentToolStripMenuItem.Name = "openRecentToolStripMenuItem";
            this.openRecentToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.openRecentToolStripMenuItem.Text = "Open &Recent";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // projectToolStripMenuItem
            // 
            this.projectToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.changeConnectionsToolStripMenuItem,
            this.runProjectToolStripMenuItem});
            this.projectToolStripMenuItem.Name = "projectToolStripMenuItem";
            this.projectToolStripMenuItem.Size = new System.Drawing.Size(56, 20);
            this.projectToolStripMenuItem.Text = "&Project";
            // 
            // changeConnectionsToolStripMenuItem
            // 
            this.changeConnectionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectionAToolStripMenuItem,
            this.connectionBToolStripMenuItem});
            this.changeConnectionsToolStripMenuItem.Image = global::DBCompare.Properties.Resources.Data;
            this.changeConnectionsToolStripMenuItem.Name = "changeConnectionsToolStripMenuItem";
            this.changeConnectionsToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.changeConnectionsToolStripMenuItem.Text = "&Change Connections";
            // 
            // connectionAToolStripMenuItem
            // 
            this.connectionAToolStripMenuItem.Image = global::DBCompare.Properties.Resources.A;
            this.connectionAToolStripMenuItem.Name = "connectionAToolStripMenuItem";
            this.connectionAToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.connectionAToolStripMenuItem.Text = "Connection &A";
            this.connectionAToolStripMenuItem.Click += new System.EventHandler(this.connectionAToolStripMenuItem_Click);
            // 
            // connectionBToolStripMenuItem
            // 
            this.connectionBToolStripMenuItem.Image = global::DBCompare.Properties.Resources.B;
            this.connectionBToolStripMenuItem.Name = "connectionBToolStripMenuItem";
            this.connectionBToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.connectionBToolStripMenuItem.Text = "Connection &B";
            this.connectionBToolStripMenuItem.Click += new System.EventHandler(this.connectionBToolStripMenuItem_Click);
            // 
            // runProjectToolStripMenuItem
            // 
            this.runProjectToolStripMenuItem.Image = global::DBCompare.Properties.Resources.Run;
            this.runProjectToolStripMenuItem.Name = "runProjectToolStripMenuItem";
            this.runProjectToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.runProjectToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            this.runProjectToolStripMenuItem.Text = "&Run Project";
            this.runProjectToolStripMenuItem.Click += new System.EventHandler(this.runProjectToolStripMenuItem_Click);
            // 
            // MainPanel
            // 
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(0, 24);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(844, 507);
            this.MainPanel.TabIndex = 2;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 531);
            this.Controls.Add(this.MainPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DB Compare";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem openRecentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem compareProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monitorProjectToolStripMenuItem;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.ToolStripMenuItem projectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeConnectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectionAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem connectionBToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runProjectToolStripMenuItem;
    }
}