using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Security.Cryptography;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.Storage.AccessCache;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CryptoApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
           
        }

        SymmetricAlgorithm algorithm = null;
        CipherMode cipherMode;
        CryptoController controller;
        CryptoView view;
        CryptoModel model;
        StorageFile file = null;
        string clearText;
        private void RadioButton_Checked_AlgorithmType(object sender, RoutedEventArgs e)
        {
            //Este radio button?
            RadioButton button = sender as RadioButton;
            if (button != null)
            {
                string type = button.Tag.ToString();
                switch (type)
                {
                    case "Aes": algorithm = Aes.Create(); break;
                    case "DES":algorithm = DES.Create();break;
                    case "RC2":algorithm = RC2.Create(); break;
                    case "Rijndael":algorithm = Rijndael.Create();break;
                    case "TripleDES":algorithm = TripleDES.Create();break;

                }
            }
        }
        private   void RadioButton_Checked_EncryptionMode(object sender, RoutedEventArgs e)
        {
            RadioButton button = sender as RadioButton;
            if (button != null)
            {
                string type = button.Tag.ToString();
              
                switch (type)
                {
                    case "CBC": cipherMode = CipherMode.CBC; break;
                    case "CFB": cipherMode = CipherMode.CFB; break;
                    case "CTS":cipherMode = CipherMode.CTS; break;
                    case "ECB": cipherMode = CipherMode.ECB;break;
                    case "OFB":cipherMode = CipherMode.OFB; break;

                }
            }
        }
        private async  void CryptoButton_Click(object sender, RoutedEventArgs e)
        {
        
            if (algorithm==null)
            {
                await new MessageDialog("You haven't selected an algorithm!").ShowAsync();
                return;
            }

         
            clearTextBox.Document.GetText(Windows.UI.Text.TextGetOptions.None, out string clearText);
            if (clearText.Trim() == string.Empty)
            {
                await new MessageDialog("No clear text :( ").ShowAsync();
                return;
            }


                view = new CryptoView();
            algorithm.GenerateKey();
            algorithm.GenerateIV();
            try
            {
                model = new CryptoModel(algorithm, cipherMode);
            }catch(InCompatibleFormatException ex)
            {
                await new MessageDialog("Incompatable").ShowAsync();
                return;
            }
                controller = new CryptoController(model, view);
      
            controller.ClearText = clearText;
                controller.UpdateView(CryptoController.Operation.Encryption,
                    (result) => cryptedTextBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, result));
          
            
        
        }

        private  async void FileButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();

            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.FileTypeFilter.Add("*");
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            StorageFile file = null;
            try
            {
                file = await openPicker.PickSingleFileAsync();
            }
            catch (Exception exception)
            {
                await new MessageDialog(exception.Message).ShowAsync();
            }

            if (file != null)
            {

                pathTextBox.Text = file.Path;


                // await new MessageDialog(file.Path).ShowAsync();
                try
                {
                  File.SetAttributes(file.Path, System.IO.FileAttributes.Normal);
                  StorageApplicationPermissions.FutureAccessList.AddOrReplace("fileOpenPicker", file);
                    clearText = string.Join("", File.ReadAllBytes(file.Path));
                   
                }
                catch (Exception ex)
                {
                    await new MessageDialog(ex.Message).ShowAsync();
                }

            }
        }
    }
}
