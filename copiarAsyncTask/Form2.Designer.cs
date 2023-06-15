namespace copiarAsyncTask
{
    partial class Form2
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
            btnIniciarCopia = new Button();
            progressBar = new ProgressBar();
            bgWorker = new System.ComponentModel.BackgroundWorker();
            button1 = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // btnIniciarCopia
            // 
            btnIniciarCopia.Location = new Point(31, 14);
            btnIniciarCopia.Name = "btnIniciarCopia";
            btnIniciarCopia.Size = new Size(202, 104);
            btnIniciarCopia.TabIndex = 0;
            btnIniciarCopia.Text = "Copiar";
            btnIniciarCopia.UseVisualStyleBackColor = true;
            btnIniciarCopia.Click += btnIniciarCopia_Click;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(34, 130);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(470, 23);
            progressBar.TabIndex = 1;
            // 
            // bgWorker
            // 
            bgWorker.WorkerReportsProgress = true;
            bgWorker.WorkerSupportsCancellation = true;
            bgWorker.DoWork += Worker_DoWork;
            bgWorker.ProgressChanged += Worker_ProgressChanged;
            bgWorker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            // 
            // button1
            // 
            button1.BackColor = Color.Wheat;
            button1.Location = new Point(263, 28);
            button1.Name = "button1";
            button1.Size = new Size(241, 76);
            button1.TabIndex = 2;
            button1.Text = "NO BLOQUEO UI";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 14F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(34, 169);
            label1.Name = "label1";
            label1.Size = new Size(22, 25);
            label1.TabIndex = 3;
            label1.Text = "0";
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(524, 209);
            Controls.Add(label1);
            Controls.Add(button1);
            Controls.Add(progressBar);
            Controls.Add(btnIniciarCopia);
            Name = "Form2";
            Text = "Form2";
            Load += Form2_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnIniciarCopia;
        private ProgressBar progressBar;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private Button button1;
        private Label label1;
    }
}