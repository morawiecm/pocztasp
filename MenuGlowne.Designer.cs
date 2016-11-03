namespace SIPS
{
    partial class MenuGlowne
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.lblUzytkownik = new System.Windows.Forms.Label();
            this.lblStatusWniosku = new System.Windows.Forms.Label();
            this.lblHasloWygasa = new System.Windows.Forms.Label();
            this.lblZglosBlad = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button1.Location = new System.Drawing.Point(44, 101);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(242, 120);
            this.button1.TabIndex = 0;
            this.button1.Text = "RESORT";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Arial", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button2.Location = new System.Drawing.Point(445, 101);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(242, 120);
            this.button2.TabIndex = 1;
            this.button2.Text = "POCZTA POLSKA";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button5.Location = new System.Drawing.Point(548, 10);
            this.button5.Margin = new System.Windows.Forms.Padding(2);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(124, 48);
            this.button5.TabIndex = 4;
            this.button5.Text = "Zamknij";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Arial", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button3.Location = new System.Drawing.Point(44, 277);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(242, 120);
            this.button3.TabIndex = 5;
            this.button3.Text = "POCZTA SPECJALNA";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Arial", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button4.Location = new System.Drawing.Point(445, 277);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(242, 120);
            this.button4.TabIndex = 6;
            this.button4.Text = "STATYSTYKI";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // lblUzytkownik
            // 
            this.lblUzytkownik.AutoSize = true;
            this.lblUzytkownik.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblUzytkownik.Location = new System.Drawing.Point(14, 23);
            this.lblUzytkownik.Name = "lblUzytkownik";
            this.lblUzytkownik.Size = new System.Drawing.Size(95, 18);
            this.lblUzytkownik.TabIndex = 7;
            this.lblUzytkownik.Text = "Użytkownik";
            // 
            // lblStatusWniosku
            // 
            this.lblStatusWniosku.AutoSize = true;
            this.lblStatusWniosku.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblStatusWniosku.Location = new System.Drawing.Point(12, 57);
            this.lblStatusWniosku.Name = "lblStatusWniosku";
            this.lblStatusWniosku.Size = new System.Drawing.Size(131, 20);
            this.lblStatusWniosku.TabIndex = 8;
            this.lblStatusWniosku.Text = "Status wniosku";
            // 
            // lblHasloWygasa
            // 
            this.lblHasloWygasa.AutoSize = true;
            this.lblHasloWygasa.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblHasloWygasa.Location = new System.Drawing.Point(327, 57);
            this.lblHasloWygasa.Name = "lblHasloWygasa";
            this.lblHasloWygasa.Size = new System.Drawing.Size(149, 20);
            this.lblHasloWygasa.TabIndex = 9;
            this.lblHasloWygasa.Text = "Hasło wygasa za:";
            // 
            // lblZglosBlad
            // 
            this.lblZglosBlad.AutoSize = true;
            this.lblZglosBlad.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblZglosBlad.Location = new System.Drawing.Point(603, 408);
            this.lblZglosBlad.Name = "lblZglosBlad";
            this.lblZglosBlad.Size = new System.Drawing.Size(79, 16);
            this.lblZglosBlad.TabIndex = 10;
            this.lblZglosBlad.TabStop = true;
            this.lblZglosBlad.Text = "Zgłoś Błąd";
            this.lblZglosBlad.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblZglosBlad_LinkClicked);
            // 
            // MenuGlowne
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 443);
            this.Controls.Add(this.lblZglosBlad);
            this.Controls.Add(this.lblHasloWygasa);
            this.Controls.Add(this.lblStatusWniosku);
            this.Controls.Add(this.lblUzytkownik);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MenuGlowne";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " Menu";
            this.Load += new System.EventHandler(this.MenuGlowne_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label lblUzytkownik;
        private System.Windows.Forms.Label lblStatusWniosku;
        private System.Windows.Forms.Label lblHasloWygasa;
        private System.Windows.Forms.LinkLabel lblZglosBlad;
    }
}