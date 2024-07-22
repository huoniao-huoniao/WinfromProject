namespace PrintClient
{
    partial class DialogFastReportDesign<TList, TEntity>
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
            this.designerControl1 = new FastReport.Design.StandardDesigner.DesignerControl();
            ((System.ComponentModel.ISupportInitialize)(this.designerControl1)).BeginInit();
            this.SuspendLayout();
            // 
            // designerControl1
            // 
            this.designerControl1.AskSave = true;
            this.designerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.designerControl1.LayoutState = resources.GetString("designerControl1.LayoutState");
            this.designerControl1.Location = new System.Drawing.Point(0, 0);
            this.designerControl1.Name = "designerControl1";
            this.designerControl1.Size = new System.Drawing.Size(853, 433);
            this.designerControl1.TabIndex = 0;
            // 
            // DialogFastReportDesign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 433);
            this.Controls.Add(this.designerControl1);
            //this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DialogFastReportDesign";
            this.Text = "设计报表";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DialogFastReportDesign_FormClosed);
            this.Load += new System.EventHandler(this.DialogFastReportDesign_Load);
            ((System.ComponentModel.ISupportInitialize)(this.designerControl1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FastReport.Design.StandardDesigner.DesignerControl designerControl1;
    }
}