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
    public sealed partial class DictionaryPage : Page
    {
        int current = 0;

        public DictionaryPage()
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

            foreach (string[] item in Database.han)
            {
                cbbWords.Items.Add(item[0] + " / " + item[2] + " / : " + item[4]);
            }
        }

        private void cmdInput_Click(object sender, RoutedEventArgs e)
        {
            string wordInput = txtWord.Text.Trim();

            for (int i = 0; i < Database.han.Length; i++)
            {
                if (Database.han[i][0].Equals(wordInput))
                {
                    current = i;
                    break;
                }
                else
                    current = -1;
            }

            if (current > -1)
            {
                Display(current);
            }
            else
            {
                SetEmpty();
            }
        }

        private void Display(int vitri)
        {
            txtPinyin.Text = Database.han[vitri][2];
            txtHan.Text = Database.han[vitri][0];
            txtViet.Text = Database.han[vitri][3];
            txtNghia.Text = Database.han[vitri][4];
            txtSentence.Text = Database.han[vitri][1];
        }

        private void SetEmpty()
        {
            txtPinyin.Text = string.Empty;
            txtHan.Text = string.Empty;
            txtViet.Text = string.Empty;
            txtNghia.Text = string.Empty;
            txtSentence.Text = string.Empty;
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

        private void cmdExample_Click(object sender, RoutedEventArgs e)
        {
            SpeakText(audioPlayer, Database.han[current][1]);
        }

        private void cbbWords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            current = cbbWords.SelectedIndex;
            Display(current);
        }
    }
}
