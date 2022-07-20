using Aspose.BarCode;
using Aspose.BarCode.Generation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace BarcodeGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            License lic = new License();
            lic.SetLicense(@"You license file path goes here");

            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            // Set default as Png
            var imageType = "Png";

            // Get the user selected image format
            if(rbPng.IsChecked == true)
            {
                imageType = rbPng.Content.ToString();
            }
            else if(rbBmp.IsChecked == true)
            {
                imageType = rbBmp.Content.ToString();
            }
            else if(rbJpg.IsChecked == true)
            {
                imageType = rbJpg.Content.ToString();
            }

            // Get image format from enum
            var imageFormat = (BarCodeImageFormat)Enum.Parse(typeof(BarCodeImageFormat), imageType.ToString());

            // Set default as Code128
            var encodeType = EncodeTypes.Code128;

            // Get the user selected barcode type
            if (!string.IsNullOrEmpty(comboBarcodeType.Text))
            {
                switch (comboBarcodeType.Text)
                {
                    case "Code128":
                        encodeType = EncodeTypes.Code128;
                        break;

                    case "ITF14":
                        encodeType = EncodeTypes.ITF14;
                        break;

                    case "EAN13":
                        encodeType = EncodeTypes.EAN13;
                        break;

                    case "Datamatrix":
                        encodeType = EncodeTypes.DataMatrix;
                        break;

                    case "Code32":
                        encodeType = EncodeTypes.Code32;
                        break;

                    case "Code11":
                        encodeType = EncodeTypes.Code11;
                        break;

                    case "PDF417":
                        encodeType = EncodeTypes.Pdf417;
                        break;

                    case "EAN8":
                        encodeType = EncodeTypes.EAN8;
                        break;

                    case "QR":
                        encodeType = EncodeTypes.QR;
                        break;
                }
            }

            // Initalize barcode object
            Barcode barcode = new Barcode();
            barcode.Text = tbCodeText.Text;
            barcode.BarcodeType = encodeType;
            barcode.ImageType = imageFormat;

            try
            {
                string imagePath = "";

                if (cbGenerateWithOptions.IsChecked == true)
                {
                    // Generate barcode with additional options and get the image path
                    imagePath = GenerateBarcodeWithOptions(barcode);
                }
                else
                {
                    // Generate barcode and get image path
                    imagePath = GenerateBarcode(barcode);
                }

                // Display the image
                Uri fileUri = new Uri(Path.GetFullPath(imagePath));
                imgDynamic.Source = new BitmapImage(fileUri);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string GenerateBarcode(Barcode barcode)
        {
            // Image path
            string imagePath = comboBarcodeType.Text + "." + barcode.ImageType;

            // Initilize barcode generator
            BarcodeGenerator generator = new BarcodeGenerator(barcode.BarcodeType, barcode.Text);
            
            // Save the image
            generator.Save(imagePath, barcode.ImageType);

            return imagePath;
        }

        private string GenerateBarcodeWithOptions(Barcode barcode)
        {
            // Image path
            string imagePath = comboBarcodeType.Text + "." + barcode.ImageType;

            // Initilize barcode generator
            BarcodeGenerator generator = new BarcodeGenerator(barcode.BarcodeType, barcode.Text);

            if(barcode.BarcodeType == EncodeTypes.QR)
            {
                generator.Parameters.Barcode.XDimension.Pixels = 4;
                //set Auto version
                generator.Parameters.Barcode.QR.QrVersion = QRVersion.Auto;
                //Set Auto QR encode type
                generator.Parameters.Barcode.QR.QrEncodeType = QREncodeType.Auto;
            } 
            else if(barcode.BarcodeType == EncodeTypes.Pdf417)
            {
                generator.Parameters.Barcode.XDimension.Pixels = 2;
                generator.Parameters.Barcode.Pdf417.Columns = 3;
            }
            else if(barcode.BarcodeType == EncodeTypes.DataMatrix)
            {
                //set DataMatrix ECC to 140
                generator.Parameters.Barcode.DataMatrix.DataMatrixEcc = DataMatrixEccType.Ecc200;
            }
            else if(barcode.BarcodeType == EncodeTypes.Code32)
            {
                generator.Parameters.Barcode.XDimension.Millimeters = 1f;
            }
            else
            {
                generator.Parameters.Barcode.XDimension.Pixels = 2;
                //set BarHeight 40
                generator.Parameters.Barcode.BarHeight.Pixels = 40;
            }

            // Save the image
            generator.Save(imagePath, barcode.ImageType);

            return imagePath;
        }
    }
}
