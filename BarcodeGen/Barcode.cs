using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aspose.BarCode;
using Aspose.BarCode.Generation;

namespace BarcodeGen
{
    public class Barcode
    {
        public string? Text { get; set; }

        public BaseEncodeType? BarcodeType { get; set; }

        public BarCodeImageFormat ImageType { get; set; }
    }
}
