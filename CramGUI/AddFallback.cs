using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Cram
{
    public partial class AddFallback : Form
    {
        public AddFallback()
        {
            InitializeComponent();
        }

        public string FallbackSize
        {

            get { return fallbackSizeEdit.Text; }
        }
    }
}