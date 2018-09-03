namespace suc.calc.distance
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.txtFile = new System.Windows.Forms.TextBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnGen = new System.Windows.Forms.Button();
            this.pgb = new System.Windows.Forms.ProgressBar();
            this.txtBaiduAk = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtFile
            // 
            this.txtFile.Location = new System.Drawing.Point(13, 16);
            this.txtFile.Multiline = true;
            this.txtFile.Name = "txtFile";
            this.txtFile.Size = new System.Drawing.Size(220, 38);
            this.txtFile.TabIndex = 0;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(239, 14);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(89, 40);
            this.btnSelect.TabIndex = 1;
            this.btnSelect.Text = "选择文件...";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnGen
            // 
            this.btnGen.Location = new System.Drawing.Point(11, 89);
            this.btnGen.Name = "btnGen";
            this.btnGen.Size = new System.Drawing.Size(315, 33);
            this.btnGen.TabIndex = 2;
            this.btnGen.Text = "生成";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
            // 
            // pgb
            // 
            this.pgb.Location = new System.Drawing.Point(12, 128);
            this.pgb.Name = "pgb";
            this.pgb.Size = new System.Drawing.Size(313, 23);
            this.pgb.TabIndex = 3;
            // 
            // txtBaiduAk
            // 
            this.txtBaiduAk.Location = new System.Drawing.Point(13, 62);
            this.txtBaiduAk.Name = "txtBaiduAk";
            this.txtBaiduAk.Size = new System.Drawing.Size(312, 21);
            this.txtBaiduAk.TabIndex = 4;
            this.txtBaiduAk.Text = "TNhtFXcoDGFGPVeZGSW6d2dauVDGY2a7";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(338, 153);
            this.Controls.Add(this.txtBaiduAk);
            this.Controls.Add(this.pgb);
            this.Controls.Add(this.btnGen);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.txtFile);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "计算距离工具";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFile;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnGen;
        private System.Windows.Forms.ProgressBar pgb;
        private System.Windows.Forms.TextBox txtBaiduAk;
    }
}

