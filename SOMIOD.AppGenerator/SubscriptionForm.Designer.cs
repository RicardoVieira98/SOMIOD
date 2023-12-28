namespace SOMIOD.AppGenerator
{
    partial class SubscriptionForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.GetSub = new System.Windows.Forms.Label();
            this.containers = new System.Windows.Forms.ListBox();
            this.subscriptions = new System.Windows.Forms.ListBox();
            this.allSubs = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // applications
            // 
            this.applications.FormattingEnabled = true;
            this.applications.ItemHeight = 16;
            this.applications.Location = new System.Drawing.Point(47, 60);
            this.applications.Name = "applications";
            this.applications.Size = new System.Drawing.Size(120, 84);
            this.applications.TabIndex = 1;
            this.applications.SelectedIndexChanged += new System.EventHandler(this.applications_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(47, 205);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 32);
            this.button1.TabIndex = 2;
            this.button1.Text = "Get Subscriptions";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // GetSub
            // 
            this.GetSub.AutoSize = true;
            this.GetSub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GetSub.Location = new System.Drawing.Point(304, 60);
            this.GetSub.Name = "GetSub";
            this.GetSub.Size = new System.Drawing.Size(0, 16);
            this.GetSub.TabIndex = 3;
            // 
            // containers
            // 
            this.containers.FormattingEnabled = true;
            this.containers.ItemHeight = 16;
            this.containers.Location = new System.Drawing.Point(198, 60);
            this.containers.Name = "containers";
            this.containers.Size = new System.Drawing.Size(120, 84);
            this.containers.TabIndex = 4;
            // 
            // subscriptions
            // 
            this.subscriptions.Enabled = false;
            this.subscriptions.FormattingEnabled = true;
            this.subscriptions.ItemHeight = 16;
            this.subscriptions.Location = new System.Drawing.Point(345, 60);
            this.subscriptions.Name = "subscriptions";
            this.subscriptions.Size = new System.Drawing.Size(120, 84);
            this.subscriptions.TabIndex = 5;
            // 
            // allSubs
            // 
            this.allSubs.Enabled = false;
            this.allSubs.FormattingEnabled = true;
            this.allSubs.ItemHeight = 16;
            this.allSubs.Location = new System.Drawing.Point(47, 243);
            this.allSubs.Name = "allSubs";
            this.allSubs.Size = new System.Drawing.Size(140, 196);
            this.allSubs.TabIndex = 6;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(286, 267);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(179, 55);
            this.button2.TabIndex = 7;
            this.button2.Text = "Create Subscription";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(286, 357);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(179, 56);
            this.button4.TabIndex = 9;
            this.button4.Text = "Delete Subscription";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // SubscriptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(491, 482);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.allSubs);
            this.Controls.Add(this.subscriptions);
            this.Controls.Add(this.containers);
            this.Controls.Add(this.GetSub);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.applications);
            this.Name = "SubscriptionForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ListBox applications;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label GetSub;
        private System.Windows.Forms.ListBox containers;
        private System.Windows.Forms.ListBox subscriptions;
        private System.Windows.Forms.ListBox allSubs;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button4;
    }
}

