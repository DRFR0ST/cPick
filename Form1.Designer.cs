namespace CPickX
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.Update_timer = new System.Windows.Forms.Timer(this.components);
            this.color_lbl = new MaterialSkin.Controls.MaterialLabel();
            this.colorPanel_pnl = new System.Windows.Forms.Panel();
            this.pick_btn = new MaterialSkin.Controls.MaterialRaisedButton();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Update_timer
            // 
            this.Update_timer.Interval = 10;
            this.Update_timer.Tick += new System.EventHandler(this.Update_timer_Tick);
            // 
            // color_lbl
            // 
            this.color_lbl.AutoSize = true;
            this.color_lbl.BackColor = System.Drawing.Color.Transparent;
            this.color_lbl.Depth = 0;
            this.color_lbl.Font = new System.Drawing.Font("Roboto", 11F);
            this.color_lbl.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.color_lbl.Location = new System.Drawing.Point(12, 184);
            this.color_lbl.MouseState = MaterialSkin.MouseState.HOVER;
            this.color_lbl.Name = "color_lbl";
            this.color_lbl.Size = new System.Drawing.Size(202, 19);
            this.color_lbl.TabIndex = 0;
            this.color_lbl.Text = "Copyright © Mike Eling, 2017";
            // 
            // colorPanel_pnl
            // 
            this.colorPanel_pnl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(92)))), ((int)(((byte)(161)))));
            this.colorPanel_pnl.Location = new System.Drawing.Point(12, 70);
            this.colorPanel_pnl.Name = "colorPanel_pnl";
            this.colorPanel_pnl.Size = new System.Drawing.Size(405, 100);
            this.colorPanel_pnl.TabIndex = 1;
            this.colorPanel_pnl.Click += new System.EventHandler(this.colorPanel_pnl_Click);
            this.colorPanel_pnl.Paint += new System.Windows.Forms.PaintEventHandler(this.colorPanel_pnl_Paint);
            // 
            // pick_btn
            // 
            this.pick_btn.AutoSize = true;
            this.pick_btn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.pick_btn.BackColor = System.Drawing.Color.Transparent;
            this.pick_btn.Depth = 0;
            this.pick_btn.Icon = null;
            this.pick_btn.Location = new System.Drawing.Point(366, 176);
            this.pick_btn.MouseState = MaterialSkin.MouseState.HOVER;
            this.pick_btn.Name = "pick_btn";
            this.pick_btn.Primary = true;
            this.pick_btn.Size = new System.Drawing.Size(51, 36);
            this.pick_btn.TabIndex = 3;
            this.pick_btn.Text = "pick";
            this.pick_btn.UseVisualStyleBackColor = false;
            this.pick_btn.Click += new System.EventHandler(this.pick_btn_Click);
            this.pick_btn.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "cPick";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::CPickX.Properties.Resources.ic_color_lens_white_48dp;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(396, 28);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Location = new System.Drawing.Point(15, 73);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(405, 100);
            this.panel1.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(431, 218);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.pick_btn);
            this.Controls.Add(this.colorPanel_pnl);
            this.Controls.Add(this.color_lbl);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(431, 218);
            this.MinimumSize = new System.Drawing.Size(431, 218);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "cPick";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.VisibleChanged += new System.EventHandler(this.Form1_VisibleChanged);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.MouseEnter += new System.EventHandler(this.Form1_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.Form1_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer Update_timer;
        private MaterialSkin.Controls.MaterialLabel color_lbl;
        private System.Windows.Forms.Panel colorPanel_pnl;
        private MaterialSkin.Controls.MaterialRaisedButton pick_btn;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
    }
}

