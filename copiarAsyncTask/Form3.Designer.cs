namespace copiarAsyncTask
{
    partial class Form3
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
            button1 = new Button();
            button2 = new Button();
            progressBar1 = new ProgressBar();
            label1 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(145, 85);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.Salmon;
            button2.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point);
            button2.Location = new Point(347, 12);
            button2.Name = "button2";
            button2.Size = new Size(145, 85);
            button2.TabIndex = 0;
            button2.Text = "NO BLOCKED UI";
            button2.UseVisualStyleBackColor = false;
            button2.Click += Button2_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 116);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(480, 23);
            progressBar1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(246, 157);
            label1.Name = "label1";
            label1.Size = new Size(22, 25);
            label1.TabIndex = 2;
            label1.Text = "0";
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(504, 218);
            Controls.Add(label1);
            Controls.Add(progressBar1);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form3";
            Text = "Form3";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private Button button2;
        private ProgressBar progressBar1;
        private Label label1;
    }
}