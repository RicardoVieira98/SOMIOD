namespace CreateApplicationExample
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
            this.CreateAppBtn = new System.Windows.Forms.Button();
            this.CreateAppResult = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.CreateContainerResult = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // CreateAppBtn
            // 
            this.CreateAppBtn.Location = new System.Drawing.Point(84, 99);
            this.CreateAppBtn.Name = "CreateAppBtn";
            this.CreateAppBtn.Size = new System.Drawing.Size(210, 66);
            this.CreateAppBtn.TabIndex = 0;
            this.CreateAppBtn.Text = "Create Lighting Application";
            this.CreateAppBtn.UseVisualStyleBackColor = true;
            this.CreateAppBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // CreateAppResult
            // 
            this.CreateAppResult.AutoSize = true;
            this.CreateAppResult.Enabled = false;
            this.CreateAppResult.Location = new System.Drawing.Point(398, 124);
            this.CreateAppResult.Name = "CreateAppResult";
            this.CreateAppResult.Size = new System.Drawing.Size(0, 16);
            this.CreateAppResult.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(84, 250);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(210, 66);
            this.button2.TabIndex = 2;
            this.button2.Text = "Create Lighting Container";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // CreateContainerResult
            // 
            this.CreateContainerResult.AutoSize = true;
            this.CreateContainerResult.Enabled = false;
            this.CreateContainerResult.Location = new System.Drawing.Point(398, 275);
            this.CreateContainerResult.Name = "CreateContainerResult";
            this.CreateContainerResult.Size = new System.Drawing.Size(0, 16);
            this.CreateContainerResult.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.CreateContainerResult);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.CreateAppResult);
            this.Controls.Add(this.CreateAppBtn);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CreateAppBtn;
        private System.Windows.Forms.Label CreateAppResult;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label CreateContainerResult;
    }
}

