using System;
using System.Collections.Generic;
using System.Text;

namespace Hotdogapp.Shared.Models
{
    public class InMemoryImageData
    {
        public InMemoryImageData(byte[] image, string label, string imageFileName)
        {
            Image = image;
            Label = label;
            ImageFileName = imageFileName;
        }
        public InMemoryImageData()
        { 
        }


        public byte[] Image;

        public string Label;

        public string ImageFileName;
    }
}
