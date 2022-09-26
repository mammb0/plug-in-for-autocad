namespace AutoCAD_ASPZ
{
    partial class Form_ASPZ_Pj
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
            this.components = new System.ComponentModel.Container();
            this.DS_RepBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.DataSetSpecification = new AutoCAD_ASPZ.DataSetSpecification();
            ((System.ComponentModel.ISupportInitialize)(this.DS_RepBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataSetSpecification)).BeginInit();
            this.SuspendLayout();
            // 
            // DS_RepBindingSource
            // 
            this.DS_RepBindingSource.DataMember = "DS_Rep";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(16, 15);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(4);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(678, 748);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // DataSetSpecification
            // 
            this.DataSetSpecification.DataSetName = "DataSetSpecification";
            this.DataSetSpecification.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // Form_ASPZ_Pj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(707, 776);
            this.Controls.Add(this.richTextBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form_ASPZ_Pj";
            this.Text = "Form_ASPZ_Pj";
            this.Load += new System.EventHandler(this.Form_ASPZ_Pj_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DS_RepBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataSetSpecification)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.BindingSource DS_RepBindingSource;
        private DataSetSpecification DataSetSpecification;
    }
}