using System;
using System.Linq;
using Windows.Media.SpeechSynthesis;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace HSK3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LearnPage : Page
    {
        //string hanci;
        int begin = 0;
        int end = 0;
        int current = 0;

        public LearnPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

            foreach (var item in Database.pages)
            {
                cbbPage.Items.Add(item[0]);
            }
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
            Frame.Navigate(typeof(MainPage));

            throw new NotImplementedException();
        }

        private async void SpeakText(MediaElement audioPlayer, string TTS)
        {
            SpeechSynthesizer ttssynthesizer = new SpeechSynthesizer();

            //Set the Voice/Speaker
            using (var Speaker = new SpeechSynthesizer())
            {
                Speaker.Voice = (SpeechSynthesizer.AllVoices.First(x => x.Language == "zh-CN"));
                ttssynthesizer.Voice = Speaker.Voice;
            }

            SpeechSynthesisStream ttsStream = await ttssynthesizer.SynthesizeTextToStreamAsync(TTS);

            audioPlayer.SetSource(ttsStream, "");
        }

        private void cmdSpeech_Click(object sender, RoutedEventArgs e)
        {
            string hanci = string.Empty;

            try
            {
                hanci = Database.han[current][5];
            }
            catch
            {
                hanci = Database.han[current][0];
            }

            SpeakText(audioPlayer, hanci);
        }

        private void cbbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            begin = int.Parse(Database.pages[cbbPage.SelectedIndex][1]);
            end = int.Parse(Database.pages[cbbPage.SelectedIndex][2]); ;

            current = begin;
            Display();
        }

        private void cmdExample_Click(object sender, RoutedEventArgs e)
        {
            SpeakText(audioPlayer, Database.han[current][1]);
        }

        private void cmdNext_Click(object sender, RoutedEventArgs e)
        {
            current++;
            Display();
        }

        private void Display()
        {
            SetEmpty();

            if (current > end)
            {
                current = begin;
            }

            //hanci = Database.han[current][0];

            txtPinyin.Text = Database.han[current][2];
            txtHan.Text = Database.han[current][0];
            txtViet.Text = Database.han[current][3];
            txtNghia.Text = Database.han[current][4];
        }

        private void SetEmpty()
        {
            txtPinyin.Text = string.Empty;
            txtHan.Text = string.Empty;
            txtNghia.Text = string.Empty;
        }
    }
}
