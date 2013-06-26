namespace DBCompare.UI
{
    partial class Compare
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.RdbViewChecked = new System.Windows.Forms.RadioButton();
            this.RdbViewAll = new System.Windows.Forms.RadioButton();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnScripts = new System.Windows.Forms.Button();
            this.BtnProject = new System.Windows.Forms.Button();
            this.BtnRerun = new System.Windows.Forms.Button();
            this.LblProgress = new System.Windows.Forms.Label();
            this.ProgressBar = new System.Windows.Forms.Label();
            this.LblCurrentOperation = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LblStatus = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.compareAnimation1 = new DBCompare.UI.CompareAnimation();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.StatusColumn = new System.Windows.Forms.DataGridViewImageColumn();
            this.TableColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RecordsColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ChangesColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DetailsColumn = new System.Windows.Forms.DataGridViewLinkColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.RdbViewChecked);
            this.panel1.Controls.Add(this.RdbViewAll);
            this.panel1.Controls.Add(this.BtnCancel);
            this.panel1.Controls.Add(this.BtnScripts);
            this.panel1.Controls.Add(this.BtnProject);
            this.panel1.Controls.Add(this.BtnRerun);
            this.panel1.Controls.Add(this.LblProgress);
            this.panel1.Controls.Add(this.ProgressBar);
            this.panel1.Controls.Add(this.LblCurrentOperation);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.LblStatus);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Controls.Add(this.compareAnimation1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(903, 221);
            this.panel1.TabIndex = 2;
            // 
            // RdbViewChecked
            // 
            this.RdbViewChecked.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RdbViewChecked.AutoSize = true;
            this.RdbViewChecked.Checked = true;
            this.RdbViewChecked.Location = new System.Drawing.Point(695, 145);
            this.RdbViewChecked.Name = "RdbViewChecked";
            this.RdbViewChecked.Size = new System.Drawing.Size(156, 21);
            this.RdbViewChecked.TabIndex = 4;
            this.RdbViewChecked.TabStop = true;
            this.RdbViewChecked.Text = "View Changed Tables";
            this.RdbViewChecked.UseVisualStyleBackColor = true;
            // 
            // RdbViewAll
            // 
            this.RdbViewAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RdbViewAll.AutoSize = true;
            this.RdbViewAll.Location = new System.Drawing.Point(695, 118);
            this.RdbViewAll.Name = "RdbViewAll";
            this.RdbViewAll.Size = new System.Drawing.Size(115, 21);
            this.RdbViewAll.TabIndex = 4;
            this.RdbViewAll.Text = "View All Tables";
            this.RdbViewAll.UseVisualStyleBackColor = true;
            this.RdbViewAll.CheckedChanged += new System.EventHandler(this.RdbViewAll_CheckedChanged);
            // 
            // BtnCancel
            // 
            this.BtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnCancel.Enabled = false;
            this.BtnCancel.Image = global::DBCompare.Properties.Resources.Cancel;
            this.BtnCancel.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnCancel.Location = new System.Drawing.Point(695, 50);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(95, 28);
            this.BtnCancel.TabIndex = 3;
            this.BtnCancel.Text = "Cancel";
            this.BtnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnCancel.UseVisualStyleBackColor = true;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnScripts
            // 
            this.BtnScripts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnScripts.Image = global::DBCompare.Properties.Resources.Script;
            this.BtnScripts.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnScripts.Location = new System.Drawing.Point(695, 84);
            this.BtnScripts.Name = "BtnScripts";
            this.BtnScripts.Size = new System.Drawing.Size(196, 28);
            this.BtnScripts.TabIndex = 3;
            this.BtnScripts.Text = "Generate Scripts";
            this.BtnScripts.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnScripts.UseVisualStyleBackColor = true;
            this.BtnScripts.Click += new System.EventHandler(this.BtnScripts_Click);
            // 
            // BtnProject
            // 
            this.BtnProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnProject.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnProject.Location = new System.Drawing.Point(695, 16);
            this.BtnProject.Name = "BtnProject";
            this.BtnProject.Size = new System.Drawing.Size(196, 28);
            this.BtnProject.TabIndex = 3;
            this.BtnProject.Text = "Go Back To Project";
            this.BtnProject.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnProject.UseVisualStyleBackColor = true;
            this.BtnProject.Click += new System.EventHandler(this.BtnProject_Click);
            // 
            // BtnRerun
            // 
            this.BtnRerun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.BtnRerun.Image = global::DBCompare.Properties.Resources.Run;
            this.BtnRerun.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.BtnRerun.Location = new System.Drawing.Point(796, 50);
            this.BtnRerun.Name = "BtnRerun";
            this.BtnRerun.Size = new System.Drawing.Size(95, 28);
            this.BtnRerun.TabIndex = 3;
            this.BtnRerun.Text = "Re-run";
            this.BtnRerun.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.BtnRerun.UseVisualStyleBackColor = true;
            this.BtnRerun.Click += new System.EventHandler(this.BtnRerun_Click);
            // 
            // LblProgress
            // 
            this.LblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblProgress.Font = new System.Drawing.Font("Tahoma", 10F);
            this.LblProgress.Location = new System.Drawing.Point(360, 149);
            this.LblProgress.Name = "LblProgress";
            this.LblProgress.Size = new System.Drawing.Size(329, 17);
            this.LblProgress.TabIndex = 2;
            this.LblProgress.Text = "<Progress>";
            // 
            // ProgressBar
            // 
            this.ProgressBar.AutoSize = true;
            this.ProgressBar.Font = new System.Drawing.Font("Tahoma", 10F);
            this.ProgressBar.Location = new System.Drawing.Point(206, 149);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(66, 17);
            this.ProgressBar.TabIndex = 2;
            this.ProgressBar.Text = "Progress:";
            // 
            // LblCurrentOperation
            // 
            this.LblCurrentOperation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblCurrentOperation.Font = new System.Drawing.Font("Tahoma", 10F);
            this.LblCurrentOperation.Location = new System.Drawing.Point(360, 114);
            this.LblCurrentOperation.Name = "LblCurrentOperation";
            this.LblCurrentOperation.Size = new System.Drawing.Size(329, 17);
            this.LblCurrentOperation.TabIndex = 2;
            this.LblCurrentOperation.Text = "<Current Operation>";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label2.Location = new System.Drawing.Point(206, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Current Operation:";
            // 
            // LblStatus
            // 
            this.LblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.LblStatus.Font = new System.Drawing.Font("Tahoma", 10F);
            this.LblStatus.Location = new System.Drawing.Point(360, 81);
            this.LblStatus.Name = "LblStatus";
            this.LblStatus.Size = new System.Drawing.Size(329, 17);
            this.LblStatus.TabIndex = 2;
            this.LblStatus.Text = "<Status>";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 24F);
            this.label3.Location = new System.Drawing.Point(205, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(144, 36);
            this.label3.TabIndex = 2;
            this.label3.Text = "Compare";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F);
            this.label1.Location = new System.Drawing.Point(206, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Status:";
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(0, 183);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(903, 38);
            this.progressBar1.TabIndex = 1;
            // 
            // compareAnimation1
            // 
            this.compareAnimation1.Location = new System.Drawing.Point(3, 4);
            this.compareAnimation1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.compareAnimation1.Name = "compareAnimation1";
            this.compareAnimation1.Size = new System.Drawing.Size(177, 177);
            this.compareAnimation1.TabIndex = 0;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.StatusColumn,
            this.TableColumn,
            this.RecordsColumn,
            this.ChangesColumn,
            this.DetailsColumn});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 221);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(903, 473);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // StatusColumn
            // 
            this.StatusColumn.HeaderText = " ";
            this.StatusColumn.Name = "StatusColumn";
            this.StatusColumn.ReadOnly = true;
            this.StatusColumn.Width = 20;
            // 
            // TableColumn
            // 
            this.TableColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.TableColumn.HeaderText = "Table";
            this.TableColumn.Name = "TableColumn";
            this.TableColumn.ReadOnly = true;
            // 
            // RecordsColumn
            // 
            this.RecordsColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.RecordsColumn.HeaderText = "Records";
            this.RecordsColumn.Name = "RecordsColumn";
            this.RecordsColumn.ReadOnly = true;
            // 
            // ChangesColumn
            // 
            this.ChangesColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ChangesColumn.HeaderText = "Changes";
            this.ChangesColumn.Name = "ChangesColumn";
            this.ChangesColumn.ReadOnly = true;
            // 
            // DetailsColumn
            // 
            this.DetailsColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.DetailsColumn.HeaderText = "Details";
            this.DetailsColumn.Name = "DetailsColumn";
            this.DetailsColumn.ReadOnly = true;
            // 
            // Compare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 10F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Compare";
            this.Size = new System.Drawing.Size(903, 694);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private CompareAnimation compareAnimation1;
        private System.Windows.Forms.Label LblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LblProgress;
        private System.Windows.Forms.Label ProgressBar;
        private System.Windows.Forms.Label LblCurrentOperation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BtnRerun;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnProject;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewImageColumn StatusColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TableColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn RecordsColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ChangesColumn;
        private System.Windows.Forms.DataGridViewLinkColumn DetailsColumn;
        private System.Windows.Forms.Button BtnScripts;
        private System.Windows.Forms.RadioButton RdbViewChecked;
        private System.Windows.Forms.RadioButton RdbViewAll;
    }
}
