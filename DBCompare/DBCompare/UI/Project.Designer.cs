namespace DBCompare.UI
{
    partial class Project
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BtnRun = new System.Windows.Forms.Button();
            this.BtnConnectionB = new System.Windows.Forms.Button();
            this.BtnConnectionA = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel2 = new System.Windows.Forms.Panel();
            this.ChkSelectAll = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.checkedListBox1 = new System.Windows.Forms.CheckedListBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.BtnRun);
            this.panel1.Controls.Add(this.BtnConnectionB);
            this.panel1.Controls.Add(this.BtnConnectionA);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(799, 107);
            this.panel1.TabIndex = 1;
            // 
            // BtnRun
            // 
            this.BtnRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRun.Image = global::DBCompare.Properties.Resources.Run72;
            this.BtnRun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnRun.Location = new System.Drawing.Point(656, 8);
            this.BtnRun.Name = "BtnRun";
            this.BtnRun.Size = new System.Drawing.Size(140, 92);
            this.BtnRun.TabIndex = 5;
            this.BtnRun.Text = "Run (Compare)";
            this.BtnRun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.BtnRun, "F5");
            this.BtnRun.UseVisualStyleBackColor = true;
            this.BtnRun.Click += new System.EventHandler(this.BtnRun_Click);
            // 
            // BtnConnectionB
            // 
            this.BtnConnectionB.Image = global::DBCompare.Properties.Resources.B64;
            this.BtnConnectionB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnConnectionB.Location = new System.Drawing.Point(199, 28);
            this.BtnConnectionB.Name = "BtnConnectionB";
            this.BtnConnectionB.Size = new System.Drawing.Size(189, 72);
            this.BtnConnectionB.TabIndex = 1;
            this.BtnConnectionB.Text = "<ConnectionB>";
            this.BtnConnectionB.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.BtnConnectionB, "Click to change");
            this.BtnConnectionB.UseVisualStyleBackColor = true;
            this.BtnConnectionB.Click += new System.EventHandler(this.BtnConnectionB_Click);
            // 
            // BtnConnectionA
            // 
            this.BtnConnectionA.Image = global::DBCompare.Properties.Resources.A64;
            this.BtnConnectionA.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnConnectionA.Location = new System.Drawing.Point(4, 28);
            this.BtnConnectionA.Name = "BtnConnectionA";
            this.BtnConnectionA.Size = new System.Drawing.Size(189, 72);
            this.BtnConnectionA.TabIndex = 0;
            this.BtnConnectionA.Text = "<ConnectionA>";
            this.BtnConnectionA.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.BtnConnectionA, "Click to change");
            this.BtnConnectionA.UseVisualStyleBackColor = true;
            this.BtnConnectionA.Click += new System.EventHandler(this.BtnConnectionA_Click);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label1.Location = new System.Drawing.Point(199, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 21);
            this.label1.TabIndex = 4;
            this.label1.Text = "Connection B";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label2.Location = new System.Drawing.Point(4, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "Connection A";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ChkSelectAll);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 107);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(799, 50);
            this.panel2.TabIndex = 2;
            // 
            // ChkSelectAll
            // 
            this.ChkSelectAll.AutoSize = true;
            this.ChkSelectAll.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.ChkSelectAll.Location = new System.Drawing.Point(7, 27);
            this.ChkSelectAll.Name = "ChkSelectAll";
            this.ChkSelectAll.Size = new System.Drawing.Size(78, 17);
            this.ChkSelectAll.TabIndex = 0;
            this.ChkSelectAll.Text = "Select All";
            this.ChkSelectAll.UseVisualStyleBackColor = true;
            this.ChkSelectAll.CheckedChanged += new System.EventHandler(this.ChkSelectAll_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F);
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(180, 19);
            this.label3.TabIndex = 0;
            this.label3.Text = "Select tables to compare";
            // 
            // checkedListBox1
            // 
            this.checkedListBox1.CheckOnClick = true;
            this.checkedListBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkedListBox1.FormattingEnabled = true;
            this.checkedListBox1.Location = new System.Drawing.Point(0, 157);
            this.checkedListBox1.Name = "checkedListBox1";
            this.checkedListBox1.Size = new System.Drawing.Size(799, 390);
            this.checkedListBox1.TabIndex = 3;
            this.checkedListBox1.SelectedIndexChanged += new System.EventHandler(this.checkedListBox1_SelectedIndexChanged);
            // 
            // Project
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.checkedListBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.Name = "Project";
            this.Size = new System.Drawing.Size(799, 547);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button BtnConnectionB;
        private System.Windows.Forms.Button BtnConnectionA;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckedListBox checkedListBox1;
        private System.Windows.Forms.CheckBox ChkSelectAll;
        private System.Windows.Forms.Button BtnRun;

    }
}
