//namespace AgGPS
//{
//    partial class Form1
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
//            this.label1 = new System.Windows.Forms.Label();
//            this.label2 = new System.Windows.Forms.Label();
//            this.buttonIsoXml = new System.Windows.Forms.Button();
//            this.buttonAgGPS = new System.Windows.Forms.Button();
//            this.textBoxIsoXml = new System.Windows.Forms.TextBox();
//            this.textBoxAgGPS = new System.Windows.Forms.TextBox();
//            this.buttonConvert = new System.Windows.Forms.Button();
//            this.SuspendLayout();
//            // 
//            // label1
//            // 
//            this.label1.AutoSize = true;
//            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
//            this.label1.Location = new System.Drawing.Point(12, 9);
//            this.label1.Name = "label1";
//            this.label1.Size = new System.Drawing.Size(58, 20);
//            this.label1.TabIndex = 0;
//            this.label1.Text = "IsoXml";
//            // 
//            // label2
//            // 
//            this.label2.AutoSize = true;
//            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
//            this.label2.Location = new System.Drawing.Point(12, 40);
//            this.label2.Name = "label2";
//            this.label2.Size = new System.Drawing.Size(63, 20);
//            this.label2.TabIndex = 1;
//            this.label2.Text = "AgGPS";
//            // 
//            // buttonIsoXml
//            // 
//            this.buttonIsoXml.Image = global::AgGPS.Properties.Resources.FOLDER;
//            this.buttonIsoXml.Location = new System.Drawing.Point(76, 6);
//            this.buttonIsoXml.Name = "buttonIsoXml";
//            this.buttonIsoXml.Size = new System.Drawing.Size(23, 23);
//            this.buttonIsoXml.TabIndex = 2;
//            this.buttonIsoXml.UseVisualStyleBackColor = true;
//            this.buttonIsoXml.Click += new System.EventHandler(this.buttonIsoXml_Click);
//            // 
//            // buttonAgGPS
//            // 
//            this.buttonAgGPS.Image = global::AgGPS.Properties.Resources.FOLDER;
//            this.buttonAgGPS.Location = new System.Drawing.Point(76, 37);
//            this.buttonAgGPS.Name = "buttonAgGPS";
//            this.buttonAgGPS.Size = new System.Drawing.Size(23, 23);
//            this.buttonAgGPS.TabIndex = 3;
//            this.buttonAgGPS.UseVisualStyleBackColor = true;
//            this.buttonAgGPS.Click += new System.EventHandler(this.buttonAgGPS_Click);
//            // 
//            // textBoxIsoXml
//            // 
//            this.textBoxIsoXml.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
//            this.textBoxIsoXml.Location = new System.Drawing.Point(105, 7);
//            this.textBoxIsoXml.Name = "textBoxIsoXml";
//            this.textBoxIsoXml.ReadOnly = true;
//            this.textBoxIsoXml.Size = new System.Drawing.Size(543, 22);
//            this.textBoxIsoXml.TabIndex = 4;
//            this.textBoxIsoXml.Text = "...";
//            // 
//            // textBoxAgGPS
//            // 
//            this.textBoxAgGPS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
//            this.textBoxAgGPS.Location = new System.Drawing.Point(105, 38);
//            this.textBoxAgGPS.Name = "textBoxAgGPS";
//            this.textBoxAgGPS.ReadOnly = true;
//            this.textBoxAgGPS.Size = new System.Drawing.Size(543, 22);
//            this.textBoxAgGPS.TabIndex = 5;
//            this.textBoxAgGPS.Text = "...";
//            // 
//            // buttonConvert
//            // 
//            this.buttonConvert.Enabled = false;
//            this.buttonConvert.Location = new System.Drawing.Point(16, 66);
//            this.buttonConvert.Name = "buttonConvert";
//            this.buttonConvert.Size = new System.Drawing.Size(75, 23);
//            this.buttonConvert.TabIndex = 6;
//            this.buttonConvert.Text = "-->";
//            this.buttonConvert.UseVisualStyleBackColor = true;
//            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
//            // 
//            // Form1
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(654, 97);
//            this.Controls.Add(this.buttonConvert);
//            this.Controls.Add(this.textBoxAgGPS);
//            this.Controls.Add(this.textBoxIsoXml);
//            this.Controls.Add(this.buttonAgGPS);
//            this.Controls.Add(this.buttonIsoXml);
//            this.Controls.Add(this.label2);
//            this.Controls.Add(this.label1);
//            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
//            this.Name = "Form1";
//            this.Text = "IsoXml -> AgGPS";
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private System.Windows.Forms.Label label1;
//        private System.Windows.Forms.Label label2;
//        private System.Windows.Forms.Button buttonIsoXml;
//        private System.Windows.Forms.Button buttonAgGPS;
//        private System.Windows.Forms.TextBox textBoxIsoXml;
//        private System.Windows.Forms.TextBox textBoxAgGPS;
//        private System.Windows.Forms.Button buttonConvert;
//    }
//}

