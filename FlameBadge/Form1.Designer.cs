namespace FlameBadge
{
    partial class Form1
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
            this.HealthNum = new System.Windows.Forms.Label();
            this.Load = new System.Windows.Forms.Panel();
            this.Save = new System.Windows.Forms.Panel();
            this.Healthbar = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(53, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(669, 669);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // HealthNum
            // 
            this.HealthNum.AutoSize = true;
            this.HealthNum.Location = new System.Drawing.Point(820, 95);
            this.HealthNum.Name = "HealthNum";
            this.HealthNum.Size = new System.Drawing.Size(13, 13);
            this.HealthNum.TabIndex = 1;
            this.HealthNum.Text = "0";
            this.HealthNum.Click += new System.EventHandler(this.label1_Click);
            // 
            // Load
            // 
            this.Load.Location = new System.Drawing.Point(839, 454);
            this.Load.Name = "Load";
            this.Load.Size = new System.Drawing.Size(148, 52);
            this.Load.TabIndex = 0;
            // 
            // Save
            // 
            this.Save.Location = new System.Drawing.Point(839, 542);
            this.Save.Name = "Save";
            this.Save.Size = new System.Drawing.Size(204, 78);
            this.Save.TabIndex = 0;
            // 
            // Healthbar
            // 
            this.Healthbar.Location = new System.Drawing.Point(839, 81);
            this.Healthbar.Name = "Healthbar";
            this.Healthbar.Size = new System.Drawing.Size(220, 42);
            this.Healthbar.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1081, 738);
            this.Controls.Add(this.HealthNum);
            this.Controls.Add(this.Healthbar);
            this.Controls.Add(this.Save);
            this.Controls.Add(this.Load);
            this.Controls.Add(this.panel1);
            this.Name = "Form1";
            this.Text = "FlameBadge";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label HealthNum;
        private System.Windows.Forms.Panel Load;
        private System.Windows.Forms.Panel Save;
        private System.Windows.Forms.Panel Healthbar;
    }
}