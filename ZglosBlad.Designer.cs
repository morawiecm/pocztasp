namespace SIPS
{
    partial class ZglosBlad
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnWyslij = new System.Windows.Forms.Button();
            this.btnZamknij = new System.Windows.Forms.Button();
            this.txtbTemat = new System.Windows.Forms.TextBox();
            this.rtxbZgloszenie = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tytuł:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Opis:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label3.Location = new System.Drawing.Point(146, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(240, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Zgłaszanie błędów programu";
            // 
            // btnWyslij
            // 
            this.btnWyslij.Location = new System.Drawing.Point(53, 299);
            this.btnWyslij.Name = "btnWyslij";
            this.btnWyslij.Size = new System.Drawing.Size(150, 23);
            this.btnWyslij.TabIndex = 3;
            this.btnWyslij.Text = "Wyślij zgłoszenie";
            this.btnWyslij.UseVisualStyleBackColor = true;
            this.btnWyslij.Click += new System.EventHandler(this.btnWyslij_Click);
            // 
            // btnZamknij
            // 
            this.btnZamknij.Location = new System.Drawing.Point(397, 299);
            this.btnZamknij.Name = "btnZamknij";
            this.btnZamknij.Size = new System.Drawing.Size(150, 23);
            this.btnZamknij.TabIndex = 4;
            this.btnZamknij.Text = "Zamknij";
            this.btnZamknij.UseVisualStyleBackColor = true;
            this.btnZamknij.Click += new System.EventHandler(this.btnZamknij_Click);
            // 
            // txtbTemat
            // 
            this.txtbTemat.Location = new System.Drawing.Point(53, 57);
            this.txtbTemat.Name = "txtbTemat";
            this.txtbTemat.Size = new System.Drawing.Size(494, 20);
            this.txtbTemat.TabIndex = 5;
            // 
            // rtxbZgloszenie
            // 
            this.rtxbZgloszenie.Location = new System.Drawing.Point(53, 83);
            this.rtxbZgloszenie.Name = "rtxbZgloszenie";
            this.rtxbZgloszenie.Size = new System.Drawing.Size(494, 197);
            this.rtxbZgloszenie.TabIndex = 6;
            this.rtxbZgloszenie.Text = "Tutaj wpisujemy opis problemu , jak do niego doszło, i przy jakich danych wprowad" +
    "zonych";
            // 
            // ZglosBlad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 337);
            this.Controls.Add(this.rtxbZgloszenie);
            this.Controls.Add(this.txtbTemat);
            this.Controls.Add(this.btnZamknij);
            this.Controls.Add(this.btnWyslij);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.Name = "ZglosBlad";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Zgłaszanie Błedów";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnWyslij;
        private System.Windows.Forms.Button btnZamknij;
        private System.Windows.Forms.TextBox txtbTemat;
        private System.Windows.Forms.RichTextBox rtxbZgloszenie;
    }
}