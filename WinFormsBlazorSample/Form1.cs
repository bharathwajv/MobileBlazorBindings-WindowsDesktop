using Microsoft.Extensions.DependencyInjection;
using Microsoft.MobileBlazorBindings;
using Microsoft.MobileBlazorBindings.Hosting;
using RazorClassLibrarySample;
using System;
using System.Windows.Forms;
using MudBlazor.Services;

namespace WinFormsBlazorSample
{
    public partial class Form1 : Form
    {
        private readonly AppState _appState;

        public Form1()
        {
            InitializeComponent();
            InitInternal();
            blazorWebView1.ComponentType = typeof(RazorClassLibrarySample.SampleWebComponent);

            var hostBuilder = BlazorWebHost.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    // Adds web-specific services such as NavigationManager
                    services.AddBlazorHybrid();
                    services.AddMudServices();
                    // Register app-specific services
                    services.AddSingleton<AppState>();
                })
                .UseWebRoot("wwwroot");

            hostBuilder.UseStaticFiles();
           
            var host = hostBuilder.Build();

            _appState = host.Services.GetRequiredService<AppState>();

            blazorWebView1.Host = host;
        }

        private void InitInternal()
        {
            MessagingCenter.Subscribe<SampleWebComponent, string>(this, "greeting_message", (sender, value) =>
            {
                string greeting = $"Welcome {value}";
                MessageBox.Show($"Current counter value is {_appState.Counter}", "Counter Value");
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SendMessage();
            //MessageBox.Show($"Current counter value is {_appState.Counter}", "Counter Value");
        }
        public void SendMessage()
        {
            string valueToSend = "Hi from Forms";
            MessagingCenter.Send(_appState, "greeting_message", valueToSend);
        }

    }
}
