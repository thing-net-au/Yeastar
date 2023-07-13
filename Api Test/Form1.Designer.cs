namespace Api_Test
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            listBox1 = new ListBox();
            tmrRefresh = new System.Windows.Forms.Timer(components);
            button1 = new Button();
            textBox1 = new TextBox();
            comboBox1 = new ComboBox();
            button2 = new Button();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            button3 = new Button();
            button4 = new Button();
            SuspendLayout();
            // 
            // listBox1
            // 
            listBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 20;
            listBox1.Location = new Point(12, 109);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(776, 324);
            listBox1.TabIndex = 0;
            // 
            // tmrRefresh
            // 
            tmrRefresh.Enabled = true;
            tmrRefresh.Interval = 1000;
            tmrRefresh.Tick += tmrRefresh_Tick;
            // 
            // button1
            // 
            button1.Location = new Point(327, 74);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 1;
            button1.Text = "Set Uncond";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(41, 75);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(263, 27);
            textBox1.TabIndex = 2;
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(41, 41);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(151, 28);
            comboBox1.TabIndex = 3;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // button2
            // 
            button2.Location = new Point(196, 41);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 4;
            button2.Text = "Query";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(526, 17);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(203, 27);
            textBox2.TabIndex = 5;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(530, 50);
            textBox3.Multiline = true;
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(199, 109);
            textBox3.TabIndex = 6;
            // 
            // button3
            // 
            button3.Location = new Point(694, 75);
            button3.Name = "button3";
            button3.Size = new Size(94, 53);
            button3.TabIndex = 7;
            button3.Text = "Send Test SMS";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.Location = new Point(694, 134);
            button4.Name = "button4";
            button4.Size = new Size(94, 53);
            button4.TabIndex = 8;
            button4.Text = "Send Test SMS";
            button4.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(button4);
            Controls.Add(button3);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(button2);
            Controls.Add(comboBox1);
            Controls.Add(textBox1);
            Controls.Add(button1);
            Controls.Add(listBox1);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox listBox1;
        private System.Windows.Forms.Timer tmrRefresh;
        private Button button1;
        private TextBox textBox1;
        private ComboBox comboBox1;
        private Button button2;
        private TextBox textBox2;
        private TextBox textBox3;
        private Button button3;
        private Button button4;
    }
}