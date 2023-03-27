using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using TimeTracker.Properties;
using System.Resources;
using System.Timers;
using static TimeTracker.Form1;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata;
using System.ComponentModel.Design;
using Microsoft.Win32;

namespace TimeTracker
{
    public partial class Form1 : Form
    {
        private DateTime startTime;
        private string ticketDateTime = "stringDate";
        private System.Windows.Forms.Timer timer;
        private bool isTimerRunning = false;
        private System.Windows.Forms.Timer minimizeTimer;
        private bool minState = false;
        NotifyIcon notifyIcon = new NotifyIcon();
        public class Ticket
        {
            public bool MainTicket { get; set; }
            public int TicketId { get; set; }
            public string TicketDate { get; set; }
            public string TicketName { get; set; }
            public string ProjectName { get; set; }
            public string TicketNumber { get; set; }
            public string TicketTime { get; set; }
        }
        List<Ticket> ticketsArray = new List<Ticket>()
        {
            new Ticket()
            {
                MainTicket = true, //The last ticket that was worked on so this will show up in the main panel when the app is opened.
                TicketDate = "Today",
                TicketName = "Fix the location bug",
                ProjectName = "Secumed",
                TicketNumber = "CSB-264",
                TicketTime = "02:15:26",
            },
            new Ticket()
            {
                TicketDate = "Today",
                TicketName = "Train the NLU model",
                ProjectName = "Secumed",
                TicketNumber = "CSB-266",
                TicketTime = "03:52:19",
            },
            new Ticket()
            {
                TicketDate = "Yesterday",
                TicketName = "Implement carousel",
                ProjectName = "Knowledgebase",
                TicketNumber = "KB-172",
                TicketTime = "04:31:46",
            },
            new Ticket()
            {
                TicketDate = "Wednesday, December 8, 2022",
                TicketName = "Build ambulance journey",
                ProjectName = "Secumed",
                TicketNumber = "CSB-328",
                TicketTime = "08:05:09",
            },
            new Ticket()
            {
                TicketDate = "Tuesday, December 7, 2022",
                TicketName = "Content admin fix",
                ProjectName = "Knowledgebase",
                TicketNumber = "KB-224",
                TicketTime = "05:32:43",
            },
            new Ticket()
            {
                TicketDate = "Tuesday, December 7, 2022",
                TicketName = "Create dashboards",
                ProjectName = "Interscope",
                TicketNumber = "ISB-502",
                TicketTime = "03:12:52",
            },
            new Ticket()
            {
                TicketDate = "NotStarted",
                TicketName = "Deploy DRDS to OCP4",
                ProjectName = "Knowledgebase",
                TicketNumber = "KB-289",
                TicketTime = "00:00:00",
            },
            new Ticket()
            {
                TicketDate = "NotStarted",
                TicketName = "DRDS errors in OCP3",
                ProjectName = "Knowledgebase",
                TicketNumber = "KB-290",
                TicketTime = "00:00:00",
            },
            new Ticket()
            {
                TicketDate = "NotStarted",
                TicketName = "Add items to the tier menu",
                ProjectName = "Interscope",
                TicketNumber = "ISB-526",
                TicketTime = "00:00:00",
            },
        };
        
