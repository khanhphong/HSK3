using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.SpeechSynthesis;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace HSK3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TestPage : Page
    {
        RandomSet obj;
        string hanci;
        int begin = 0;
        int end = 0;
        int count = 0;

        public TestPage()
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

        private void cbbPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            begin = int.Parse(Database.pages[cbbPage.SelectedIndex][1]);
            end = int.Parse(Database.pages[cbbPage.SelectedIndex][2]); ;

            obj = new RandomSet(begin, end);
            Speech();
        }

        private void Speech()
        {
            txtPinyin.Text = string.Empty;
            txtHan.Text = string.Empty;
            txtNghia.Text = string.Empty;
            txtSentence.Text = string.Empty;

            try
            {
                count = obj.Next();
            }
            catch (Exception)
            {
            }

            if (count < 0)
            {
                obj = new RandomSet(begin, end);
                count = obj.Next();
            }

            try
            {
                hanci = Database.han[count][5];
            }
            catch
            {
                hanci = Database.han[count][0];
            }

            SpeakText(audioPlayer, hanci);
        }

        private void cmdSpeech_Click(object sender, RoutedEventArgs e)
        {
            SpeakText(audioPlayer, hanci);
        }

        private void cmdExample_Click(object sender, RoutedEventArgs e)
        {
            SpeakText(audioPlayer, Database.han[count][1]);
        }

        private void cmdMean_Click(object sender, RoutedEventArgs e)
        {
            txtPinyin.Text = Database.han[count][2];
            txtHan.Text = Database.han[count][0];
            txtNghia.Text = Database.han[count][4];
            txtSentence.Text = Database.han[count][1];
        }

        private void cmdNext_Click(object sender, RoutedEventArgs e)
        {
            Speech();
        }
    }
}
