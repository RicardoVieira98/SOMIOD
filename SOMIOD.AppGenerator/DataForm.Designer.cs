namespace SOMIOD.AppGenerator
{
    partial class DataForm
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
            this.applications = new System.Windows.Forms.ListBox();
            this.containers = new System.Windows.Forms.ListBox();
            this.subscriptions = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.allDatas = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // applications
            // 
            this.applications.FormattingEnabled = true;
            this.applications.Location = new System.Drawing.Point(11, 33);
            this.applications.Margin = new System.Windows.Forms.Padding(2);
            this.applications.Name = "applications";
            this.applications.Size = new System.Drawing.Size(91, 69);
            this.applications.TabIndex = 2;
            this.applications.SelectedIndexChanged += new System.EventHandler(this.applications_SelectedIndexChanged);
            // 
            // containers
            // 
            this.containers.FormattingEnabled = true;
            this.containers.Location = new System.Drawing.Point(122, 33);
            this.containers.Margin = new System.Windows.Forms.Padding(2);
            this.containers.Name = "containers";
            this.containers.Size = new System.Drawing.Size(91, 69);
            this.containers.TabIndex = 5;
            this.containers.SelectedIndexChanged += new System.EventHandler(this.containers_SelectedIndexChanged);
            // 
            // subscriptions
            // 
            this.subscriptions.Enabled = false;
            this.subscriptions.FormattingEnabled = true;
            this.subscriptions.Location = new System.Drawing.Point(229, 33);
            this.subscriptions.Margin = new System.Windows.Forms.Padding(2);
            this.subscriptions.Name = "subscriptions";
            this.subscriptions.Size = new System.Drawing.Size(91, 69);
            this.subscriptions.TabIndex = 6;
            this.subscriptions.SelectedIndexChanged += new System.EventHandler(this.subscriptions_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 142);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 26);
            this.button1.TabIndex = 7;
            this.button1.Text = "Get Subscriptions";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // allDatas
            // 
            this.allDatas.FormattingEnabled = true;
            this.allDatas.Location = new System.Drawing.Point(11, 183);
            this.allDatas.Margin = new System.Windows.Forms.Padding(2);
            this.allDatas.Name = "allDatas";
            this.allDatas.Size = new System.Drawing.Size(129, 160);
            this.allDatas.TabIndex = 8;
            this.allDatas.SelectedIndexChanged += new System.EventHandler(this.allDatas_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(160, 183);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(134, 45);
            this.button2.TabIndex = 9;
            this.button2.Text = "Create Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(160, 254);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(134, 46);
            this.button4.TabIndex = 10;
            this.button4.Text = "Delete Data";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(11, 380);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(74, 24);
            this.button5.TabIndex = 11;
            this.button5.Text = "Voltar";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // DataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 416);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.allDatas);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.subscriptions);
            this.Controls.Add(this.containers);
            this.Controls.Add(this.applications);
            this.Name = "DataForm";
            this.Text = "DataForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox applications;
        private System.Windows.Forms.ListBox containers;
        private System.Windows.Forms.ListBox subscriptions;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox allDatas;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
    }
}