        public Form1()
        {
            //Application setup
            ShowInTaskbar = false;
            Text = "XIBuddy";
            Icon = new Icon("XIBLogo.ico");
            this.TopMost = true;
            this.Padding = new Padding(0);
            notifyIcon.Icon = new System.Drawing.Icon("XIBLogo.ico");
            notifyIcon.Text = "XIBuddy";
            notifyIcon.Visible = true;
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Show", null, Show_Click);
            menu.Items.Add("Hide", null, Hide_Click);
            menu.Items.Add("Close", null, new System.EventHandler((sender, e) => Exit_Click(sender, e, notifyIcon)));
            notifyIcon.ContextMenuStrip = menu;
            InitializeComponent();
            notifyIcon.DoubleClick += notifyIcon_DoubleClick;
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
            //Variables
            minimizeTimer = new System.Windows.Forms.Timer();
            minimizeTimer.Interval = 10;
            minimizeTimer.Tick += new EventHandler(minimizeTimer_Tick);

            //Container for the body of the application
            FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel.WrapContents = false;
            flowLayoutPanel.Dock = DockStyle.Fill;
            flowLayoutPanel.Padding = new Padding(0, 0, 0, 0);
            this.Controls.Add(flowLayoutPanel);

            // Header Start =====================================================================================================================
            Panel header = new Panel();
            header.Width = 382;
            header.Height = 40;
            header.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            header.Padding = new Padding(0);
            header.Margin = new Padding(0);

            Button Refresh = new Button();
            Refresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            Image refreshImage = Image.FromFile("refreshButton.png");
            Image refreshThumbnail = refreshImage.GetThumbnailImage(18, 18, null, IntPtr.Zero);
            Refresh.BackgroundImage = refreshThumbnail;
            Refresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            Refresh.FlatAppearance.BorderSize = 0;
            Refresh.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            Refresh.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            Refresh.Location = new System.Drawing.Point(0, 0);
            Refresh.Size = new System.Drawing.Size(40, 40);
            Refresh.Click += new System.EventHandler(this.Refresh_Click);
            header.Controls.Add(Refresh);

            Button Hide = new Button();
            Hide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            Image hideImage = Image.FromFile("hideButton.png");
            Image hideThumbnail = hideImage.GetThumbnailImage(20, 20, null, IntPtr.Zero);
            Hide.BackgroundImage = hideThumbnail;
            Hide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            Hide.FlatAppearance.BorderSize = 0;
            Hide.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            Hide.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            Hide.Location = new System.Drawing.Point(40, 0);
            Hide.Size = new System.Drawing.Size(40, 40);
            Hide.Click += new System.EventHandler(this.Hide_Click);
            header.Controls.Add(Hide);

            Label heading = new Label();
            heading.Text = "XIBuddy";
            heading.Size = new System.Drawing.Size(120, 39);
            heading.Font = new System.Drawing.Font("Palatino Linotype", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            heading.ForeColor = System.Drawing.Color.White;
            heading.Location = new Point(140, 3);
            heading.SendToBack();
            header.Controls.Add(heading);

            Button Exit = new Button();
            Exit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            Image exitImage = Image.FromFile("exitButton.png");
            Image exitThumbnail = exitImage.GetThumbnailImage(18, 18, null, IntPtr.Zero);
            Exit.BackgroundImage = exitThumbnail;
            Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            Exit.FlatAppearance.BorderSize = 0;
            Exit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            Exit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            Exit.Location = new System.Drawing.Point(339, 0);
            Exit.Size = new System.Drawing.Size(40, 40);
            Exit.Click += new System.EventHandler((sender, e) => Exit_Click(sender, e, notifyIcon));
            header.Controls.Add(Exit);

            Button Minimize = new Button();
            Minimize.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            Image minimizeImage = Image.FromFile("minimizeButton.png");
            Image minimizeThumbnail = minimizeImage.GetThumbnailImage(18, 18, null, IntPtr.Zero);
            Minimize.BackgroundImage = minimizeThumbnail;
            Minimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            Minimize.FlatAppearance.BorderSize = 0;
            Minimize.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            Minimize.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            Minimize.Location = new System.Drawing.Point(300, 0);
            Minimize.Size = new System.Drawing.Size(40, 40);
            Minimize.Click += new System.EventHandler(this.Minimize_Click);
            Minimize.BringToFront();
            header.Controls.Add(Minimize);
            flowLayoutPanel.Controls.Add(header);
            // Header End =====================================================================================================================

            //Container for the historical section of the application
            FlowLayoutPanel historicalPanel = new FlowLayoutPanel();
            historicalPanel.FlowDirection = FlowDirection.TopDown;
            historicalPanel.WrapContents = false;
            historicalPanel.AutoScroll = true;
            historicalPanel.VerticalScroll.Visible = false;
            historicalPanel.HorizontalScroll.Visible = false;
            historicalPanel.Size = new Size(this.Width, this.Height - header.Height - 70);
            
            // mainTicket Start =====================================================================================================================
            Panel mainTicket = new Panel();
            mainTicket.Padding = new Padding(2, 0, 2, 0);
            mainTicket.Width = 372;
            mainTicket.Height = 61;
            mainTicket.BackColor = System.Drawing.Color.White;
            if (ticketsArray[0].MainTicket)
            {
                Label mainTicketNameLabel = new Label();
                mainTicketNameLabel.Name = "mainTicketLabel";
                mainTicketNameLabel.Text = ticketsArray[0].TicketName;
                mainTicketNameLabel.AutoSize = true;
                mainTicketNameLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                mainTicketNameLabel.Location = new Point(10, 10);
                mainTicketNameLabel.Size = new System.Drawing.Size(134, 18);
                mainTicket.Controls.Add(mainTicketNameLabel);

                Label mainProjectNameLabel = new Label();
                mainProjectNameLabel.Name = "mainProjectLabel";
                mainProjectNameLabel.Text = ticketsArray[0].ProjectName + ": " + ticketsArray[0].TicketNumber;
                mainProjectNameLabel.AutoSize = true;
                mainProjectNameLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                mainProjectNameLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
                mainProjectNameLabel.Location = new Point(10, 30);
                mainProjectNameLabel.Size = new System.Drawing.Size(126, 18);
                mainTicket.Controls.Add(mainProjectNameLabel);

                Label mainTicketTimeLabel = new Label();
                mainTicketTimeLabel.Name = "mainTicketTimeLabel";
                mainTicketTimeLabel.Text = ticketsArray[0].TicketTime;
                mainTicketTimeLabel.AutoSize = true;
                mainTicketTimeLabel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                mainTicketTimeLabel.Location = new Point(225, 20);
                mainTicketTimeLabel.Size = new System.Drawing.Size(74, 21);
                mainTicket.Controls.Add(mainTicketTimeLabel);

                Button startStopButton = new Button();
                startStopButton.BackgroundImage = Image.FromFile("startButton.png");
                startStopButton.BackgroundImageLayout = ImageLayout.Stretch;
                startStopButton.FlatAppearance.BorderSize = 0;
                startStopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                startStopButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
                startStopButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
                startStopButton.Location = new Point(305, 13);
                startStopButton.Size = new System.Drawing.Size(36, 36);
                startStopButton.UseVisualStyleBackColor = true;
                startStopButton.Click += (s, e) => startStopButton_Click(s, e, mainTicketTimeLabel, mainTicketNameLabel, historicalPanel, mainProjectNameLabel, mainTicket);
                mainTicket.Controls.Add(startStopButton);

                Button removeCurrentTicketButton = new Button();
                removeCurrentTicketButton.Name = "selectTicketsButton";
                removeCurrentTicketButton.BackgroundImage = Image.FromFile("selectTicketsButton.png");
                removeCurrentTicketButton.BackgroundImageLayout = ImageLayout.Stretch;
                removeCurrentTicketButton.FlatAppearance.BorderSize = 0;
                removeCurrentTicketButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                removeCurrentTicketButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
                removeCurrentTicketButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
                removeCurrentTicketButton.Location = new Point(345, 21);
                removeCurrentTicketButton.Size = new System.Drawing.Size(20, 20);
                removeCurrentTicketButton.UseVisualStyleBackColor = true;
                removeCurrentTicketButton.Click += (s, e) => selectNewTicket_Click(s, e, mainTicket, historicalPanel);
                mainTicket.Controls.Add(removeCurrentTicketButton);
            }
            else 
            {
                ComboBox selectTicketsDropdown = new ComboBox();
                selectTicketsDropdown.Location = new Point(10, 14);
                selectTicketsDropdown.Size = new Size(350, 20);
                selectTicketsDropdown.Text = "What are we working on today?";
                foreach (var ticket in ticketsArray)
                {
                    selectTicketsDropdown.Items.Add(ticket.TicketNumber + ": " + ticket.TicketName);
                }
                selectTicketsDropdown.SelectedIndexChanged += (s, e) => selectTicketsDropdownItem_Click(s, e,mainTicket, historicalPanel);
                mainTicket.Controls.Add(selectTicketsDropdown);
            }
            flowLayoutPanel.Controls.Add(mainTicket);
            // mainTicket End =======================================================================================================================
            
            flowLayoutPanel.Controls.Add(historicalPanel);
            
            // historicalTickets Start ==============================================================================================================
            foreach (var ticket in ticketsArray)
            {
                if (ticket.TicketDate != "NotStarted")
                {
                    //Add date and total time spent on tickets for that day panel
                    if (ticket.TicketDate != ticketDateTime)
                    {
                        ticketDateTime = ticket.TicketDate;
                        Panel ticketsDate = new Panel();
                        ticketsDate.Width = 350;
                        ticketsDate.Height = 20;
                        ticketsDate.BackColor = System.Drawing.Color.WhiteSmoke;
                        TimeSpan timeTotalForToday = TimeSpan.Parse("00:00:00");
                        foreach (var ticketDuplicate in ticketsArray)
                        {
                            if (ticketDuplicate.TicketDate == ticket.TicketDate)
                            {
                                TimeSpan ticketTime = TimeSpan.Parse(ticketDuplicate.TicketTime);
                                timeTotalForToday = timeTotalForToday.Add(ticketTime);
                            }
                        }

                        //Add the date label to the Panel
                        Label ticketsDateLabel = new Label();
                        ticketsDateLabel.Text = ticket.TicketDate;
                        ticketsDateLabel.AutoSize = true;
                        ticketsDateLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                        ticketsDateLabel.ForeColor = System.Drawing.Color.DimGray;
                        ticketsDateLabel.Dock = DockStyle.Left;
                        ticketsDateLabel.Size = new System.Drawing.Size(134, 18);
                        ticketsDate.Controls.Add(ticketsDateLabel);

                        //Add time label to the Panel
                        Label ticketTotalTime = new Label();
                        ticketTotalTime.Text = timeTotalForToday.ToString();
                        ticketTotalTime.AutoSize = true;
                        ticketTotalTime.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                        ticketTotalTime.ForeColor = System.Drawing.Color.DimGray;
                        ticketTotalTime.Dock = DockStyle.Right;
                        ticketTotalTime.Size = new System.Drawing.Size(134, 18);
                        ticketsDate.Controls.Add(ticketTotalTime);
                        historicalPanel.Controls.Add(ticketsDate);
                    }

                    // Create a new historicalTicket
                    Panel historicalTicket = new Panel();
                    historicalTicket.Width = 350;
                    historicalTicket.Height = 61;
                    historicalTicket.BackColor = System.Drawing.Color.White;

                    Label ticketNameLabel = new Label();
                    ticketNameLabel.Text = ticket.TicketName;
                    ticketNameLabel.AutoSize = true;
                    ticketNameLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                    ticketNameLabel.Location = new Point(10, 10);
                    ticketNameLabel.Size = new System.Drawing.Size(134, 18);
                    historicalTicket.Controls.Add(ticketNameLabel);

                    Label projectNameLabel = new Label();
                    projectNameLabel.Text = ticket.ProjectName + ": " + ticket.TicketNumber;
                    projectNameLabel.AutoSize = true;
                    projectNameLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                    projectNameLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
                    projectNameLabel.Location = new Point(10, 30);
                    projectNameLabel.Size = new System.Drawing.Size(126, 18);
                    historicalTicket.Controls.Add(projectNameLabel);

                    Button playButton = new Button();
                    playButton.Name = "playButton";
                    playButton.BackgroundImage = Image.FromFile("playButton.png");
                    playButton.FlatAppearance.BorderSize = 0;
                    playButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                    playButton.Location = new Point(238, 17);
                    playButton.Size = new System.Drawing.Size(28, 31);
                    playButton.UseVisualStyleBackColor = true;
                    playButton.Click += (s, e) => playButton_Click(s, e, ticket.TicketName, ticket.ProjectName + ": " + ticket.TicketNumber, ticket.TicketTime, mainTicket, historicalPanel);
                    historicalTicket.Controls.Add(playButton);

                    Label ticketTimeLabel = new Label();
                    ticketTimeLabel.Text = ticket.TicketTime;
                    ticketTimeLabel.AutoSize = true;
                    ticketTimeLabel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                    ticketTimeLabel.Location = new Point(268, 20);
                    ticketTimeLabel.Size = new System.Drawing.Size(74, 21);
                    historicalTicket.Controls.Add(ticketTimeLabel);
                    historicalPanel.Controls.Add(historicalTicket);
                }
            }
            // historicalTickets End =====================================================================================================================
            
            //Bottom of the application was cut off so added an empty filler panel at the bottom
            Panel filler = new Panel();
            filler.Height = 5;
            filler.Width = 50;
            historicalPanel.Controls.Add(filler);
        }
        
        // Round the corners of the application
        [System.Runtime.InteropServices.DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(int nLeftRect, int nTopRect,int nRightRect,int nBottomRect,int nWidthEllipse,int nHeightEllipse);

        int elapsedSeconds = 0;
        private void startStopButton_Click(object sender, EventArgs e, Label mainTicketTimeLabel, Label mainticketName, FlowLayoutPanel historicalPanel, Label mainProjectNameLabel, Panel mainTicket)
        {
            Button startStopButton = this.Controls[0].Controls[1].Controls[3] as Button;
            if (timer == null)
            {
                elapsedSeconds = 0;
                timer = new System.Windows.Forms.Timer();
                timer.Interval = 1000;
                timer.Tick += (sender, e) => Timer_Tick(sender, e);
                timer.Start();
                Button selectTicketsButton = this.Controls[0].Controls[1].Controls["selectTicketsButton"] as Button;
                selectTicketsButton.Enabled = false;
                startStopButton.BackgroundImage = Image.FromFile("stopButton.png");
                //Disable all playButtons while timer is running
                foreach (Control control in historicalPanel.Controls)
                {
                    foreach (Control item in control.Controls)
                    {
                        if (item is Button && item.Name == "playButton" || item is Button && item.Name == "selectTicketsButton")
                        {
                            Button button = item as Button;
                            button.Enabled = false;
                        }
                    }
                }
            }
            else
            {
                if (!timer.Enabled)
                {
                    elapsedSeconds = 0;
                    timer.Start();
                    Button selectTicketsButton = this.Controls[0].Controls[1].Controls["selectTicketsButton"] as Button;
                    selectTicketsButton.Enabled = false;
                    startStopButton.BackgroundImage = Image.FromFile("stopButton.png");
                    //Disable all playButtons while timer is running
                    foreach (Control control in historicalPanel.Controls)
                    {
                        foreach (Control item in control.Controls)
                        {
                            if (item is Button && item.Name == "playButton")
                            {
                                Button button = item as Button;
                                button.Enabled = false;
                            }
                        }
                    }
                }
                else
                {
                    timer.Stop();
                    Button selectTicketsButton = this.Controls[0].Controls[1].Controls["selectTicketsButton"] as Button;
                    selectTicketsButton.Enabled = true;
                    int ticketIndex = ticketsArray.FindIndex(t => t.TicketName == mainticketName.Text && t.TicketDate == "Today");
                    if (ticketIndex > -1)
                    {
                        //Update the historicalTicketPanel that has the same ticketName and falls under the Today section
                        foreach (Control control in historicalPanel.Controls)
                        {
                            foreach (Control item in control.Controls)
                            {
                                if (item is System.Windows.Forms.Label && item.Text == mainticketName.Text)
                                {
                                    Label ticketTimeLabel = control.Controls[control.Controls.IndexOf(item) + 3] as Label;
                                    ticketTimeLabel.Text = mainTicketTimeLabel.Text;
                                    elapsedSeconds = 0;
                                    goto End;
                                }
                                //Update the total time in the Today section
                                if (item is System.Windows.Forms.Label && item.Text == "Today")
                                {
                                    Label todaysTimeLabel = control.Controls[control.Controls.IndexOf(item) + 1] as Label;
                                    TimeSpan newTime = TimeSpan.Parse(todaysTimeLabel.Text).Add(TimeSpan.FromSeconds(elapsedSeconds));
                                    elapsedSeconds = 0;
                                    todaysTimeLabel.Text = newTime.ToString();
                                }
                            }
                        }
                        End: 
                        {
                            ticketsArray[ticketIndex].TicketTime = mainTicketTimeLabel.Text;
                        }
                    }
                    else
                    {
                        //Update the ticketsArray with the new entry for today
                        ticketsArray.Insert(0,
                            new Ticket()
                            {
                                MainTicket = true,
                                TicketId = 5,
                                TicketDate = "Today",
                                TicketName = mainticketName.Text,
                                ProjectName = mainProjectNameLabel.Text.Substring(0, mainProjectNameLabel.Text.IndexOf(":")),
                                TicketNumber = mainProjectNameLabel.Text.Substring(mainProjectNameLabel.Text.IndexOf(":") + 2),
                                TicketTime = mainTicketTimeLabel.Text,
                            }
                        ) ;
                        //Remove MainTicket = true from all other tickets
                        for (int i = 1; i < ticketsArray.Count; i++)
                        {
                            ticketsArray[i].MainTicket = false;
                        }

                        foreach (Control control in historicalPanel.Controls)
                        {
                            foreach (Control item in control.Controls)
                            {
                                //Update the total time in the Today section
                                if (item is System.Windows.Forms.Label && item.Text == "Today")
                                {
                                    Label todaysTimeLabel = control.Controls[control.Controls.IndexOf(item) + 1] as Label;
                                    TimeSpan newTime = TimeSpan.Parse(todaysTimeLabel.Text).Add(TimeSpan.FromSeconds(elapsedSeconds));
                                    elapsedSeconds = 0;
                                    todaysTimeLabel.Text = newTime.ToString();
                                }
                            }
                        }

                        //Add a panel under the Today section for the mainTicket
                        if (historicalPanel.Controls.OfType<Panel>().Any(panel => panel.Controls.OfType<Label>().Any(label => label.Text == "Today")))
                        {
                            //Create a new historicalTicket for the mainTicket and place it under the Today section
                            Panel historicalTicket = new Panel();
                            historicalTicket.Width = 350;
                            historicalTicket.Height = 61;
                            historicalTicket.BackColor = System.Drawing.Color.White;

                            Label ticketNameLabel = new Label();
                            ticketNameLabel.Text = mainticketName.Text;
                            ticketNameLabel.AutoSize = true;
                            ticketNameLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                            ticketNameLabel.Location = new Point(10, 10);
                            ticketNameLabel.Size = new System.Drawing.Size(134, 18);
                            historicalTicket.Controls.Add(ticketNameLabel);

                            Label projectNameLabel = new Label();
                            projectNameLabel.Text = mainProjectNameLabel.Text;
                            projectNameLabel.AutoSize = true;
                            projectNameLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                            projectNameLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
                            projectNameLabel.Location = new Point(10, 30);
                            projectNameLabel.Size = new System.Drawing.Size(126, 18);
                            historicalTicket.Controls.Add(projectNameLabel);

                            Button playButton = new Button();
                            playButton.Name = "playButton";
                            playButton.BackgroundImage = Image.FromFile("playButton.png");
                            playButton.FlatAppearance.BorderSize = 0;
                            playButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            playButton.Location = new Point(238, 17);
                            playButton.Size = new System.Drawing.Size(28, 31);
                            playButton.UseVisualStyleBackColor = true;
                            playButton.Click += (s, e) => playButton_Click(s, e, ticketNameLabel.Text, projectNameLabel.Text, mainTicketTimeLabel.Text, mainTicket, historicalPanel);
                            historicalTicket.Controls.Add(playButton);

                            Label ticketTimeLabel = new Label();
                            ticketTimeLabel.Text = mainTicketTimeLabel.Text;
                            ticketTimeLabel.AutoSize = true;
                            ticketTimeLabel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                            ticketTimeLabel.Location = new Point(268, 20);
                            ticketTimeLabel.Size = new System.Drawing.Size(74, 21);
                            historicalTicket.Controls.Add(ticketTimeLabel);
                            historicalPanel.Controls.Add(historicalTicket);
                            historicalPanel.Controls.SetChildIndex(historicalTicket, 1);
                            }
                        else
                        {
                            //Add a Today and TodaysTime panel
                            Panel ticketsDate = new Panel();
                            ticketsDate.Width = 350;
                            ticketsDate.Height = 20;
                            ticketsDate.BackColor = System.Drawing.Color.WhiteSmoke;
                            //Add the date label to the Panel
                            Label ticketsDateLabel = new Label();
                            ticketsDateLabel.Text = "Today";
                            ticketsDateLabel.AutoSize = true;
                            ticketsDateLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                            ticketsDateLabel.ForeColor = System.Drawing.Color.DimGray;
                            ticketsDateLabel.Dock = DockStyle.Left;
                            ticketsDateLabel.Size = new System.Drawing.Size(134, 18);
                            ticketsDate.Controls.Add(ticketsDateLabel);
                            //Add time label to the Panel
                            Label ticketTotalTime = new Label();
                            ticketTotalTime.Text = mainTicketTimeLabel.Text;
                            ticketTotalTime.AutoSize = true;
                            ticketTotalTime.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                            ticketTotalTime.ForeColor = System.Drawing.Color.DimGray;
                            ticketTotalTime.Dock = DockStyle.Right;
                            ticketTotalTime.Size = new System.Drawing.Size(134, 18);
                            ticketsDate.Controls.Add(ticketTotalTime);
                            historicalPanel.Controls.Add(ticketsDate);
                            historicalPanel.Controls.SetChildIndex(ticketsDate, 1);

                            //Create a new historicalTicket for the mainTicket and place it under the Today section
                            Panel historicalTicket = new Panel();
                            historicalTicket.Width = 350;
                            historicalTicket.Height = 61;
                            historicalTicket.BackColor = System.Drawing.Color.White;

                            Label ticketNameLabel = new Label();
                            ticketNameLabel.Text = mainticketName.Text;
                            ticketNameLabel.AutoSize = true;
                            ticketNameLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                            ticketNameLabel.Location = new Point(10, 10);
                            ticketNameLabel.Size = new System.Drawing.Size(134, 18);
                            historicalTicket.Controls.Add(ticketNameLabel);

                            Label projectNameLabel = new Label();
                            projectNameLabel.Text = mainProjectNameLabel.Text;
                            projectNameLabel.AutoSize = true;
                            projectNameLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                            projectNameLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
                            projectNameLabel.Location = new Point(10, 30);
                            projectNameLabel.Size = new System.Drawing.Size(126, 18);
                            historicalTicket.Controls.Add(projectNameLabel);

                            Button playButton = new Button();
                            playButton.Name = "playButton";
                            playButton.BackgroundImage = Image.FromFile("playButton.png");
                            playButton.FlatAppearance.BorderSize = 0;
                            playButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            playButton.Location = new Point(238, 17);
                            playButton.Size = new System.Drawing.Size(28, 31);
                            playButton.UseVisualStyleBackColor = true;
                            playButton.Click += (s, e) => playButton_Click(s, e, mainticketName.Text, mainProjectNameLabel.Text, mainTicketTimeLabel.Text, mainTicket, historicalPanel);
                            historicalTicket.Controls.Add(playButton);

                            Label ticketTimeLabel = new Label();
                            ticketTimeLabel.Text = mainTicketTimeLabel.Text;
                            ticketTimeLabel.AutoSize = true;
                            ticketTimeLabel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                            ticketTimeLabel.Location = new Point(268, 20);
                            ticketTimeLabel.Size = new System.Drawing.Size(74, 21);
                            historicalTicket.Controls.Add(ticketTimeLabel);
                            historicalPanel.Controls.Add(historicalTicket);
                            historicalPanel.Controls.SetChildIndex(historicalTicket, 1);
                        }

                    }
                    startStopButton.BackgroundImage = Image.FromFile("startButton.png");
                    //Enable all playButtons when timer is stopped
                    foreach (Control control in historicalPanel.Controls)
                    {
                        foreach (Control item in control.Controls)
                        {
                            if (item is Button && item.Name == "playButton")
                            {
                                Button button = item as Button;
                                button.Enabled = true;
                            }
                        }
                    }
                }

            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            elapsedSeconds++;
            // Parse the current time in the ticketsArray object
            TimeSpan currentTime = TimeSpan.Parse(this.Controls[0].Controls[1].Controls[2].Text);

            // Increment the current time by the elapsed seconds
            TimeSpan newTime = currentTime.Add(TimeSpan.Parse("00:00:01"));

            // Update the TicketTime value in the ticketsArray object
            this.Controls[0].Controls[1].Controls[2].Text = newTime.ToString();
            SystemEvents.SessionSwitch += new SessionSwitchEventHandler(SystemEvents_SessionSwitch);
        }
        private void SystemEvents_SessionSwitch(object s, SessionSwitchEventArgs e) 
        {
            if (e.Reason == SessionSwitchReason.SessionLock)
            {
                if (timer.Enabled) { 
                    startStopButton_Click(s, e, this.Controls[0].Controls[1].Controls[2] as Label, this.Controls[0].Controls[1].Controls[0] as Label, this.Controls[0].Controls[2] as FlowLayoutPanel, this.Controls[0].Controls[1].Controls[1] as Label, this.Controls[0].Controls[1] as Panel);
                }
            }
        }
        private void playButton_Click(object sender, EventArgs e, string ticketName, string projectName, string ticketTime, Panel mainTicket, FlowLayoutPanel historicalPanel)
        {
            try
            {
                mainTicket.Controls[0].Text = ticketName;
                mainTicket.Controls[1].Text = projectName;
                if (ticketsArray.FindIndex(t => t.TicketName == ticketName && t.TicketDate == "Today") > -1)
                {
                    mainTicket.Controls[2].Text = ticketsArray[ticketsArray.FindIndex(t => t.TicketName == ticketName && t.TicketDate == "Today")].TicketTime;
                }
                else
                {
                    mainTicket.Controls[2].Text = "00:00:00";
                }
            }
            catch (System.ArgumentOutOfRangeException)
            {
                mainTicket.Controls.Clear();

                Label mainTicketNameLabel = new Label();
                mainTicketNameLabel.Name = "mainTicketLabel";
                mainTicketNameLabel.Text = ticketName;
                mainTicketNameLabel.AutoSize = true;
                mainTicketNameLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                mainTicketNameLabel.Location = new Point(10, 10);
                mainTicketNameLabel.Size = new System.Drawing.Size(134, 18);
                mainTicket.Controls.Add(mainTicketNameLabel);

                Label mainProjectNameLabel = new Label();
                mainProjectNameLabel.Name = "mainProjectLabel";
                mainProjectNameLabel.Text = projectName;
                mainProjectNameLabel.AutoSize = true;
                mainProjectNameLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
                mainProjectNameLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
                mainProjectNameLabel.Location = new Point(10, 30);
                mainProjectNameLabel.Size = new System.Drawing.Size(126, 18);
                mainTicket.Controls.Add(mainProjectNameLabel);

                Label mainTicketTimeLabel = new Label();
                mainTicketTimeLabel.Name = "mainTicketTimeLabel";
                mainTicketTimeLabel.Text = ticketTime;
                mainTicketTimeLabel.AutoSize = true;
                mainTicketTimeLabel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
                mainTicketTimeLabel.Location = new Point(225, 20);
                mainTicketTimeLabel.Size = new System.Drawing.Size(74, 21);
                mainTicket.Controls.Add(mainTicketTimeLabel);

                Button startStopButton = new Button();
                startStopButton.BackgroundImage = Image.FromFile("startButton.png");
                startStopButton.BackgroundImageLayout = ImageLayout.Stretch;
                startStopButton.FlatAppearance.BorderSize = 0;
                startStopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                startStopButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
                startStopButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
                startStopButton.Location = new Point(305, 13);
                startStopButton.Size = new System.Drawing.Size(36, 36);
                startStopButton.UseVisualStyleBackColor = true;
                startStopButton.Click += (s, e) => startStopButton_Click(s, e, mainTicketTimeLabel, mainTicketNameLabel, historicalPanel, mainProjectNameLabel, mainTicket);
                mainTicket.Controls.Add(startStopButton);

                Button removeCurrentTicketButton = new Button();
                removeCurrentTicketButton.Name = "selectTicketsButton";
                removeCurrentTicketButton.BackgroundImage = Image.FromFile("selectTicketsButton.png");
                removeCurrentTicketButton.BackgroundImageLayout = ImageLayout.Stretch;
                removeCurrentTicketButton.FlatAppearance.BorderSize = 0;
                removeCurrentTicketButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                removeCurrentTicketButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
                removeCurrentTicketButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
                removeCurrentTicketButton.Location = new Point(345, 21);
                removeCurrentTicketButton.Size = new System.Drawing.Size(20, 20);
                removeCurrentTicketButton.UseVisualStyleBackColor = true;
                removeCurrentTicketButton.Click += (s, e) => selectNewTicket_Click(s, e, mainTicket, historicalPanel);
                mainTicket.Controls.Add(removeCurrentTicketButton);
            }
        }
        private void Show_Click(object sender, EventArgs e)
        {
            this.Show();
        }
        private void Hide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void Refresh_Click(object sender, EventArgs e)
        {
            /*Form popup = new Form();
            popup.ShowInTaskbar = false;
            popup.Size = new Size(20, 20);
            popup.StartPosition = FormStartPosition.Manual;
            popup.FormBorderStyle = FormBorderStyle.None;
            popup.Location = new Point(1515, 515);
            popup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            Label label = new Label();
            label.Text = "Updating..";
            label.Location = new Point(45, 2);
            label.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            label.ForeColor = System.Drawing.SystemColors.Control;
            popup.Controls.Add(label);
            popup.Show();
            System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000000;
            timer.Tick += (s, e) => { popup.Close(); };
            timer.Start();*/
        }
        private void Minimize_Click(object sender, EventArgs e)
        {
            Button Minimize = (Button)sender;
            minState = !minState;
            if (minState)
            {
                Image minimizeImage = Image.FromFile("expandButton.png");
                Image minimizeThumbnail = minimizeImage.GetThumbnailImage(20, 20, null, IntPtr.Zero);
                Minimize.BackgroundImage = minimizeThumbnail;
            }
            else
            {
                Image minimizeImage = Image.FromFile("minimizeButton.png");
                Image minimizeThumbnail = minimizeImage.GetThumbnailImage(18, 18, null, IntPtr.Zero);
                Minimize.BackgroundImage = minimizeThumbnail;
            }
            minimizeTimer.Start();
        }
        private void minimizeTimer_Tick(object sender, EventArgs e)
        {
            if (minState)
            {
                this.Size = new Size(this.Size.Width, this.Size.Height - 10);
                this.Location = new Point(this.Location.X, this.Location.Y + 10);
                if (this.Size.Height <= 110)
                {
                    minimizeTimer.Stop();
                }
            }
            else
            {
                this.Size = new Size(this.Size.Width, this.Size.Height + 10);
                this.Location = new Point(this.Location.X, this.Location.Y - 10);
                if (this.Size.Height >= 469)
                {
                    minimizeTimer.Stop();
                }
            }
        }
        private void Exit_Click(object sender, EventArgs e, NotifyIcon notifyIcon)
        {
            notifyIcon.Dispose();
            Application.Exit();
        }
        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Left = Screen.PrimaryScreen.Bounds.Width - this.Width-25;
            this.Top = Screen.PrimaryScreen.Bounds.Height - this.Height-70;
        }
        private void selectNewTicket_Click(object sender, EventArgs e, Panel mainTicket, FlowLayoutPanel historicalPanel)
        {
            mainTicket.Controls.Clear();
            ComboBox selectTicketsDropdown = new ComboBox();
            selectTicketsDropdown.Location = new Point(10, 14);
            selectTicketsDropdown.Size = new Size(350, 20);
            selectTicketsDropdown.Text = "What are we working on today?";
            List<string> dropdownItems = new List<string> {};
            foreach (var ticket in ticketsArray)
            {
                dropdownItems.Add(ticket.TicketNumber + ": " + ticket.TicketName);
            }
            dropdownItems = dropdownItems.Distinct().ToList();
            dropdownItems.Sort();
            foreach (var item in dropdownItems)
            {
            selectTicketsDropdown.Items.Add(item);
            }
            selectTicketsDropdown.SelectedIndexChanged += (s, e) => selectTicketsDropdownItem_Click(s, e, mainTicket, historicalPanel);
            mainTicket.Controls.Add(selectTicketsDropdown);
        }
        private void selectTicketsDropdownItem_Click(object sender, EventArgs e, Panel mainTicket, FlowLayoutPanel historicalPanel)
        {
            ComboBox selectTicketsDropdown = (ComboBox)sender;
            string selectedItem = selectTicketsDropdown.SelectedItem.ToString();
            string projectName = "unknown";
            string ticketTime = "00:00:00";
            switch (selectedItem.Substring(0, selectedItem.IndexOf("-")))
            {
                case "CSB":
                    projectName = "Secumed";
                    break;
                case "KB":
                    projectName = "Knowledgebase";
                    break;
                case "ISB":
                    projectName = "Interscope";
                    break;
            };
            foreach (var ticket in ticketsArray)
            {
                if (ticket.TicketName == selectedItem.Substring(selectedItem.IndexOf(":") + 2) && ticket.TicketDate == "Today")
                {
                    ticketTime = ticket.TicketTime.ToString();
                }
            }
            mainTicket.Controls.Remove(selectTicketsDropdown);

            Label mainTicketNameLabel = new Label();
            mainTicketNameLabel.Name = "mainTicketLabel";
            mainTicketNameLabel.Text = selectedItem.Substring(selectedItem.IndexOf(":")+2);
            mainTicketNameLabel.AutoSize = true;
            mainTicketNameLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            mainTicketNameLabel.Location = new Point(10, 10);
            mainTicketNameLabel.Size = new System.Drawing.Size(134, 18);
            mainTicket.Controls.Add(mainTicketNameLabel);

            Label mainProjectNameLabel = new Label();
            mainProjectNameLabel.Name = "mainProjectLabel";
            mainProjectNameLabel.Text = projectName + ": " + selectedItem.Substring(0, selectedItem.IndexOf(":"));
            mainProjectNameLabel.AutoSize = true;
            mainProjectNameLabel.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            mainProjectNameLabel.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            mainProjectNameLabel.Location = new Point(10, 30);
            mainProjectNameLabel.Size = new System.Drawing.Size(126, 18);
            mainTicket.Controls.Add(mainProjectNameLabel);

            Label mainTicketTimeLabel = new Label();
            mainTicketTimeLabel.Name = "mainTicketTimeLabel";
            mainTicketTimeLabel.Text = ticketTime;
            mainTicketTimeLabel.AutoSize = true;
            mainTicketTimeLabel.Font = new System.Drawing.Font("Calibri", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            mainTicketTimeLabel.Location = new Point(225, 20);
            mainTicketTimeLabel.Size = new System.Drawing.Size(74, 21);
            mainTicket.Controls.Add(mainTicketTimeLabel);

            Button startStopButton = new Button();
            startStopButton.BackgroundImage = Image.FromFile("startButton.png");
            startStopButton.BackgroundImageLayout = ImageLayout.Stretch;
            startStopButton.FlatAppearance.BorderSize = 0;
            startStopButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            startStopButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            startStopButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            startStopButton.Location = new Point(305, 13);
            startStopButton.Size = new System.Drawing.Size(36, 36);
            startStopButton.UseVisualStyleBackColor = true;
            startStopButton.Click += (s, e) => startStopButton_Click(s, e, mainTicketTimeLabel, mainTicketNameLabel, historicalPanel, mainProjectNameLabel, mainTicket);
            mainTicket.Controls.Add(startStopButton);

            Button removeCurrentTicketButton = new Button();
            removeCurrentTicketButton.Name = "selectTicketsButton";
            removeCurrentTicketButton.BackgroundImage = Image.FromFile("selectTicketsButton.png");
            removeCurrentTicketButton.BackgroundImageLayout = ImageLayout.Stretch;
            removeCurrentTicketButton.FlatAppearance.BorderSize = 0;
            removeCurrentTicketButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            removeCurrentTicketButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.White;
            removeCurrentTicketButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.White;
            removeCurrentTicketButton.Location = new Point(345, 21);
            removeCurrentTicketButton.Size = new System.Drawing.Size(20, 20);
            removeCurrentTicketButton.UseVisualStyleBackColor = true;
            removeCurrentTicketButton.Click += (s, e) => selectNewTicket_Click(s, e, mainTicket, historicalPanel);
            mainTicket.Controls.Add(removeCurrentTicketButton);
        }
    }
}
