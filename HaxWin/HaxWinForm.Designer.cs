﻿/*
HaxWin - lightweight xulrunner based browser for play haxball
Copyright (C) 2016  Oskari Pöntinen <mail.morko@gmail.com>

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/
namespace HaxWin
{
    partial class HaxWinForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HaxWinForm));
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.urlTextBox = new System.Windows.Forms.TextBox();
            this.goButton = new System.Windows.Forms.Button();
            this.autoJoinButton = new System.Windows.Forms.CheckBox();
            this.alwaysOnTopButton = new System.Windows.Forms.CheckBox();
            this.browser = new Skybound.Gecko.GeckoWebBrowser();
            this.chatOverlayPanel = new System.Windows.Forms.Panel();
            this.chatOverlayButton = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.flowLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.flowLayoutPanel1.Controls.Add(this.label1);
            this.flowLayoutPanel1.Controls.Add(this.urlTextBox);
            this.flowLayoutPanel1.Controls.Add(this.goButton);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(0, 5, 0, 5);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(625, 36);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 26);
            this.label1.TabIndex = 0;
            this.label1.Text = "URL";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // urlTextBox
            // 
            this.urlTextBox.Location = new System.Drawing.Point(65, 8);
            this.urlTextBox.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.urlTextBox.Name = "urlTextBox";
            this.urlTextBox.Size = new System.Drawing.Size(524, 20);
            this.urlTextBox.TabIndex = 1;
            this.urlTextBox.DoubleClick += new System.EventHandler(this.urlTextBox_DoubleClick);
            this.urlTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.urlTextBox_KeyPress);
            // 
            // goButton
            // 
            this.goButton.Location = new System.Drawing.Point(589, 7);
            this.goButton.Margin = new System.Windows.Forms.Padding(0, 2, 0, 3);
            this.goButton.Name = "goButton";
            this.goButton.Size = new System.Drawing.Size(30, 22);
            this.goButton.TabIndex = 2;
            this.goButton.Text = "Go";
            this.goButton.UseVisualStyleBackColor = true;
            this.goButton.Click += new System.EventHandler(this.goButton_Click);
            // 
            // autoJoinButton
            // 
            this.autoJoinButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.autoJoinButton.AutoSize = true;
            this.autoJoinButton.BackColor = System.Drawing.Color.FloralWhite;
            this.autoJoinButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.LawnGreen;
            this.autoJoinButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.autoJoinButton.Location = new System.Drawing.Point(3, 6);
            this.autoJoinButton.Name = "autoJoinButton";
            this.autoJoinButton.Size = new System.Drawing.Size(58, 23);
            this.autoJoinButton.TabIndex = 4;
            this.autoJoinButton.Text = "AutoJoin";
            this.autoJoinButton.UseVisualStyleBackColor = false;
            this.autoJoinButton.CheckedChanged += new System.EventHandler(this.autoJoinButton_CheckedChanged);
            // 
            // alwaysOnTopButton
            // 
            this.alwaysOnTopButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.alwaysOnTopButton.AutoSize = true;
            this.alwaysOnTopButton.BackColor = System.Drawing.Color.FloralWhite;
            this.alwaysOnTopButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.LawnGreen;
            this.alwaysOnTopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.alwaysOnTopButton.Location = new System.Drawing.Point(67, 6);
            this.alwaysOnTopButton.Name = "alwaysOnTopButton";
            this.alwaysOnTopButton.Size = new System.Drawing.Size(89, 23);
            this.alwaysOnTopButton.TabIndex = 5;
            this.alwaysOnTopButton.Text = "Always On Top";
            this.alwaysOnTopButton.UseVisualStyleBackColor = false;
            this.alwaysOnTopButton.CheckedChanged += new System.EventHandler(this.alwaysOnTopButton_CheckedChanged);
            // 
            // browser
            // 
            this.browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.browser.Location = new System.Drawing.Point(-30, -11);
            this.browser.Name = "browser";
            this.browser.Size = new System.Drawing.Size(1085, 772);
            this.browser.TabIndex = 2;
            // 
            // chatOverlayPanel
            // 
            this.chatOverlayPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.chatOverlayPanel.Location = new System.Drawing.Point(0, 550);
            this.chatOverlayPanel.Name = "chatOverlayPanel";
            this.chatOverlayPanel.Size = new System.Drawing.Size(740, 146);
            this.chatOverlayPanel.TabIndex = 3;
            this.chatOverlayPanel.Visible = false;
            // 
            // chatOverlayButton
            // 
            this.chatOverlayButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.chatOverlayButton.AutoSize = true;
            this.chatOverlayButton.BackColor = System.Drawing.Color.FloralWhite;
            this.chatOverlayButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.LawnGreen;
            this.chatOverlayButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chatOverlayButton.Location = new System.Drawing.Point(162, 6);
            this.chatOverlayButton.Name = "chatOverlayButton";
            this.chatOverlayButton.Size = new System.Drawing.Size(78, 23);
            this.chatOverlayButton.TabIndex = 5;
            this.chatOverlayButton.Text = "Chat Overlay";
            this.chatOverlayButton.UseVisualStyleBackColor = false;
            this.chatOverlayButton.CheckedChanged += new System.EventHandler(this.chatOverlayButton_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.panel1.Controls.Add(this.chatOverlayButton);
            this.panel1.Controls.Add(this.autoJoinButton);
            this.panel1.Controls.Add(this.alwaysOnTopButton);
            this.panel1.Location = new System.Drawing.Point(622, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(243, 36);
            this.panel1.TabIndex = 3;
            // 
            // HaxWinForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(147)))), ((int)(((byte)(158)))), ((int)(((byte)(127)))));
            this.ClientSize = new System.Drawing.Size(864, 696);
            this.Controls.Add(this.chatOverlayPanel);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.browser);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HaxWinForm";
            this.Text = "HaxWin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HaxWinForm_FormClosing);
            this.Load += new System.EventHandler(this.HaxWin_Load);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox urlTextBox;
        private System.Windows.Forms.Button goButton;
        private Skybound.Gecko.GeckoWebBrowser browser;
        private System.Windows.Forms.CheckBox autoJoinButton;
        private System.Windows.Forms.CheckBox alwaysOnTopButton;
        private System.Windows.Forms.Panel chatOverlayPanel;
        private System.Windows.Forms.CheckBox chatOverlayButton;
        private System.Windows.Forms.Panel panel1;
    }
}
