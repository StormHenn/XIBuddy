using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TimeTracker
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            // Header Start =====================================================================================================================
            Panel header = new Panel();
            header.Width = 430;
            header.Height = 40;
            header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            header.Padding = new Padding(0);
            header.Margin = new Padding(0);

            Button Exit = new Button();
            Exit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            Exit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            Image exitImage = Image.FromFile("exitButton.png");
            Image exitThumbnail = exitImage.GetThumbnailImage(18, 18, null, IntPtr.Zero);
            Exit.BackgroundImage = exitThumbnail;
            Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            Exit.FlatAppearance.BorderSize = 0;
            Exit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            Exit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            Exit.Location = new System.Drawing.Point(390, 0);
            Exit.Size = new System.Drawing.Size(40, 40);
            Exit.Click += new System.EventHandler(this.Exit_Click);
            header.Controls.Add(Exit);
            this.Controls.Add(header);
            // Header End =====================================================================================================================

            // Body Start =====================================================================================================================
            Panel loginBody = new Panel();
            loginBody.Size = new Size(this.Width, this.Height);
            loginBody.BackColor = System.Drawing.Color.White;
            loginBody.Dock = DockStyle.Fill;

            Label heading = new Label();
            heading.Text = "XIB Portal";
            heading.Size = new System.Drawing.Size(200, 39);
            heading.Font = new System.Drawing.Font("Palatino Linotype", 21F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            heading.ForeColor = System.Drawing.Color.Orange;
            heading.Location = new Point((loginBody.Width - heading.Width) / 2, header.Bottom + 65);
            heading.TextAlign = ContentAlignment.MiddleCenter;
            loginBody.Controls.Add(heading);

            Label loginMessage = new Label();
            loginMessage.Text = "Log in to start tracking your time and connect to the XIB platform!";
            loginMessage.Size = new System.Drawing.Size(340, 80);
            loginMessage.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            loginMessage.ForeColor = System.Drawing.Color.Gray;
            loginMessage.Location = new Point((loginBody.Width - loginMessage.Width) / 2, heading.Bottom + 10);
            loginMessage.TextAlign = ContentAlignment.MiddleCenter;
            loginBody.Controls.Add(loginMessage);

            Button loginButton = new Button();
            loginButton.Text = "Login";
            loginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            loginButton.FlatAppearance.BorderSize = 0;
            loginButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            loginButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            loginButton.Size = new System.Drawing.Size(330, 40);
            loginButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            loginButton.ForeColor = System.Drawing.Color.White;
            loginButton.Location = new Point((loginBody.Width - loginMessage.Width) / 2, loginMessage.Bottom + 10);
            loginButton.TextAlign = ContentAlignment.MiddleCenter;
            loginButton.Click += new System.EventHandler(this.Login_Click);
            loginBody.Controls.Add(loginButton);

            LinkLabel registerMessage = new LinkLabel();
            registerMessage.Text = "New here? Create an account";
            registerMessage.LinkArea = new LinkArea(10, 17);
            registerMessage.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            registerMessage.LinkBehavior = LinkBehavior.NeverUnderline;
            registerMessage.LinkClicked += new LinkLabelLinkClickedEventHandler(registerMessage_LinkClicked);
            registerMessage.Size = new System.Drawing.Size(300, 80);
            registerMessage.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            registerMessage.ForeColor = System.Drawing.Color.Gray;
            registerMessage.Location = new Point((loginBody.Width - registerMessage.Width) / 2, loginButton.Bottom);
            registerMessage.TextAlign = ContentAlignment.MiddleCenter;
            loginBody.Controls.Add(registerMessage);

            this.Controls.Add(loginBody);
            // Body End =====================================================================================================================
        }

        //Round the forms corners
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidthEllipse, int nHeightEllipse);
        
        private void registerMessage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //redirect to register page
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Login_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            this.Hide();
        }

    }
}
