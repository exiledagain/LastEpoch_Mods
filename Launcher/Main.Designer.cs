namespace Launcher
{
    partial class Main
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.btn_online = new System.Windows.Forms.Button();
            this.btn_offline = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_online
            // 
            this.btn_online.Location = new System.Drawing.Point(12, 22);
            this.btn_online.Name = "btn_online";
            this.btn_online.Size = new System.Drawing.Size(366, 125);
            this.btn_online.TabIndex = 0;
            this.btn_online.Text = "Online";
            this.btn_online.UseVisualStyleBackColor = true;
            this.btn_online.Click += new System.EventHandler(this.btn_online_Click);
            // 
            // btn_offline
            // 
            this.btn_offline.Location = new System.Drawing.Point(395, 22);
            this.btn_offline.Name = "btn_offline";
            this.btn_offline.Size = new System.Drawing.Size(366, 125);
            this.btn_offline.TabIndex = 1;
            this.btn_offline.Text = "Offline";
            this.btn_offline.UseVisualStyleBackColor = true;
            this.btn_offline.Click += new System.EventHandler(this.btn_offline_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(19F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 165);
            this.Controls.Add(this.btn_offline);
            this.Controls.Add(this.btn_online);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Text = "Last Epoch Launcher";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_online;
        private System.Windows.Forms.Button btn_offline;
    }
}

